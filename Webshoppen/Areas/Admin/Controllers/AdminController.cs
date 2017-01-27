using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Webshoppen.Factories;
using Webshoppen.Models;

namespace Webshoppen.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Session["Member"] == null && Request.RawUrl.ToLower().Contains("login") == false)
            {
                Response.Redirect("/Admin/Admin/Login");
            }
            base.OnActionExecuting(filterContext);
        }

        HomeFactory homeFac = new HomeFactory();
        ContactFactory contactFac = new ContactFactory();
        ProductFactory productFac = new ProductFactory();
        CategoryFactory categoryFac = new CategoryFactory();
        MemberFactory memberFac = new MemberFactory();

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

        public ActionResult CreateProduct()
        {
            ViewBag.Categories = categoryFac.GetAll();
            return View();
        }

        [HttpPost]
        public ActionResult CreateProductSubmit(Product product, HttpPostedFileBase ImageFile)
        {

            if (ImageFile != null && ImageFile.ContentLength > 0)
            {
                string fileName = ImageFile.FileName;
                string path = Request.PhysicalApplicationPath + @"\Content\Images\Products\" + fileName;
                ImageFile.SaveAs(path);
                product.Image = fileName;
            }

            TempData["MSG"] = "A product: " + product.Name + " has been added.";

            // DDMF = productFac.Insert(product);
            productFac.Add(product);

            return RedirectToAction("Products");
        }

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
        public ActionResult EditProductSubmit(Product product, HttpPostedFileBase ImageFile)
        {

            if (ImageFile != null && ImageFile.ContentLength > 0)
            {
                string fileName = ImageFile.FileName;
                string path = Request.PhysicalApplicationPath + @"\Content\Images\Products\" + fileName;
                ImageFile.SaveAs(path);
                product.Image = fileName;
            }

            productFac.Update(product);
            TempData["MSG"] = "Product: " + product.Name + " has been updated.";
            return RedirectToAction("Products");
        }


        /* Manage Login */

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LoginSubmit(string username, string password)
        {
            SHA512 encryptedPassword = new SHA512Managed();
            encryptedPassword.ComputeHash(Encoding.ASCII.GetBytes(password));

            string hashedPassword = BitConverter.ToString(encryptedPassword.Hash).Replace("-", "");

            // DDMF: Member memberToLogin = MemberFactory.ExecuteSQL<Member>("")[0];
            Member memberToLogin = memberFac.SqlQuery("SELECT * FROM Member WHERE Username='" + username +
                                                      "' AND Password='" + hashedPassword + "'");

            if (memberToLogin.ID > 0 && memberToLogin.Admin == true)
            {
                Session["Member"] = memberToLogin;
            }
            else
            {
                TempData["MSG"] = "Wrong username or password.";
                return RedirectToAction("Login");
            }
            return RedirectToAction("Index");
        }

        public ActionResult CreateMember()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateMemberSubmit(Member member)
        {
            SHA512 encryptedPassword = new SHA512Managed();
            encryptedPassword.ComputeHash(Encoding.ASCII.GetBytes(member.Password));

            string hashedPassword = BitConverter.ToString(encryptedPassword.Hash).Replace("-", "");

            member.Password = hashedPassword.ToLower();

            memberFac.Add(member);

            TempData["MSG"] = "A member has been created.";

            return RedirectToAction("Members");
        }

        public ActionResult Members()
        {
            List<Member> allMembers = memberFac.GetAll();
            return View(allMembers);
        }
    }
}