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

        [Display(Name = "Имя")]
        public string FirstName { get; set; }

        [Display(Name = "Фамилия")]
        public string LastName { get; set; }

        [Display(Name = "Возраст")]
        public int Age { get; set; }

        [Display(Name = "Номер квартиры")]
        public int ApartmentNumber { get; set; }

        [Display(Name = "Tелефон")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Дополнительный телефон")]
        public string SecondPhoneNumber { get; set; } 

        [Display(Name = "Навыки и умения")]
        public string Skills { get; set; } 

        [Display(Name = "Увлечения")]
        public string Hobbies { get; set; }

        [HiddenInput(DisplayValue = false)]
        public byte[] ImageData { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string ImageMimeType { get; set; }
    }
}