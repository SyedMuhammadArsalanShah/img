using Lecimg.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
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




        public ActionResult Edit(int? id)
        {
            if (id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }



            var tbl_img = db.img_table.Find(id);
            Session["imgpath"] = tbl_img.img;
            if (tbl_img==null)
            {
                return HttpNotFound();
            }
            return View(tbl_img);
        }
        [HttpPost]
        public ActionResult Edit(HttpPostedFileBase file, img_table emp)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {

                    string filename = Path.GetFileName(file.FileName);
                    string _filename = DateTime.Now.ToString("yymmssfff") + filename;
                    string extension = Path.GetExtension(file.FileName);
                    string path = Path.Combine(Server.MapPath("~/images/"), _filename);
                    emp.img = "~/images/" + _filename;
                    if (extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".png")
                    {
                        if (file.ContentLength <= 1000000)
                        {
                            db.Entry(emp).State = EntityState.Modified;
                            string oldImagePath = Request.MapPath(Session["imgpath"].ToString());
                            if (db.SaveChanges() >= 0)
                            {
                                file.SaveAs(path);



                                if (System.IO.File.Exists(oldImagePath))
                                {
                                    System.IO.File.Delete(oldImagePath);
                                }
                                TempData["msg"] = "Record Updated";
                                //ViewBag.msg = "Record Added";
                                //ModelState.Clear();
                            }

                        }
                        else { ViewBag.msg = "size is not valid "; }

                    }
                }
            }
            else
            {
                emp.img = Session["imgPath"].ToString();
                db.Entry(emp).State = EntityState.Modified;
                if (db.SaveChanges() >= 0)
                {
                    TempData["msg"] = "Data Updated";
                    return RedirectToAction("Index");
                }


            }

            return View();






        }



        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }



            var tbl_img = db.img_table.Find(id);
            //Session["imgpath"] = tbl_img.img;
            if (tbl_img == null)
            {
                return HttpNotFound();
            }
            return View(tbl_img);
        }


        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }



            var tbl_img = db.img_table.Find(id);
            //Session["imgpath"] = tbl_img.img;
            if (tbl_img == null)
            {
                return HttpNotFound();
            }


            string currentimage = Request.MapPath(tbl_img.img);
            db.Entry(tbl_img).State = EntityState.Deleted;

            if (db.SaveChanges() >= 0)
            {
             

                if (System.IO.File.Exists(currentimage))
                {
                    System.IO.File.Delete(currentimage);
                }
                TempData["msg"] = "data deleted";
                return RedirectToAction("Index");
               
            }



            return View();
        }
    }
}