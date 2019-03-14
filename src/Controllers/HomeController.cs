using System.Collections.Generic;
using System.Web.Mvc;
using Npgsql;
using WebApplication4.Models;
using System.Data;
using WebApplication4.Utils;
using WebApplication4.classes;

namespace WebApplication4.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Search()
        {
            Search _search = new Search();
            return View(_search);
        }
        
        [HttpPost]
        public ActionResult CreateSearch(Search s)
        {
            string and = "";
            string query = "select * from shooters.shooter natural join shooters.massShootingrel natural join shooters.massshootings natural join shooters.stateAbr where ";
            string male = "gender = 'Male' OR gender = 'M'";
            string female = "gender = 'Female' OR gender = 'F'";
            string gOther = "gender = 'Male & Female'";
            string weapon_details = " weapon_details LIKE '%" + s.gunType + "%'";
            string firstName = " first LIKE '%" + s.first + "%'";
            string lastName = " last LIKE '%" + s.last + "%'";
            string state = " state LIKE '%" + s.state + "%'";
            string postal = " postal LIKE upper('%" + s.state + "%')";
            string perpage = " perpage = " + s.age;
            string summary = " upper(summary) LIKE upper('%" + s.summaryKeyWords + "%')";
            string deathRow = "select * from shooters.shooter natural join shooters.deathrowpoprel";
            string executed = "select * from shooters.shooter natural join shooters.executedRel";
            if (s.genderMale)
            {
                query += and + male;
                and = " AND ";
            }
            if (s.genderFemale)
            {
                query += and + female;
                and = " AND ";
            }
            if (s.genderOther)
            {
                query += and + gOther;
                and = " AND ";
            }
            if (s.gunType != null && s.gunType.Length > 1)
            {
                query += and + weapon_details;
                and = " AND ";
            }
            if (s.first != null && s.first.Length > 1)
            {
                query += and + firstName;
                and = " AND ";
            }
            if (s.last != null && s.last.Length > 1)
            {
                query += and + lastName;
                and = " AND ";
            }
           
            if (s.state != null && s.state.Length == 2)
            {
                query += and + postal;
                and = " AND ";
            }
            else if (s.state != null && s.state.Length > 2)
            {
                query += and + state;
                and = " AND ";
            }
            if (s.age != 0)
            {
                query += and + perpage;
                and = " AND ";
            }
            if (s.summaryKeyWords != null && s.summaryKeyWords.Length > 1)
            {
                query += and + summary;
                and = " AND ";
            }

            

            if (s.executed)
                return RedirectToAction("SearchResults", "Home", new { search = executed });
            else if (s.onDeathRow)
                return RedirectToAction("SearchResults", "Home", new { search = deathRow });

            if (and.Length < 1)
                return RedirectToAction("Search", "Home");
            return RedirectToAction("SearchResults", "Home", new { search = query });
        }

        public ActionResult SearchResults(string search)
        {
            List<actor> _actors = TableUtils.queryToTable<actor>(search);
            return View(_actors);
        }

        public ActionResult Jobs()
        {
            Jobs _jobs = new Jobs();
            //select count(gName) from shooters.gun where gname NOT in (select gName from shooters.gun natural join shooters.gunTypeRel)
            string query = "select count(gName) from shooters.gun where gname NOT in (select gName from shooters.gun natural join shooters.gunTypeRel)";
            _jobs.numGunsNoType = QueryUtils.queryForScalar(query);
            query = "select count(*) from shooters.shooter where first NOT IN (select first from shooters.gunPurchase) AND last NOT IN (select last from shooters.gunPurchase)";
            _jobs.numGunmenNoGuns = QueryUtils.queryForScalar(query);
            query = "select count(*) from shooters.shooter where first NOT IN (select first from shooters.incitingincident) AND last NOT IN (select last from shooters.incitingincident)";
            _jobs.numGunmenNoIncidents = QueryUtils.queryForScalar(query);
            query = "SELECT count (*) from shooters.shooter where birth - death >= 0 OR birth is NULL or death is NULL";
            int birthGreatDeath = QueryUtils.queryForScalar(query);
            _jobs.numGunmenWrongBirthDeath = QueryUtils.queryForScalar(query);
            query = "SELECT count(*) FROM shooters.gunpurchase where sName is NULL";
            _jobs.numGunsSoldNoSeller = QueryUtils.queryForScalar(query);
            return View(_jobs);
        }
            /*
             * select * from shooters.shooter, shooters.deathrowpop where drNAME LIKE '%' || Upper(first) || '%' AND drName LIKE '%' || Upper(last) || '%' 
              */

            //  select* from shooters.shooter, shooters.executed where Upper(ename) LIKE '%' || Upper(first) || '%' AND upper(ename) LIKE '%' || Upper(last) || '%'
        private void populateDeathRowPopTXTState()
        {
            string path = @"C:\Users\Will Regan\Desktop\PSU\databases\deathrowpop\US Military.txt";
            List<deathrow> _list = TableUtils.populateDeathRowPopTXTState<deathrow>(path, "US");
            for (int i = 0; i < _list.Count;i++)
            {
                string query = "insert into shooters.deathrowpop (drState, drId, drName, drRace) values (@drState, @drId, @drName, @DrRace)";
                List<addWithValue> _values = new List<addWithValue>();
                _values.Add(new addWithValue("@drState", _list[i].drState));
                _values.Add(new addWithValue("@drId", _list[i].drId));
                _values.Add(new addWithValue("@drName", _list[i].drName));
                _values.Add(new addWithValue("@drRace", _list[i].drRace));
                QueryUtils.query(query, _values);
            }
        }

        private void populateFromCSV()
        {
            /* old 
            string path = @"C:\Users\Will Regan\Desktop\massShootings.csv";
            List<massShooting> _list = TableUtils.CreateListFromCSVPath<massShooting>(path, 22);
            for (int i = 0; i < _list.Count; i++)
            {
                addWithValue _value = new addWithValue("@mName", _list[i].mName);
                massShooting ms = TableUtils.queryToObject<massShooting>("select * from shooters.massShootings where mName = @mName", _value);
                if (ms.total_victims > 0) continue;
                string query = "insert into shooters.massShootings (first,last,middle,mName,location,mDate,summary,fatalities,injured,total_victims,locDesc,perpAge,perpHealth,perpHealthDesc,weapons_obtained_legally,where_obtained,weapon_type,weapon_details,race,gender,latitude,longitude) values('" +
              _list[i].first + "',@last,@middle,@mName,'"
              + _list[i].location + "','" + _list[i].mDate + "',@summary," + _list[i].fatalities + "," + _list[i].injured + "," + _list[i].total_victims
              + ",'" + _list[i].locDesc + "'," + _list[i].perpAge + ",'" + _list[i].perpHealth + "',@perpHealthDesc"
              + ",'" + _list[i].weapons_optained_legally + "',@whereObtained,@weapon_type,@weapon_details"
               + ",'" + _list[i].race + "','" + _list[i].gender + "'," + _list[i].longitude + ","
              + _list[i].latitude + ")";
                List<addWithValue> _values = new List<addWithValue>();
                _values.Add(new addWithValue("@summary", _list[i].summary));
                _values.Add(new addWithValue("@perpHealthDesc", _list[i].perpHealthDesc));
                _values.Add(new addWithValue("@whereObtained", _list[i].where_obtained));
                _values.Add(new addWithValue("@last", _list[i].last));
                _values.Add(new addWithValue("@middle", _list[i].middle));
                _values.Add(new addWithValue("@weapon_type", _list[i].weapon_type));
                _values.Add(new addWithValue("@weapon_details", _list[i].weapon_details));
                _values.Add(new addWithValue("@mName", _list[i].mName));
                int num = QueryUtils.query(query, _values);
            }
            */
        }

        public static int npgsqlNonQuerySQL(string query)
        {
            NpgsqlConnection connection = new NpgsqlConnection(RouteConfig.connectString);
            connection.Open();
            NpgsqlCommand command = new NpgsqlCommand(query, connection);
            int num = command.ExecuteNonQuery();
            connection.Close();
            return num;
        }
        public ActionResult Shooter()
        {
            
            string connstring = RouteConfig.connectString;
            NpgsqlConnection connection = new NpgsqlConnection(connstring);
            connection.Open();
            NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM shooters.shooter", connection);
            NpgsqlDataReader dataReader = command.ExecuteReader();
            NpgsqlDataAdapter da = new NpgsqlDataAdapter();
            DataTable dt = new DataTable();
            //for (int i = 0; dataReader.Read(); i++){}
            while (dataReader.Read()) { }
            dt.Load(dataReader);
            da.SelectCommand = command;
            da.Fill(dt);
            List<actor> _list = TableUtils.CreateListFromTable<actor>(dt); 
            connection.Close();

            var model = new Shooters()
            {
                
            };
            model.Actors = _list;
            return View(model);
           // return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
 