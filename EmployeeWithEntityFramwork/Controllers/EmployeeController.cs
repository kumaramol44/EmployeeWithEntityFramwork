using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EmployeeWithEntityFramwork.Models;

namespace EmployeeWithEntityFramwork.Controllers
{
    public class EmployeeController : Controller
    {
        private EmployeeContext db = new EmployeeContext();
        //
        // GET: /Student/

        public ActionResult Index()
        {
            ViewData["AllEmployees"] = db.Employees.ToList();
            return View("Employee");
        }

        //
        // GET: /Student/Details/5

        //public ActionResult Details(string id = null)
        //{
        //    Employee employee = db.Employees.Find(id);
        //    if (employee == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View();
        //}

        //
        // GET: /Student/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Student/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Employees employee)
        {
            if (ModelState.IsValid)
            {
                var emp = new Employee
                {
                    Name = employee.Name,
                    Department = employee.Department,
                    Image = GetBase64(employee)
                };
                try
                {
                    db.Employees.Add(emp);
                    db.SaveChanges();
                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            System.Console.WriteLine("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }
                }
                return RedirectToAction("Index");
            }
            return View("Employee");
        }

        //
        // GET: /Student/Edit/5

        //public ActionResult Edit(string id = null)
        //{
        //    //empl student = db.Employees.Find(id);
        //    if (student == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(student);
        //}

        //
        // POST: /Student/Edit/5

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(Employee employee)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(employee).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(employee);
        //}

        //
        // GET: /Student/Delete/5

        public ActionResult Delete(string id = null)
        {
            Employee student = db.Employees.Find(id);
            db.Employees.Remove(student);
            db.SaveChanges();
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        //
        // POST: /Student/Delete/5

        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(string id)
        //{
        //    Employee employee = db.Employees.Find(id);
        //    db.Employees.Remove(employee);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public string GetBase64(Employees employee)
        {
            byte[] fileInBytes;
            using (BinaryReader theReader = new BinaryReader(employee.EmployeeImage.InputStream))
            {
                fileInBytes = theReader.ReadBytes(employee.EmployeeImage.ContentLength);
            }
            return "data:image/png;base64," + Convert.ToBase64String(fileInBytes.ToArray());
        }
    }
}