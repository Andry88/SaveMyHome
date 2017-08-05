using SaveMyHome.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using SaveMyHome.Infrastructure;
using SaveMyHome.Models;
using System.Security.Claims;
using System.Threading;
using System.Web.Mvc;
using System.Web.UI.WebControls;

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