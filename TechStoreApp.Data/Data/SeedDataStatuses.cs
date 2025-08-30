using System;
using System.Collections.Generic;
using TechStoreApp.Data.Models;

namespace TechStoreApp.Data.Data
{
    public class SeedDataStatuses
    {
        public static List<Status> GetStatuses()
        {
            return new List<Status>
            {
                new Status()
                {
                    Description = "Order Received"
                },
                new Status()
                {
                    Description = "Payment Pending"
                },
                new Status()
                {
                    Description = "Payment Confirmed"
                },
                new Status()
                {
                    Description = "Order Processing"
                },
                new Status()
                {
                    Description = "Shipped"
                },
                new Status()
                {
                    Description = "In Transit"
                },
                new Status()
                {
                    Description = "Delivered"
                },
                new Status()
                {
                    Description = "Cancelled"
                },
                new Status()
                {
                    Description = "Refunded"
                },
                new Status()
                {
                    Description = "Refunding in process"
                },
                new Status()
                {
                    Description = "Returned"
                },
                new Status()
                {
                    Description = "Awaiting Pickup"
                },
                new Status()
                {
                    Description = "Failed Delivery"
                }
            };
        }
    }
}
