using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BlogMvcApp.Models
{
    //DropCreateDatabaseIfModelChanges<BlogContext> BlogContext'deki sınıflarımızdan(Model) herangi birinde değişim olursa database'i silip tekrardan oluşturuyor
    public class BlogInitializer: DropCreateDatabaseIfModelChanges<BlogContext>
    {

        //Seed() methodu veri tabanına test verileri eklememizi sağlıyor
        protected override void Seed(BlogContext context)
        {
            List<Category> katagoriler = new List<Category>()
            {
                new Category(){KatagoriAdi="C#"},
                new Category(){KatagoriAdi="Asp.net MVC"},
                new Category(){KatagoriAdi="Asp.net WebForm"},
                new Category(){KatagoriAdi="WindowsForm"}
            };  

            foreach(var category in katagoriler)
            {
                context.Categories.Add(category);
            }

            context.SaveChanges();


            List<Blog> Bloglar = new List<Blog>()
            {
                new Blog() {Baslik="C# Delegates Hakkında", Aciklama="C# Delegates HakkındaC# Delegates HakkındaC# Delegates Hakkında", EklemeTarihi=DateTime.Now.AddDays(-10), AnaSayfa=true, Onay=true, Icerik="C# Delegates Hakkında C# Delegates HakkındaC# Delegates HakkındaC# Delegates HakkındaC# Delegates Hakkında",Resim="1.jpg",CategoryId=1},
                new Blog() {Baslik="C# Delegates Hakkında", Aciklama="C# Delegates HakkındaC# Delegates HakkındaC# Delegates Hakkında", EklemeTarihi=DateTime.Now.AddDays(-5), AnaSayfa=false, Onay=true, Icerik="C# Delegates Hakkında C# Delegates HakkındaC# Delegates HakkındaC# Delegates HakkındaC# Delegates Hakkında",Resim="1.jpg",CategoryId=1},
                new Blog() {Baslik="C# Delegates Hakkında", Aciklama="C# Delegates HakkındaC# Delegates HakkındaC# Delegates Hakkında", EklemeTarihi=DateTime.Now.AddDays(-15), AnaSayfa=true, Onay=true, Icerik="C# Delegates Hakkında C# Delegates HakkındaC# Delegates HakkındaC# Delegates HakkındaC# Delegates Hakkında",Resim="2.jpg",CategoryId=2},
                new Blog() {Baslik="C# Delegates Hakkında", Aciklama="C# Delegates HakkındaC# Delegates HakkındaC# Delegates Hakkında", EklemeTarihi=DateTime.Now.AddDays(-20), AnaSayfa=false, Onay=true, Icerik="C# Delegates Hakkında C# Delegates HakkındaC# Delegates HakkındaC# Delegates HakkındaC# Delegates Hakkında",Resim="2.jpg",CategoryId=2},
                new Blog() {Baslik="C# Delegates Hakkında", Aciklama="C# Delegates HakkındaC# Delegates HakkındaC# Delegates Hakkında", EklemeTarihi=DateTime.Now.AddDays(-10), AnaSayfa=true, Onay=false, Icerik="C# Delegates Hakkında C# Delegates HakkındaC# Delegates HakkındaC# Delegates HakkındaC# Delegates Hakkında",Resim="2.jpg",CategoryId=2},
                new Blog() {Baslik="C# Delegates Hakkında", Aciklama="C# Delegates HakkındaC# Delegates HakkındaC# Delegates Hakkında", EklemeTarihi=DateTime.Now.AddDays(-12), AnaSayfa=true, Onay=true, Icerik="C# Delegates Hakkında C# Delegates HakkındaC# Delegates HakkındaC# Delegates HakkındaC# Delegates Hakkında",Resim="3.jpg",CategoryId=3},
                new Blog() {Baslik="C# Delegates Hakkında", Aciklama="C# Delegates HakkındaC# Delegates HakkındaC# Delegates Hakkında", EklemeTarihi=DateTime.Now.AddDays(-5), AnaSayfa=false, Onay=true, Icerik="C# Delegates Hakkında C# Delegates HakkındaC# Delegates HakkındaC# Delegates HakkındaC# Delegates Hakkında",Resim="3.jpg",CategoryId=3},
                new Blog() {Baslik="C# Delegates Hakkında", Aciklama="C# Delegates HakkındaC# Delegates HakkındaC# Delegates Hakkında", EklemeTarihi=DateTime.Now.AddDays(-10), AnaSayfa=true, Onay=true, Icerik="C# Delegates Hakkında C# Delegates HakkındaC# Delegates HakkındaC# Delegates HakkındaC# Delegates Hakkında",Resim="4.jpg",CategoryId=4}
            };

            foreach (var blog in Bloglar)
            {
                context.Blogs.Add(blog);
            }

            context.SaveChanges();

            base.Seed(context);
        }

    }
}