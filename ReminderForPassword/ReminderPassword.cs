using CMS.Entities;
using CMS.Exceptions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ReminderForPassword
{
    public class ReminderPassword
    {
        public static void GetWillExpireOn()
        {
            try
            {
                using (CMSDBContext context=new CMSDBContext())
                {
                    var user = context.ODZUsers.ToList();
                    foreach (var item in user)
                    {
                        DateTime upDatedOn = (DateTime)context.ODZUsers.Select(u => u.PasswordUpdatedOn).FirstOrDefault();
                        TimeSpan diff = DateTime.Now - upDatedOn;
                        int passwordExpirydays = Convert.ToInt32(ConfigurationManager.AppSettings["PasswordExpirydays"]);
                        int reminderDelta = Convert.ToInt32(ConfigurationManager.AppSettings["ReminderDelta"]);
                        if (diff.Days >= reminderDelta && diff.Days < passwordExpirydays)
                        {
                            MailMessage mail = new MailMessage();
                                mail.To.Add(new MailAddress("sushma.sanda99@gmail.com"));
                                mail.From = new MailAddress(ConfigurationManager.AppSettings["SMTPuser"]);
                                mail.Subject = "Password Expiry";
                                int remaineDaysForExpire = passwordExpirydays - diff.Days;
                                mail.Body = "Your Password will Expire in " + remaineDaysForExpire + " Days, So Change the Password";
                                SmtpClient smtp = new SmtpClient();
                                smtp.Host = "smtp.gmail.com";
                                smtp.Port = 587;
                                smtp.UseDefaultCredentials = false;
                                smtp.Credentials = new System.Net.NetworkCredential
                                (ConfigurationManager.AppSettings["SMTPuser"], ConfigurationManager.AppSettings["SMTPpassword"]);// Enter seders User name and password
                                smtp.EnableSsl = true;
                                smtp.Send(mail);
                            
                        }
                    }
                    
                }
            }
            catch (CmsExceptions ex)
            {
                
                throw ex;
            }
        }
        //private static void SendEmail()
        //{
        //    MailMessage mail = new MailMessage();
        //    mail.To.Add(new MailAddress("sushma.sanda99@gmail.com"));
        //    mail.From = new MailAddress(ConfigurationManager.AppSettings["SMTPuser"]);
        //    mail.Subject = "Password Expiry";
        //    mail.Body = "Your Password will Expire in "+diff.Days+" Days, So Change the Password";
        //    SmtpClient smtp = new SmtpClient();
        //    smtp.Host = "smtp.gmail.com";
        //    smtp.Port = 587;
        //    smtp.UseDefaultCredentials = false;
        //    smtp.Credentials = new System.Net.NetworkCredential
        //    (ConfigurationManager.AppSettings["SMTPuser"], ConfigurationManager.AppSettings["SMTPpassword"]);// Enter seders User name and password
        //    smtp.EnableSsl = true;
        //    smtp.Send(mail);
        //    // return true;
        //}
    }
}
