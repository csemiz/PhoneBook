using Microsoft.AspNetCore.Mvc;
using PhoneBookBusinessLayer.EmailSenderBusiness;
using PhoneBookBusinessLayer.InterfacesOfManagers;
using PhoneBookEntityLayer.ViewModels;
using PhoneBookUI.Models;
using System.Security.Cryptography;
using System.Text;

namespace PhoneBookUI.Controllers
{
    public class AccountController : Controller
    {
        private readonly IMemberManager _memberManager;
        private readonly IEmailSender _emailSender;

        const int keySize = 64;
        const int iterations = 350000;
        HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;

        public AccountController(IMemberManager memberManager, IEmailSender emailSender)
        {
            _memberManager = memberManager;
            _emailSender = emailSender;
        }

        public IActionResult Register()
        {
            //Bu metot sayfayi sadece getirir.HTTPGET
            return View();//bu metot geriye bir sayfa gonderecek.

        }

        [HttpPost]//Sayfadaki submit turundeki butona tikladiginda yazdigi bu metoda dusecektir.
        public IActionResult Register(RegisterViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)//Gelen bilgiler class icindeki annotationslara uygun degilse
                {
                    ModelState.AddModelError("", "Gerekli alanları lütfen doldurunuz!");
                    return View(model);
                }

                //ekleme islemleri yapılacaktır.

                //1)Ayni emailden tekrar kayit olamaz!
                //var isSameEmail = _memberManager.GetByConditions(x => x.Email.ToLower() == model.Email.ToLower());

                //if (isSameEmail != null)
                //{
                //    ModelState.AddModelError("", "Dikkat bu kullanıcı sistemde zaten mevcuttur !");
                //    return View(model);
                //}
                MemberViewModel member = new MemberViewModel()
                {
                    Email = model.Email,
                    Name = model.Name,
                    Surname = model.Surname,
                    Gender = model.Gender,
                    BirthDate = model.BirthDate,
                    CreatedDate = DateTime.Now,
                    IsRemoved = false
                };
                member.PasswordHash = HashPasword(model.Password, out byte[] salt);
                member.Salt = salt;

                var result = _memberManager.Add(member);
                if (result.IsSuccess)
                {

                    //Hosgeldiniz emaili gonderilecek
                    var email = new EmailMessage()
                    {
                        To = new string[] { member.Email },
                        Subject = $"503 Telefon Rehberi - HOŞGELDİNİZ!",
                        //Body icine html yaziliyor
                        Body = $"<html lang='tr'><head></head><body>" +
                        $"Merhaba Sayın {member.Name} {member.Surname},<br/>" +
                        $"Sisteme kaydınız gerçekleşmiştir." +
                        $"</body></html>"
                    };

                    //sonra async'ye cevirelim
                    _emailSender.SendEmail(email);


                    //login sayfasina yonlendirilecek
                    //buraya kaydiniz gerceklesti yazilacak
                    return RedirectToAction("Login", "Account", new { email = model.Email });
                }
                else
                {
                    ModelState.AddModelError("", result.Message);
                    return View(model);
                }
            }

            catch (Exception ex)
            {
                ModelState.AddModelError("", "Beklenmedik bir sorun oluştu!" + ex.Message);// ex loglanmali biz simdi gecici olarak yazdik.
                return View(model);// Burada return View(model) parametre olarak model vermemizin sebebi sayfadaki bilgiler silinmesin.
            }
        }

        [HttpGet]
        public IActionResult Login(string? email)
        {
            if (!string.IsNullOrEmpty(email))
            {
                LoginViewModel model = new LoginViewModel()
                {
                    Email = email
                };
                return View(model);
            }
            
            return View(new LoginViewModel());
        }

        private string HashPasword(string password, out byte[] salt)
        {
            salt = RandomNumberGenerator.GetBytes(keySize);
            var hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                salt,
                iterations,
                hashAlgorithm,
                keySize);
            return Convert.ToHexString(hash);
        }
        private bool VerifyPassword(string password, string hash, byte[] salt)
        {
            var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, hashAlgorithm, keySize);

            return hashToCompare.SequenceEqual(Convert.FromHexString(hash));
        }
    }
}
