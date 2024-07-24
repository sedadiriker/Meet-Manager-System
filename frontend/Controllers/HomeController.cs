using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using frontend.Models;

namespace frontend.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [Route("/anasayfa")]
        public IActionResult Index()
        {
            
            return View();
        }

        [Route("/toplantı_listesi")]
        public IActionResult Toplantı_Listesi()
        {
            return View();
        }

        [Route("/toplantı_ekle")]
        public IActionResult Toplantı_Ekle()
        {
            return View();
        }

        [Route("/")]
        public IActionResult Login()
        {
            return View();
        }

        [Route("/register")]
        public IActionResult Register()
        {
            return View();
        }

        [Route("/toplantı_raporları")]
        public IActionResult Toplantı_Raporları()
        {
            return View();
        }

        [Route("/tablolar")]
        public IActionResult Tablolar()
        {
            return View();
        }
        [Route("/profil")]
        public IActionResult Profil()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
