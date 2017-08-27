using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Resources;

namespace SaveMyHome.ViewModels
{
    public abstract class EmailMetadata
    {
        [Display(Name = "Email"), EmailAddress]
        [Required(ErrorMessageResourceType = typeof(Account), ErrorMessageResourceName = "EmailRequired")]
        public string Email { get; set; }
    }

    public abstract class CredentialsMetadata : EmailMetadata
    {
        [Display(Name = "Password", ResourceType = typeof(Account)), DataType(DataType.Password)]
        [Required(ErrorMessageResourceType = typeof(Account), ErrorMessageResourceName = "EnterPassword")]
        [StringLength(100, ErrorMessageResourceType = typeof(Account), ErrorMessageResourceName = "PasswordLength", MinimumLength = 6)]
        public string Password { get; set; }
    }

    public abstract class CredentialsWithPasswordConfirmationMetadata : CredentialsMetadata
    {
        [DataType(DataType.Password)]
        [Display(Name = "PasswordConfirmation", ResourceType = typeof(Account))]
        [Compare("Password", ErrorMessageResourceType = typeof(Account), ErrorMessageResourceName = "PasswordCompare")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginViewModel : CredentialsMetadata
    {
        [Display(Name = "RememberMe", ResourceType = typeof(Account))]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel : CredentialsWithPasswordConfirmationMetadata
    {
        [Display(Name = "Hobbies", ResourceType = typeof(UserVM)), DataType(DataType.MultilineText)]
        public string Hobbies { get; set; }

        [Display(Name = "Skills", ResourceType = typeof(UserVM)), DataType(DataType.MultilineText)]
        public string Skills { get; set; }

        [Display(Name = "SecondPhoneNumber", ResourceType = typeof(UserVM)), DataType(DataType.PhoneNumber)]
        public string SecondPhoneNumber { get; set; }

        [Display(Name = "PhoneNumber", ResourceType = typeof(UserVM)), DataType(DataType.PhoneNumber)] 
        [Required(ErrorMessageResourceType = typeof(Account), ErrorMessageResourceName = "PhoneNumberRequired")]
        public string PhoneNumber { get; set; }

        [Display(Name = "ApartmentNumber", ResourceType = typeof(UserVM))]
        [Required(ErrorMessageResourceType = typeof(Account), ErrorMessageResourceName = "ApartmentNumberRequired")]
        [Range(1, 71, ErrorMessageResourceType = typeof(Account), ErrorMessageResourceName = "ApartmentNumberRange")]
        public int ApartmentNumber { get; set; }

        [Display(Name = "Age", ResourceType = typeof(UserVM))]
        [Required(ErrorMessageResourceType = typeof(Account), ErrorMessageResourceName = "AgeRequired")]
        [Range(8, 100, ErrorMessageResourceType = typeof(Account), ErrorMessageResourceName = "AgeRange")]
        public int Age { get; set; }

        [Display(Name = "LastName", ResourceType = typeof(UserVM))]
        [Required(ErrorMessageResourceType = typeof(Account), ErrorMessageResourceName = "LastNameRequired")]
        [StringLength(15, ErrorMessageResourceType = typeof(Account), ErrorMessageResourceName = "LastNameLength", MinimumLength = 2)]
        public string LastName { get; set; }

        [Display(Name = "FirstName", ResourceType = typeof(UserVM))]
        [Required(ErrorMessageResourceType = typeof(Account), ErrorMessageResourceName = "FirstNameRequired")]
        [StringLength(15, ErrorMessageResourceType = typeof(Account), ErrorMessageResourceName = "FirstNameLength", MinimumLength = 2)]
        public string FirstName { get; set; }

        public byte[] ImageData { get; set; }
        public string ImageMimeType { get; set; }
    }

    public class EditAccountViewModel : RegisterViewModel 
    {
        [NotMapped]
        public string Id { get; set; }
    }

    public class ExternalLoginConfirmationViewModel : EmailMetadata
    {
        [Display(Name = "ApartmentNumber", ResourceType = typeof(UserVM))]
        [Required(ErrorMessageResourceType = typeof(Account), ErrorMessageResourceName = "ApartmentNumberRequired")]
        [Range(1, 71, ErrorMessageResourceType = typeof(Account), ErrorMessageResourceName = "ApartmentNumberRange")]
        public int ApartmentNumber { get; set; }

        [Display(Name = "FirstName", ResourceType = typeof(UserVM))]
        [Required(ErrorMessageResourceType = typeof(Account), ErrorMessageResourceName = "FirstNameRequired")]
        [StringLength(15, ErrorMessageResourceType = typeof(Account), ErrorMessageResourceName = "FirstNameLength", MinimumLength = 2)]
        public string FirstName { get; set; }

        [Display(Name = "LastName", ResourceType = typeof(UserVM))]
        [Required(ErrorMessageResourceType = typeof(Account), ErrorMessageResourceName = "LastNameRequired")]
        [StringLength(15, ErrorMessageResourceType = typeof(Account), ErrorMessageResourceName = "LastNameLength", MinimumLength = 2)]
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

    public class ForgotViewModel : EmailMetadata
    {}

    public class ResetPasswordViewModel : CredentialsWithPasswordConfirmationMetadata
    {
        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel : EmailMetadata
    {}
}
