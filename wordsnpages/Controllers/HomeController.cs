using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using wordsnpages.Models;
using wordsnpages.Repositories.Interfaces;
using wordsnpages.Utilities;

namespace wordsnpages.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index(string categoryId)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (claim != null)
            {
                HttpContext.Session.SetInt32(SD.SessionCart,
                    _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value).Count());
            }

            IEnumerable<Product> productList = _unitOfWork.Product.GetAll(includeProperties: "Category");

            // Check if categoryId is provided and is a valid value
            if (!string.IsNullOrEmpty(categoryId))
            {
                switch (categoryId.ToLower())
                {
                    case "action":
                        productList = productList.Where(u => u.CategoryId == 1);
                        break;
                    case "thriller":
                        productList = productList.Where(u => u.CategoryId == 2);
                        break;
                    case "romance":
                        productList = productList.Where(u => u.CategoryId == 3);
                        break;
                    case "mystery":
                        productList = productList.Where(u => u.CategoryId == 4);
                        break;
                    case "fantasy":
                        productList = productList.Where(u => u.CategoryId == 5);
                        break;
                    // Add more cases for additional categories
                    default:
                        // Handle default case if needed
                        break;
                }
            }

            return View(productList);
        }

        public IActionResult Details(int productId)
        {
            ShoppingCart cart = new ShoppingCart()
            {
                Product  = _unitOfWork.Product.Get(u => u.Id==productId, includeProperties: "Category"),
                Count = 1,
                ProductId=productId
            };
            return View(cart);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            //get user ID
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            shoppingCart.ApplicationUserId = userId;

            //check if the product is already in the shopping cart
            ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.Get(u =>
                u.ApplicationUserId == userId && u.ProductId == shoppingCart.ProductId);

            if (cartFromDb != null)
            {
                //update the existing shopping cart item
                cartFromDb.Count += shoppingCart.Count;
                _unitOfWork.ShoppingCart.Update(cartFromDb);
                _unitOfWork.Save();

            }
            else
            {
                //add a new shopping cart item
                _unitOfWork.ShoppingCart.Add(shoppingCart);
                _unitOfWork.Save();
                //number of items that the user has
                HttpContext.Session.SetInt32(SD.SessionCart,
                    _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId).Count());
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}