using Microsoft.AspNetCore.Mvc;
using PhoneBookBusinessLayer.InterfacesOfManagers;

namespace PhoneBookUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    //Route("a/[Controller]/[Action]/{id?}")] bu route verildiginde [Action] yazan yere action un tam adiniyazmadan sayfa acilmaz.
    [Route("a/h")]//bu route verildiginde controller e nasil ulasildigi belirtilir ve action a ulasilma konusundaki kurali action üzerine yazilan kural belirler.
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
        [Route("d")] //Action un ismi cok uzun olabilir url'e action un isminin hepsini yazmak istemezsek action a Route verebiliriz.
        public IActionResult Dashboard()
        {
            //bu ay sisteme kayit olan uye sayisi
            DateTime thisMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            ViewBag.MontlyMemberCount = _memberManager.GetAll(x => x.CreatedDate > thisMonth.AddDays(-1)).Data.Count();

            //bu ay sisteme eklenen numara sayisi
            ViewBag.MontlyMemberCount = _memberPhoneManager.GetAll(x => x.CreatedDate > thisMonth.AddDays(-1)).Data.Count();

            var members = _memberManager.GetAll().Data;
            //en son eklenen adi soyadi
            ViewBag.LastMember = $"{members.LastOrDefault()?.Name} {members.LastOrDefault()?.Surname}";

            //Rehbere en son eklenen kisinin adi soyadi
            var contacts = _memberPhoneManager.GetAll().Data;

            ViewBag.LastContact = contacts.LastOrDefault()?.FriendNameSurname;

            return View();
        }
    }
}
