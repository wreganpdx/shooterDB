using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using Npgsql;

namespace WebApplication4.Utils
{
    public class TableUtils
    {     
        // function that creates a list of an object from the given data table
        public static List<T> queryToTable<T>(string query) where T : new()
        {
            NpgsqlConnection connection = new NpgsqlConnection(RouteConfig.connectString);
            connection.Open();
            NpgsqlCommand command = new NpgsqlCommand(query, connection);
            NpgsqlDataReader dataReader = command.ExecuteReader();
            NpgsqlDataAdapter da = new NpgsqlDataAdapter();
            DataTable dt = new DataTable();
            for (int i = 0; dataReader.Read(); i++)
            {

            }
            dt.Load(dataReader);
            da.SelectCommand = command;
            da.Fill(dt);
            List<T> _list = TableUtils.CreateListFromTable<T>(dt);
            connection.Close();
            return _list;
        }

        public static T queryToObject<T>(string query) where T : new()
        {
            NpgsqlConnection connection = new NpgsqlConnection(RouteConfig.connectString);
            connection.Open();
            NpgsqlCommand command = new NpgsqlCommand(query, connection);
            NpgsqlDataReader dataReader = command.ExecuteReader();
            NpgsqlDataAdapter da = new NpgsqlDataAdapter();
            DataTable dt = new DataTable();
            for (int i = 0; dataReader.Read(); i++)
            {

            }
            dt.Load(dataReader);
            da.SelectCommand = command;
            da.Fill(dt);
            T obj = TableUtils.CreateObjectFromTable<T>(dt);
            connection.Close();
            return obj;
        }
        public static T queryToObject<T>(string query, addWithValue _val) where T : new()
        {
            NpgsqlConnection connection = new NpgsqlConnection(RouteConfig.connectString);
            connection.Open();
            NpgsqlCommand command = new NpgsqlCommand(query, connection);
            command.Parameters.AddWithValue(_val.VAR, _val.VAL);
            NpgsqlDataReader dataReader = command.ExecuteReader();
            NpgsqlDataAdapter da = new NpgsqlDataAdapter();
            DataTable dt = new DataTable();
            for (int i = 0; dataReader.Read(); i++)
            {

            }
            dt.Load(dataReader);
            da.SelectCommand = command;
            da.Fill(dt);
            T obj = TableUtils.CreateObjectFromTable<T>(dt);
            connection.Close();
            return obj;
        }
        public static T queryToObject<T>(string query, List<addWithValue> _vals) where T : new()
        {
            NpgsqlConnection connection = new NpgsqlConnection(RouteConfig.connectString);
            connection.Open();
            NpgsqlCommand command = new NpgsqlCommand(query, connection);
            for (int i = 0; i < _vals.Count;i++)
            { 
                command.Parameters.AddWithValue(_vals[i].VAR, _vals[i].VAL);
            }
            NpgsqlDataReader dataReader = command.ExecuteReader();
            NpgsqlDataAdapter da = new NpgsqlDataAdapter();
            DataTable dt = new DataTable();
            for (int i = 0; dataReader.Read(); i++)
            {

            }
            dt.Load(dataReader);
            da.SelectCommand = command;
            da.Fill(dt);
            T obj = TableUtils.CreateObjectFromTable<T>(dt);
            connection.Close();
            return obj;
        }
        // function that creates a list of an object from the given data table
        public static List<T> CreateListFromTable<T>(DataTable tbl) where T : new()
        {
            // define return list
            List<T> lst = new List<T>();

            // go through each row
            foreach (DataRow r in tbl.Rows)
            {
                // add to the list
                lst.Add(CreateItemFromRow<T>(r));
            }

            // return the list
            return lst;
        }

        public static T CreateObjectFromTable<T>(DataTable tbl) where T : new()
        {
            // define return list
            T item = new T();
            // go through each row
            foreach (DataRow r in tbl.Rows)
            {
                // add to the list
                SetItemFromRow(item, r);
                break;
            }

            // return the list
            return item;
        }

        // function that creates an object from the given data row
        public static T CreateItemFromRow<T>(DataRow row) where T : new()
        {
            // create a new object
            T item = new T();

            // set the item
            SetItemFromRow(item, row);

            // return 
            return item;
        }
        /*
        public static void SetItemFromRow<T>(T item, DataRow row) where T : new()
        {
            // go through each column
            foreach (DataColumn c in row.Table.Columns)
            {
                // find the property for the column
                PropertyInfo p = item.GetType().GetProperty(c.ColumnName, 
                BindingFlags.IgnoreCase |
                 BindingFlags.Public |
                 BindingFlags.Instance);

                // if exists, set the value
                
                if (p != null && row[c] != null)
                {
                    if (p != null && row[c] != DBNull.Value)
                    {
                        p.SetValue(item, row[c], null);
                    }
                }
            }
        }
        */

        public static void SetItemFromRow<T>(T item, DataRow row) where T : new()
        {
            // go through each column
            foreach (DataColumn c in row.Table.Columns)
            {
                // find the property for the column
                PropertyInfo p = item.GetType().GetProperty(c.ColumnName,
                BindingFlags.IgnoreCase |
                 BindingFlags.Public |
                 BindingFlags.Instance);

                // if exists, set the value

                if (p != null && row[c] != null)
                {
                    if (p != null && row[c] != DBNull.Value)
                    {
                        if (p.PropertyType.Name == "String")
                        {
                            p.SetValue(item, row[c], null);
                        }
                        else
                            p.SetValue(item, Parse(p.PropertyType, row[c].ToString()));
                    }
                }
            }
        }

        public static object Parse(Type type, string str)
        {
            try
            {
                var parse = type.GetMethod("Parse", new[] { typeof(string) });
                if (parse == null) throw new NotSupportedException();
                return parse.Invoke(null, new object[] { str });
            }
            //or don't catch
            catch (Exception)
            {
                return null;
            }
        }

        public static List<T> CreateListFromCSVPath<T>(string filePath, int numberOfColumns) where T : new()
        {
            DataTable tbl = createTableFromPath(filePath, numberOfColumns);
            return CreateListFromTable<T>(tbl);
        }
        public static List<T> populateDeathRowPopTXTState<T>(string filePath, string state) where T : new()
        {
            DataTable tbl = createTableFromPathDeathRow(filePath, state);
            return CreateListFromTable<T>(tbl);
        }
        private static DataTable createTableFromPathDeathRow(string filePath, string state)
        {
            DataTable tbl = new DataTable();
            string[] lines = System.IO.File.ReadAllLines(filePath);
            tbl.Columns.Add(new DataColumn("drState"));
            tbl.Columns.Add(new DataColumn("drId"));
            tbl.Columns.Add(new DataColumn("drName"));
            tbl.Columns.Add(new DataColumn("drRace"));

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                Regex txtParser = new Regex(@"\. | \(");
                string[] cols = txtParser.Split(line);
                if (cols.Length < 3) continue;
                DataRow dr = tbl.NewRow();
                dr.SetField(0, state);
                for (int cIndex = 0; cIndex < 3; cIndex++)
                {
                    dr.SetField(cIndex +1, cols[cIndex]);
                }
                tbl.Rows.Add(dr);
            }

            return tbl;
        }
        private static DataTable createTableFromPath(string filePath, int numberOfColumns)
        {
            DataTable tbl = new DataTable();
            


            string[] lines = System.IO.File.ReadAllLines(filePath);

            string line = lines[0];
            var cols = line.Split(',');
            for (int col = 0; col < numberOfColumns; col++)
            {
                tbl.Columns.Add(new DataColumn(cols[col]));
            }
           
            for (int i = 1; i < lines.Length;i++)
            {

                Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");

                //Separating columns to array
                string[] X = CSVParser.Split(lines[i]);
             //   var cols = line.Split(',');

                DataRow dr = tbl.NewRow();
                for (int cIndex = 0; cIndex < numberOfColumns; cIndex++)
                {
                    dr.SetField(cIndex, X[cIndex]);
                   // dr[cIndex] = X[cIndex];
                }

                tbl.Rows.Add(dr);
            }

            return tbl;
        }
    }
}