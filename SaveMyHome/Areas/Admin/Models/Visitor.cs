using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Resources;
using System.Web.Mvc;

namespace SaveMyHome.Areas.Admin.Models
{
    [Table("VisitorsOfSite")]
    public class Visitor
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }
        [Display(Name = "Login", ResourceType = typeof(Account))]
        public string Login { get; set; }
        [Display(Name = "IP")]
        public string Ip { get; set; }
        [Display(Name = "AddressOfVisitedResource", ResourceType = typeof(Resources.Admin))]
        public string Url { get; set; }
        [Display(Name = "VisitDate", ResourceType = typeof(Resources.Admin))]
        public DateTime Date { get; set; }
    }
}