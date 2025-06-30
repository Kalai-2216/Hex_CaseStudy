using Case_Study;

namespace Product_Created_Or_Not
{
    public class Tests
    {
        OrderProcessorRepositoryImpl repo;

        [SetUp]
        public void SetUp()
        {
            repo = new OrderProcessorRepositoryImpl();
        }
        [Test]
        public void Product_created()
        {

            var p = new Product(0, "TestProduct_", 199.99, "Test Description", 10);


            bool result = repo.CreateProduct(p);


            Assert.IsTrue(result, "Product creation should return true");

        }
        [Test]
        public void Checking_Cart()
        {
            var customer = new Customer(CustomerID: 1001, CustomerName: "Kalai", CustomerEmail: "pvkalai7@gmail.com", password: "Kalai_6515");
            var product = new Product(ProductID: 2501, ProductName: "Airpods pro 2", Price: 20000, ProductDescription: "TWS", StockQuantity: 10);
            int quantity = 2;

            bool result = repo.AddToCart(customer, product, quantity);

            Assert.IsTrue(result, "Product should be added to cart successfully.");
        }
        [Test]
        public void Checking_Order_Placement()
        {
            var customer = new Customer(1001, "Kalai", "pvkalai7@gmail.com", "Kalai_6515");
            var product = new Product(2501, "Airpods pro 2", 20000, "TWS", 10);

            var orderDetails = new Dictionary<Product, int> { { product,1 } };
            var orderList = new List<Dictionary<Product, int>> { orderDetails };

            string address = "123 Test Street";

            bool result = repo.PlaceOrder(customer, orderList, address);

            Assert.IsTrue(result, "Order should be placed successfully.");
        }

        [Test]
        public void ThrowProductNotFoundException()
        {
            int NonExistingProductId = 9999;

            Assert.Throws<ProductNotFoundException>(() =>
            {
                repo.DeleteProduct(NonExistingProductId);
            });
        }

    }
}