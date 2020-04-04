using BatchProcessor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;

namespace BatchProcessor.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        //public ActionResult About()
        //{
        //    ViewBag.Message = "Your application description page.";

        //    return View();
        //}

        public ActionResult LogOut()
        {
            ViewBag.Message = "Your contact page.";
            TempData["logout"] = "You have successfully logged out!";
            return RedirectToAction("Index", "Home");
            //return View();
        }

        public ActionResult Processor()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Processor(ProcessorViewModel model)
        {
            Process proc = null;
            try
            {
                string targetDir = string.Format(@"{0}", System.Configuration.ConfigurationManager.AppSettings["filePath"].ToString());   //this is where mybatch.bat lies
                proc = new Process();
                proc.StartInfo.WorkingDirectory = targetDir;
                proc.StartInfo.FileName = model.SelectedFile;
                proc.StartInfo.Arguments = String.Format("\"{0}\" \"{1}\"", model.SelectedDate, model.SelectedDate);  //this is argument
                proc.StartInfo.CreateNoWindow = false;
                proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;  //this is for hiding the cmd window...so execution will happen in back ground.
                proc.Start();
                //proc.WaitForExit();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception Occurred :{0},{1}", ex.Message, ex.StackTrace.ToString());
            }
            TempData["Success"] = "Automation Job for "+ model.SelectedFile + " executed successfully!";
            return RedirectToAction("Processor", "Home");
            //return View();
        }

        [HttpPost]
        public ActionResult Index(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if ( (model.Email.ToLower() == System.Configuration.ConfigurationManager.AppSettings["username"].ToString() && model.Password == System.Configuration.ConfigurationManager.AppSettings["password"].ToString() ) ||
                 (model.Email.ToLower() == System.Configuration.ConfigurationManager.AppSettings["secondusername"].ToString() && model.Password == System.Configuration.ConfigurationManager.AppSettings["secondpassword"].ToString() ) ||
                 (model.Email.ToLower() == System.Configuration.ConfigurationManager.AppSettings["thirdusername"].ToString() && model.Password == System.Configuration.ConfigurationManager.AppSettings["thirdpassword"].ToString() ) )
            {
                Session["User"] = "CurrentUser";
                return View("Processor");
            }
            else {
                return View("Index");
            }
        }

    }
}