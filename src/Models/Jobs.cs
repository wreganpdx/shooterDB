using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication4.Models
{
    public class Jobs
    {
        public int numGunmenWrongBirthDeath { get; set; }
        public int numGunmenNoGuns { get; set; }
        public int numGunsNoType { get; set; }
        public int numGunsSoldNoSeller { get; set; }
        public int numGunmenNoIncidents { get; set; }
    }
}