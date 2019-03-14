using System.Web.Mvc;
using WebApplication4.Utils;
using WebApplication4.classes;
using PagedList;
namespace WebApplication4.Controllers
{
    public class DataController :Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ViewResult MassShootings(int? page)
        {
            int pageSize = 5;
            int pageNumber = (page ?? 1);

            string query = "select * from shooters.massshootings order by mDate";
            var _massShootings = TableUtils.queryToTable<massShooting>(query);

            return View(_massShootings.ToPagedList(pageNumber, pageSize));
        }

        public ViewResult Executions(int? page)
        {
            int pageSize = 6;
            int pageNumber = (page ?? 1);

            string query = "select * from shooters.executed order by eDate";
            var _executed = TableUtils.queryToTable<execution>(query);

            return View(_executed.ToPagedList(pageNumber, pageSize));
        }

        public ViewResult DeathRow(int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            string query = "select * from shooters.deathrowpop order by drState";
            var _executed = TableUtils.queryToTable<deathrow>(query);

            return View(_executed.ToPagedList(pageNumber, pageSize));
        }
    }
}