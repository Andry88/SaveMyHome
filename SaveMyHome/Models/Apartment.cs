using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SaveMyHome.Helpers;

namespace SaveMyHome.Models
{
    public class Apartment
    {
        #region Properties
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Number { get; set; }
        #endregion

        #region NavigationProperties
        public virtual ICollection<ClientProfile> ClientProfiles { get; set; }
        public virtual ICollection<Reaction> Reactions { get; set; }
        #endregion

        #region Methods
        [Display(Name = "Этаж")]
        public int Floor => (int)Math.Ceiling((decimal)Number / House.ApartmentsAmount * House.FloorsCount);
        #endregion
    }
}