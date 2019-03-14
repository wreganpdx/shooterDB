using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication4.classes;

namespace WebApplication4.Models
{
    public class ShooterGP
    {
        public actor _actor { get; set; }
        public List<gunPurchase> _guns { get; set; }
    }
}