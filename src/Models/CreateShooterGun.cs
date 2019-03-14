using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication4.classes;

namespace WebApplication4.Models
{
    public class CreateShooterGun :gun
    {
        public string First { get; set; }
        public string Last { get; set; }
    }
}