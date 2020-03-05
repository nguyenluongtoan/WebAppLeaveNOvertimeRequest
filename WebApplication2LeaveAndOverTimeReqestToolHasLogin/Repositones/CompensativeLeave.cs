using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication2LeaveAndOverTimeReqestToolHasLogin.Models;
using WebApplication2LeaveAndOverTimeReqestToolHasLogin.Services;

namespace WebApplication2LeaveAndOverTimeReqestToolHasLogin.Repositones
{
    public class CompensativeLeave
    {
        
        static Dictionary<string, int> namceountKeyActualTimeValue;
        private CompensativeLeave()
        {
            Init();
        }
        private ApplicationDbContext db = new ApplicationDbContext();
        private static CompensativeLeave instance = null;
        public static CompensativeLeave Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CompensativeLeave();
                }
                return instance;
            }
        }
        //= new Dictionary<string, int>();
        public static void Init()
        {
            GoogleSheet googleSheet = new GoogleSheet();
            googleSheet.Url = "https://docs.google.com/spreadsheets/d/1lYBNEs-YhKCHwYCFeHQNZXM1wW1NnJH6LUsr8aix0-U";
            //googleSheet.TabName = "2019";
            googleSheet.TabName = "2020";
            googleSheet.Range = "A2:J";
            googleSheet.Init();
            IList<IList<Object>> values = googleSheet.ResponseValues();
            HashSet<string> namceounts = new HashSet<string>();
            namceountKeyActualTimeValue = new Dictionary<string, int>();
            if (values != null && values.Count > 0)
            {
                foreach (var row in values)
                {
                    int countCell = 0;
                    foreach (var cell in row)
                    {
                        countCell++;
                        if (countCell == 4)
                        {
                            namceounts.Add(cell.ToString());
                        }

                    }
                }
            }
            foreach (string namceount in namceounts)
            {
                namceountKeyActualTimeValue.Add(namceount, 0);
            }
            if (values != null && values.Count > 0)
            {
                foreach (var row in values)
                {
                    int countCell = 0;
                    string namceount = "";
                    foreach (var cell in row)
                    {
                        countCell++;

                        if (countCell == 1)
                        {
                            namceount = "";
                        }
                        if (countCell == 4)
                        {
                            namceount = cell.ToString();
                        }
                        if (countCell == 7)
                        {
                            if (!namceountKeyActualTimeValue.ContainsKey(namceount))
                            {
                                continue;
                            }
                            int val = namceountKeyActualTimeValue[namceount];
                            try
                            {
                                val = val + int.Parse(cell.ToString());
                            }
                            catch (Exception e)
                            {
                                string g = e.Message;
                            }

                            namceountKeyActualTimeValue[namceount] = val;
                        }
                    }
                }
            }
        }
        public double GetNoCompensativeLeaveDay(string account)
        {
            if (!namceountKeyActualTimeValue.ContainsKey(account))
            {
                return 0;
            }
            double no = namceountKeyActualTimeValue[account] / 8;
            return no;
        }
        public bool Allowable(string account)
        {
            if (GetNoCompensativeLeaveDay(account) > 0)
            {
                return true;
            }
            return false;
        }

        public double GetCountLeaveByAccount(string typeOfLeave, string account)
        {
            var leaveRequests = from s in db.LeaveRequests select s;
            //if (!account.Contains("lqa.com.vn"))
            //{
            //    account += "@lqa.com.vn";
            //}
            var list = leaveRequests.Where(s => s.Account == account && s.TypeOfLeave == typeOfLeave).ToList();
            double count = 0;
            foreach(LeaveRequest leaveRequest in list)
            {
                count += leaveRequest.NoDayOff;
            }
            return count;
        }
    }

}