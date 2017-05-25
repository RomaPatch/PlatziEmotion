using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using EmotionPlatzi.Web.Models;
using EmotionPlatzi.Web.Util;

namespace EmotionPlatzi.Web.Controllers
{
    public class EmoUploaderController : Controller
    {
        private string serverFolderPath;
        private EmotionHelper emoHelper;
        private string key;
        EmotionPlatziWebContext db = new EmotionPlatziWebContext();

        public EmoUploaderController()
        {
            key = ConfigurationManager.AppSettings["EMOTION_KEY"];
            serverFolderPath = ConfigurationManager.AppSettings["UPLOAD_DIR"];

            emoHelper = new EmotionHelper(key);
        }
        // GET: EmoUploader
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Index(HttpPostedFileBase file)
        {

            if (file?.ContentLength > 0)
            {
                var pictureName = Guid.NewGuid().ToString();
                pictureName += Path.GetExtension(file.FileName);

                var route = Server.MapPath(serverFolderPath);
                route += "/" + pictureName;
                file.SaveAs(route);

               var emoPicture =  await emoHelper.DetectAndExtractFacesAsync(file.InputStream);

                emoPicture.Name = file.FileName;
                emoPicture.Path = $"{serverFolderPath}/{pictureName}";
                db.EmoPictures.Add(emoPicture);
                await db.SaveChangesAsync();

                return RedirectToAction("Details", "EmoPictures", new {Id = emoPicture.Id});
            }

            return View();
        }
    }
}