using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication4.Models
{
    public class Search
    {
        public string location { get; set; }
        public Boolean genderMale { get; set; }
        public Boolean genderFemale { get; set; }
        public Boolean genderOther { get; set; }
        public int age { get; set; }
        public string first { get; set; }
        public string last { get; set; }
        public string summaryKeyWords { get; set; }
        public string gunType { get; set; }
        public string state { get; set; }
        public Boolean onDeathRow { get; set; }
        public Boolean executed { get; set; }
    }
}