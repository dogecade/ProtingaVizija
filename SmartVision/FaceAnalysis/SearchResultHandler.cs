using NotificationService;

namespace FaceAnalysis
{
    public class SearchResultHandler
    {
        private static string highProbabilityEmailSubject = "HIGH possibility that your missing person was detected!";
        private static string highProbabilitySmsBodyBeginning ="Good afternoon. There's a HIGH possibility, that your missing person ";
        private static string highProbabilitySmsBodyEnding =" was detected. Please check you email for more detailed information.";
        private static string highProbabilityEmailBodyBeginning = "Good afternoon. There's a HIGH possibility, that your missing person ";
        private static string highProbabilityEmailBodyEnding = " was detected. Please find attached frame in which your person was spotted.";

        private static string veryHighProbabilityEmailSubject ="VERY HIGH possibility that your missing person was detected!";
        private static string veryHighProbabilitySmsBodyBeginning ="Good afternoon. There's a VERY HIGH possibility, that your missing person ";
        private static string veryHighProbabilitySmsBodyEnding =" was detected. Please check you email for more detailed information.";
        private static string veryHighProbabilityEmailBodyBeginning = "Good afternoon. There's a VERY HIGH possibility, that your missing person ";
        private static string veryHighProbabilityEmailBodyEnding = " was detected. Please find attached frame in which your person was spotted.";


        public static void HandleSearchResult(LikelinessConfidence result, string matchedFace)
        {
            NecessaryContactInformation information;

            switch (result)
            {
                case LikelinessConfidence.LowProbability:
                    break;

                case LikelinessConfidence.NormalProbability:
                    // Should we do smth here?
                    break;

                case LikelinessConfidence.HighProbability:
                    information = GetMissingPersonData(matchedFace);
                    Mail.SendMail(information.contactPersonEmailAddress, highProbabilityEmailSubject,
                        highProbabilityEmailBodyBeginning + information.missingPersonFirstName + " " +
                        information.missingPersonLastName + highProbabilityEmailBodyEnding);

                    Sms.SendSms(information.contactPersonPhoneNumber,highProbabilitySmsBodyBeginning + information.missingPersonFirstName + " " + information.missingPersonLastName + highProbabilitySmsBodyEnding);
                    break;

                case LikelinessConfidence.VeryHighProbability:
                    information = GetMissingPersonData(matchedFace);
                    Mail.SendMail(information.contactPersonEmailAddress, veryHighProbabilityEmailSubject,
                        veryHighProbabilityEmailBodyBeginning + information.missingPersonFirstName + " " +
                        information.missingPersonLastName + veryHighProbabilityEmailBodyEnding);

                    Sms.SendSms(information.contactPersonPhoneNumber,veryHighProbabilitySmsBodyBeginning + information.missingPersonFirstName + " " + information.missingPersonLastName + veryHighProbabilitySmsBodyEnding);
                    break;
            }
        }

        private static NecessaryContactInformation GetMissingPersonData(string matchedFace)
        {
            // Get missing person from db where identifier is facetoken
            string missingPersonFirstName = "name";
            string missingPersonLastName = "lastname";
            string contactPersonPhoneNumber = "+37068959812";
            string contactPersonEmailAddress = "deividas.brazenas@gmail.com";

            return new NecessaryContactInformation(missingPersonFirstName, missingPersonLastName,
                contactPersonPhoneNumber, contactPersonEmailAddress);
        }
    }
}