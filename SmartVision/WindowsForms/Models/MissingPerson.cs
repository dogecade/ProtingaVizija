using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsForms.Models
{
    public class MissingPerson
    {
        public int Id { get; set; }
        public string faceToken { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string lastSeenDate { get; set; }
        public string lastSeenLocation { get; set; }
        public string Additional_Information { get; set; }
        public string dateOfBirth { get; set; }
        public string faceImg { get; set; }
    }
}
