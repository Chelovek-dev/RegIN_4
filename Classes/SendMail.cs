using System.Net.Mail;
using System.Net;

namespace RegIN_Bulatov_Perevozshikova.Classes
{
    public class SendMail
    {
        public static void SendMessage(string Message, string To)
        {
            var smtpClient = new SmtpClient("smtp.yandex.ru")
            {
                Port = 587,
                Credentials = new NetworkCredential("@yandex.ru", "jwstegerpyndznoy"),
                EnableSsl = true,
            };
            smtpClient.Send("@yandex.ru", To, "RegIn", Message);
        }
    }
}