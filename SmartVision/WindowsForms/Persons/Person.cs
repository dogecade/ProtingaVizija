namespace WindowsForms.Persons
{
    class Person
    {
        protected string firstName;
        protected string lastName;

        /// <summary>
        /// Returns first name of the person
        /// </summary>
        /// <returns></returns>
        public string GetFirstName()
        {
            return firstName;
        }

        /// <summary>
        /// Returns last name of the person
        /// </summary>
        /// <returns></returns>
        public string GetLastName()
        {
            return lastName;
        }
    }
}
