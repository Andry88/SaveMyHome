using System;
using System.ComponentModel.DataAnnotations;

namespace SaveMyHome.Models
{
    public class Message
    {
        #region Properties
        public int Id { get; set; }

        [Display(Name = "Текст сообщения:"), Required(ErrorMessage = "Напишите сообщение")]
        public string Text { get; set; }

        [Display(Name = "Время отправки сообщения: "), DataType(DataType.DateTime)]
        public DateTime Time { get; set; }

        public bool IsHead { get; set; }
        #endregion

        #region NavigationProperties
        public virtual ApplicationUser User { get; set; }

        public int EventId { get; set; }
        public virtual Event Event { get; set; }
        #endregion
    }
}