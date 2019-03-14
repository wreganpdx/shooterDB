using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication4.classes;

namespace WebApplication4.Models
{
    public class GunsAndSellers
    {
        public List<gun> _guns { get; set; }
        public List<gunSeller> _sellers { get; set; }
        public List<gunType> _types { get; set; }
    }
}