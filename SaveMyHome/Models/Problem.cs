using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SaveMyHome.Models
{
    
    public class Problem
    {
        #region Properties
        public int Id { get; set; }

        [Display(Name = "Проблема:"), Required]
        [StringLength(15, ErrorMessage = "Название проблемы должно содержать от {2} до {1} символов", MinimumLength = 1)]
        public string Name { get; set; }
        #endregion

        #region NavigationProperties
        public virtual ICollection<Event> Events { get; set; }
        #endregion
    }
}