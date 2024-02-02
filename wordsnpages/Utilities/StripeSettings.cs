namespace wordsnpages.Utilities
{
    //when we go to stripe it creates session, if session is successful it give us payment intent id
    public class StripeSettings
    {
        public string SecretKey { get; set; }
        public string PublishableKey { get; set; }
    }
}
