using Ecom_Application.DAO;
using Ecom_Application.Entity;
using System.Data.SqlClient;
using System.Configuration;

namespace Ecom_Application_Test
{
    public class Ecom_Application_Testing
    {
        private OrderProcessorRepository _processorRepository;
        [SetUp]
        public void Setup()
        {
            AppDomain.CurrentDomain.SetData("APP_CONFIG_FILE", string.Format("{0}\\app.config", AppDomain.CurrentDomain.BaseDirectory));
            _processorRepository = new OrderProcessorRepositoryImpl();
        }


        [Test]
        public void CreateProduct_WhenPassed()
        {
            Products product = new Products() { Name = "Paracetamol", Price = 13.99M, Description = "Tablet", StockQuantity = 195 };
            bool actual = _processorRepository.createProduct(product);
            Assert.That(actual, Is.EqualTo(true));
        }

        [Test]
        public void AddToCart_WhenPassed()
        {
            Customers customers = new Customers() { Customer_id = 2 };
            Products product = new Products() { Product_id = 2 };
            int quantity = 10;
            bool addedtocart = _processorRepository.addToCart(customers, product, quantity);
            Assert.That(addedtocart, Is.EqualTo(true));
        }

        [Test]
        public void AddToCart_WhenFailed()
        {
            Customers customers = new Customers() { Customer_id = 20 };
            Products product = new Products() { Product_id = 8 };
            int quantity = 10;
            bool addedtocart = _processorRepository.addToCart(customers, product, quantity);
            Assert.That(addedtocart, Is.EqualTo(false));
        }

        [Test]
        public void PlaceOrder_WhenPassed() 
        {
            Customers customers = new Customers() { Customer_id = 6 };
            Products products=new Products() { Product_id = 2 };
            int quantity = 10;
            List<Tuple<Products, int>> testinglist = new List<Tuple<Products, int>>();
            testinglist.Add(Tuple.Create(products, quantity));
            string adddress = "Bangalore";
            bool placed=_processorRepository.PlaceOrder(customers, testinglist, adddress);
            Assert.That(placed, Is.EqualTo(true));
        }

        [Test]
        public void PlaceOrder_WhenFailed()
        {
            Customers customers = new Customers() { Customer_id = 500 };
            Products products = new Products() { Product_id = 2 };
            int quantity = 10;
            List<Tuple<Products, int>> testinglist = new List<Tuple<Products, int>>();
            testinglist.Add(Tuple.Create(products, quantity));
            string adddress = "Bangalore";
            bool placed = _processorRepository.PlaceOrder(customers, testinglist, adddress);
            Assert.That(placed, Is.EqualTo(false));
        }
    }
}