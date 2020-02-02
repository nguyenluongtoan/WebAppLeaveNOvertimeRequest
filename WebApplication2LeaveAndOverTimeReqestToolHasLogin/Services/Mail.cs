using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebApplication2LeaveAndOverTimeReqestToolHasLogin.Models;
using WebApplication2LeaveAndOverTimeReqestToolHasLogin.Utils;
using System.Net;

namespace WebApplication2LeaveAndOverTimeReqestToolHasLogin.Services
{
    public class Mail
    {
        static string apiKey = "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";
        static SmtpClient client;
        static string plainTextContent = "and easy to do anywhere, even with C#";
        static MailAddress from;
        static void Init()
        {
            //client = new SendGridClient(apiKey);
            Template.Prefix = Constants.LOCALHOST_DEVELOP;
            from = new MailAddress("toannl@lqa.com.vn", "LQA System");
            string fromPassword = "HHH432qa2$jk";
            
            client = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(from.Address, fromPassword)
            };

            /*
             *  Constants.LOCALHOST_PUBLISHED;
             *   
           Constants.LOCALHOST_DEVELOP;
           Constants.DATALQA_PUBLISHED;
           */
        }


        public static void Execute(LeaveRequest leaveRequest)
        {
            var subject = "[LQA][HR][Leave Request] " + leaveRequest.Account.ToUpper() + " xin phép vắng mặt ngày " +
                leaveRequest.LeaveDate.Day + "/" + leaveRequest.LeaveDate.Month + "/" + leaveRequest.LeaveDate.Year;
            var to = new MailAddress(leaveRequest.EmailAddress, "LQA HR");
            var htmlContent = Template.Leave(leaveRequest);
            string fromPassword = "hpz@NCncQ7y5";
            
            try
            {
                using (var message = new MailMessage(from, to))
                {
                    message.CC.Add("hr@lqa.com.vn");
                    message.Body = htmlContent;
                    message.IsBodyHtml = true;
                    message.Subject = subject;
                    client.Send(message);
                }
            }
            catch (Exception e)
            {
                string g = e.Message;
            }
        }


        public static async Task SendCreatedReqMail2Member(LeaveRequest leaveRequest)
        {
            Init();
            var subject = "[LQA][HR][Leave Request] " + leaveRequest.Account.ToUpper() + " xin phép vắng mặt ngày " +
               leaveRequest.LeaveDate.Day + "/" + leaveRequest.LeaveDate.Month + "/" + leaveRequest.LeaveDate.Year;
            var to = new MailAddress(leaveRequest.EmailAddress, "LQA HR");
            try
            {
                using (var message = new MailMessage(from, to))
                {
                    //message.CC.Add("hr@lqa.com.vn");
                    message.Body = Template.Leave(leaveRequest);
                    message.IsBodyHtml = true;
                    message.Subject = subject;
                    client.Send(message);
                }
            }
            catch (Exception e)
            {
                string g = e.Message;
            }

            //var from = new EmailAddress("hr@lqa.com.vn", "LQA HR");
            //var subject = "[LQA][HR][Leave Request] " + leaveRequest.Account.ToUpper() + " xin phép vắng mặt ngày " +
            //    leaveRequest.LeaveDate.Day + "/" + leaveRequest.LeaveDate.Month + "/" + leaveRequest.LeaveDate.Year;
            //var to = new EmailAddress(leaveRequest.EmailAddress, "LQA HR");
            //var htmlContent = Template.Leave(leaveRequest);
            //var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            //var response = await client.SendEmailAsync(msg);

        }
        public static async Task SendCreatedReqMail2Leader(LeaveRequest leaveRequest,int receiver)
        {
            Init();
            //var from = new EmailAddress("hr@lqa.com.vn", "LQA HR");
            //var subject = "[LQA][HR][Leave Request] " + leaveRequest.Account.ToUpper() + " xin phép vắng mặt ngày " + 
            //    leaveRequest.LeaveDate.Day + "/" + leaveRequest.LeaveDate.Month + "/" + leaveRequest.LeaveDate.Year;
            //string toEmail = leaveRequest.LeaderEmailAddress;
            //if (receiver == Constants.LEADER)
            //{}
            //else if (receiver == Constants.HR)
            //{
            //    toEmail = "hr@lqa.com.vn";
            //}
            //var to = new EmailAddress(toEmail, "LQA HR");
            //var htmlContent = Template.ApproveRequest(leaveRequest);
            //var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            //var response = await client.SendEmailAsync(msg);

            var to = new MailAddress(leaveRequest.LeaderEmailAddress, "LQA HR");
            try
            {
                using (var message = new MailMessage(from, to))
                {
                    //message.CC.Add("hr@lqa.com.vn");
                    message.Body = Template.ApproveRequest(leaveRequest);
                    message.IsBodyHtml = true;
                    message.Subject = "[LQA][HR][Leave Request] " + leaveRequest.Account.ToUpper() + " xin phép vắng mặt ngày " +
                leaveRequest.LeaveDate.Day + "/" + leaveRequest.LeaveDate.Month + "/" + leaveRequest.LeaveDate.Year;
                    client.Send(message);
                }
            }
            catch (Exception e)
            {
                string g = e.Message;
            }
        }

        public static async Task SendNotice(LeaveRequest leaveRequest, int receiver, int target)
        {
            Init();
            //var from = new EmailAddress("hr@lqa.com.vn", "LQA HR");
            //var subject = "[LQA][HR][Leave Request] " + leaveRequest.Account.ToUpper() + " xin phép vắng mặt ngày " +
            //    leaveRequest.LeaveDate.Day + "/" + leaveRequest.LeaveDate.Month + "/" + leaveRequest.LeaveDate.Year;
            //string toEmail = leaveRequest.EmailAddress;
            //if (receiver == Constants.MEMBER)
            //{

            //}
            //else if (receiver == Constants.LEADER)
            //{
            //    toEmail = leaveRequest.LeaderEmailAddress;
            //}
            //else if (receiver == Constants.HR)
            //{
            //    toEmail = "hr@lqa.com.vn";
            //}
            //var to = new EmailAddress(toEmail, "LQA HR");

            //var htmlContent = Template.Approved(leaveRequest);
            //if (target == Constants.APPROVED)
            //{
            //}
            //else if (target == Constants.REJECTED)
            //{
            //    htmlContent = Template.Rejected(leaveRequest);
            //}
            //var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            //var response = await client.SendEmailAsync(msg);


            var htmlContent = Template.Approved(leaveRequest);
            if (target == Constants.APPROVED)
            {}
            else if (target == Constants.REJECTED)
            {
                htmlContent = Template.Rejected(leaveRequest);
            }
            var to = new MailAddress(leaveRequest.EmailAddress, "LQA HR");
            try
            {
                using (var message = new MailMessage(from, to))
                {
                    //message.CC.Add("hr@lqa.com.vn");
                    message.CC.Add(leaveRequest.LeaderEmailAddress);
                    message.Body = htmlContent;
                    message.IsBodyHtml = true;
                    message.Subject = "[LQA][HR][Leave Request] " + leaveRequest.Account.ToUpper() + " xin phép vắng mặt ngày " +
                leaveRequest.LeaveDate.Day + "/" + leaveRequest.LeaveDate.Month + "/" + leaveRequest.LeaveDate.Year;
                    client.Send(message);
                }
            }
            catch (Exception e)
            {
                string g = e.Message;
            }

        }
       
       
      
    }
}