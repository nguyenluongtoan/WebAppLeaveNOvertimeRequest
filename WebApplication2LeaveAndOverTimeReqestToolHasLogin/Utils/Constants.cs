using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2LeaveAndOverTimeReqestToolHasLogin.Utils
{
    public class Constants
    {
        public const int OPEN = 0;
        public const int APPROVED = 1;
        public const int REJECTED = 2;

        public const string LOCALHOST_DEVELOP = "https://localhost:44392/";
        //44392  44334
        public const string LOCALHOST_PUBLISHED = "http://10.10.21.87:8079/";
        public const string DATALQA_PUBLISHED = "http://leave.lqa.com.vn/";

        public const int MEMBER = 0;
        public const int LEADER = 1;
        public const int HR = 2;

        public const int FULL_DAY = 0;
        public const int AM = 1;
        public const int PM = 2;

    }

    public class TYPE_OF_LEAVE_REQUEST
    {
        public const string SICK_LEAVE = "Sick leave (Illness or Injury) - Nghỉ ốm có giấy của bệnh Viện";
        public const string ANNUAL_LEAVE = "Annual leave (Nghỉ Phép)";
        public const string COMPESATIVE_LEAVE = "Compensative leave (Nghỉ bù OT)";
        public const string OT_LAST_YEAR_LEAVE = "OT/Last year compensation";
        public const string WITHOUT_PAY_LEAVE = "Leave without pay (Nghỉ không lương)";
        public const string COMPASSTIONATE_LEAVE = "Compassionate leave (Nghỉ tang(1-3d))";
        public const string ENGAGEMENT_LEAVE = "Engagement leave (Nghỉ kết hôn(3d))";
        public const string MATERNITY_3_LEAVE = "Maternity leave (Nghỉ chăm con < 3t ốm (20d))";
        public const string MATERNITY_7_LEAVE = "Maternity leave (Nghỉ chăm con 3-7t ốm (15d))";
        //"Maternity leave (Nghỉ thai sản(5d))";





    }

}