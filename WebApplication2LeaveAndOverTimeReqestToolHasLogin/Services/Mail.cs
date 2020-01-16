using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebApplication2LeaveAndOverTimeReqestToolHasLogin.Models;
using WebApplication2LeaveAndOverTimeReqestToolHasLogin.Utils;

namespace WebApplication2LeaveAndOverTimeReqestToolHasLogin.Services
{
    public class Mail
    {
        
      
        
        static string apiKey = "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";
        static SendGridClient client;
        static string plainTextContent = "and easy to do anywhere, even with C#";
        static void Init()
        {
            client = new SendGridClient(apiKey);
            Template.Prefix =Constants.DATALQA_PUBLISHED;


            /*
             *  Constants.LOCALHOST_PUBLISHED;
             *   Constants.LOCALHOST_DEVELOP; 
           
           */
        }
        //public static async Task Execute()
        //{
        //    var apiKey = Environment.GetEnvironmentVariable("NAME_OF_THE_ENVIRONMENT_VARIABLE_FOR_YOUR_SENDGRID_KEY");
        //    var from = new EmailAddress("toannl@lqa.com.vn", "LQA System");
        //    var subject = "Sending with SendGrid is Fun";
        //    var to = new EmailAddress("toannl@lqa.com.vn", "LQA System");
        //    var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
        //    var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
        //    var response = await client.SendEmailAsync(msg);
        //}


        public static async Task SendCreatedReqMail2Member(LeaveRequest leaveRequest)
        {
            Init();
            var from = new EmailAddress("hr@lqa.com.vn", "LQA HR");
            var subject = "[LQA][HR][Leave Request] "+leaveRequest.Account.ToUpper()+" xin phép vắng mặt ngày "+
                leaveRequest.LeaveDate.Day +"/"+ leaveRequest.LeaveDate.Month + "/" + leaveRequest.LeaveDate.Year;
            var to = new EmailAddress(leaveRequest.EmailAddress, "LQA HR");
            var htmlContent = Template.Leave(leaveRequest);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }
        public static async Task SendCreatedReqMail2Leader(LeaveRequest leaveRequest,int receiver)
        {
            Init();
            var from = new EmailAddress("hr@lqa.com.vn", "LQA HR");
            var subject = "[LQA][HR][Leave Request] " + leaveRequest.Account.ToUpper() + " xin phép vắng mặt ngày " + 
                leaveRequest.LeaveDate.Day + "/" + leaveRequest.LeaveDate.Month + "/" + leaveRequest.LeaveDate.Year;
            string toEmail = leaveRequest.LeaderEmailAddress;
            if (receiver == Constants.LEADER)
            {}
            else if (receiver == Constants.HR)
            {
                toEmail = "hr@lqa.com.vn";
            }
            var to = new EmailAddress(toEmail, "LQA HR");
            var htmlContent = Template.ApproveRequest(leaveRequest);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }
     
        public static async Task SendNotice(LeaveRequest leaveRequest, int receiver, int target)
        {
            Init();
            var from = new EmailAddress("hr@lqa.com.vn", "LQA HR");
            var subject = "[LQA][HR][Leave Request] " + leaveRequest.Account.ToUpper() + " xin phép vắng mặt ngày " + 
                leaveRequest.LeaveDate.Day + "/" + leaveRequest.LeaveDate.Month + "/" + leaveRequest.LeaveDate.Year;
            string toEmail = leaveRequest.EmailAddress;
            if (receiver == Constants.MEMBER)
            {

            }
            else if (receiver == Constants.LEADER)
            {
                toEmail = leaveRequest.LeaderEmailAddress;
            }
            else if (receiver == Constants.HR)
            {
                toEmail = "hr@lqa.com.vn";
            }
            var to = new EmailAddress(toEmail, "LQA HR");
            
            var htmlContent = Template.Approved(leaveRequest);
            if (target == Constants.APPROVED)
            {

            }
            else if (target == Constants.REJECTED)
            {
                htmlContent = Template.Rejected(leaveRequest);
            }
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }
       
       
      
    }
}