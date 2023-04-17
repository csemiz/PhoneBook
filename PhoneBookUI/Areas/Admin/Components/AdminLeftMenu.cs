using Microsoft.AspNetCore.Mvc;
using PhoneBookBusinessLayer.InterfacesOfManagers;
using PhoneBookUI.Areas.Admin.Models;

namespace PhoneBookUI.Areas.Admin.Components
{
    public class AdminLeftMenu : ViewComponent
    {
        private readonly IMemberManager _memberManager;
        private readonly IPhoneTypeManager _phoneTypeManager;
        private readonly IMemberPhoneManager _memberPhoneManager;

        public AdminLeftMenu(IMemberManager memberManager, IPhoneTypeManager phoneTypeManager, IMemberPhoneManager memberPhoneManager)
        {
            _memberManager = memberManager;
            _phoneTypeManager = phoneTypeManager;
            _memberPhoneManager = memberPhoneManager;
        }
        //Eger email gonderimi yapilacaksa buraya IEmailSender eklenmelidir.

        public IViewComponentResult Invoke()
        {
            try
            {
                AdminLeftMenuDataCountModel model = new AdminLeftMenuDataCountModel()
                {
                    //toplam uye sayisi TempData["TotalMEmberCount"]
                    TotalMemberCount = _memberManager.GetAll().Data.Count,
                    //toplam telefon tipi sayisi
                    TotalPhoneTypeCount = _phoneTypeManager.GetAll().Data.Count,
                    //toplam numara sayisi
                    TotalContactNumberCount = _memberPhoneManager.GetAll().Data.Count
                };

                return View(model);
            }
            catch (Exception ex)
            {
                //TempData ile burada olusan hata gonderilebilir.
                return View(new AdminLeftMenuDataCountModel());

            }
        }
    }
}
