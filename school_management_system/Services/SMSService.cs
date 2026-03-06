using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace school_management_system.Services
{
    public class SMSService
    {
        public void SendSMS(string phone, string message)
        {
            string accountSid = "AC6fa709aa733e93ed48b9da9aab89efbc";
            string authToken = "d1ba807b8de43cd7bd254881b4b5cbc9";

            TwilioClient.Init(accountSid, authToken);

            var sms = MessageResource.Create(
                to: new PhoneNumber(phone),
                from: new PhoneNumber("+1 812 365 1291"),
                body: message
            );
        }
    }
}