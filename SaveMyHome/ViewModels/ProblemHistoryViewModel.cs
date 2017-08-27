using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Resources;

namespace SaveMyHome.ViewModels
{
    public class ProblemHistoryViewModel
    {
        [Display(Name = "Name", ResourceType = typeof(ProblemHistoryVM))]
        public string Name { get; set; }

        [Display(Name = "Detected", ResourceType = typeof(ProblemHistoryVM))]
        public DateTime Start { get; set; }

        [Display(Name = "Fixed", ResourceType = typeof(ProblemHistoryVM))]
        public DateTime? End { get; set; }

        [Display(Name = "ApartmentCulprit", ResourceType = typeof(ProblemHistoryVM))]
        public int Culprit { get; set; }

        [Display(Name = "ApartmentsVictims", ResourceType = typeof(ProblemHistoryVM))]
        public IEnumerable<int> Victims { get; set; } 
    }
}