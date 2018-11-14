using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsForms
{
    class Messages
    {
        public const string failedDbConnection = "Failed to establish database connection";
        public const string incorrectMissingFirstNamePattern = "Missing person First name should contain only letters and cannot be empty!";
        public const string incorrectLastNamePattern = "Missing person Last name should contain only letters and cannot be empty!";
        public const string incorrectContactFirstNamePattern = "Contact person First name should contain only letters and cannot be empty!";
        public const string incorrectContactLastNamePattern = "Contact person Last name should contain only letters and cannot be empty!";
        public const string incorrectPhoneNumberPattern = "Contact person Phone number should be in a valid format(+[Country code]00...00) and cannot be empty!";
        public const string incorrectEmailPattern = "Contact person Email address should be in a valid format (foo@bar.baz) and cannot be empty!";
        public const string invalidApiResponse = "Invalid API Response";
        public const string personSubmitSuccessful = "Missing person submitted successfully.";
        public const string invalidImage = "Please upload a valid picture!";
        public const string errorWhileSavingPerson = "An error occured while saving missing person, please try again later";
        public const string errorWhileAnalysingImage = "An error occured while analysing the image, please try again later";
        public const string noFacesInImage = "Unfortunately, no faces have been detected in the picture! \n" + "Please try another one.";
        public const string cameraNotFound = "Input camera was not found!";
    }
}
