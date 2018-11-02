using System.Collections.Generic;

namespace Objects.Person
{
    public class MissingPerson
    {
        public MissingPerson()
        {
            this.ContactPersons = new HashSet<ContactPerson>();
        }
    
        public int Id { get; set; }
        public string faceToken { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string lastSeenDate { get; set; }
        public string lastSeenLocation { get; set; }
        public string Additional_Information { get; set; }
        public string dateOfBirth { get; set; }
        public string faceImg { get; set; }
    
        public virtual ICollection<ContactPerson> ContactPersons { get; set; }
    }
}