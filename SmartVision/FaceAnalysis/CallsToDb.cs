using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceAnalysis
{
    public class CallsToDb
    {
        private const string getMisingPersonsUrl = "http://viltomas.eu/api/MissingPersons";


        /// <summary>
        /// Grabs missing person data from DB.
        /// </summary>
        /// <param name="matchedFace">ID of face to grab</param>
        /// <returns>Contact information</returns>
        public NecessaryContactInformation GetMissingPersonData(string matchedFace)
        {
            HttpClientWrapper wrapper = new HttpClientWrapper();
            var missingPersonsJson = wrapper.Get(getMisingPersonsUrl).Result;
            var missingPersonsList = JsonConvert.DeserializeObject<List<Api.Models.MissingPerson>>(missingPersonsJson);
            var foundMissingPerson = missingPersonsList.First(x => x.faceToken == matchedFace);

            string missingPersonFirstName = foundMissingPerson.firstName;
            string missingPersonLastName = foundMissingPerson.lastName;

            var contactPerson = foundMissingPerson.ContactPersons.FirstOrDefault();
            string contactPersonPhoneNumber = contactPerson.phoneNumber;
            string contactPersonEmailAddress = contactPerson.emailAddress;

            return new NecessaryContactInformation(missingPersonFirstName, missingPersonLastName,
                contactPersonPhoneNumber, contactPersonEmailAddress);
        }

        /// <summary>
        /// Grabs missing person data from DB.
        /// </summary>
        /// <returns>A list of missing people</returns>
        public List<Api.Models.MissingPerson> GetPeopleData()
        {
            HttpClientWrapper wrapper = new HttpClientWrapper();
            var missingPersonsJson = wrapper.Get(getMisingPersonsUrl).Result;
            var missingPersonsList = JsonConvert.DeserializeObject<List<Api.Models.MissingPerson>>(missingPersonsJson);

            return missingPersonsList;

        }
    }
}
