using CMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS.MVC.Models
{
    public class CascadeCaseModel
    {
        public CascadeCaseModel()
        {
            this.IncidentType = "";
            this.IncidentDescription = "";
            this.Location = "";
            this.ServiceType = new List<SelectListItem>();
            this.Service = new List<SelectListItem>();
            this.EmailId = "";
        }
        
        public string IncidentType { get; set; }
        public string IncidentDescription { get; set; }
        public string Location { get; set; }
        public int ServiceId { get; set; }
        public List<SelectListItem> ServiceType { get; set; }
        public List<SelectListItem> Service { get; set; }
        public string EmailId { get; set; }

    }
}