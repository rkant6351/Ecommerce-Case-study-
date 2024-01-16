using Ecom_Application.DAO;
using Ecom_Application.Entity;
using System.Data.SqlClient;
using System.Configuration;

namespace Ecom_Application_Test
{
    public class Tests
    {
        private OrderProcessorRepository _processorRepository;
        [SetUp]
        public void Setup()
        {
            AppDomain.CurrentDomain.SetData("APP_CONFIG_FILE", string.Format("{0}\\app.config", AppDomain.CurrentDomain.BaseDirectory));
            _processorRepository = new OrderProcessorRepositoryImpl();
        }


        [Test]
        public void CreateProducttest()
        {
            Products product = new Products() { Name = "Paracetamol", Price = 13.99M, Description = "Tablet", StockQuantity = 195 };
            bool actual = _processorRepository.createProduct(product);
            Assert.That(actual, Is.EqualTo(true));
        }

        [Test]
        public void addedtocarttest()
        {
            Customers customers = new Customers() { Customer_id = 10 };
            Products product = new Products() { Product_id = 2 };
            int quantity = 10;
            bool addedtocart = _processorRepository.addToCart(customers, product, quantity);
            Assert.That(addedtocart, Is.EqualTo(true));
        }

        [Test]
        public void notaddedtocarttest()
        {
            Customers customers = new Customers() { Customer_id = 20 };
            Products product = new Products() { Product_id = 8 };
            int quantity = 10;
            bool addedtocart = _processorRepository.addToCart(customers, product, quantity);
            Assert.That(addedtocart, Is.EqualTo(false));
        }

        [Test]
        public void orderplacedtest() 
        {
            Customers customers = new Customers() { Customer_id = 10 };
            Products products=new Products() { Product_id = 2 };
            int quantity = 10;
            List<Tuple<Products, int>> testinglist = new List<Tuple<Products, int>>();
            testinglist.Add(Tuple.Create(products, quantity));
            string adddress = "Bangalore";
            bool placed=_processorRepository.PlaceOrder(customers, testinglist, adddress);
            Assert.That(placed, Is.EqualTo(true));
        }

        [Test]
        public void notorderplacedtest()
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