using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using Resources;

namespace SaveMyHome.ViewModels
{
    [DisplayName("Подробные сведения")]
    public class UserViewModel
    {
        [NotMapped, HiddenInput(DisplayValue = false)]
        public string Id { get; set; }

        [Display(Name = "Email")]
        [UIHint("EmailAddress")]
        public string Email { get; set; }

        [Display(Name = "FirstName", ResourceType = typeof(UserVM))] 
        public string FirstName { get; set; }

        [Display(Name = "LastName", ResourceType = typeof(UserVM))]
        public string LastName { get; set; }

        [Display(Name = "Age", ResourceType = typeof(UserVM))]
        public int Age { get; set; }

        [Display(Name = "ApartmentNumber", ResourceType = typeof(UserVM))]
        public int ApartmentNumber { get; set; }

        [Display(Name = "PhoneNumber", ResourceType = typeof(UserVM))]
        public string PhoneNumber { get; set; }

        [Display(Name = "SecondPhoneNumber", ResourceType = typeof(UserVM))]
        public string SecondPhoneNumber { get; set; } 

        [Display(Name = "Skills", ResourceType = typeof(UserVM))]
        public string Skills { get; set; } 

        [Display(Name = "Hobbies", ResourceType = typeof(UserVM))]
        public string Hobbies { get; set; }

        [HiddenInput(DisplayValue = false)]
        public byte[] ImageData { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string ImageMimeType { get; set; }
    }
}