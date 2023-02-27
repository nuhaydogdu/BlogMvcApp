using BlogMvcApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlogMvcApp.Controllers
{
    public class HomeController : Controller
    {

        private BlogContext context = new BlogContext();

        // GET: Home
        public ActionResult Index()
        {
            /*
             * blogs sınıfı içerisinde istediğimiz kolonları alabilmek için BlogModel sınıfı tanımlayıp içerisine prop'lar ekledik (kullanacağımız alanları paketlemiş gibi olduk)
            */
            var bloglar = context.Blogs
                .Where(i => i.Onay == true && i.AnaSayfa == true)
                .Select(i => new BlogModel()
                {
                    Id = i.Id,
                    Baslik = i.Baslik.Length > 100 ? i.Baslik.Substring(0, 100) + "..." : i.Baslik,
                    Aciklama = i.Aciklama,
                    EklemeTarihi = i.EklemeTarihi,
                    AnaSayfa = i.AnaSayfa,
                    Onay = i.Onay,  
                    Resim = i.Resim
                });

            return View(bloglar.ToList());
        }
    }
}