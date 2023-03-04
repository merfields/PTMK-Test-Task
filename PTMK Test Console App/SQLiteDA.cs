using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SQLite;
using Dapper;

namespace PTMK_Test_Console_App
{
    public class SQLiteDA
    {
        public static void CreatePeopleTable()
        {
            using (IDbConnection connection = new SQLiteConnection(GetConnectionString()))
            {
                try
                {
                    connection.Execute("CREATE TABLE IF NOT EXISTS Person( " +
                        "FullName TEXT(50)," +
                        "DateOfBirth TEXT(10) CHECK(date(DateOfBirth) IS NOT NULL)," +
                        "Gender TEXT(5) check (gender in ('Male','Female')))", new DynamicParameters());
                    connection.Execute("CREATE INDEX IF NOT EXISTS idx_FN_Gender ON Person (FullName,Gender)", new DynamicParameters());
                    Console.WriteLine("Table 'Person' was created");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        public static List<PersonEntity> GetPeopleWithAge()
        {
            using (IDbConnection connection = new SQLiteConnection(GetConnectionString()))
            {
                try
                {
                    var queryResult = connection.Query<PersonEntity>("Select DISTINCT FullName, DateOfBirth, Gender" +
                        " FROM Person GROUP BY FullName, DateOfBirth ORDER BY FullName", new DynamicParameters());
                    return queryResult.ToList();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Table was not yet created");
                    return new List<PersonEntity>();
                }


            }
        }

        public static void AddNewPerson(PersonEntity personToAdd, bool addingMultiplePeople = false)
        {
            using (IDbConnection connection = new SQLiteConnection(GetConnectionString()))
            {
                try
                {
                    connection.Execute("INSERT INTO Person( FullName, DateOfBirth, Gender) Values(@FullName, date(@DateOfBirth), @Gender)", personToAdd);
                    if (addingMultiplePeople == false)
                    {
                        Console.WriteLine("New person added");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Invalid data entered " + e.Message);
                    Console.WriteLine("Example: LebedevKA 1999-05-12 Male");
                }

            }
        }

        public static void AddMultiplePeople(List<PersonEntity> personEntities)
        {
            SQLiteConnection connection = new SQLiteConnection(GetConnectionString());
            connection.Open();
            SQLiteCommand command = connection.CreateCommand();

            SQLiteTransaction transaction = connection.BeginTransaction();

            command.CommandText = "INSERT OR IGNORE INTO Person( FullName, DateOfBirth, Gender) Values(@FullName, date(@DateOfBirth), @Gender)";

            command.Parameters.AddWithValue("@FullName", "");
            command.Parameters.AddWithValue("@DateOfBirth", "");
            command.Parameters.AddWithValue("@Gender", "");

            foreach (var item in personEntities)
            {
                InsertResultItem(item.FullName, item.DateOfBirth, item.Gender, command);
            }

            transaction.Commit();
            command.Dispose();
            connection.Close();
            connection.Dispose();
        }

        private static int InsertResultItem(string fullName, string dateOfBirth, string gender, SQLiteCommand command)
        {
            command.Parameters["@FullName"].Value = fullName;
            command.Parameters["@DateOfBirth"].Value = dateOfBirth;
            command.Parameters["@Gender"].Value = gender;
            return command.ExecuteNonQuery();
        }

        public static List<PersonEntity> FindMalesOnF()
        {
            using (IDbConnection connection = new SQLiteConnection(GetConnectionString()))
            {
                try
                {
                    var queryResult = connection.Query<PersonEntity>("SELECT * From Person" +
                        " WHERE FullName Like 'F%' AND Gender = 'Male'", new DynamicParameters());
                    return queryResult.ToList();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Table was not yet created");
                    return new List<PersonEntity>();
                }

            }
        }

        private static string GetConnectionString(string connectionStringName = "DefaultName")
        {
            return ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
        }
    }
}
