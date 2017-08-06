using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaveMyHome.Models
{
    public class Apartment
    {
        #region Properties
        [Display(Name = "Номер квартиры"), Required(ErrorMessage = "Укажите номер Вашей квартиры")]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Range(1, 71, ErrorMessage = "{0} должен быть не меньше {1} и не больше {2}")]
        public int Number { get; set; }
        #endregion

        #region NavigationProperties
        public virtual ICollection<ApplicationUser> Users { get; set; }
        public virtual ICollection<Reaction> Reactions { get; set; }
        #endregion

        #region Methods
        [Display(Name = "Этаж")]
        public int Floor => (int)Math.Ceiling((decimal)Number / House.ApartmentsAmount * House.FloorsCount);
        #endregion
    }
}