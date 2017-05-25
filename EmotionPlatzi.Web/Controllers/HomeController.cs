using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EmotionPlatzi.Web.Models;

namespace EmotionPlatzi.Web.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            ViewBag.WelcomeMessage = "Hola Mundo";
            ViewBag.ValorEntero = 1;
            return View();
        }

        public ActionResult IndexAlt()
        {
           var model = new Home();
            model.WelcomeMessage = "Hola Mundo desde el modelo";
            return View(model);
        }
    }
}