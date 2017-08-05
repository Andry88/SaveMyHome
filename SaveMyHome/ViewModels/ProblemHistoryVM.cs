using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SaveMyHome.ViewModels
{
    public class ProblemHistoryVM
    {
        [Display(Name = "Проблема:")]
        public string Name { get; set; }

        [Display(Name = "Обнаружена: ")]
        public DateTime Start { get; set; }

        [Display(Name = "Устранена: ")]
        public DateTime? End { get; set; }

        [Display(Name = "Квартирa-виновник:")]
        public int Culprit { get; set; }

        [Display(Name = "Квартиры-жертвы:")]
        public IEnumerable<int> Victims { get; set; } 
    }
}