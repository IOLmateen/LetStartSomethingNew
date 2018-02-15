using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LetStartSomethingNew.Models.GeneralMaster
{
    public class Product
    {
        public int id { get; set; }
        public string name { get; set; }
        public decimal price { get; set; }
        public string department { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public List<Product> ListProduct { get; set; }

        public Product()
        {

        }

        public Product(int id,string name,decimal price,string department)
        {
            this.id = id;
            this.name = name;
            this.price = price;
            this.department = department;
        }

        public static List<Product> GetSampleProducts()
        {
            return new List<Product>
                  {
                      new Product(id:1, name: "Remote Car", price:9.99m, department:"Toys"),
                      new Product(id:2, name: "Boll Pen", price:2.99m, department:"Stationary"),
                      new Product(id:3, name: "Teddy Bear", price:6.99m, department:"Toys"),
                      new Product(id:4, name: "Tennis Boll", price:6.99m, department:"Toys"),
                      new Product(id:5, name: "Super Man", price:6.99m, department:"Toys"),
                      new Product(id:6, name: "Bikes", price:4.99m, department:"Toys"),
                      new Product(id:7, name: "Books", price:7.99m, department:"Stationary"),
                      new Product(id:8, name: "Mobiles", price:5.99m, department:"Toys"),
                      new Product(id:9, name: "Laptops", price:15.99m, department:"Toys"),
                      new Product(id:10, name: "Note Books", price:2.99m, department:"Stationary")
                  };
        }
    }
}