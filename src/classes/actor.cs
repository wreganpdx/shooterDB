using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication4.classes
{
    public class actor
    {
        public string First { get; set; }
        public string Last { get; set; }
        public string Middle { get; set; }
        public DateTime birth { get; set; }
        public DateTime death { get; set; }
    }
}