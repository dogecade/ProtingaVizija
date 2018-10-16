using System;
using System.Drawing;

namespace FaceAnalysis.Persons
{
    public class MissingPerson : Person
    {
        private string description;
        private string lastSeenLocation;
        private DateTime dateOfBirth;
        private DateTime lastSeenDate;
        private Bitmap missingPersonImage;

        public MissingPerson(string firstName, string lastName, string description, string lastSeenLocation, 
            DateTime dateOfBirth, DateTime lastSeenDate, Bitmap missingPersonImage)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.description = description;
            this.lastSeenLocation = lastSeenLocation;
            this.dateOfBirth = dateOfBirth;
            this.lastSeenDate = lastSeenDate;
            this.missingPersonImage = missingPersonImage;
        }

        public string GetDescription()
        {
            return description;
        }
        public string GetLastSeenLocation()
        {
            return lastSeenLocation;
        }
        public string GetDateOfBirth()
        {
            return dateOfBirth.ToString();
        }
        public string GetLastSeenDate()
        {
            return lastSeenDate.ToString();
        }
    }
}
