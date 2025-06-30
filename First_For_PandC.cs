using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Case_Study
{
    public class Customer
    {
        public int Customer_ID { get; set; }
        public string Customer_Name { get; set; }
        public string Email { get; set; }
        public string password { get; set; }

        public Customer(int CustomerID, string CustomerName, string CustomerEmail, string password)
        {
            this.Customer_ID = CustomerID;
            this.Customer_Name = CustomerName;
            this.Email = CustomerEmail;
            this.password = password;
        }
    }

    public class Product
    {
        public int Product_ID { get; set; }
        public string Product_Name { get; set; }
        public double Product_Price { get; set; }
        public string Description { get; set; }
        public int Stock_Quant { get; set; }

        public Product(int ProductID, string ProductName, double Price, string ProductDescription, int StockQuantity)
        {
            this.Product_ID = ProductID;
            this.Product_Name = ProductName;
            this.Product_Price = Price;
            this.Description = ProductDescription;
            this.Stock_Quant = StockQuantity;
        }
    }
}
