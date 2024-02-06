using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Versioning;
using Stripe;
using Stripe.Checkout;
using System.Diagnostics;
using System.Security.Claims;
using wordsnpages.Models;
using wordsnpages.Models.ViewModels;
using wordsnpages.Repositories.Interfaces;
using wordsnpages.Utilities;

namespace wordsnpages.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        [BindProperty]
        public OrderVM OrderVM { get; set; }
        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Details(int orderId)
        {

            OrderVM = new()
            {
                OrderHeader = _unitOfWork.OrderHeader.Get(u => u.Id==orderId, includeProperties: "ApplicationUser"),
                OrderDetail = _unitOfWork.OrderDetail.GetAll(u => u.OrderHeaderId==orderId, includeProperties: "Product")

            };

            return View(OrderVM);
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_Admin+","+SD.Role_Employee)]
        public IActionResult UpdateOrderDetail()
        {
            var orderHeaderFromDb = _unitOfWork.OrderHeader.Get(u => u.Id==OrderVM.OrderHeader.Id);

            orderHeaderFromDb.Name=OrderVM.OrderHeader.Name;
            orderHeaderFromDb.PhoneNumber=OrderVM.OrderHeader.PhoneNumber;
            orderHeaderFromDb.StreetAddress=OrderVM.OrderHeader.StreetAddress;
            orderHeaderFromDb.City=OrderVM.OrderHeader.City;
            orderHeaderFromDb.State=OrderVM.OrderHeader.State;
            orderHeaderFromDb.PostalCode=OrderVM.OrderHeader.PostalCode;

            if (!string.IsNullOrEmpty(OrderVM.OrderHeader.Carrier))
            {
                orderHeaderFromDb.Carrier=OrderVM.OrderHeader.Carrier;
            }
            if (!string.IsNullOrEmpty(OrderVM.OrderHeader.TrackingNumber))
            {
                orderHeaderFromDb.Carrier=OrderVM.OrderHeader.TrackingNumber;
            }
            _unitOfWork.OrderHeader.Update(orderHeaderFromDb);
            _unitOfWork.Save();

            TempData["Success"]="Order Details Updated Successfully.";

            return RedirectToAction(nameof(Details), new { orderId = orderHeaderFromDb.Id });
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_Admin+","+SD.Role_Employee)]
        public IActionResult StartProcessing()
        {
            // Retrieve order header from the database
            var orderHeaderFromDb = _unitOfWork.OrderHeader.Get(u => u.Id == OrderVM.OrderHeader.Id);

            // Update order status to "In Process"
            orderHeaderFromDb.OrderStatus = SD.StatusInProcess;

            // Update the order header in the database
            _unitOfWork.OrderHeader.Update(orderHeaderFromDb);
            _unitOfWork.Save();

            TempData["Success"] = "Order Details Updated Successfully.";
            return RedirectToAction(nameof(Details), new { orderId = OrderVM.OrderHeader.Id });
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_Admin+","+SD.Role_Employee)]
        public IActionResult ShipOrder()
        {
            var orderHeader = _unitOfWork.OrderHeader.Get(u => u.Id ==OrderVM.OrderHeader.Id);
            //update
            orderHeader.TrackingNumber = OrderVM.OrderHeader.TrackingNumber;
            orderHeader.Carrier = OrderVM.OrderHeader.Carrier;
            orderHeader.OrderStatus = SD.StatusShipped;
            orderHeader.ShippingDate = DateTime.Now;

            // If the payment was delayed, set the payment due date
            if (orderHeader.PaymentStatus==SD.PaymentStatusDelayedPayment)
            {
                orderHeader.PaymentDueDate=DateOnly.FromDateTime(DateTime.Now.AddDays(30));
            }

            // Update the order header in the database
            _unitOfWork.OrderHeader.Update(orderHeader);
            _unitOfWork.Save();

            TempData["Success"]="Order Shipped Successfully.";

            return RedirectToAction(nameof(Details), new { orderId = OrderVM.OrderHeader.Id });

        }


        [HttpPost]
        [Authorize(Roles = SD.Role_Admin+","+SD.Role_Employee)]
        public IActionResult CancelOrder()
        {
            var orderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == OrderVM.OrderHeader.Id);

            // If the order was paid, process a refund using Stripe
            if (orderHeader.PaymentStatus == SD.PaymentStatusApproved)
            {
                try
                {
                    // Processing refund using Stripe
                    var options = new RefundCreateOptions
                    {
                        Reason = RefundReasons.RequestedByCustomer,
                        PaymentIntent = orderHeader.PaymentIntentId
                    };
                    var service = new RefundService();
                    Refund refund = service.Create(options);
                }
                catch (Exception ex)
                {
                    // Log or handle the exception
                    TempData["Error"] = "Failed to process refund: " + ex.Message;
                    return RedirectToAction(nameof(Details), new { orderId = OrderVM.OrderHeader.Id });
                }

                // Update order status and payment status
                _unitOfWork.OrderHeader.UpdateStatus(orderHeader.Id, SD.StatusCancelled, SD.StatusRefunded);
            }
            else
            {
                // Update order status
                _unitOfWork.OrderHeader.UpdateStatus(orderHeader.Id, SD.StatusCancelled, SD.StatusCancelled);
            }

            _unitOfWork.Save();
            TempData["Success"] = "Order Cancelled Successfully.";
            return RedirectToAction(nameof(Details), new { orderId = OrderVM.OrderHeader.Id });
        }

        [ActionName("Details")]
        [HttpPost]
        public IActionResult Details_PAY_NOW()
        {
            // Retrieve order header and details
            OrderVM.OrderHeader = _unitOfWork.OrderHeader
                .Get(u => u.Id == OrderVM.OrderHeader.Id, includeProperties: "ApplicationUser");
            OrderVM.OrderDetail = _unitOfWork.OrderDetail
                .GetAll(u => u.OrderHeaderId == OrderVM.OrderHeader.Id, includeProperties: "Product");

            // Create a new Stripe session for payment
            var domain = Request.Scheme+ "://"+Request.Host.Value +"/";
            var options = new SessionCreateOptions
            {
                SuccessUrl=domain+$"/order/PaymentConfirmation?orderHeaderId={OrderVM.OrderHeader.Id}",
                CancelUrl=domain+$"/order/details?orderId={OrderVM.OrderHeader.Id}",
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
            };

            // Add line items to the Stripe session
            foreach (var item in OrderVM.OrderDetail)
            {
                var sessionLineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions()
                    {
                        UnitAmount=(long)(item.Price*100),
                        Currency="usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name=item.Product.Title
                        }
                    },
                    Quantity=item.Count
                };

                options.LineItems.Add(sessionLineItem);
            }

            // Create the Stripe session
            var service = new SessionService();
            Session session = service.Create(options);

            // Update the order header with Stripe payment details
            _unitOfWork.OrderHeader.UpdateStripePaymentID(OrderVM.OrderHeader.Id, session.Id, session.PaymentIntentId);
            _unitOfWork.Save();

            // Redirect to the Stripe payment page
            Response.Headers.Add("location", session.Url);
            return new StatusCodeResult(303);
        
        }

        public IActionResult PaymentConfirmation(int orderHeaderId)
        {
            // Based on id retrieves the complete order header
            OrderHeader orderHeader = _unitOfWork.OrderHeader.Get(u => u.Id==orderHeaderId);
            if (orderHeader.PaymentStatus==SD.PaymentStatusDelayedPayment)
            {
                // Order by company, retrieve stripe session
                var service = new SessionService(); //built in class in stripe
                Session session = service.Get(orderHeader.SessionId);

                if (session.PaymentStatus.ToLower()=="paid")
                {
                    var paymentIntentId = session.PaymentIntentId;
                    if (paymentIntentId!=null)
                    {
                        _unitOfWork.OrderHeader.UpdateStripePaymentID(orderHeaderId, session.Id, session.PaymentIntentId);
                    }
                    _unitOfWork.OrderHeader.UpdateStatus(orderHeaderId, orderHeader.OrderStatus, SD.PaymentStatusApproved);
                    _unitOfWork.Save();

                }
            }

            return View(orderHeaderId);
        }


        #region API CALLS

        [HttpGet]
        public IActionResult GetAll(string status)
        {
            IEnumerable<OrderHeader> objOrderHeaders;

            if (User.IsInRole(SD.Role_Admin) || User.IsInRole(SD.Role_Employee))
            {
                objOrderHeaders  = _unitOfWork.OrderHeader.GetAll(includeProperties: "ApplicationUser").ToList();
            }
            else
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

                objOrderHeaders = _unitOfWork.OrderHeader
                    .GetAll(u => u.ApplicationUserId==userId, includeProperties: "ApplicationUser");
            }

            switch (status)
            {

                case "pending":
                    objOrderHeaders=objOrderHeaders.Where(u => u.PaymentStatus==SD.PaymentStatusDelayedPayment);
                    break;
                case "inprocess":
                    objOrderHeaders=objOrderHeaders.Where(u => u.OrderStatus==SD.StatusInProcess);
                    break;
                case "completed":
                    objOrderHeaders=objOrderHeaders.Where(u => u.OrderStatus==SD.StatusShipped);
                    break;
                case "approved":
                    objOrderHeaders=objOrderHeaders.Where(u => u.OrderStatus==SD.StatusApproved);
                    break;
                default:
                    break;
            }

            return Json(new { data = objOrderHeaders });
        }


        #endregion
    }
}
