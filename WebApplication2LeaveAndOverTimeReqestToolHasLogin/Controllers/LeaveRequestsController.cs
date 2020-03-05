using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication2LeaveAndOverTimeReqestToolHasLogin.Models;
using WebApplication2LeaveAndOverTimeReqestToolHasLogin.Utils;
using WebApplication2LeaveAndOverTimeReqestToolHasLogin.Services;
using WebApplication2LeaveAndOverTimeReqestToolHasLogin.Repositones;
using OfficeOpenXml;

namespace WebApplication2LeaveAndOverTimeReqestToolHasLogin.Controllers
{
    
    public class LeaveRequestsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private CompensativeLeave cl = CompensativeLeave.Instance;
        private static List<LeaveRequest> excelUsed;
        //public LeaveRequestsController()
        //{
        //    cl = CompensativeLeave.Instance;
        //}
        [Authorize(Roles = "Admin,Manager")]
        // GET: LeaveRequests
        public async Task<ActionResult> Index(string sortOrder, string searchMemString, string searchLeadString)
        {
            // add col sort link
            //      ViewBag.NameSortParm
            // end add col sort link
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "account_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            
            var leaveRequests = from s in db.LeaveRequests select s;
            //leaveRequests = leaveRequests.Where(
            //    s => User.Identity.Name.Contains(s.Account));
            if (!String.IsNullOrEmpty(searchMemString))
            {
                leaveRequests = leaveRequests.Where(s => s.Account.Contains(searchMemString) || s.EmailAddress.Contains(searchMemString)
                                      
                                 );
            }
            if (!String.IsNullOrEmpty(searchLeadString))
            {
                leaveRequests = leaveRequests.Where(s =>  s.LeaderAccount.Contains(searchLeadString) || s.LeaderEmailAddress.Contains(searchLeadString)
                                 );
            }
            switch (sortOrder)
            {
                case "account_desc":
                    leaveRequests = leaveRequests.OrderByDescending(s => s.Account);
                    break;
                case "Date":
                    leaveRequests = leaveRequests.OrderBy(s => s.LeaveDate);
                    break;
                case "date_desc":
                    leaveRequests = leaveRequests.OrderByDescending(s => s.LeaveDate);
                    break;
                default:
                    leaveRequests = leaveRequests.OrderBy(s => s.Account);
                    break;
            }
            excelUsed = await leaveRequests.ToListAsync();
            ViewBag.vueData = await leaveRequests.ToListAsync();
            return View(await leaveRequests.ToListAsync());
        }

        public async Task<ActionResult> IndexJson(  )
        { 
            var leaveRequests = from s in db.LeaveRequests select s; 
            var data = (await leaveRequests.ToListAsync());
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);


        }
        [Authorize(Roles = "Admin,Manager,Employee")]
        public async Task<ActionResult> Individual()
        {
            var leaveRequests = from s in db.LeaveRequests select s;
            leaveRequests = leaveRequests.Where(
                s => User.Identity.Name.Contains(s.Account));
            var leaveRequestsInOpen = leaveRequests.Where(s => s.Status == Constants.OPEN).ToList();
            List<int> ids = new List<int>();
            foreach(LeaveRequest openR in leaveRequestsInOpen)
            {
                ids.Add(openR.LeaveRequestID);
            }
            ViewBag.OpenRequestIds = ids;

            return View(await leaveRequests.ToListAsync());
        }
        

        [Authorize(Roles = "Admin,Manager,Employee")]
        // GET: LeaveRequests/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LeaveRequest leaveRequest = await db.LeaveRequests.FindAsync(id);
            if (leaveRequest == null)
            {
                return HttpNotFound();
            }
            return View(leaveRequest);
        }

        // GET: LeaveRequests/Create
        [Authorize(Roles = "Admin,Manager,Employee")]
        public ActionResult Create()
        {
            ViewBag.AllLeaders = Utils.Csv.GetLeaderAccount();
            ViewBag.AllLeaders2 = Utils.Csv.GetLeaderInfo();
            ViewBag.AllMembers = Utils.Csv.GetMemberAccount();
            ViewBag.Type = new int[3] { 1, 2, 3 };
            ViewBag.AllowCompensativeLeave = true;
            return View();
        }

        public ActionResult Test()
        {

            return RedirectToAction("Index");
        }

        // POST: LeaveRequests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "LeaveRequestID," +
            "TimeStamp," +
            "Account,EmailAddress,LeaderAccount,LeaderEmailAddress,LeaveDate,NoDayOff,FullAmPm,TypeOfLeave,ReasonForLeave,Status,LeaderComment,Month")] LeaveRequest leaveRequest)
        {
            ViewBag.AllLeaders = Utils.Csv.GetLeaderAccount();
            ViewBag.AllLeaders2 = Utils.Csv.GetLeaderInfo();
            ViewBag.AllMembers = Utils.Csv.GetMemberAccount();
            ViewBag.Type = new int[3] { 1, 2, 3 };
            leaveRequest.TimeStamp = DateTime.Now;
            leaveRequest.Status = Constants.OPEN;
            leaveRequest.Month = 0;
            leaveRequest.LastEditedByAccount = leaveRequest.Account;

            if (ModelState.IsValid)
            {
                if(leaveRequest.TypeOfLeave == "Compensative leave (Nghỉ bù OT)")
                {
                    //if (cl.Allowable(leaveRequest.Account))
                    //{
                    //    ViewBag.AllowCompensativeLeave = true;
                    //}
                    //else
                    //{
                    //    ViewBag.AllowCompensativeLeave = false;
                    //    return View(leaveRequest);
                    //}
                }
                
                db.LeaveRequests.Add(leaveRequest);
                await db.SaveChangesAsync();
                await Mail.SendCreatedReqMail2Member(leaveRequest);
                await Mail.SendCreatedReqMail2Leader(leaveRequest, Constants.LEADER);
                return RedirectToAction("Contact", "Home");

            }

            return View(leaveRequest);
        }
        [Authorize(Roles = "Admin,Manager")]
        // GET: LeaveRequests/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LeaveRequest leaveRequest = await db.LeaveRequests.FindAsync(id);
            
            ViewBag.AllLeaders = Utils.Csv.GetLeaderAccount();
            ViewBag.AllLeaders2 = Utils.Csv.GetLeaderInfo();
            ViewBag.AllMembers = Utils.Csv.GetMemberAccount();
            ViewBag.Type = new int[3] { 1, 2, 3 };
            ViewBag.AllowCompensativeLeave = true;
            if (leaveRequest == null)
            {
                return HttpNotFound();
            }
            return View(leaveRequest);
        }
        [Authorize]
        // POST: LeaveRequests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "LeaveRequestID,TimeStamp,Account,EmailAddress,LeaderAccount,LeaderEmailAddress,LeaveDate,NoDayOff,FullAmPm,TypeOfLeave,ReasonForLeave,Status,LeaderComment,Month")] LeaveRequest leaveRequest)
        {
            if (ModelState.IsValid)
            {
                leaveRequest.LastEditedByAccount = User.Identity.Name;
                db.Entry(leaveRequest).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(leaveRequest);
        }
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult> ApproveOrReject(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LeaveRequest leaveRequest = await db.LeaveRequests.FindAsync(id);
            if (leaveRequest == null)
            {
                return HttpNotFound();
            }
            return View(leaveRequest);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ApproveOrReject([Bind(Include = "LeaveRequestID,TimeStamp,Account,EmailAddress,LeaderAccount,LeaderEmailAddress,LeaveDate,NoDayOff,FullAmPm,TypeOfLeave,ReasonForLeave,Status,LeaderComment")] LeaveRequest leaveRequest)
        {
            leaveRequest.TimeStamp = DateTime.Now;

            if (ModelState.IsValid)
            {
                string comment = leaveRequest.LeaderComment;

                LeaveRequest leaveRequest1 = await db.LeaveRequests.FindAsync(leaveRequest.LeaveRequestID);
                leaveRequest1.Status = Constants.REJECTED;
                leaveRequest1.LeaderComment = comment;
                leaveRequest1.LastEditedByAccount = User.Identity.Name;
                db.Entry(leaveRequest1).State = EntityState.Modified;
                await db.SaveChangesAsync();
                await Mail.SendNotice(leaveRequest1, Constants.MEMBER, Constants.REJECTED);
                
                return RedirectToAction("Index");
            }
            return View(leaveRequest);
        }
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult> ApproveRequest(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LeaveRequest leaveRequest = await db.LeaveRequests.FindAsync(id);
            if (leaveRequest == null)
            {
                return HttpNotFound();
            }
            leaveRequest.TimeStamp = DateTime.Now;
            leaveRequest.Status = Constants.APPROVED;
            leaveRequest.LastEditedByAccount = User.Identity.Name;
            db.Entry(leaveRequest).State = EntityState.Modified;
            await db.SaveChangesAsync();
            await Mail.SendNotice(leaveRequest, Constants.MEMBER, Constants.APPROVED);
            return RedirectToAction("Index");
        }
        //unused, using ApproveOrReject HttpPost
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult> RejectRequest(int? id, string leaderComment)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LeaveRequest leaveRequest = await db.LeaveRequests.FindAsync(id);
            if (leaveRequest == null)
            {
                return HttpNotFound();
            }
            leaveRequest.TimeStamp = DateTime.Now;
            leaveRequest.LeaderComment = leaderComment;
            leaveRequest.Status = Constants.REJECTED;
            leaveRequest.LastEditedByAccount = User.Identity.Name;
            db.Entry(leaveRequest).State = EntityState.Modified;
            await db.SaveChangesAsync();
            await Mail.SendNotice(leaveRequest, Constants.MEMBER, Constants.REJECTED);
            
            return RedirectToAction("Index");
        }
        // GET: LeaveRequests/Delete/5
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LeaveRequest leaveRequest = await db.LeaveRequests.FindAsync(id);
            if (leaveRequest == null)
            {
                return HttpNotFound();
            }
            return View(leaveRequest);
        }

        // POST: LeaveRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            LeaveRequest leaveRequest = await db.LeaveRequests.FindAsync(id);
            db.LeaveRequests.Remove(leaveRequest);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        public async Task<ActionResult> Data2019()
        {
            Services.GoogleSheet googleSheet = new GoogleSheet();
            googleSheet.Url = "https://docs.google.com/spreadsheets/d/1lYBNEs-YhKCHwYCFeHQNZXM1wW1NnJH6LUsr8aix0-U";
            googleSheet.TabName = "2019";
            googleSheet.Range = "A2:J";

            //googleSheet.Url = "https://docs.google.com/spreadsheets/d/1BxiMVs0XRA5nFMdKvBdBZjgmUUqptlbs74OgvE2upms";
            //googleSheet.TabName = "Class Data";
            //googleSheet.Range = "A2:E";
            googleSheet.Init();
            
            ViewBag.Data2019 = googleSheet.ResponseValues();
            IList<IList<Object>> values = googleSheet.ResponseValues();
            HashSet<string> namceounts = new HashSet<string>();
            Dictionary<string, int> namceountKeyActualTimeValue = new Dictionary<string, int>();
            if (values != null && values.Count > 0)
            {
                foreach (var row in values)
                {
                    int countCell = 0;
                    foreach (var cell in row)
                    {
                        countCell++;
                        if(countCell == 4)
                        {
                            namceounts.Add(cell.ToString());
                        }
                         
                    }
                }
            }
            foreach(string namceount in namceounts)
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
                            catch(Exception e)
                            {
                                string g = e.Message;
                            }
                           
                            namceountKeyActualTimeValue[namceount] = val;
                        }
                    }
                }
            }
            ViewBag.Total = namceountKeyActualTimeValue;

            return View();
        }
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult> DataSummary()
        {
            List<KeyValuePair<string, int>> keyValuePairs = Utils.Csv.GetMemberLeaveDays();
            int keyValuePairsCount = keyValuePairs.Count;
            ViewBag.GetMemberLeaveDays = keyValuePairs;
            var leaveRequests = from s in db.LeaveRequests select s;
            var list = leaveRequests.ToList();
            foreach(LeaveRequest lv in list )
            {
                int t = 0;
            }
            
            int[] CountANNUAL_LEAVE = new int[keyValuePairsCount];
            int[] CountANNUAL_LEAVE_OFF = new int[keyValuePairsCount];

            double[] CountCOMPESATIVE_LEAVE = new double[keyValuePairsCount];
            double[] CountCOMPESATIVE_LEAVE_OFF = new double[keyValuePairsCount];
            double[] memberOtHours = new double[keyValuePairsCount];


            double[] CountCOMPASSTIONATE_LEAVE = new double[keyValuePairsCount]; ;
            double[] CountENGAGEMENT_LEAVE = new double[keyValuePairsCount]; ;
            double[] CountMATERNITY_3_LEAVE = new double[keyValuePairsCount]; ;
            double[] CountMATERNITY_7_LEAVE = new double[keyValuePairsCount]; ;
            double[] CountOT_LAST_YEAR_LEAVE = new double[keyValuePairsCount]; ;
            double[] CountSICK_LEAVE = new double[keyValuePairsCount]; ;
            double[] CountWITHOUT_PAY_LEAVE = new double[keyValuePairsCount]; ;

            int iANNUAL_LEAVE = 0;
            foreach (KeyValuePair<string, int> keyValuePair in keyValuePairs)
            {
                string account = keyValuePair.Key;
                if(account == "Hai")
                {
                    int t = 0;
                }
                CountANNUAL_LEAVE[iANNUAL_LEAVE] = 
                leaveRequests.Where(s => s.Account == keyValuePair.Key &&
                    s.TypeOfLeave == TYPE_OF_LEAVE_REQUEST.ANNUAL_LEAVE
                ).Count();

                memberOtHours[iANNUAL_LEAVE] = CompensativeLeave.Instance.
                    GetNoCompensativeLeaveDay(keyValuePair.Key);

                CountCOMPESATIVE_LEAVE[iANNUAL_LEAVE] = CompensativeLeave.Instance
                    .GetCountLeaveByAccount(TYPE_OF_LEAVE_REQUEST.COMPESATIVE_LEAVE, account);
                CountANNUAL_LEAVE_OFF[iANNUAL_LEAVE] = keyValuePair.Value - CountANNUAL_LEAVE[iANNUAL_LEAVE];
                CountCOMPESATIVE_LEAVE_OFF[iANNUAL_LEAVE] = memberOtHours[iANNUAL_LEAVE] - CountCOMPESATIVE_LEAVE[iANNUAL_LEAVE];

                CountCOMPASSTIONATE_LEAVE[iANNUAL_LEAVE] = CompensativeLeave.Instance
                    .GetCountLeaveByAccount(TYPE_OF_LEAVE_REQUEST.COMPASSTIONATE_LEAVE, account);
               
                CountENGAGEMENT_LEAVE[iANNUAL_LEAVE] = CompensativeLeave.Instance
                    .GetCountLeaveByAccount(TYPE_OF_LEAVE_REQUEST.ENGAGEMENT_LEAVE, account);
                
                CountMATERNITY_3_LEAVE[iANNUAL_LEAVE] = CompensativeLeave.Instance
                    .GetCountLeaveByAccount(TYPE_OF_LEAVE_REQUEST.MATERNITY_3_LEAVE, account);
                
                CountMATERNITY_7_LEAVE[iANNUAL_LEAVE] = CompensativeLeave.Instance
                    .GetCountLeaveByAccount(TYPE_OF_LEAVE_REQUEST.MATERNITY_7_LEAVE, account);
                
                CountOT_LAST_YEAR_LEAVE[iANNUAL_LEAVE] = CompensativeLeave.Instance
                    .GetCountLeaveByAccount(TYPE_OF_LEAVE_REQUEST.OT_LAST_YEAR_LEAVE, account);
                
                CountSICK_LEAVE[iANNUAL_LEAVE] = CompensativeLeave.Instance
                    .GetCountLeaveByAccount(TYPE_OF_LEAVE_REQUEST.SICK_LEAVE, account);
                
                CountWITHOUT_PAY_LEAVE[iANNUAL_LEAVE] = CompensativeLeave.Instance
                    .GetCountLeaveByAccount(TYPE_OF_LEAVE_REQUEST.WITHOUT_PAY_LEAVE, account);

                iANNUAL_LEAVE++;



            }



            ViewBag.ANNUAL_LEAVE = CountANNUAL_LEAVE;
            ViewBag.ANNUAL_LEAVE_OFF = CountANNUAL_LEAVE_OFF;


            ViewBag.GetMemberCOMPESATIVEDays = memberOtHours;

            ViewBag.COMPESATIVE_LEAVE = CountCOMPESATIVE_LEAVE;
            ViewBag.COMPASSTIONATE_LEAVE = CountCOMPASSTIONATE_LEAVE;
            ViewBag.COMPESATIVE_LEAVE_OFF = CountCOMPESATIVE_LEAVE_OFF;
            ViewBag.ENGAGEMENT_LEAVE = CountENGAGEMENT_LEAVE;
            ViewBag.MATERNITY_3_LEAVE = CountMATERNITY_3_LEAVE;
            ViewBag.MATERNITY_7_LEAVE = CountMATERNITY_7_LEAVE;
            ViewBag.OT_LAST_YEAR_LEAVE = CountOT_LAST_YEAR_LEAVE;
            ViewBag.SICK_LEAVE = CountSICK_LEAVE;
            ViewBag.WITHOUT_PAY_LEAVE = CountWITHOUT_PAY_LEAVE;


            return View();
        }
        public async Task<ActionResult> Export2Excel()
        {
            using (var p = new ExcelPackage())
            {
                //A workbook must have at least on cell, so lets add one... 
                var ws = p.Workbook.Worksheets.Add("MySheet");
                //To set values in the spreadsheet use the Cells indexer.
                int rowStart = 2;
                ws.Cells[string.Format("A{0}", 1)].Value = "Account";
                ws.Cells[string.Format("B{0}", 1)].Value = "EmailAddress";
                ws.Cells[string.Format("C{0}", 1)].Value = "Leader EmailAddress";
                ws.Cells[string.Format("D{0}", 1)].Value = "Leave Date";
                ws.Cells[string.Format("E{0}", 1)].Value = "No Day Off";
                ws.Cells[string.Format("F{0}", 1)].Value = "Full/Am/Pm";
                ws.Cells[string.Format("G{0}", 1)].Value = "Type Of Leave";
                ws.Cells[string.Format("H{0}", 1)].Value = "Reason For Leave";
                ws.Cells[string.Format("I{0}", 1)].Value = "status";
                foreach (LeaveRequest leaveRequest in excelUsed)
                {
                    string fap = "Fullday";
                    if(leaveRequest.FullAmPm == Constants.AM)
                    {
                        fap = "AM";
                    }
                    else if(leaveRequest.FullAmPm == Constants.PM)
                    {
                        fap = "PM";
                    }

                    string status = "OPEN";
                    if(leaveRequest.Status == Constants.APPROVED)
                    {
                        status = "APPROVED";
                    }
                    else if (leaveRequest.Status == Constants.REJECTED)
                    {
                        status = "REJECTED";
                    }

                    ws.Cells[string.Format("A{0}",rowStart)].Value = leaveRequest.Account;
                    ws.Cells[string.Format("B{0}", rowStart)].Value = leaveRequest.EmailAddress;
                    ws.Cells[string.Format("C{0}", rowStart)].Value = leaveRequest.LeaderEmailAddress;
                    ws.Cells[string.Format("D{0}", rowStart)].Value = leaveRequest.LeaveDate.ToString("MM/dd/yyyy");
                    ws.Cells[string.Format("E{0}", rowStart)].Value = leaveRequest.NoDayOff;
                    ws.Cells[string.Format("F{0}", rowStart)].Value = fap;
                    ws.Cells[string.Format("G{0}", rowStart)].Value = leaveRequest.TypeOfLeave;
                    ws.Cells[string.Format("H{0}", rowStart)].Value = leaveRequest.ReasonForLeave;
                    ws.Cells[string.Format("I{0}", rowStart)].Value = status;
                    rowStart++;
                }



                //Save the new workbook. We haven't specified the filename so use the Save as method.
                //p.SaveAs(new FileInfo(@"c:\workbooks\myworkbook.xlsx"));
                try
                {
                    var filename = @"REPORT_" + DateTime.Now.ToString("dd-MM-yyyy_hh-mm-ss") + ".xlsx";
                    Response.Clear();
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment: filename=" + filename);
                    Response.BinaryWrite(p.GetAsByteArray());
                    Response.End();
                }
                catch (Exception ex)
                {
                    // any error handling mechanism
                }
                finally
                {
                    //Application.CompleteRequest();
                }
            }
            return RedirectToAction("Index");
        }

    }
}
