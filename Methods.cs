using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Case_Study
{
    public interface IOrderProcessorRepository
    {
        public bool CreateCustomer(Customer customer);
        public bool CreateProduct(Product product);
        public bool DeleteProduct(int productID);
        public bool DeleteCustomer(int customerID);
        public bool AddToCart(Customer customer, Product product, int Quantity);
        public bool RemoveFromCart(Customer customer, Product product);
        List<Product> GetAllFromCart(Customer customer);
        bool PlaceOrder(Customer customer, List<Dictionary<Product, int>> products, string shippingAddress);
        List<Dictionary<Product, int>> GetOrdersByCustomer(int customerId);

    }
}
