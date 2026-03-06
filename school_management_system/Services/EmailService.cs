using System.Net;
using System.Net.Mail;

public class EmailService
{
    public void SendEmail(string toEmail, string subject, string body)
    {
        var fromEmail = "islammontaherul@gmail.com";
        var password = "athu qxxm jtcc duos";

        SmtpClient client = new SmtpClient("smtp.gmail.com", 587)
        {
            Credentials = new NetworkCredential(fromEmail, password),
            EnableSsl = true
        };

        MailMessage mail = new MailMessage(fromEmail, toEmail, subject, body);

        client.Send(mail);
    }
}