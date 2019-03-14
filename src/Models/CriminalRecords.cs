using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication4.classes;

namespace WebApplication4.Models
{
    public class CriminalRecords
    {
        public string first { get; set; }
        public string last { get; set; }
        public List<criminalRecord> _records;
    }
}