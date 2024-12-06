using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStoreApp.Common
{
    public static class EntityValidationConstraints
    {
        public static class Address
        {
            public const int minAddressStringLength = 10;
            public const int maxAddressStringLength = 500;

            public const int minCountryStringLength = 2;
            public const int maxCountryStringLength = 100;

            public const int minCityStringLength = 2;
            public const int maxCityStringLength = 100;

            public const int minPostalCode = 0;
            public const int maxPostalCode = 10000;
        }

        public static class User
        {
            public const int maxPfpImageUrlStringLength = 200;
        }

        public static class Cart
        {

        }

        public static class CartItem
        {
            public const int minQuantityCount = 0;
        }

        public static class Category
        {
            public const int minDescriptionStringLength = 2; 
            public const int maxDescriptionStringLength = 50;
        }

        public static class Favorited
        {

        }

        public static class Order
        {
            public const double minTotalAmount = 0;
            public const int minShippingAddressStringLength = 10;
            public const int maxShippingAddressStringLength = 500;
        }

        public static class OrderDetail
        {
            public const int minQuantityCount = 0;
            public const double minUnitPrice = 0;
        }

        public static class Product
        {
            public const int minNameStringLength = 2;
            public const int maxNameStringLength = 100;

            public const int minDescriptionStringLength = 2;
            public const int maxDescriptionStringLength = 100;

            public const double minPrice = 0;

            public const int minStock = 0;

            public const int maxImageUrlStringLength = 200;
        }

        public static class Review
        {
            public const int minRating = 1;
            public const int maxRating = 5;

            public const int minCommentStringLength = 2;
            public const int maxCommentStringLength = 500;
        }

        public static class Status
        {
            public const int minDescriptionStringLength = 2;
            public const int maxDescriptionStringLength = 50;
        }
        public static class PaymentDetail
        {
            public const int minPaymentTypeStringLength = 3;
            public const int maxPaymentTypeStringLength = 50;
            public const int minDescriptionTypeStringLength = 5;
            public const int maxDescriptionTypeStringLength = 100;
        }
    }
}
