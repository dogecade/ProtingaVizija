using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsForms
{
    class ContactPerson : Person
    {
        private string phoneNumber;
        private string emailAddress;

        public ContactPerson(string firstName, string lastName, string phoneNumber, string emailAddress)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.phoneNumber = phoneNumber;
            this.emailAddress = emailAddress;
        }
    }
}
