using System.Net.Mail;
using System.Net;

namespace MixMeal.EmailSender
{
    public class SendEmail
    {

        public void SendEmailWithPDF(string recipientEmail, string subject, string body, string attachmentPath)
        {
            try
            {
                using (SmtpClient smtpClient = new SmtpClient("smtp.ethereal.email")) 
                {
                    smtpClient.Port = 587; 
                    smtpClient.Credentials = new NetworkCredential("waldo.gorczany@ethereal.email", "Z4hhXUjdutbGHVy8DZ"); 
                    smtpClient.EnableSsl = true; 

                    using (MailMessage mailMessage = new MailMessage())
                    {
                        mailMessage.From = new MailAddress("waldo.gorczany@ethereal.email"); 
                        mailMessage.To.Add(recipientEmail);
                        mailMessage.Subject = subject;
                        mailMessage.Body = body;

                        // Attach the PDF file
                        Attachment attachment = new Attachment(attachmentPath);
                        mailMessage.Attachments.Add(attachment);

                        smtpClient.Send(mailMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to send email: " + ex.Message);
            }
        }



    }
}
