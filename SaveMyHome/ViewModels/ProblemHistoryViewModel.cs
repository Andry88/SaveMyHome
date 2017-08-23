using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SaveMyHome.ViewModels
{
    public class ProblemHistoryViewModel
    {
        [Display(Name = "Name", ResourceType = typeof(Resources.ProblemHistoryVM))]
        public string Name { get; set; }

        [Display(Name = "Detected", ResourceType = typeof(Resources.ProblemHistoryVM))]
        public DateTime Start { get; set; }

        [Display(Name = "Fixed", ResourceType = typeof(Resources.ProblemHistoryVM))]
        public DateTime? End { get; set; }

        [Display(Name = "ApartmentCulprit", ResourceType = typeof(Resources.ProblemHistoryVM))]
        public int Culprit { get; set; }

        [Display(Name = "ApartmentsVictims", ResourceType = typeof(Resources.ProblemHistoryVM))]
        public IEnumerable<int> Victims { get; set; } 
    }
}