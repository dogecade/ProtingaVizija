using System.Collections.Generic;

namespace Objects.Person
{
    public class ContactPerson
    {
        public ContactPerson()
        {
            this.MissingPersons = new HashSet<MissingPerson>();
        }

        public int Id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string phoneNumber { get; set; }
        public string emailAddress { get; set; }
        public virtual ICollection<MissingPerson> MissingPersons { get; set; }
    }
}