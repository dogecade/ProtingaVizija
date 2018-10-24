using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WindowsForms.Models
{
    public class MissingContact
    {
        public MissingPerson missingPerson { get; set; }
        public ContactPerson contactPerson { get; set; }
    }
}