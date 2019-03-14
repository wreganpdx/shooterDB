using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication4.classes;

namespace WebApplication4.Models
{
    public class GunSpec
    {
        public gun _gun { get; set; }
        public List<gunType> _specs { get; set; }
    }
}