using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SaveMyHome.Areas.Admin.Models
{
    [Table("VisitorsOfSite")]
    public class Visitor
    {
        public int Id { get; set; }
        [Display(Name = "Логин")]
        public string Login { get; set; }
        [Display(Name = "IP")]
        public string Ip { get; set; }
        [Display(Name = "Адрес посещенного ресурса")]
        public string Url { get; set; }
        [Display(Name = "Дата посещения")]
        public DateTime Date { get; set; }
    }
}