using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SaveMyHome.Models
{
    public class ClientProfile
    {
        #region Properties
        [Key]
        [ForeignKey("ApplicationUser")]
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => FirstName + " " + LastName;
        public int Age { get; set; }
        public string SecondPhoneNumber { get; set; }
        public string Skills { get; set; }
        public string Hobbies { get; set; }
        public byte[] ImageData { get; set; }
        public string ImageMimeType { get; set; }
        #endregion

        #region NavigationProperties

        public virtual ApplicationUser ApplicationUser { get; set; }

        public int ApartmentNumber { get; set; }
        [ForeignKey("ApartmentNumber")]
        public virtual Apartment Apartment { get; set; }

        public virtual ICollection<Message> Messages { get; set; }
        #endregion
    }
}