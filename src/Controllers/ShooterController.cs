using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Npgsql;
using WebApplication4.Models;
using System.Data;
using WebApplication4.Utils;
using WebApplication4.classes;
using PagedList;

namespace WebApplication4.Controllers
{
    public class ShooterController : Controller
    {
        public ActionResult EditShooter(string first, string last)
        {
            ViewBag.First = first;
            ViewBag.Last = last;
            NpgsqlConnection connection = new NpgsqlConnection(RouteConfig.connectString);
            connection.Open();
            NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM shooters.shooter Where first = '" + first +"' AND last = '" + last  +"'", connection);
            NpgsqlDataReader dataReader = command.ExecuteReader();
            NpgsqlDataAdapter da = new NpgsqlDataAdapter();
            DataTable dt = new DataTable();
            for (int i = 0; dataReader.Read(); i++)
            {

            }
            dt.Load(dataReader);
            da.SelectCommand = command;
            da.Fill(dt);
            var model = TableUtils.CreateObjectFromTable<actor>(dt);
            connection.Close();
            return View(model);
        }

        public ViewResult ShooterWrongDates(int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            string query = "SELECT * from shooters.shooter natural join shooters.massshootingrel natural join shooters.massshootings " +
                "where (birth - death >= 0 OR birth is NULL or death is NULL) " +
                "AND (last NOT IN (select last from shooters.deathrowpoprel) AND first NOT IN (select first from shooters.deathrowpoprel)) " +
                "order by mdate asc";
            var _shooters = TableUtils.queryToTable<actorWithDate>(query);

            return View(_shooters.ToPagedList(pageNumber, pageSize));
        }

        public ViewResult ShooterNoIncidents(int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            string query = "select * from shooters.shooter where first NOT IN (select first from shooters.incitingincident) AND last NOT IN (select last from shooters.incitingincident)"; ;
            var _shooters = TableUtils.queryToTable<actor>(query);

            return View(_shooters.ToPagedList(pageNumber, pageSize));
        }



        [HttpPost]
        public ActionResult CommitRecord(criminalRecord cr)
        {
            String query = "Insert Into shooters.criminalRecord (First, Last, cDate, offense, offenseDescription, convicted, trial, punishmentDescription)" + "" +
                " values('" + cr.First + "','" + cr.Last + "','" + cr.cDate + "','" + cr.offense + "','" + cr.offenseDescription + "','" + cr.convicted + "','" + cr.trial + "','" + cr.punishmentDescription + "')";
            QueryUtils.query(query);
            return RedirectToAction("EditShooter", new { first = cr.First, last = cr.Last });
        }

        [HttpPost]
        public ActionResult CommitIncident(incitingIncident ii)
        {
            int uniqueKey = 0;

            List<incitingIncident> _list = TableUtils.queryToTable<incitingIncident>("SELECT * FROM shooters.IncitingIncident");
            Boolean uniqueI = false;
            while (uniqueI != true)
            {
                uniqueI = true;
                uniqueKey++;
                for (int k = 0; k < (_list.Count); k++)
                {
                    if (uniqueKey == _list[k].iKey)
                        uniqueI = false;
                }
            }

            ii.iKey = uniqueKey;

            NpgsqlConnection connection = new NpgsqlConnection(RouteConfig.connectString);
            connection.Open();
           
            NpgsqlCommand command = new NpgsqlCommand("Insert Into shooters.IncitingIncident (First, Last, iDate, iDescription, iKey) values('" + ii.first + "','" + ii.last + "','" + ii.iDate + "', @iDescription,'" + ii.iKey + "')", connection);
            command.Parameters.AddWithValue("@iDescription", ii.iDescription);
            int num = command.ExecuteNonQuery();
            connection.Close();
            if (num > 0)
                ViewBag.Message = "Query Succesful : " + num + " rows created";
            else
                ViewBag.Message = "Query Unsucessful: ";
            return RedirectToAction("EditShooter", new { first = ii.first, last = ii.last });
        }

        [HttpPost]
        public ActionResult CommitIncidentEdit(incitingIncident ii)
        {
            //  string connstring = "Server=" + RouteConfig.ip + "; Port=5432; User Id=postgres; Password=Pixies2019; Database=postgres;";
            NpgsqlConnection connection = new NpgsqlConnection(RouteConfig.connectString);
            connection.Open();
            NpgsqlCommand command = new NpgsqlCommand("Update shooters.incitingIncident set iDate = '" + ii.iDate + "', iDescription = @iDescription where iKey = '" + ii.iKey + "'", connection);
            command.Parameters.AddWithValue("@iDescription", ii.iDescription);
            int num = command.ExecuteNonQuery();
            connection.Close();
            if (num > 0)
                ViewBag.Message = "Query Succesful : " + num + " rows modified";
            else
                ViewBag.Message = "Query Unsucessful";
            return RedirectToAction("EditShooter", new { first = ii.first, last = ii.last });
        }


        [HttpPost]
        public ActionResult CommitShooter(actor a)
        {
            string query = "Update shooters.shooter set Middle = '" + a.Middle + "', birth = '" + a.birth + "', Death = '" + a.death + "' where First = '" + a.First + "' AND Last = '" + a.Last + "'";
            QueryUtils.query(query);
            return RedirectToAction("EditShooter", new { first = a.First, last = a.Last });
        }

        [HttpPost]
        public ActionResult CommitNewShooter(actor a)
        {
        string commitShooter = "select shooters.createShooter('" + a.First + "','" + a.Last + "')";
        int success = QueryUtils.queryForScalar(commitShooter);
        if (success == 0)
        {
            return CommitShooter(a);
        }
        else
        {
            return RedirectToAction("ShooterAlreadyExists", "Error");
        }
      //  string query = "insert into shooters.shooter (First, Last, Middle, birth, death) values('" + a.First + "','" + a.Last + "','" + a.Middle + "','" + a.birth + "','" + a.death + "')";
         /////   QueryUtils.query(query);
          //  return RedirectToAction("EditShooter", new { first = a.First, last = a.Last });
        }

        public ActionResult AddRecord(string first, string last)
        {
            ViewBag.First = first;
            ViewBag.Last = last;
            return View();
        }

        public ActionResult AddIncident(string first, string last)
        {
            ViewBag.First = first;
            ViewBag.Last = last;
            return View();
        }

        public ActionResult AddShooter()
        {
            actor model = new actor();
            return View(model);
        }


        public ActionResult Shooter(string first, string last)
        {
            string query = "SELECT * FROM shooters.shooter";
            List<actor> _actors = TableUtils.queryToTable<actor>(query);
            Shooters _shooters = new Shooters();
            _shooters.Actors = _actors;
            return View(_shooters);
        }

        public ActionResult ViewRecord(string first, string last)
        {
            
            string query = "SELECT * FROM shooters.criminalRecord where first= '" + first + "' AND last = '" + last + "'";

            List<criminalRecord> _list = TableUtils.queryToTable<criminalRecord>(query);
            CriminalRecords records = new CriminalRecords();
            records._records = _list;
            records.first = first;
            records.last = last;
            return View(records);
        }

        public ActionResult EditIncident(int iKey)
        {
            string query = "SELECT * FROM shooters.incitingIncident where iKey= '" + iKey + "'";
            var _model = TableUtils.queryToObject<incitingIncident>(query);
            return View(_model);
        }


        public ActionResult ViewIncident(string first, string last)
        {
            List<incitingIncident> list = TableUtils.queryToTable<incitingIncident>("SELECT * FROM shooters.incitingIncident where first= '" + first + "' AND last = '" + last + "' order by iDate");
            var model = new Incidents()
            {

            };
            model._incidents = list;
            return View(model);
        }
    }
}
 