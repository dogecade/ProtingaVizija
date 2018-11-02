namespace Objects.ContactInformation
{
    public class ContactInformation
    {
        public string missingPersonFirstName { get; set; }
        public string missingPersonLastName { get; set; }
        public string contactPersonPhoneNumber { get; set; }
        public string contactPersonEmailAddress { get; set; }

        public ContactInformation(string missingPersonFirstName, string missingPersonLastName, string contactPersonPhoneNumber, string contactPersonEmailAddress)
        {
            this.missingPersonFirstName = missingPersonFirstName;
            this.missingPersonLastName = missingPersonLastName;
            this.contactPersonPhoneNumber = contactPersonPhoneNumber;
            this.contactPersonEmailAddress = contactPersonEmailAddress;
        }
    }
}