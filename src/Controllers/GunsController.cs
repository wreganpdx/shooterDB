using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Npgsql;
using WebApplication4.Models;
using WebApplication4.Utils;
using WebApplication4.classes;
using PagedList;

namespace WebApplication4.Controllers
{
    public class GunsController : Controller
    {
        private void setSuccess(int num)
        {
            if (num > 0)
                ViewBag.Message = "Query Succesful : " + num + " rows modified";
            else
                ViewBag.Message = "Query Unsucessful";
        }


        public ViewResult GunsNoType(int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            string query = "select * from shooters.gun where gname NOT in (select gName from shooters.gun natural join shooters.gunTypeRel)";
            var _guns = TableUtils.queryToTable<gun>(query);
            return View(_guns.ToPagedList(pageNumber, pageSize));
        }


        public ActionResult ShooterCommitGun(string first, string last, string gunName)
        {
            int uniqueKey = 0;

            List<gunPurchase> _list = TableUtils.queryToTable<gunPurchase>("SELECT * FROM shooters.gunPurchase");
            Boolean uniqueI = false;
            while(uniqueI != true)
            {
                uniqueI = true;
                uniqueKey++;
                for (int k = 0; k < (_list.Count); k++)
                {
                    if (uniqueKey == _list[k].gpId)
                        uniqueI = false;
                }
            }
            string query = "insert into shooters.gunPurchase (first, last, gpId, gName) values ('" + first + "','" + last + "','" + uniqueKey + "','" + gunName + "')";
            setSuccess(QueryUtils.query(query));
            return RedirectToAction("ViewShooterGuns", "Guns", new { first = first, last = last });
        }

        public ActionResult EditGun(string gName)
        {
            gun _gun = TableUtils.queryToObject<gun>("SELECT * FROM shooters.gun where gName= '" + gName + "'");
            GunSpec gs = new GunSpec();
            gs._gun = _gun;
            List<gunType> _types = TableUtils.queryToTable<gunType>("SELECT * FROM shooters.gunTypeRel natural join shooters.gunType where gName= '" + gName + "'");
            gs._specs = _types;
            ViewBag.gName = gName;
            return View(gs);
        }
        [HttpPost]
        public ActionResult CommitGunEdit(gun g)
        {
            string query = "update shooters.gun" + " set gDesc = '" + g.gDesc + "'" + " where gName = '" + g.gName + "'";

            setSuccess(QueryUtils.query(query));
            return RedirectToAction("EditGun", "Guns", new { gName = g.gName });
        }
        public ActionResult EditSeller(string sName)
        {
            var model = TableUtils.queryToObject<gunSeller>("SELECT * FROM shooters.gunSeller where sName= '" + sName + "'");
            return View(model);
        }
        [HttpPost]
        public ActionResult CommitSellerEdit(gunSeller gs)
        {
            string query = "update shooters.gunSeller" +
                                                      " set sAddress = '" + gs.sAddress + "'," +
                                                      "  sCity = '" + gs.sCity + "'," +
                                                      "  sCountry = '" + gs.sCountry + "'," +
                                                      "  sState = '" + gs.sState + "'," +
                                                      "  sZip = '" + gs.sZip + "'" +
                                                      " where sName = '" + gs.sName + "'";
            
            setSuccess(QueryUtils.query(query));
            return RedirectToAction("Guns");
        }



        public ActionResult DeleteShooterGun(string first, string last, int gpId)
        {
            QueryUtils.query("DELETE FROM shooters.gunPurchase where gpId = '" + gpId + "'");
            return RedirectToAction("ViewShooterGuns", "Guns", new { first = first, last = last });
        }

        public ActionResult ViewShooterGuns(string first, string last)
        {
            actor _actor = TableUtils.queryToObject<actor>("SELECT * FROM shooters.shooter where first= '" + first + "' AND last = '" + last + "'");
            List<gunPurchase> _list = TableUtils.queryToTable<gunPurchase>("SELECT * FROM shooters.gunPurchase where first= '" + first + "' AND last = '" + last + "'");

            ShooterGP model = new ShooterGP();
            model._actor = _actor;
            model._guns = _list;
            return View(model);
        }
        [HttpPost]
        public ActionResult CommitGPChanges(gunPurchase g)
        {
            gunSeller _obj = TableUtils.queryToObject<gunSeller>("select * from shooters.gunSeller where sName = '" + g.sName + "'");
            if (_obj.sName == null)
            {
                //if seller does not exist in sellers, create it.
                QueryUtils.query("insert into shooters.gunSeller (sName) values ('" + g.sName + "')");
            }

            string query = "update shooters.gunPurchase" +
                                                      " set sName = '" + g.sName + "'," +
                                                      "  gpDate = '" + g.gpDate + "'," +
                                                      "  gpCost = '" + g.gpCost + "'," +
                                                      "  gName = '" + g.gName + "'," +
                                                      "  gpDesc = '" + g.gpDesc + "'" +
                                                      " where gpId = '" + g.gpId + "'";
            setSuccess(QueryUtils.query(query));
            return RedirectToAction("Guns");
        }

        [HttpPost]
        public ActionResult CommitGunAddToShooter(CreateShooterGun g)
        {
            CommitGun(g); 
            return ShooterCommitGun(g.First,g.Last,g.gName);
        }

        [HttpPost]
        public ActionResult CommitGun(gun g)
        {
            NpgsqlConnection connection = new NpgsqlConnection(RouteConfig.connectString);
            connection.Open();
            NpgsqlCommand command = new NpgsqlCommand("insert into shooters.gun (gDesc, gName) values (@gDesc,'" + g.gName + "')", connection);
            command.Parameters.AddWithValue("@gDesc", g.gDesc);
            int num = command.ExecuteNonQuery();
            connection.Close();
            return RedirectToAction("Guns");
        }

        public ActionResult CreateAndAddGun(string first, string last)
        {
            CreateShooterGun _gun = new CreateShooterGun();
            _gun.First = first;
            _gun.Last = last;
            return View(_gun);
        }

        public ActionResult AddGun()
        {
            return View();
        }

        public ActionResult EditGunPurchase(int gpId = 9)
        {
            string query = "select * from shooters.gunPurchase where gpid = " + gpId;
            gunPurchase gp = TableUtils.queryToObject<gunPurchase>(query);
            return View(gp);
        }

        public ViewResult ShooterNoGuns(int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            string query = "select * from shooters.shooter where first NOT IN (select first from shooters.gunPurchase) AND last NOT IN (select last from shooters.gunPurchase)"; ;
            var _shooters = TableUtils.queryToTable<actor>(query);
           // return View(_shooters);
            return View(_shooters.ToPagedList(pageNumber, pageSize));
        }

        public ViewResult GunPurchaseNoSeller(int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            string query = "SELECT * FROM shooters.gunpurchase where sName is NULL";
            var _gp = TableUtils.queryToTable<gunPurchase>(query);
            // return View(_shooters);
            return View(_gp.ToPagedList(pageNumber, pageSize));
        }

        [HttpPost]
        public ActionResult AddGunsToShooter(ShooterGuns _shooterGuns)
        {
            int pageSize = 10;
            int pageNumber = 1;
            PagedList<gun> _list = new PagedList<gun>(TableUtils.queryToTable<gun>("SELECT * FROM shooters.gun where gDesc LIKE '%" + _shooterGuns._search + "%'"), pageNumber, pageSize);
            List<gunsOwned> _owned = TableUtils.queryToTable<gunsOwned>("SELECT gName, gDesc, count(gName) as owned FROM shooters.gun natural join shooters.gunPurchase where first = '" + _shooterGuns._actorFirst + "' AND last = '" + _shooterGuns._actorLast + "' group by gName, gDesc");
            actor _actor = TableUtils.queryToObject<actor>("SELECT * FROM shooters.shooter where first= '" + _shooterGuns._actorFirst + "' AND last = '" + _shooterGuns._actorLast + "'");
            massShooting ms = TableUtils.queryToObject<massShooting>("select * from shooters.massshootings natural join shooters.massshootingrel where first = '" + _shooterGuns._actorFirst + "' AND last = '" + _shooterGuns._actorLast + "'");
            _shooterGuns._massShooting = ms;
            _shooterGuns._guns = _list;
            _shooterGuns._gunsOwned = _owned;
            _shooterGuns._actor = _actor; 
            return View(_shooterGuns);
        }

        public ActionResult AddGunsToShooter(string first, string last, int? page)
        {
            int pageSize = 10;
            int pageNumber = page ?? 1;
            massShooting ms = TableUtils.queryToObject<massShooting>("select * from shooters.massshootings natural join shooters.massshootingrel where first = '" + first + "' AND last = '" + last + "'");
            PagedList<gun> _list = new PagedList<gun>(TableUtils.queryToTable<gun>("SELECT * FROM shooters.gun"), pageNumber, pageSize);
            List<gunsOwned> _owned = TableUtils.queryToTable<gunsOwned>("SELECT gName, gDesc, count(gName) as owned FROM shooters.gun natural join shooters.gunPurchase where first = '" + first + "' AND last = '" + last + "' group by gName, gDesc");
            actor _actor = TableUtils.queryToObject<actor>("SELECT * FROM shooters.shooter where first= '" + first + "' AND last = '" + last + "'");
            ShooterGuns model = new ShooterGuns();
            model._actorFirst = first;
            model._actorLast = last;
            model._massShooting = ms;
            model._guns = _list;
            model._gunsOwned = _owned;
            model._actor = _actor;
            return View(model);
        }
        

        public ActionResult Guns(string first, string last)
        {
            List<gun> _list = TableUtils.queryToTable<gun>("SELECT * FROM shooters.Gun");
            Guns model = new Guns();
            model._guns = _list;
            return View(model);
           // return View();
        }
        [HttpPost]
        public ActionResult CommitNewSpec(NewSpec gt)
        {
            int uniqueKey = 0;

            List<gunType> _list = TableUtils.queryToTable<gunType>("SELECT * FROM shooters.gunType");
            Boolean uniqueI = false;
            while (uniqueI != true)
            {
                uniqueI = true;
                uniqueKey++;
                for (int k = 0; k < (_list.Count); k++)
                {
                    if (uniqueKey == _list[k].tId)
                        uniqueI = false;
                }
            }

            setSuccess(QueryUtils.query("insert into shooters.gunType (tName, tDesc, tId) values ('" + gt.tName + "','" + gt.tDesc + "','" + uniqueKey + "')"));
            setSuccess(QueryUtils.query("insert into shooters.gunTypeRel (gName, tId) values ('" + gt.gName + "','" + uniqueKey + "')"));
            return RedirectToAction("EditGun", "Guns", new { gName = gt.gName });
        }

        public ActionResult GunTypeAdd(string gName)
        {
            List<gunType> _curTypes = TableUtils.queryToTable<gunType>("SELECT * FROM shooters.gunType natural join shooters.gunTypeRel where gName = '" + gName + "'");
            List<gunType> _newTypes = TableUtils.queryToTable<gunType>("SELECT * FROM shooters.gunType");
            gun _gun = TableUtils.queryToObject<gun>("SELECT * FROM shooters.gun where gName = '" + gName + "'");
            GunTypeAdder _tAdder = new GunTypeAdder();
            _tAdder._currTypes = _curTypes;
            _tAdder._newTypes = _newTypes;
            _tAdder._gun = _gun;
            return View(_tAdder);
        }

        public ActionResult CreateGunSpec(string gName)
        {
            ViewBag.gName = gName;
            NewSpec model = new NewSpec();
            model.gName = gName;
            return View(model);
        }

        public ActionResult EditType(int tId)
        {
            gunType model = TableUtils.queryToObject<gunType>("SELECT * FROM shooters.gunType where tId ='" + tId + "'");
            return View(model);
        }

        public ActionResult RemoveType(string gName, int tId)
        {
            setSuccess(QueryUtils.query("Delete from shooters.gunTypeRel where gName ='" + gName + "' AND tId ='" + tId + "'"));
            return RedirectToAction("EditGun", "Guns", new { gName = gName });
        }

        public ActionResult AddType(string gName, int tId)
        {
            setSuccess(QueryUtils.query("insert into shooters.gunTypeRel (gName, tId) values ('" + gName + "' , '" + tId + "')"));
            return RedirectToAction("EditGun", "Guns", new { gName = gName });
        }

        public ActionResult GunsAndSellers()
        {
            List<gun> _list = TableUtils.queryToTable<gun>("SELECT * FROM shooters.gun");
            List<gunSeller> _sellerList = TableUtils.queryToTable<gunSeller>("SELECT sName, count(sName) as sSold FROM shooters.gunSeller natural join shooters.gunPurchase group by sName");
            List<gunType> _gunTypes = TableUtils.queryToTable<gunType>("SELECT tId, tName, count(tName) as tNum FROM shooters.gunType natural join shooters.gunTypeRel group by tId, tName");
            GunsAndSellers model = new GunsAndSellers();
            model._guns = _list;
            model._sellers = _sellerList;
            model._types = _gunTypes;
            return View(model);
        }

        private void cleanUnusedTypes()
        {
            QueryUtils.query("DELETE FROM shooters.guntype where tId<> ALL(select tId from shooters.gunTypeRel");
        }
    }
}
 