using System.ComponentModel.DataAnnotations.Schema;

namespace SaveMyHome.Models
{
    public enum ProblemStatus
    {
        None,
        Culprit,
        Victim,
        PotentialCulprit,
        PotentialVictim
    }

    public class Reaction
    {
        #region Properties
        public int Id { get; set; }
        public ProblemStatus ProblemStatus { get; set; }
        public bool Notifier { get; set; }
        public bool Reacted { get; set; }
        #endregion

        #region NavigationProperties
        public int ApartmentNumber { get; set; }
        [ForeignKey("ApartmentNumber")]
        public virtual Apartment Apartment { get; set; }

        public int EventId { get; set; }
        public virtual Event Event { get; set; }
        #endregion
    }
}