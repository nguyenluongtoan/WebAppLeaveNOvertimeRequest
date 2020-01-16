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

    }
}