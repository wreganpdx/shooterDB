using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication4.classes;

namespace WebApplication4.Models
{
    public class GunTypeAdder
    {
        public gun _gun { get; set; }
        public List<gunType> _newTypes { get; set; }
        public List<gunType> _currTypes { get; set; }
    }
}