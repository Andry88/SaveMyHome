using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaveMyHome.ViewModels
{
    public class LoginViewModel
    {
        [Display(Name = "Email"), Required]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Пароль")]
        [DataType(DataType.Password), Required]
        public string Password { get; set; }

        [Display(Name = "Запомнить меня?")]
        public bool RememberMe { get; set; }
    }
    public class RegisterViewModel
    {
        [Display(Name = "Пароль"), Required(ErrorMessage = "Введите пароль"), DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "{0} должен быть длиной минимум {2} символа.", MinimumLength = 6)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Подтверждение пороля")]
        [Compare("Password", ErrorMessage = "Пароль и подтверждение пороля не совпадают.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Email"), Required(ErrorMessage = "Укажите Ваш email"), EmailAddress]
        public string Email { get; set; }
        [Display(Name = "Имя"), Required(ErrorMessage = "Укажите Ваше имя")]
        [StringLength(15, ErrorMessage = "{0} должно содержать от {2} до {1} символов", MinimumLength = 2)]
        public string FirstName { get; set; }

        [Display(Name = "Фамилия"), Required(ErrorMessage = "Укажите Вашу фамилию")]
        [StringLength(15, ErrorMessage = "{0} должна содержать от {2} до {1} символов", MinimumLength = 2)]
        public string LastName { get; set; }

        [Display(Name = "Возраст"), Required(ErrorMessage = "Укажите Ваш возраст")]
        [Range(8, 100, ErrorMessage = "{0} должен быть не меьше {1} и не более {2} лет")]
        public int Age { get; set; }

        [Display(Name = "Номер квартиры"), Required(ErrorMessage = "Укажите номер Вашей квартиры")]
        [Range(1, 71, ErrorMessage = "{0} должен быть не меньше {1} и не больше {2}")]
        public int ApartmentNumber { get; set; }

        [Display(Name = "Tелефон"), Required(ErrorMessage = "Укажите номер телефона"), DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Display(Name = "Дополнительный телефон"), DataType(DataType.PhoneNumber)]
        public string SecondPhoneNumber { get; set; }

        [Display(Name = "Навыки и умения"), DataType(DataType.MultilineText)]
        public string Skills { get; set; }

        [Display(Name = "Увлечения"), DataType(DataType.MultilineText)]
        public string Hobbies { get; set; }

        public byte[] ImageData { get; set; }
        public string ImageMimeType { get; set; }
    }

    public class EditAccountVM : RegisterViewModel
    {
        [NotMapped]
        public string Id { get; set; }
    }
    public class ExternalLoginConfirmationViewModel
    {
        [Display(Name = "Email"), Required]
        public string Email { get; set; }

        [Display(Name = "Номер квартиры"), Required(ErrorMessage = "Укажите номер Вашей квартиры")]
        [Range(1, 71, ErrorMessage = "{0} должен быть не меньше {1} и не больше {2}")]
        public int ApartmentNumber { get; set; }

        [Display(Name = "Имя"), Required(ErrorMessage = "Укажите Ваше имя")]
        [StringLength(15, ErrorMessage = "{0} должно содержать от {2} до {1} символов", MinimumLength = 2)]
        public string FirstName { get; set; }

        [Display(Name = "Фамилия"), Required(ErrorMessage = "Укажите Вашу фамилию")]
        [StringLength(15, ErrorMessage = "{0} должна содержать от {2} до {1} символов", MinimumLength = 2)]
        public string LastName { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

   

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
