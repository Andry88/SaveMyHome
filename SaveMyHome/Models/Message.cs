using System;
using System.ComponentModel.DataAnnotations;

namespace SaveMyHome.Models
{
    public class Message
    {
        #region Properties
        public int Id { get; set; }
        public string Text { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime Time { get; set; }
        public bool IsHead { get; set; }
        #endregion

        #region NavigationProperties
        public virtual ClientProfile ClientProfile { get; set; }

        public int EventId { get; set; }
        public virtual Event Event { get; set; }
        #endregion
    }
}