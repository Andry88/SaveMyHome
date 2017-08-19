using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaveMyHome.Models
{
    [Table("ProblemHistory")]
    public class Event
    {
        #region Properties
        public int Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime? End { get; set; }
        #endregion

        #region NavigationProperties
        public int ProblemId { get; set; }
        public virtual Problem Problem { get; set; }

        public virtual ICollection<Reaction> Reactions { get; set; }
        
        public virtual ICollection<Message> Messages { get; set; }
        #endregion
    }
}