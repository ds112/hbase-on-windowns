using System.Web.Mvc;

namespace MVCSampleBrowser.Controllers.Home
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            Response.Redirect(Url.Content("~/Introduction/IntroductionDefault"));
            return View();
        }
    }
}