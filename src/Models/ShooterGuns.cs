using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication4.classes;
using PagedList;
using System.Web.Mvc;

namespace WebApplication4.Models
{
    public class ShooterGuns
    {
        public massShooting _massShooting { get; set; }
        public actor _actor { get; set; }
        [HiddenInput]
        public string _actorFirst { get; set; }
        [HiddenInput]
        public string _actorLast { get; set; }
        public PagedList<gun> _guns { get; set; }
        public List<gunsOwned> _gunsOwned { get; set; }
        public string _search { get; set; }

    }
}