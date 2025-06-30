using Microsoft.Data.SqlClient;
using System.Linq.Expressions;

namespace Case_Study
{

    class Program
    {
        static void Main(string[] args)
        {
            OrderProcessorRepositoryImpl repo = new OrderProcessorRepositoryImpl();

            while (true)
            {
                Console.WriteLine("\nChoose an action:");
                Console.WriteLine("1. Create Customer");
                Console.WriteLine("2. Create Product");
                Console.WriteLine("3. Add to Cart");
                Console.WriteLine("4. Remove from Cart");
                Console.WriteLine("5. Delete Product");
                Console.WriteLine("6. Delete Customer");
                Console.WriteLine("7. View All From Cart");
                Console.WriteLine("8. Place Order");
                Console.WriteLine("9. View Orders");
                Console.WriteLine("0. Exit");

                Console.Write("Enter your choice: ");
                int choice = Convert.ToInt32(Console.ReadLine());

                if (choice == 0) break;

                switch (choice)
                {
                    case 1:
                        Console.Write("Name: ");
                        string cname = Console.ReadLine();
                        Console.Write("Email: ");
                        string email = Console.ReadLine();
                        Console.Write("Password: ");
                        string pass = Console.ReadLine();
                        Customer c = new Customer(0, cname, email, pass);
                        repo.CreateCustomer(c);
                        break;

                    case 2:
                        Console.Write("Product Name: ");
                        string pname = Console.ReadLine();
                        Console.Write("Price of Product : ");
                        double price = Convert.ToDouble(Console.ReadLine());
                        Console.Write("Description: ");
                        string desc = Console.ReadLine();
                        Console.Write("Stock in Quantity: ");
                        int stock = Convert.ToInt32(Console.ReadLine());
                        Product p = new Product(0, pname, price, desc, stock);
                        repo.CreateProduct(p);
                        break;

                    case 3:
                        Console.Write("Customer ID : ");
                        int acustId = Convert.ToInt32(Console.ReadLine());
                        Console.Write("Product ID : ");
                        int aprodId = Convert.ToInt32(Console.ReadLine());
                        Console.Write("Quantity : ");
                        int qty = Convert.ToInt32(Console.ReadLine());
                        repo.AddToCart(new Customer(acustId, "", "", ""), new Product(aprodId, "", 0, "", 0), qty);
                        break;

                    case 4:
                        Console.Write("Customer ID: ");
                        int rcid = Convert.ToInt32(Console.ReadLine());
                        Console.Write("Product ID: ");
                        int rpid = Convert.ToInt32(Console.ReadLine());
                        repo.RemoveFromCart(new Customer(rcid, "", "", ""), new Product(rpid, "", 0, "", 0));
                        break;

                    case 5:
                        try
                        {
                            Console.Write("Product ID to delete: ");
                            int dpId = Convert.ToInt32(Console.ReadLine());
                            bool result = repo.DeleteProduct(dpId);
                            if (result)
                            {
                                Console.WriteLine("Product deleted Successfully");
                            }
                        }
                        catch (ProductNotFoundException ex)
                        {
                            Console.WriteLine("Error : " + ex.Message);
                        }
                        break;

                    case 6:
                        try
                        {
                            Console.Write("Customer ID to delete: ");
                            int dcId = Convert.ToInt32(Console.ReadLine());
                            bool result = repo.DeleteCustomer(dcId);
                            if (result)
                            {
                                Console.WriteLine("Customer deleted Successfully");
                            }
                        }
                        catch (CustomerNotFoundException ex)
                        {
                            Console.WriteLine("Error : " + ex.Message);
                        }
                        break;


                    case 7:
                        try {
                            Console.Write("Customer ID: ");
                            int viewCust = Convert.ToInt32(Console.ReadLine());
                            var cartItems = repo.GetAllFromCart(new Customer(viewCust, "", "", ""));
                            foreach (var item in cartItems)
                                Console.WriteLine($"Product: {item.Product_Name}, Price: {item.Product_Price}");
                        }
                        catch(ProductNotFoundException ex)
                        {
                            Console.WriteLine("Error : "+ ex.Message);
                        }
                        break;

                    case 8:
                        Console.Write("Customer ID: ");
                        int orderCust = Convert.ToInt32(Console.ReadLine());
                        Console.Write("Shipping Address: ");
                        string addr = Console.ReadLine();
                       
                        Console.Write("How many products in order?: ");
                        int count = Convert.ToInt32(Console.ReadLine());

                        List<Dictionary<Product, int>> orderList = new List<Dictionary<Product, int>>();
                        for (int i = 0; i < count; i++)
                        {
                            Console.Write("Product ID: ");
                            int pid = Convert.ToInt32(Console.ReadLine());
                            Console.Write("Quantity: ");
                            int pq = Convert.ToInt32(Console.ReadLine());
                            orderList.Add(new Dictionary<Product, int> { { new Product(pid, "", 0, "", 0), pq } });
                        }
                        bool placed = repo.PlaceOrder(new Customer(orderCust, "", "", ""), orderList, addr);

                        if (placed)
                        {
                            Console.WriteLine("Order Placed ");
                        }
                        else
                        {
                            Console.WriteLine("Order Not Placed");
                        }
                            break;

                    case 9:
                        try
                        {
                            Console.Write("Customer ID: ");
                            int ordCust = Convert.ToInt32(Console.ReadLine());
                            var orders = repo.GetOrdersByCustomer(ordCust);
                            foreach (var ord in orders)
                            {
                                Console.WriteLine("-- Order --");
                                foreach (var pair in ord)
                                {
                                    Console.WriteLine($"Product: {pair.Key.Product_Name}, Quantity: {pair.Value}");
                                }
                            }
                        }
                        catch(OrderNotFoundException e)
                        {
                            Console.WriteLine("Error : "+ e.Message);
                        }
                        break;

                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
        }
    }
}