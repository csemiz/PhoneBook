using Microsoft.AspNetCore.Mvc;
using PhoneBookBusinessLayer.InterfacesOfManagers;

namespace PhoneBookUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    //Route("a/[Controller]/[Action]/{id?}")] bu route verildiginde [Action] yazan yere action un tam adiniyazmadan sayfa acilmaz.
    [Route("admin")]//bu route verildiginde controller e nasil ulasildigi belirtilir ve action a ulasilma konusundaki kurali action üzerine yazilan kural belirler.
    public class HomeController : Controller
    {
        private readonly IMemberManager _memberManager;
        private readonly IPhoneTypeManager _phoneTypeManager;
        private readonly IMemberPhoneManager _memberPhoneManager;

        public HomeController(IMemberManager memberManager, IPhoneTypeManager phoneTypeManager, IMemberPhoneManager memberPhoneManager)
        {
            _memberManager = memberManager;
            _phoneTypeManager = phoneTypeManager;
            _memberPhoneManager = memberPhoneManager;
        }

        [HttpGet]
        [Route("dsh")] //Action un ismi cok uzun olabilir url'e action un isminin hepsini yazmak istemezsek action a Route verebiliriz.
        public IActionResult Dashboard()
        {
            //bu ay sisteme kayit olan uye sayisi
            DateTime thisMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            ViewBag.MontlyMemberCount = _memberManager.GetAll(x => x.CreatedDate > thisMonth.AddDays(-1)).Data.Count();

            //bu ay sisteme eklenen numara sayisi
            ViewBag.MontlyMemberCount = _memberPhoneManager.GetAll(x => x.CreatedDate > thisMonth.AddDays(-1)).Data.Count();

            var members = _memberManager.GetAll().Data.OrderBy(x => x.CreatedDate);
            //en son eklenen uyenin adi soyadi
            ViewBag.LastMember = $"{members.LastOrDefault()?.Name} {members.LastOrDefault()?.Surname}";

            //Rehbere en son eklenen kisinin adi soyadi
            var contacts = _memberPhoneManager.GetAll().Data.OrderBy(x => x.CreatedDate);

            ViewBag.LastContact = contacts.LastOrDefault()?.FriendNameSurname;

            return View();
        }
        [Route("/admin/GetPhoneTypePieData")]
        public JsonResult GetPhoneTypePieData()
        {
            try
            {
                Dictionary<string, int> model = new Dictionary<string, int>();//iki data tutmamiz gerektigi icin dictionary kullaniyoruz.
                var data = _memberPhoneManager.GetAll().Data;
                foreach (var item in data)
                {
                    if (model.ContainsKey(item.PhoneType.Name))//wissen kurs tipinden var mi?
                    {
                        //sayiyi 1 arttirsin
                        model[item.PhoneType.Name] += 1;
                    }
                    else
                    {
                        model.Add(item.PhoneType.Name, 1);
                    }
                }//foreach bitti.


                return Json(new
                {
                    isSuccess = true,
                    message = "Veriler geldi",
                    types = model.Keys.ToArray(),
                    points = model.Values.ToArray()
                });

            }
            catch (Exception ex)
            {
                return Json(new { isSuccess = false, message = "Veriler getirilemedi!" });
            }
        }

        [HttpGet]
        [Route("uye")]
        public IActionResult MemberIndex()
        {
            try
            {
                var data = _memberManager.GetAll().Data;

                return View(data);
            }
            catch (Exception ex)
            {

                ModelState.AddModelError("", "Beklenmedik bir hata oldu!" + ex.Message);
                return View();
            }
        }
    }
}
