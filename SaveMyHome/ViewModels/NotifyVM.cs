using SaveMyHome.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SaveMyHome.ViewModels
{
    public class NotifyVM
    {
        [Required(ErrorMessage = "Выберите как минимум одну квартиру для оповещения")]
        public IList<int> Apartments { get; set; }

        public ProblemStatus ProblemStatus { get; set; }
        [Display(Name = "Отправить email")]
        public bool SendEmail { get; set; }

        [Display(Name = "Текст сообщения:"), Required(ErrorMessage = "Напишите сообщение")]
        public string HeadMessage { get; set; }

        public int ProblemId { get; set; }
        public bool IsSecondNotify { get; set; }
    }
}