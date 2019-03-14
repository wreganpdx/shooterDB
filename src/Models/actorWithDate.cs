using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication4.classes;

namespace WebApplication4.Models
{
    public class actorWithDate :actor
    {
        public DateTime mDate { get; set; }
    }
}