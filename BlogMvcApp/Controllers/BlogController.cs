using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Web.Mvc;
using BlogMvcApp.Models;

namespace BlogMvcApp.Controllers
{
    public class BlogController : Controller
    {
        private BlogContext db = new BlogContext();

//------------------------------!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!--------------------------------------------
        public ActionResult List(int? id,string AnahtarKelime)
        {
            var bloglar = db.Blogs
                .Where(i => i.Onay == true)
                .Select(i => new BlogModel()
                {
                    // Ekran tasarımına göre ihtiyacımız olan değerlere göre yeni bir sınıf olan BlogModel oluşturup içerisinde tanımladığımız değerleri doldurarak view'e gönderdik
                    Id = i.Id,
                    Baslik = i.Baslik.Length > 100 ? i.Baslik.Substring(0, 100) + "..." : i.Baslik,
                    Aciklama = i.Aciklama,
                    EklemeTarihi = i.EklemeTarihi,
                    AnaSayfa = i.AnaSayfa,
                    Onay = i.Onay,
                    Resim = i.Resim,
                    CategoryId=i.CategoryId
                }).AsQueryable(); //.AsQueryable(); sonradan extra Where'ler ekleyebilmemizi sağlayacak


        
            if(string.IsNullOrEmpty(AnahtarKelime)== false)
            {
                //Burada Search Partial viewinde belirttiğimiz anahtar kelimenin başlık ve açıklama içerisindeki varlığına bakıp bloglar olarak ilgili viewe döndük
                bloglar = bloglar.Where(i => i.Baslik.Contains(AnahtarKelime) || i.Aciklama.Contains(AnahtarKelime));
            }


            if(id != null)
            {
                //Burda seçilen katagori ismine göre gelen id değeriyle de bloglar üzerinde Where işlemi yaparak CategoryId değerleri gelen ıd ye eşit olan değerleri döndük
                bloglar = bloglar.Where(i => i.CategoryId == id);
            }

            return View(bloglar.ToList());
        }

//------------------------------------------------------------------------------------------------------

        // GET: Blog
        public ActionResult Index()
        {
            var blogs = db.Blogs.Include(b => b.Category).OrderByDescending(i=>i.EklemeTarihi);      
            //Include(b => b.Category) Her Blogun category bilgiside eklenerek gelecek demek oluyor
            return View(blogs.ToList());
        }



        // GET: Blog/Create
        public ActionResult Create()
        {
            //Veritabanındaki bütün katagorileri alıyor. bütün katagorilerin Id alanını value, KatagoriAdi alanlarını ise text olarak ayarlıyor.
            //Kullanıcının gördüğü alan KatagoriAdi adı olacak ama seçtiği alanın değerini Id olarak kullanacağız
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "KatagoriAdi");
            return View();
        }

        // POST: Blog/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Baslik,Aciklama,Resim,Icerik,CategoryId")] Blog blog)
        {                  
            //Formdan beklediğimiz değerleri buraya belirttik ([Bind(Include = "Baslik,Aciklama,Resim,Icerik,CategoryId")] Blog blog)         
            //ModelState Form içerisinde bulduğu Id ler ile eşleşen kontrollerin bilgilerini blog içerisindeki parametrelerle eşleştrip direkt olarak kopyalamayı bizim adımıza yapıyor
            //Eğerli ModelState içerisindeki bütün kontrollerimiz eşleşirse veriyi güncelleme kısmına geçebiliriz
            if (ModelState.IsValid)
            {
                blog.EklemeTarihi = DateTime.Now;
               
                db.Blogs.Add(blog);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "KatagoriAdi", blog.CategoryId);
            return View(blog); //kullanıcını girdiği bilgiler hatalı da olsa onlar tekrardan view'e yansıtılacak
        }



        // GET: Blog/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = db.Blogs.Find(id);
            if (blog == null)
            {
                return HttpNotFound();
            }
            return View(blog);
        }


        // GET: Blog/Edit/5
        //Edit(int? id) -parametre olarak id bekliyor (int? nuulable int boş geçilebilir)
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = db.Blogs.Find(id);
            if (blog == null)
            {
                return HttpNotFound();
            }
            //bu sayfada da katagoriler dropdown'u olduğu için 
            //Veritabanındaki bütün katagorileri alıyor. bütün katagorilerin Id alanını value, KatagoriAdi alanlarını ise text olarak ayarlıyor.
            //Kullanıcının gördüğü alan KatagoriAdi adı olacak ama seçtiği alanın değerini Id olarak kullanacağız
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "KatagoriAdi", blog.CategoryId);
            return View(blog);
        }

        // POST: Blog/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Baslik,Aciklama,Resim,Icerik,Onay,AnaSayfa,CategoryId")] Blog blog)
        {
            //ModelState Form içerisinde bulduğu Id ler ile eşleşen kontrollerin bilgilerini bir üst satırdaki(90) blog içerisindeki parametrelerle eşleştrip direkt olarak kopyalamayı bizim adımıza yapıyor
            //Eğerli ModelState içerisindeki bütün lontrollerimiz eşleşirse veriyi güncelleme kısmına geçebiliriz
            if (ModelState.IsValid)
            {  
                var entity= db.Blogs.Find(blog.Id);
                if(entity != null)
                {
                    entity.Baslik=blog.Baslik;
                    entity.Aciklama=blog.Aciklama;
                    entity.Resim=blog.Resim;
                    entity.Icerik=blog.Icerik;
                    entity.Onay=blog.Onay;
                    entity.AnaSayfa=blog.AnaSayfa;
                    entity.CategoryId = blog.CategoryId;

                    db.SaveChanges();
                    TempData["Blog"]=entity;
                    //RedirectToACtion kullandığımız içi veri taşıma işleminde TempData'yı kullandık (ViewBag RedirectToAction da sıfırlanıyor)
                    return RedirectToAction("index"); 
                }
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "KatagoriAdi", blog.CategoryId);
            return View(blog);
        }

        // GET: Blog/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = db.Blogs.Find(id);
            if (blog == null)
            {
                return HttpNotFound();
            }
            return View(blog);
        }

        // POST: Blog/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Blog blog = db.Blogs.Find(id);
            db.Blogs.Remove(blog);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
