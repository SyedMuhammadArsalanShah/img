using Lecimg.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lecimg.Controllers
{
    public class ImageController : Controller
    {
        image_Entities db = new image_Entities();
        // GET: Image
        public ActionResult Index()
        {
            return View(db.img_table.ToList());
        }


        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(HttpPostedFileBase file ,
            img_table emp)
        {
            string filename = Path.GetFileName(file.FileName);
            string _filename = DateTime.Now.ToString("yymmssfff")+filename;
            string extension= Path.GetExtension(file.FileName);
            string path = Path.Combine(Server.MapPath("~/images/"), _filename);
            emp.img = "~/images/" + _filename;
            if (extension.ToLower() ==".jpg" || extension.ToLower() == ".jpeg"|| extension.ToLower() == ".png")
            {
                if (file.ContentLength<=1000000)
                {
                    db.img_table.Add(emp);
                    if (db.SaveChanges() >=0)
                    {
                        file.SaveAs(path);
                        ViewBag.msg = "Record Added";
                        ModelState.Clear();
                    }

                }
                else { ViewBag.msg = "size is not valid "; }

            }
            return View();
        }
    }
}