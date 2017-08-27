using SaveMyHome.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Resources;

namespace SaveMyHome.ViewModels
{
    public class AnswerViewModel
    {
        public bool IsFromPotentialCulprit { get; set; }
        public IEnumerable<int> Apartments { get; set; }
        public ProblemStatus VisitorProblemStatus { get; set; }
        
        public Message HeadMessage { get; set; }

        [Display(Name = "Text", ResourceType = typeof(AnswerVM))]
        [Required(ErrorMessageResourceType = typeof(AnswerVM), ErrorMessageResourceName = "CurrAnswerRequired")]
        public string CurrAnswer { get; set; }

        public IEnumerable<Message> AnswersMessages { get; set; }
    }
}