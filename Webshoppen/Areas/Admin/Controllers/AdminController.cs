using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webshoppen.Factories;
using Webshoppen.Models;

namespace Webshoppen.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {
        HomeFactory homeFac = new HomeFactory();
        ContactFactory contactFac = new ContactFactory();
        ProductFactory productFac = new ProductFactory();
        CategoryFactory categoryFac = new CategoryFactory();

        public ActionResult Index()
        {
            Home home = homeFac.Get(1);
            return View(home);
        }

        [HttpPost]
        public ActionResult IndexSubmit(Home home)
        {
            homeFac.Update(home);
            TempData["MSG"] = "Home has been updated.";
            return RedirectToAction("Index");
        }

        public ActionResult Contact()
        {
            Contact contact = contactFac.Get(1);
            return View(contact);
        }

        [HttpPost]
        public ActionResult ContactSubmit(Contact contact)
        {
            contactFac.Update(contact);
            TempData["MSG"] = "Contact has been updated.";
            return RedirectToAction("Contact");
        }

        public ActionResult Products()
        {
            List<Product> allProducts = productFac.GetAll();
            return View(allProducts);
        }

        /* Manage Products */

        public ActionResult DeleteProduct(int id = 0)
        {
            productFac.Delete(id);
            TempData["MSG"] = "Product with id: " + id + " has been deleted.";
            return RedirectToAction("Products");
        }

        public ActionResult EditProduct(int id = 0)
        {
            ViewBag.Categories = categoryFac.GetAll();
            Product productToEdit = productFac.Get(id);
            return View(productToEdit);
        }

        [HttpPost]
        public ActionResult EditProductSubmit(Product product)
        {
            productFac.Update(product);
            TempData["MSG"] = "Product: " + product.Name + " has been updated.";
            return RedirectToAction("Products");
        }
    }
}