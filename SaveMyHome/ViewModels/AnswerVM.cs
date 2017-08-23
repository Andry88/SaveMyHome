using SaveMyHome.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SaveMyHome.ViewModels
{
    public class AnswerVM
    {
        public bool IsFromPotentialCulprit { get; set; }
        public IEnumerable<int> Apartments { get; set; }
        public ProblemStatus VisitorProblemStatus { get; set; }
        
        public Message HeadMessage { get; set; }

        [Display(Name = "Text", ResourceType = typeof(Resources.AnswerViewModel))]
        [Required(ErrorMessageResourceType = typeof(Resources.AnswerViewModel),ErrorMessageResourceName = "CurrAnswerRequired")]
        public string CurrAnswer { get; set; }

        public IEnumerable<Message> AnswersMessages { get; set; }
    }
}