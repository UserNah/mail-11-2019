using System;
using System.Net.Mail;
using System.Web;
using System.Configuration;
using System.IO;
using System.Linq;

namespace notifMail
{
    class Program
    {
        static string SmtpServer = "";
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Начало--------------------------------------------------------------------------\r\n", String.Empty);
                string fileName = System.IO.Path.GetFullPath(Directory.GetCurrentDirectory() + @"\NotificationsData\\text.txt");

                string msg = File.ReadAllText(fileName);
                SmtpServer = "smtp.mail.ru";// "mail2.tsgarant.local";
                SendMail("test@gmail.com", "aldakulovb@mail.ru", "Привет!", ref msg, ref msg);
                Console.WriteLine("Конец--------------------------------------------------------------------------\r\n\r\n", String.Empty);
                Console.ReadKey();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка в Main", ex.ToString());
            }
        }
        static bool SendMail(String sender, String receiver, String subject, ref String body, ref String msg)
        {
            try
            {
                System.Net.Mail.MailMessage mm = new System.Net.Mail.MailMessage();
                mm.From = new System.Net.Mail.MailAddress(sender);
                foreach (string rec in receiver.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Distinct())
                    mm.To.Add(new System.Net.Mail.MailAddress(rec.Trim()));
                mm.Subject = subject;
                mm.IsBodyHtml = true;
                mm.Body = body;
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(body, null, "text/html");
                LinkedResource imagelink = new LinkedResource(Directory.GetCurrentDirectory() + @"\NotificationsData\\png_optimized.png", "image/png");
                imagelink.ContentId = "imageId";
                imagelink.TransferEncoding = System.Net.Mime.TransferEncoding.Base64;
                htmlView.LinkedResources.Add(imagelink);
                mm.AlternateViews.Add(htmlView);
                System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient(SmtpServer, 25);
                client.Send(mm);
                return true;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                Console.WriteLine("Ошибка при отправке письма", ex.ToString());
                return false;
            }
        }
    }
}
