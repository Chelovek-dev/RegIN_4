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
                Credentials = new NetworkCredential("KekMem11@yandex.ru", "jwstegerpyndznoy"),
                EnableSsl = true,
            };
            smtpClient.Send("KekMem11@yandex.ru", To, "RegIn", Message);
        }
    }
}