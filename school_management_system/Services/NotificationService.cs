using school_management_system.Services;

public class NotificationService
{
    private readonly SMSService _sms;
    private readonly EmailService _email;

    public NotificationService(SMSService sms, EmailService email)
    {
        _sms = sms;
        _email = email;
    }

    public void SendNotification(string phone, string email, string message)
    {
        try
        {
            _sms.SendSMS(phone, message);
        }
        catch
        {
            _email.SendEmail(email, "School Notification", message);
        }
    }
}