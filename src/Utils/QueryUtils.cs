using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Reflection;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Npgsql;
using WebApplication4.Models;
using System.Data;
using WebApplication4.Utils;
using WebApplication4.classes;

namespace WebApplication4.Utils
{
    public class addWithValue
    {
        public addWithValue(String _var, object _val)
        {
              VAR = _var;
            VAL = _val;
        }
        public string VAR;
        public object VAL;
    }
    public class QueryUtils
    {        //public static string connectionString = ConfigurationManager.ConnectionStrings["YourWebConfigConnection"].ConnectionString;

        // function that creates a list of an object from the given data table
        public static int query(string query) 
        {
            NpgsqlConnection connection = new NpgsqlConnection(RouteConfig.connectString);
            connection.Open();
            NpgsqlCommand command = new NpgsqlCommand(query, connection);
            int num = command.ExecuteNonQuery();
            connection.Close();
            return num;
        }
        public static int query(string query, List<addWithValue> values)
        {
            NpgsqlConnection connection = new NpgsqlConnection(RouteConfig.connectString);
            connection.Open();
            NpgsqlCommand command = new NpgsqlCommand(query, connection);
            for (int i = 0; i < values.Count;i++)
            {
                command.Parameters.AddWithValue(values[i].VAR, values[i].VAL);
            }
            int num = command.ExecuteNonQuery();
            connection.Close();
            return num;
        }

        public static int queryForScalar(string query)
        {
            NpgsqlConnection connection = new NpgsqlConnection(RouteConfig.connectString);
            connection.Open();
            NpgsqlCommand command = new NpgsqlCommand(query, connection);
            int test= Convert.ToInt32(command.ExecuteScalar());
            connection.Close();
            return test;
        }


    }
}