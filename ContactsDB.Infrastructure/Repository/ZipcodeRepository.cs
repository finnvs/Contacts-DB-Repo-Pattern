using ContactsDB.Domain.Models;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace ContactsDB.Infrastructure.Repository
{
    public class ZipcodeRepository : GenericRepository<Zipcode>, IEnumerable<Zipcode>
    {

        public ZipcodeRepository(ApplicationContext context) : base(context)
        {

        }

        private List<Zipcode> list = new List<Zipcode>();

        public IEnumerator<Zipcode> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Search(string code, string city)
        {
            // Metoden bruges ikke, er udkommenteret som et eksempel på ADO.Net SQL kald som set i modsætning til et EF Find()
            //try
            //{
            //    // Oprindelig kode
            //    //SqlCommand command = new SqlCommand("SELECT * FROM Zipcodes WHERE Code LIKE @Code AND City LIKE @City", connection);
            //    //command.Parameters.Add(CreateParam("@Code", code + "%", SqlDbType.NVarChar));
            //    //command.Parameters.Add(CreateParam("@City", city + "%", SqlDbType.NVarChar));
            //    //connection.Open();
            //    //SqlDataReader reader = command.ExecuteReader();
            //    //list.Clear();
            //    //while (reader.Read()) list.Add(new Zipcode(reader[0].ToString(), reader[1].ToString()));
            //    //OnChanged(DbOperation.SELECT, DbModeltype.Zipcodes);

            //    // Repo Pattern kode
            //    // list.Clear();
            //    // list = 
            //}
            //catch (Exception ex)
            //{
            //    throw new DbException("Error in Zipcode repositiory: " + ex.Message);
            //}
            //finally
            //{
            //    if (connection != null && connection.State == ConnectionState.Open) connection.Close();
            //}
        }

        public void Add(string code, string city)
        {    
            // Add new entity to DB by calling INSERT on base (generic repo) class
            Zipcode entityToInsert = new Zipcode(code, city);
            base.Insert(entityToInsert);
            base.SaveChanges();
        }

        public void Update(string code, string city)
        {
            // Update entity from DB by calling UPDATE on base (generic repo) class
            Zipcode entityToUpdate = ReturnZipCode(code);
            base.Update(entityToUpdate);
            base.SaveChanges();
        }

        public void Remove(string code)
        {
           // Remove entity from DB by calling DELETE on base (generic repo) class
            Zipcode entityToDelete = ReturnZipCode(code);
            base.Delete(entityToDelete);
            base.SaveChanges();

        }

        public Zipcode ReturnZipCode(string code)
        {
            return context.Find<Zipcode>(code);
        }

        // SQL kald 'GetCity' fra oprindelig kodebase, til dels modsvarende ReturnZipCode ovenfor
        public static string GetCity(string code)
        {
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(ConfigurationManager.ConnectionStrings["post"].ConnectionString);
                SqlCommand command = new SqlCommand("SELECT City FROM Zipcodes WHERE Code = @Code", connection);
                SqlParameter param = new SqlParameter("@Code", SqlDbType.NVarChar);
                param.Value = code;
                command.Parameters.Add(param);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read()) return reader[0].ToString();
            }
            catch
            {
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open) connection.Close();
            }
            return "";
        }

    }
}
