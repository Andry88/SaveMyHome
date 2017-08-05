using SaveMyHome.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SaveMyHome.ViewModels
{
    public class AnswerVM
    {
        public bool IsFromPotentialCulprit { get; set; }
        public IEnumerable<int> Apartments { get; set; }
        public ProblemStatus VisitorProblemStatus { get; set; }
        
        public Message HeadMessage { get; set; }

        [Display(Name = "Отправить сообщение:"), Required(ErrorMessage = "Введите сообщение")]
        public string CurrAnswer { get; set; }

        public IEnumerable<Message> AnswersMessages { get; set; }
    }
}