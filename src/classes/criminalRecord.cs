using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication4.classes
{
    public class criminalRecord
    {
        public string First { get; set; }//foreign key
        public string Last { get; set; }//foreign key
        public DateTime cDate { get; set; }
        public string offense { get; set; }
        public string offenseDescription { get; set; }
        public string punishmentDescription { get; set; }
        public Boolean convicted { get; set; }
        public Boolean trial { get; set; }
    }
}