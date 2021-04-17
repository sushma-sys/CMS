using ReminderForPassword;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace MailService
{
    public partial class Service1 : ServiceBase
    {
       private Timer timer = null;

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            EventLog.WriteEntry("Mail Service Started");
            // one second=1000
            timer = new Timer();
            timer.Interval = 60000;
            timer.Elapsed += new ElapsedEventHandler(timer_Tick);
           // timer.AutoReset = false;
             timer.Enabled = true;
        }

        private void timer_Tick(object sender, ElapsedEventArgs e)
        {

           if (DateTime.Now.Hour==11&DateTime.Now.Minute==00)
           {
                ReminderPassword.GetWillExpireOn(); 
           }
        }

        //private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        //{
        //   // if (DateTime.Now.Hour==12 && DateTime.Now.Minute==0)
        //   // {
        //        MailMessage mail = new MailMessage();
        //        mail.To.Add(new MailAddress("sushma.sanda99@gmail.com"));
        //        mail.From = new MailAddress(ConfigurationManager.AppSettings["SMTPuser"]);
        //        mail.Subject = "Windows Service";
        //        // message.Body = string.Format(body, model.FromName, model.FromEmail, model.Message);
        //        // mail.Body = string.Format(body, issue.EmailId, issue.IncidentType, issue.IncidentDescription, issue.ServiceId, issue.Location, issue.CustomerPhone);
        //        //StringBuilder Body = new StringBuilder();
        //        //Body.Append("EmailId : " + issue.EmailId);
        //        //Body.Append("IncidentType : " + issue.IncidentType);
        //        //Body.Append("IncidentDescription : " + issue.IncidentDescription);
        //        //Body.Append("Location : " + issue.Location);
        //        //Body.Append("CustomerPhone : " + issue.CustomerPhone);
        //        //mail.Body = Body.ToString();
        //        // mail.Body = Model.Case;
        //        // mail.IsBodyHtml = true;
        //        mail.Body = "Hai this is Windows Serives";
        //        SmtpClient smtp = new SmtpClient();
        //        smtp.Host = "smtp.gmail.com";
        //        smtp.Port = 587;
        //        smtp.UseDefaultCredentials = false;
        //        smtp.Credentials = new System.Net.NetworkCredential
        //        (ConfigurationManager.AppSettings["SMTPuser"], ConfigurationManager.AppSettings["SMTPpassword"]);// Enter seders User name and password
        //        smtp.EnableSsl = true;
        //        smtp.Send(mail);
        //   // }
        //}

        protected override void OnStop()
        {
            EventLog.WriteEntry("Mail Service Stopped");
            timer.Enabled = false;
        }
    }
}
