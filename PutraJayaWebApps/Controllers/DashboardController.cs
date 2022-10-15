using PutraJayaWebApps.App_Start;
using PutraJayaWebApps.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PutraJayaWebApps.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Dashboard
        [Auth]
        public ActionResult Index()
        {
            return View();
        }
        [Auth]
        public ActionResult Customer()
        {
            var db = new Entities.PutraJayaAppsEntities();
            var customerData = db.MsCustomers.Where(x => x.IsDeleted == false);
            ViewData.Model = customerData.ToList();
            return View();
        }
        [Auth]
        public ActionResult AddEdit(int id)
        {
            var db = new Entities.PutraJayaAppsEntities();
            if (id == 0)
            {
                ViewData.Model = new Entities.MsCustomer();
            }
            else
            {
                var data = db.MsCustomers.FirstOrDefault(x => x.CustomerID == id);
                if (data != null)
                {
                    ViewData.Model = data;
                }
                else
                {
                    ViewData.Model = new Entities.MsCustomer();
                }
            }
            return View();
        }
        [Auth]
        [HttpPost]
        public ActionResult AddEdit(MsCustomer data)
        {
            var db = new Entities.PutraJayaAppsEntities();
            if (data.CustomerID == 0)
            {
                var newData = db.MsCustomers.Add(data);
                newData.IsDeleted = false;
                newData.CreatedBy = Session["username"].ToString();
                newData.CreatedDate = DateTime.Now;
                db.SaveChanges();
            }
            else
            {
                var oldData = db.MsCustomers.FirstOrDefault(x => x.CustomerID == data.CustomerID);
                if (oldData != null)
                {
                    oldData = data;
                    oldData.UpdatedBy = Session["username"].ToString();
                    oldData.UpdatedDate = DateTime.Now;
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Customer", "Dashboard", new { Message = "Success" });
        }
        public ActionResult DeleteCustomer(int id)
        {
            var db = new Entities.PutraJayaAppsEntities();
            var data = db.MsCustomers.FirstOrDefault(x => x.CustomerID == id);
            if (data != null)
            {
                data.IsDeleted = true;
                data.UpdatedBy = Session["username"].ToString() ;
                data.UpdatedDate = DateTime.Now;
                db.SaveChanges();
            }
            return RedirectToAction("Customer", "Dashboard", new { Message = "Success" });
        }
    }
}