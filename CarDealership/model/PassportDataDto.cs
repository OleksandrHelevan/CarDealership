using CarDealership.enums;

namespace CarDealership.model;

public class PassportDataDto
{
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PassportNumber { get; set; }


        public PassportDataDto(string firstName, string lastName, string passportNumber)
        {
                FirstName = firstName;
                LastName = lastName;
                PassportNumber = passportNumber;
        }
}