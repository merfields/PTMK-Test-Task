using ConsoleTables;
using PTMK_Test_Console_App;
using System;

namespace PTMKTestConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            switch (args[0])
            {
                case "1":
                    {
                        SQLiteDA.CreatePeopleTable();
                        break;
                    }
                case "2":
                    {
                        PersonEntity personToAdd = new PersonEntity(args[1], args[2], args[3]);
                        SQLiteDA.AddNewPerson(personToAdd);
                        break;
                    }

                case "3":
                    {
                        List<PersonEntity> people = SQLiteDA.GetPeopleWithAge();
                        DisplayQueryResult(people);
                        break;
                    }

                case "4":
                    {
                        try
                        {
                            DataGenerator dataGenerator = new DataGenerator();
                            List<PersonEntity> generatedPeople = dataGenerator.GeneratePeople(1000000);

                            Console.WriteLine("Adding people...");

                            SQLiteDA.AddMultiplePeople(generatedPeople);
                            Console.WriteLine("Finished adding people");

                            generatedPeople = dataGenerator.GeneratePeople(100, true);
                            Console.WriteLine("Generating additional 100 males on F");

                            Console.WriteLine("Adding people...");

                            SQLiteDA.AddMultiplePeople(generatedPeople);

                            Console.WriteLine("Finished adding people");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Table was not yet created");
                        }
                        break;
                    }

                case "5":
                    {
                        var watch = new System.Diagnostics.Stopwatch();
                        watch.Start();
                        List<PersonEntity> people = SQLiteDA.FindMalesOnF();
                        watch.Stop();

                        DisplayQueryResult(people);

                        Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");
                        break;
                    }

                default:
                    {
                        Console.WriteLine("Incorrect argument");
                        break;
                    }
            }
        }

        private static void DisplayQueryResult(List<PersonEntity> queryResult)
        {
            var table = new ConsoleTable("FullName", "Date of Birth", "Gender", "Age");
            foreach (PersonEntity person in queryResult)
            {
                table.AddRow(person.FullName, person.DateOfBirth, person.Gender, (person.GetAgeFromDateOfBirth()).ToString());
            }
            table.Write();
        }
    }
}