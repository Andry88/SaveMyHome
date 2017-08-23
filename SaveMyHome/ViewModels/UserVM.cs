using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace SaveMyHome.ViewModels
{
    [DisplayName("Подробные сведения")]
    public class UserVM
    {
        [NotMapped, HiddenInput(DisplayValue = false)]
        public string Id { get; set; }

        [Display(Name = "Email")]
        [UIHint("EmailAddress")]
        public string Email { get; set; }

        [Display(Name = "FirstName", ResourceType = typeof(Resources.UserVM))] 
        public string FirstName { get; set; }

        [Display(Name = "LastName", ResourceType = typeof(Resources.UserVM))]
        public string LastName { get; set; }

        [Display(Name = "Age", ResourceType = typeof(Resources.UserVM))]
        public int Age { get; set; }

        [Display(Name = "ApartmentNumber", ResourceType = typeof(Resources.UserVM))]
        public int ApartmentNumber { get; set; }

        [Display(Name = "PhoneNumber", ResourceType = typeof(Resources.UserVM))]
        public string PhoneNumber { get; set; }

        [Display(Name = "SecondPhoneNumber", ResourceType = typeof(Resources.UserVM))]
        public string SecondPhoneNumber { get; set; } 

        [Display(Name = "Skills", ResourceType = typeof(Resources.UserVM))]
        public string Skills { get; set; } 

        [Display(Name = "Hobbies", ResourceType = typeof(Resources.UserVM))]
        public string Hobbies { get; set; }

        [HiddenInput(DisplayValue = false)]
        public byte[] ImageData { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string ImageMimeType { get; set; }
    }
}