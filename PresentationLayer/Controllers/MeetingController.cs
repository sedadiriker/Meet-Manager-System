using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PresentationLayer.Models;

namespace PresentationLayer.Controllers
{
    public class MeetingController : Controller
    {
        private readonly ILogger<MeetingController> _logger;

        public MeetingController(ILogger<MeetingController> logger)
        {
            _logger = logger;
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
        [Route("/toplantılarım")]
        public IActionResult Toplantılarım()
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
       
    }
}
