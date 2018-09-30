namespace WindowsForms.Persons
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
