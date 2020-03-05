using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication2LeaveAndOverTimeReqestToolHasLogin.Models;

namespace WebApplication2LeaveAndOverTimeReqestToolHasLogin.Services
{
    public class Template
    {
        public static string Prefix { get; set; } 
        public static string Leave(LeaveRequest leaveRequest)
        {
            return ShowInfo(leaveRequest) + DetailLink(leaveRequest.LeaveRequestID);
        }

        public static string ApproveRequest(LeaveRequest leaveRequest)
        {
            return ShowInfo(leaveRequest) + ApproveLink(leaveRequest.LeaveRequestID);
        }
         
        public static string Rejected(LeaveRequest leaveRequest)
        {
                return "<span>Request xin nghỉ của bạn "+leaveRequest.Account+" đã bị hủy bỏ," +
                "<br/>Vui lòng truy cập vào link dưới đây để biết thêm chi tiết</span><br/>" +
                DetailLink(leaveRequest.LeaveRequestID);
        }
        public static string Approved(LeaveRequest leaveRequest)
        {
                return "<span>Request xin nghỉ của bạn "+leaveRequest.Account + " đã được đồng ý từ leader, " +
                "<br/>Vui lòng truy cập vào link dưới đây để biết thêm chi tiết</span><br/>" +
                DetailLink(leaveRequest.LeaveRequestID);
        }

        public static string DetailLink(int LeaveRequestID)
        {
            return "<a href='" + Prefix + "LeaveRequests/Details/" + LeaveRequestID + "'>Details</a>";
        }
        public static string ApproveLink(int LeaveRequestID)
        {
            return "<a href='" + Prefix + "LeaveRequests/ApproveOrReject/" + LeaveRequestID + "'>Approve Or Reject</a>";
        }

        public static string ShowInfo(LeaveRequest leaveRequest)
        {
            return "<span>Hệ thống thông báo có 1 request xin nghỉ của bạn " + leaveRequest.Account + "<br>tới leader " + leaveRequest.LeaderAccount + "," +
            "<br>Ngày bắt đầu nghỉ: " + 
                leaveRequest.LeaveDate.Day + "/" + leaveRequest.LeaveDate.Month + "/" + leaveRequest.LeaveDate.Year +
            "<br>Số ngày nghỉ: " + leaveRequest.NoDayOff +
            "<br>Lí do xin nghỉ: " + leaveRequest.ReasonForLeave +
            "<br>Loại nghỉ: " + leaveRequest.TypeOfLeave +
            "<br>Vui lòng truy cập vào link dưới đây để biết thêm chi tiết</span><br/>";
        }

        public static string ForgotPass(string email, string code, string url)
        {
            return "<span>Hệ thống thông báo có 1 request xin reset password, Active code gửi tới email "+email+"<span><br/>"+
                code+"<br/>Click vào link dưới đây để reset<br/>"+url;
        }
    }
}