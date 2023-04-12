using Microsoft.AspNetCore.Mvc;
using PhoneBookBusinessLayer.InterfacesOfManagers;

namespace PhoneBookUI.Components
{
    public class MenuViewComponent:ViewComponent
    {
        //Asp Net Core'da ViewComponentler sayfa parcalarini yonettigimiz yapidir.
        //Bu nedenle controllerlarin icinde yaptigimiz DI'lari burada da yapabiliriz.
        private readonly IMemberManager _memberManager;

        public MenuViewComponent(IMemberManager memberManager)
        {
            _memberManager = memberManager; 
        }

        public IViewComponentResult Invoke()
        {
            string? useremail = HttpContext.User.Identity?.Name; //Emaile bakacak
            TempData["LoggedInUserNameSurname"] = null;
            if (useremail!=null)
            {
                var user = _memberManager.GetById(useremail).Data;
                TempData["LoggedInUserNameSurname"] = $"{user.Name} {user.Surname}";
            }
            return View();
        }
    }
}
