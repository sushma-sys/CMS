using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS.MVC.Models
{
    public class CaseViewModel
    {
        public int CaseId { get; set; }
        public string IncidentType { get; set; }
        public string IncidentDescription { get; set; }
        public string Location { get; set; }
        public string ServiceType { get; set; }
        public string Service { get; set; }
    }
}