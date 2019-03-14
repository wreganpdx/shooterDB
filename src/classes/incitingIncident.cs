using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication4.classes
{
    public class incitingIncident
    {
        public string first { get; set; }//foreign key
        public string last { get; set; }
        public int iKey { get; set; }
        public DateTime iDate { get; set; }
        public String iDescription { get; set; }
    }
}