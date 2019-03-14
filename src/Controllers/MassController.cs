using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication4.classes;
using WebApplication4.Utils;

namespace WebApplication4.Controllers
{
    public class MassController : Controller
    {
        public ActionResult ViewSummary(string mName)
        {
            string query = "select * from shooters.massshootings where mName = '" + mName + "'";
            massShooting ms = TableUtils.queryToObject<massShooting>(query);
            return View(ms);
        }

        public ActionResult ViewGunDetails(string mName)
        {
            string query = "select * from shooters.massshootings where mName = '" + mName + "'";
            massShooting ms = TableUtils.queryToObject<massShooting>(query);
            return View(ms);
        }

        public ActionResult ViewHealthDetails(string mName)
        {
            string query = "select * from shooters.massshootings where mName = '" + mName + "'";
            massShooting ms = TableUtils.queryToObject<massShooting>(query);
            return View(ms);
        }
    }
}