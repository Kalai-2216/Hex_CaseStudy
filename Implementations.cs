using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Case_Study
{
    public class OrderProcessorRepositoryImpl : IOrderProcessorRepository
    {
        //string con= @"Data Source=KBEAST\SQLEXPRESS;Initial Catalog=E_Commerce_App;Integrated Security=True;Trust Server Certificate=True";
        public bool CreateCustomer(Customer customer)
        {
            SqlConnection con = DBConnection.GetConnection();
            try
            {
                string query = "Insert into Customers (Customer_Name,Email,Password) Values (@Customer_Name,@Email,@Password)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Customer_Name", customer.Customer_Name);
                cmd.Parameters.AddWithValue("@Email", customer.Email);
                cmd.Parameters.AddWithValue("@Password", customer.password);
                con.Open();
                cmd.ExecuteNonQuery();
                return true;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while creating Customer : " + ex.Message);
                return false;
            }
            finally
            {
                con.Close();
            }
        }
        public bool CreateProduct(Product product)
        {
            SqlConnection con = DBConnection.GetConnection();
            try
            {
                string query = "INSERT INTO Products ( Product_Name, Product_Price, Description, Stock_Quant) " +
                               "VALUES ( @Product_Name, @Price, @Description, @Stock_Quant)";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Product_Name", product.Product_Name);
                cmd.Parameters.AddWithValue("@Price", product.Product_Price);
                cmd.Parameters.AddWithValue("@Description", product.Description);
                cmd.Parameters.AddWithValue("@Stock_Quant", product.Stock_Quant);

                con.Open();
                cmd.ExecuteNonQuery();
                return true;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while creating product : " + ex.Message);
                return false;
            }
            finally
            {
                con.Close();
            }
        }

        public bool DeleteProduct(int productId)
        {
            SqlConnection con = DBConnection.GetConnection();
            try
            {
                string query = "DELETE FROM Products WHERE Product_ID = @Product_ID";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Product_ID", productId);

                con.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected == 0)
                {
                    throw new ProductNotFoundException(productId.ToString());
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while Delete Product " + ex.Message);
            }
            finally
            {
                con.Close();
            }
            return false;
        }

        public bool DeleteCustomer(int customer_ID)
        {
            SqlConnection con = DBConnection.GetConnection();
            try
            {
                string query = "Delete from Customers where Customer_ID=@Customer_ID";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Customer_ID", customer_ID);
                con.Open();
                int rows_affected = cmd.ExecuteNonQuery();
                if (rows_affected == 0)
                {
                    throw new CustomerNotFoundException(customer_ID.ToString());
                }
                return true;

            }
            catch (CustomerNotFoundException ex)
            {
                Console.WriteLine("Error while Delete Customer : " + ex.Message);
                return false;
            }
            finally
            {
                con.Close();
            }
        }

        public bool AddToCart(Customer customer, Product product, int quantity)
        {
            SqlConnection con = DBConnection.GetConnection();
            try
            {
                string query = "Insert into cart (Customer_ID,Product_ID,Quantity) values (@Customer_ID,@Product_ID,@Quantity)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Customer_ID", customer.Customer_ID);
                cmd.Parameters.AddWithValue("@Product_ID", product.Product_ID);
                cmd.Parameters.AddWithValue("@Quantity", quantity);
                con.Open();
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (ProductNotFoundException ex)
            {
                Console.WriteLine("Error while adding to cart: " + ex.Message);
                return false;
            }
            finally
            {
                con.Close();
            }
        }

        public bool RemoveFromCart(Customer customer, Product product)
        {
            SqlConnection con = DBConnection.GetConnection();
            try
            {
                string query = "Delete from cart where Customer_ID=@Customer_ID and Product_ID=@Product_ID";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Customer_ID", customer.Customer_ID);
                cmd.Parameters.AddWithValue("@Product_ID", product.Product_ID);
                con.Open();
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while adding to cart: " + ex.Message);
                return false;
            }
            finally
            {
                con.Close();
            }
        }

        public List<Product> GetAllFromCart(Customer customer)
        {
            List<Product> products = new List<Product>();
            SqlConnection con = DBConnection.GetConnection();
            try
            {
                string query = "Select p.product_ID,p.Product_name,p.Description,p.Product_Price,p.Stock_Quant,c.Quantity " +
                    "from Cart c join Products p on c.Product_ID=p.Product_Id where c.Customer_ID=@Customer_ID";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Customer_ID", customer.Customer_ID);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Product p = new Product(Convert.ToInt32(reader["Product_ID"]),
                        reader["Product_Name"].ToString(),
                        Convert.ToDouble(reader["Product_Price"]),
                        reader["Description"].ToString(),
                        Convert.ToInt32(reader["Stock_Quant"])
                    );
                    products.Add(p);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : " + ex.Message);
            }
            finally
            {
                con.Close();
            }
            return products;
        }
        public bool PlaceOrder(Customer customer, List<Dictionary<Product, int>> products, string shippingAddress)
        {
            SqlConnection con = DBConnection.GetConnection();
            SqlTransaction transaction = null;

            try
            {
                con.Open();
                transaction = con.BeginTransaction();

                double totalPrice = 0;

                foreach (var pair in products)
                {
                    foreach (var kv in pair)
                    {
                        int productId = kv.Key.Product_ID;
                        int quantity = kv.Value;

                        string priceQuery = "SELECT Product_Price FROM Products WHERE Product_ID = @Product_ID";
                        SqlCommand priceCmd = new SqlCommand(priceQuery, con, transaction);
                        priceCmd.Parameters.AddWithValue("@Product_ID", productId);

                        object result = priceCmd.ExecuteScalar();
                        if (result != null)
                        {
                            double price = Convert.ToDouble(result);
                            totalPrice += price * quantity;
                        }
                        else
                        {
                            Console.WriteLine($"Product with ID {productId} not found. Skipping.");
                        }
                    }
                }
                string orderQuery = "INSERT INTO Orders (Customer_ID, Order_Date, Total_Price, Shipping_access) " +
                                    "VALUES (@Customer_ID, @Order_Date, @Total_Price, @Shipping_access); SELECT SCOPE_IDENTITY();";

                SqlCommand orderCmd = new SqlCommand(orderQuery, con, transaction);
                orderCmd.Parameters.AddWithValue("@Customer_ID", customer.Customer_ID);
                orderCmd.Parameters.AddWithValue("@Order_Date", DateTime.Now);
                orderCmd.Parameters.AddWithValue("@Total_Price", totalPrice);
                orderCmd.Parameters.AddWithValue("@Shipping_access", shippingAddress);

                int orderId = Convert.ToInt32(orderCmd.ExecuteScalar());
                foreach (var pair in products)
                {
                    foreach (var kv in pair)
                    {
                        string itemQuery = "INSERT INTO Order_Items (Order_ID, Product_ID, OD_Quantity) " +
                                           "VALUES (@Order_ID, @Product_ID, @OD_Quantity)";

                        SqlCommand itemCmd = new SqlCommand(itemQuery, con, transaction);
                        itemCmd.Parameters.AddWithValue("@Order_ID", orderId);
                        itemCmd.Parameters.AddWithValue("@Product_ID", kv.Key.Product_ID);
                        itemCmd.Parameters.AddWithValue("@OD_Quantity", kv.Value);
                        itemCmd.ExecuteNonQuery();
                    }
                }

                transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                transaction?.Rollback();
                Console.WriteLine("Error placing order: " + ex.Message);
                return false;
            }
            finally
            {
                con.Close();
            }
        }
        public List<Dictionary<Product, int>> GetOrdersByCustomer(int customerId)
        {
            List<Dictionary<Product, int>> orders = new List<Dictionary<Product, int>>();
            SqlConnection con = DBConnection.GetConnection();

            try
            {
                string query = "SELECT o.Order_ID, oi.Product_ID, p.Product_Name, p.Product_Price, p.Description, p.Stock_Quant, oi.OD_Quantity " +
                               "FROM Orders o JOIN Order_items oi ON o.Order_ID = oi.Order_ID " +
                               "JOIN Products p ON oi.Product_ID = p.Product_ID WHERE o.Customer_ID = @Customer_ID";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Customer_ID", customerId);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                Dictionary<int, Dictionary<Product, int>> grouped = new Dictionary<int, Dictionary<Product, int>>();

                while (reader.Read())
                {
                    int orderId = Convert.ToInt32(reader["Order_ID"]);
                    Product p = new Product(
                        Convert.ToInt32(reader["Product_ID"]),
                        reader["Product_Name"].ToString(),
                        Convert.ToDouble(reader["Product_Price"]),
                        reader["Description"].ToString(),
                        Convert.ToInt32(reader["Stock_Quant"])
                    );
                    int quantity = Convert.ToInt32(reader["OD_Quantity"]);

                    if (!grouped.ContainsKey(orderId))
                        grouped[orderId] = new Dictionary<Product, int>();

                    grouped[orderId].Add(p, quantity);
                }

                foreach (var item in grouped.Values)
                    orders.Add(item);
            }
            catch (Exception ex)
            {
                Console.Write("Error : " + ex.Message);
            }
            finally
            {
                con.Close();
            }
            return orders;
        }
    }
}
