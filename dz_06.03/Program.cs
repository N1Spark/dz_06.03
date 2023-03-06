using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace dz_06._03
{
    class Program
    {
        public class Order
        {
            public int Id { get; set; }
            public string NameCust { get; set; }
            public List<Product> Products { get; set; }
            public Order(int id, string nameCust, List<Product> products)
            {
                Id = id;
                NameCust = nameCust;
                Products = products;
            }
        }
        public class Product
        {
            public string Name { get; set; }
            public double Price { get; set; }
            public Product(string name, double price)
            {
                Name = name;
                Price = price;
            }
        }
        static void Main(string[] args)
        {
            Random random = new Random();
            List<Order> orders = new List<Order>()
            {
                new Order(random.Next(1000, 9999), "Никита", new List<Product>() { new Product("Видеокарта",39999.0)}),
                new Order(random.Next(1000, 9999), "Даня", new List<Product>() { new Product("Процессор",19000) }),
                new Order(random.Next(1000, 9999), "Пет", new List<Product>() { new Product("Мат. Плата",12000)}),
                new Order(random.Next(1000, 9999), "Дима", new List<Product>() { new Product("SSD", 3599)})
            };
            XmlTextWriter writer = new XmlTextWriter("../../Order.xml", Encoding.UTF8);
            writer.WriteStartElement("Orders");
            writer.Formatting = Formatting.Indented;
            writer.IndentChar = '\t';
            writer.Indentation = 1;
            foreach (Order order in orders)
            {
                writer.WriteStartElement("Order");
                writer.WriteAttributeString("Id", order.Id.ToString());
                writer.WriteAttributeString("Customer", order.NameCust);
                foreach (Product product in order.Products)
                {
                    writer.WriteStartElement("Product");
                    writer.WriteAttributeString("Name", product.Name);
                    writer.WriteAttributeString("Price", product.Price.ToString());
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
            writer.Close();
            Console.WriteLine("Данные сохранены в XML-файл");
            XDocument doc = XDocument.Load("../../Order.xml");
            IEnumerable<XElement> orderElements = doc.Descendants("Order");
            orders = new List<Order>();
            foreach (XElement orderElement in orderElements)
            {
                Order order = new Order(Convert.ToInt32(orderElement.Attribute("Id").Value),
                    orderElement.Attribute("Customer").Value, new List<Product>());
                IEnumerable<XElement> productElements = orderElement.Descendants("Product");
                foreach (XElement productElement in productElements)
                {
                    Product product = new Product(productElement.Attribute("Name").Value,
                        Convert.ToDouble(productElement.Attribute("Price").Value));
                    order.Products.Add(product);
                }
                orders.Add(order);
            }
            foreach (Order ord in orders)
            {
                Console.WriteLine($"Order {ord.Id} {ord.NameCust}:");
                foreach (Product product in ord.Products)
                    Console.WriteLine($"\t{product.Name} - {product.Price} грн");
                Console.WriteLine();
            }
        }
    }
}
