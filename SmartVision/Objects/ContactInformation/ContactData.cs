namespace Objects.ContactInformation
{
    public class ContactData
    {
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }

        public ContactData(string phoneNumber, string emailAddress)
        {
            PhoneNumber = phoneNumber;
            EmailAddress = emailAddress;
        }
    }
}