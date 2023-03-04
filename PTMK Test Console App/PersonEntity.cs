using AgeCalculator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTMK_Test_Console_App
{
    public class PersonEntity
    {
        public string FullName { get; set; }
        public string DateOfBirth { get; set; }
        public string Gender { get; set; }

        public PersonEntity(string fullName, string dateOfBirth, string gender)
        {
            FullName = fullName;
            DateOfBirth = dateOfBirth;
            Gender = gender;
        }

        public int GetAgeFromDateOfBirth()
        {
            DateTime date = DateTime.Parse(DateOfBirth);
            var age = new Age(date, DateTime.Today);
            return age.Years;
        }
    }
}
