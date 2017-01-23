using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webshoppen.Models;
using Webshoppen.Factories;

namespace Webshoppen.Controllers
{
    public class HomeController : Controller
    {
        ProductFactory productFac = new ProductFactory();
        CategoryFactory categoryFac = new CategoryFactory();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Products()
        {
            ViewBag.Categories = categoryFac.GetAll();
            List<Product> products = TempData["SortedProducts"] as List<Product>;
            if (products == null)
            {
                List<Product> allProducts = productFac.GetAll();
                return View(allProducts);
            }
            else
            {
                return View(products);
            }
        }

        [HttpPost]
        public ActionResult SortProducts(int id = 0)
        {
            List<Product> sortedProducts = null;
            if (id > 0)
            {
                sortedProducts = productFac.GetBy("CategoryID", id);
            }

            TempData["SortedProducts"] = sortedProducts;
            return RedirectToAction("Products");
        }

        public ActionResult ShowProduct(int id = 0)
        {
            Product productToView = productFac.Get(id);

            ViewBag.Category = categoryFac.Get(productToView.CategoryID);

            return View(productToView);
        }
    }
}