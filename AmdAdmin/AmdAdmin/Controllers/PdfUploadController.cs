using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using AmdAdmin;

namespace AmdAdmin.Controllers
{
    [SessionState(SessionStateBehavior.Required)]
    public class PdfUploadController : Controller
    {
        private TestAmbEntity db = new TestAmbEntity();
        // GET: /PdfUpload/
        public ActionResult Index()
        {
            if (Session["username"]==null && Session["userid"]==null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(db.Pdfs.ToList());
        }
        // GET: /PdfUpload/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["username"] == null && Session["userid"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pdf pdf = db.Pdfs.Find(id);
            if (pdf == null)
            {
                return HttpNotFound();
            }
            return View(pdf);
        }

        // GET: /PdfUpload/Create
        public ActionResult Create()
        {
            if (Session["username"] == null && Session["userid"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        // POST: /PdfUpload/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Pdf pdf)
        {
            if (Session["username"] == null && Session["userid"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    foreach (string hpt in Request.Files)
                    {
                        HttpPostedFileBase file = Request.Files[hpt];
                        if (file.ContentLength > 0)
                        {
                            string _FileName = Path.GetFileName(file.FileName);
                            string _path = Path.Combine(Server.MapPath("~/Pdf"), _FileName);
                            file.SaveAs(_path);

                            string[] name = _FileName.Split('.');
                            pdf.PdfName = name[0];
                            pdf.PdfPath = _path;
                            pdf.PdfSize = file.ContentLength;
                            db.Pdfs.Add(pdf);
                            db.SaveChanges();
                            ViewBag.Message = "File Uploaded Successfully!!";
                        }
                        else
                        {
                            ViewBag.Message = "Choose File Please!";
                            return View("Create");
                        }   
                    }
                    return View("Create");
                }
                catch
                {
                    ViewBag.Message = "File upload failed!!";
                    return View("Create");
                }
            }
            return View("Index");
        }
        //Show PDF file
        //GET:/PdfUpload/FileResult/7
        public ActionResult ShowPdf(int? id)
        {
            if (Session["username"] == null && Session["userid"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pdf pdf = db.Pdfs.Find(id);
            if (pdf == null)
            {
                return HttpNotFound();
            }
            var fullPathToFile = pdf.PdfPath;
            var mimeType = "application/pdf";
            var fileContents = System.IO.File.ReadAllBytes(fullPathToFile);
            Response.AddHeader("Content-Disposition", "inline; filename=test.pdf");
            return File(fileContents,mimeType);
        }

        // POST: /PdfUpload/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        // GET: /Demo/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["username"] == null && Session["userid"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pdf pdf = db.Pdfs.Find(id);
            if (pdf == null)
            {
                return HttpNotFound();
            }
            return View(pdf);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="PdfId,PdfSize,PdfPath,PdfName,PdfDetail")] Pdf pdf)
        {
            if (Session["username"] == null && Session["userid"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {

                db.Entry(pdf).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(pdf);
        }

        // GET: /PdfUpload/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["username"] == null && Session["userid"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Pdf pdf = db.Pdfs.Find(id);
                System.IO.File.Delete(pdf.PdfPath);
                if (pdf == null)
                {
                    return HttpNotFound();
                }
                return View(pdf);
            }
            catch (Exception e)
            {
                ViewBag.Message = "File upload failed!!";
                return View("Delete");
            }
        }

        // POST: /PdfUpload/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["username"] == null && Session["userid"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            Pdf pdf = db.Pdfs.Find(id);
            db.Pdfs.Remove(pdf);
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
