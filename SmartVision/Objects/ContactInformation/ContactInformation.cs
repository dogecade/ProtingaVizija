using System.Collections.Generic;

namespace Objects.ContactInformation
{
    public class ContactInformation
    {
        public string MissingPersonFirstName { get; set; }
        public string MissingPersonLastName { get; set; }
        public List<ContactData> ContactsData { get; set; }
        public ContactInformation(string missingPersonFirstName, string missingPersonLastName, List<ContactData> contactsData)
        {
            MissingPersonFirstName = missingPersonFirstName;
            MissingPersonLastName = missingPersonLastName;
            ContactsData = contactsData;
        }
    }
}