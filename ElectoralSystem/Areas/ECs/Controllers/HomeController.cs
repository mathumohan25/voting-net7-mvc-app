using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ElectoralSystem.Areas.ECs.Controllers
{
    [Area("ECs")]
    [Authorize(Roles = "EC_Admin")]
    public class HomeController : Controller
    {
        // GET: HomeController
        public ActionResult Index()
        {
            return View();
        }        
    }
}
