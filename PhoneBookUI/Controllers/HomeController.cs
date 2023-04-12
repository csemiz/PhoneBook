using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhoneBookBusinessLayer.InterfacesOfManagers;
using PhoneBookEntityLayer.ViewModels;
using PhoneBookUI.Models;
using System.Diagnostics;

namespace PhoneBookUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPhoneTypeManager _phoneTypeManager;
        private readonly IMemberPhoneManager _memberPhoneManager;

        public HomeController(ILogger<HomeController> logger, IPhoneTypeManager phoneTypeManager, IMemberPhoneManager memberPhoneManager)
        {
            _logger = logger;
            _phoneTypeManager = phoneTypeManager;
            _memberPhoneManager = memberPhoneManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        [Authorize]//authorize login olmadan sayfaya erisimi onler.
        public IActionResult AddPhone()
        {
            try
            {
                ViewBag.PhoneTypes = _phoneTypeManager.GetAll().Data;//not IsRemoved viewModel in icine eklensin.
                MemberPhoneViewModel model = new MemberPhoneViewModel()
                {
                    MemberId = HttpContext.User.Identity?.Name
                };
                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Beklenmedik bir hata oluştu!" + ex.Message);
                ViewBag.PhoneTypes = new List<PhoneTypeViewModel>();
                return View();
            }
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddPhone(MemberPhoneViewModel model)
        {
            try
            {
                ViewBag.PhoneTypes = _phoneTypeManager.GetAll().Data;//not IsRemoved viewModel in icine eklensin.
                if (!ModelState.IsValid)
                {
                    //Gerekli alanlari doldurunuzu bu sefer yazmadik
                    return View(model);
                }
                //1) Ayni telefondan var mi?
                var samePhone = _memberPhoneManager.GetByConditions(x => x.MemberId == model.MemberId && x.Phone == model.Phone).Data;
                if (samePhone!=null)
                {
                    ModelState.AddModelError("", $"Bu telefon {samePhone.PhoneType.Name} türünde zaten eklenmiştir.");
                    return View(model);
                }
                //2)Telefonu ekle
                //Diger seceneginin senaryosunu yarin yazacagiz.
                model.CreatedDate = DateTime.Now;
                model.IsRemoved = false;

                if (!_memberPhoneManager.Add(model).IsSuccess)
                {
                    ModelState.AddModelError("", "EKLEME BAŞARISIZ! TEKRAR DENEYİNİZ!");
                    return View(model);
                }
                TempData["AddPhoneSuccessMsg"] = "Yeni numara rehbere eklendi.";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {

                ModelState.AddModelError("", "Beklenmedik bir hata oluştu!" + ex.Message);
                ViewBag.PhoneTypes = new List<PhoneTypeViewModel>();
                return View();
            }
        }

    }
}