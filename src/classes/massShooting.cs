using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication4.classes
{
    public class massShooting
    {
        public string first { get; set; }
        public string last { get; set; }
        public string middle { get; set; }
        public string mName { get; set;}
        public string mCity { get; set; }
        public string postal { get; set; }

        public DateTime mDate { get; set; }
        public string summary { get; set; }
        public int fatalities { get; set; }
        public int injured { get; set; }
        public int total_victims { get; set; }

        public string locDesc { get; set; }
        public int perpAge { get; set; }
        public string perpHealth { get; set; }
        public string perpHealthDesc { get; set; }
        public string weapons_optained_legally { get; set; }

        public string where_obtained { get; set; }
        public string weapon_type { get; set; }
        public string weapon_details { get; set; }
        public string race { get; set; }
        public string gender { get; set; }
        
        public float latitude { get; set; } 
        public float longitude { get; set; }
    }
}