using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BlogMvcApp.Models;

namespace BlogMvcApp.Controllers
{
    public class CategoryController : Controller
    {
        private BlogContext db = new BlogContext();

//----------------------------------------Kategoriler ile ilkili PartialView ------------------------
     //PartialView herhangi bir Layout kullanmıyor View'in bir parçası anlamında
     //PartialView'ı kullanacağımız yerde çağırmamız gerekiyor. (Html.RenderAction("KategoriListesi","Category") -Actionİsmi,Bulunduğu controller ismi
        public PartialViewResult KategoriListesi()
        {
            return PartialView(db.Categories.ToList());
        }
 //--------------------------------------------------------------------------------------------------


        // GET: Category
        public ActionResult Index()
        {
            // Ekran tasarımına göre ihtiyacımız olan değerlere göre yeni bir sınıf olan CategoryModeli oluşturup içerisinde tanımladığımız değerleri doldurarak view'e gönderdik
            var Categories = db.Categories.Select(i => new CategoryModel()       
            {
                Id = i.Id, 
                KategoriAdi = i.KatagoriAdi,
                BlogSayisi=i.Bloglar.Count()
            });

            return View(Categories.ToList());
        }

        // GET: Category/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: Category/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Category/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "KatagoriAdi")] Category category) 
        {
            //Category category -Formdan Category cinsinde bir Sınıf(Model) beklediğimizi belirtiyoruz.
            //Include -içerisinde belrttiklerimiz formadan gönderilmesini beklediğimiz değerler!!!!!!!!
            if (ModelState.IsValid)
            {
                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(category);
        }

        // GET: Category/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Category/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,KatagoriAdi")] Category category)
        {
            //Form üzerinden gelen değerler ModelState ile Category modele bağlanıyor 
            if (ModelState.IsValid)
            {
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();

                TempData["Kategori"]=category;
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // GET: Category/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = db.Categories.Find(id);
            db.Categories.Remove(category);
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
