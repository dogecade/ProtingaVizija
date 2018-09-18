using System;
using System.Drawing;

namespace WindowsForms
{
    class MissingPerson
    {
        string name;
        string surname;
        string description;
        DateTime birthday;
        Image photo;


        public MissingPerson(string name, string surname, string description, DateTime birthday, Image photo)
        {
            this.name = name;
            this.surname = surname;
            this.description = description;
            this.birthday = birthday;
            this.photo = photo;
        }

    }
}
