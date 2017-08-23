using SaveMyHome.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SaveMyHome.ViewModels
{
    public class NotifyViewModel
    {
        [Required(ErrorMessageResourceType = typeof(Resources.NotificationVM), ErrorMessageResourceName = "ApartmentsRequired")]
        public IList<int> Apartments { get; set; }

        public ProblemStatus ProblemStatus { get; set; }

        [Display(Name = "SendEmail", ResourceType = typeof(Resources.NotificationVM))]
        public bool SendEmail { get; set; }

        [Display(Name = "Text", ResourceType = typeof(Resources.NotificationVM))]
        [Required(ErrorMessageResourceType = typeof(Resources.NotificationVM), ErrorMessageResourceName = "HeadMessageRequired")]
        public string HeadMessage { get; set; }

        public int ProblemId { get; set; }
        public bool IsSecondNotify { get; set; }
    }
}