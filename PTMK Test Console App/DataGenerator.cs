using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PTMK_Test_Console_App
{
    public class DataGenerator
    {
        private string GenerateRandomName(bool generateStartingOnF = false)
        {
            const int maxNameLength = 16;
            const int minNameLength = 5;
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            Random random = new Random();

            int nameLength = random.Next(minNameLength, maxNameLength);
            char[] name = new char[nameLength];

            for (int i = 0; i < nameLength; i++)
            {
                name[i] = chars[random.Next(chars.Length)];
            }
            if (generateStartingOnF)
            {
                name[0] = 'F';
            }
            return new string(name);
        }

        private Gender GenerateGender(bool generateMales = false)
        {
            if (generateMales)
            {
                return Gender.Male;
            }
            else
            {
                Random random = new Random();
                return (Gender)random.Next(0, 2);
            }
        }
        DateTime start = new DateTime(1940, 1, 1);
        int range;

        private string RandomDay()
        {
            Random random = new Random();
            return start.AddDays(random.Next(range)).ToString("yyyy-MM-dd");
        }

        public List<PersonEntity> GeneratePeople(int numberOfPeople, bool generateMalesStartingOnF = false)
        {
            Gender gender;
            range = (DateTime.Today - start).Days;
            List<PersonEntity> personEntities = new List<PersonEntity>();
            for (int i = 0; i < numberOfPeople; i++)
            {
                gender = GenerateGender();
                string fullName = GenerateRandomName(generateMalesStartingOnF);

                PersonEntity personEntity = new PersonEntity(fullName, RandomDay(), gender.ToString());
                personEntities.Add(personEntity);
            }

            Console.WriteLine("Finished generating people");
            return personEntities;
        }
    }

    public enum Gender
    {
        Male,
        Female
    }
}
