using TechStoreApp.Data.Models;

namespace TechStoreApp.Data.Seeding
{
    public class SeedDataPaymentTypes
    {
        public static List<PaymentDetail> GetPaymentTypes()
        {
            return new List<PaymentDetail>
            {
                new PaymentDetail
                {
                    PaymentType = "Credit Card",
                    Description = "Pay securely using your credit or debit card."
                },
                new PaymentDetail
                {
                    PaymentType = "PayPal",
                    Description = "Use your PayPal account for fast and secure checkout."
                },
                new PaymentDetail
                {
                    PaymentType = "Bank Transfer",
                    Description = "Transfer funds directly from your bank account."
                },
                new PaymentDetail
                {
                    PaymentType = "Cash on Delivery",
                    Description = "Pay with cash when your order is delivered."
                },
                new PaymentDetail
                {
                    PaymentType = "Cryptocurrency",
                    Description = "Pay using Bitcoin or other supported cryptocurrencies."
                }
            };
        }
    }
}
