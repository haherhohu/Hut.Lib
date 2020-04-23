using System;
using System.Net.Mail;
using Microsoft.Exchange.WebServices.Data;

namespace Hut
{
    public class HutMailClient
    {
        private string SMTPADDRESS = @"mail.kasi.re.kr";
        //string SMTPADDRESS = @"ex2010-1st.kasi.re.kr/owa/";
        private string SMTPID = @"ds_scheduler@kasi.re.kr";
        private string SMTPPASSWORD = @"Neo0212)@!@";
        //        string SMTPADDRESS = @"selab.co.kr";
        //        string SMTPID = @"ysryu";
        //        string SMTPPASSWORD = @"ftptkd12";
        private string SENDERID = @"ds_scheduler@kasi.re.kr";
        private string SENDERNAME = @"DEEP-South Scheduler";

        public HutMailClient()
        {

        }

        // filename: fullpath
        public void sendMail(string date, string observatory, string mailaddress, string filename, string[] asteroids = null, string[] targetfields = null, int[] gapproperties = null)
        {
            string subject = "[DEEP-South] " + observatory + ":" + date + " observation OCF.";
            //            string body = "Auto-Generated OCF";

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(SENDERID, SENDERNAME, System.Text.Encoding.UTF8);
            mail.To.Add(mailaddress);
            mail.Subject = subject;
            mail.Body = makeBody(date, observatory, asteroids, targetfields, gapproperties);
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.SubjectEncoding = System.Text.Encoding.UTF8;

            System.Net.Mail.Attachment attatchment = new System.Net.Mail.Attachment(filename);
            mail.Attachments.Add(attatchment);

            SmtpClient smtpclient = new SmtpClient(SMTPADDRESS);
            smtpclient.UseDefaultCredentials = false;

            smtpclient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpclient.Credentials = new System.Net.NetworkCredential(SMTPID, SMTPPASSWORD);
            smtpclient.EnableSsl = false;
            smtpclient.Send(mail);
        }

        public void sendMailWithEWS(string date, string observatory, string mailaddress, string[] ccmailaddress, string filename, string[] asteroids = null, string[] targetfields = null, int[] gapproperties = null, string[] fcfilename = null)
        {
            string subject = @"[DEEP-South] " + observatory + @":" + date + @" observation OCF.";
            string body = makeBody(date, observatory, asteroids, targetfields, gapproperties);

            try
            {
                ExchangeService service = new ExchangeService(ExchangeVersion.Exchange2010_SP1);
                service.Credentials = new WebCredentials(SMTPID, SMTPPASSWORD);
                service.AutodiscoverUrl(SMTPID);

                EmailMessage message = new EmailMessage(service);
                message.Subject = subject;
                message.Body = body;
                message.Body.BodyType = BodyType.Text;
                message.ToRecipients.Add(mailaddress);

                if (ccmailaddress != null && ccmailaddress.Length >= 0)
                    foreach (string cc in ccmailaddress)
                    {
                        message.CcRecipients.Add(cc);
                    }

                message.Attachments.AddFileAttachment(filename);

                foreach (string fcfile in fcfilename)
                {
                    message.Attachments.AddFileAttachment(fcfile);
                }

                message.Send();


            }
            catch (Exception e)
            {
                Console.WriteLine(@"ERROR: " + e.Message);
                Console.ReadLine();
            }
        }

        public void sendFindingChartWithEWS(string date, string observatory, string mailaddress, string[] ccmailaddress, string filename, string[] asteroids = null, string[] targetfields = null, int[] gapproperties = null, string[] fcfilename = null)
        {
            string subject = @"[DEEP-South] " + observatory + @":" + date + @" observation Finding Chart.";
            string body = makeBody(date, observatory, asteroids, targetfields, gapproperties);

            try
            {
                ExchangeService service = new ExchangeService(ExchangeVersion.Exchange2010_SP1);
                service.Credentials = new WebCredentials(SMTPID, SMTPPASSWORD);
                service.AutodiscoverUrl(SMTPID);

                EmailMessage message = new EmailMessage(service);
                message.Subject = subject;
                message.Body = body;
                message.Body.BodyType = BodyType.Text;
                message.ToRecipients.Add(mailaddress);

                if (ccmailaddress != null && ccmailaddress.Length >= 0)
                    foreach (string cc in ccmailaddress)
                    {
                        message.CcRecipients.Add(cc);
                    }

                //message.Attachments.AddFileAttachment(filename);

                foreach (string fcfile in fcfilename)
                {
                    message.Attachments.AddFileAttachment(fcfile);
                }

                message.Send();


            }
            catch (Exception e)
            {
                Console.WriteLine(@"ERROR: " + e.Message);
                Console.ReadLine();
            }
        }

        public string makeBody(string date, string observatory, string[] asteroids = null, string[] targetfields = null, int[] gapproperties = null)
        {
            string body = @"DEEP-South OCF (Auto-Generated)" + Environment.NewLine;
            body += Environment.NewLine;

            body += @"Obs  : " + observatory + Environment.NewLine;
            body += @"Date : " + date + Environment.NewLine;
            body += Environment.NewLine;

            // TODO: get TO target. but we can't know that info
            //body += @"Target (TO)" + Environment.NewLine;
            //body += Environment.NewLine;

            body += @"Target (OC)    TF" + Environment.NewLine;
            if (asteroids != null)
                for (int i = 0; i < asteroids.Length; i++)
                {
                    body += string.Format("{0,2}) {1,-10} {2,-7}", (i + 1).ToString(), asteroids[i], targetfields[i]);
                    if (gapproperties[i] != 0)
                        body += @" (*)";

                    body += Environment.NewLine;
                }
            else
                body += @"   No Scheduled Asteroids." + Environment.NewLine;
            body += Environment.NewLine;

            body += Environment.NewLine;
            body += @"End of Mail" + Environment.NewLine;

            return body;
        }

        public void test()
        {
            try
            {
                ExchangeService service = new ExchangeService(ExchangeVersion.Exchange2010_SP1);
                service.Credentials = new WebCredentials(SMTPID, SMTPPASSWORD);
                service.AutodiscoverUrl(SMTPID);

                EmailMessage message = new EmailMessage(service);
                message.Subject = @"tq";
                message.Body = @"tqtq";
                message.ToRecipients.Add(@"ysryu@selab.co.kr");
                message.Attachments.AddFileAttachment(@"OCF_CTIO_20150720.txt");
                message.Send();


            }
            catch (Exception e)
            {
                Console.WriteLine(@"ERROR: " + e.Message);
                Console.ReadLine();
            }

            ////            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls;

            //            string mailaddress = @"ysryu@selab.co.kr";
            //            string subject = @"mailtest";
            //            string body = @"neng mu";

            //            MailMessage mail = new MailMessage();
            //            mail.From = new MailAddress(SENDERID, SENDERNAME, System.Text.Encoding.UTF8);
            //            mail.To.Add(mailaddress);
            //            mail.Subject = subject;
            //            mail.Body = body;
            //            mail.BodyEncoding = System.Text.Encoding.UTF8;
            //            mail.SubjectEncoding = System.Text.Encoding.UTF8;

            ////            System.Net.Mail.Attachment attatchment = new System.Net.Mail.Attachment(filename);
            ////            mail.Attachments.Add(attatchment);

            //            SmtpClient smtpclient = new SmtpClient(SMTPADDRESS, 25);
            //            smtpclient.UseDefaultCredentials = true;

            //            smtpclient.DeliveryMethod = SmtpDeliveryMethod.Network;
            //            smtpclient.Credentials = new System.Net.NetworkCredential(SMTPID, SMTPPASSWORD);
            //            //smtpclient.EnableSsl = false;
            //            smtpclient.EnableSsl = true;
            //            //            ServicePointManager.SecurityProtocol.
            ////            smtpclient.ClientCertificates = new System.Security.Cryptography.X509Certificates
            //            try
            //            {
            //                smtpclient.Send(mail);

            //            }
            //            catch (Exception e)
            //            {
            //                Console.WriteLine(e.Message);
            //                Console.WriteLine(e.InnerException);
            //            }
            //            smtpclient.Dispose();
            //            smtpclient = null;
            //            mail = null;

            ////            DeepSouthLogger.Instance.logI(@"Mail Send Complete");

        }
    }
}