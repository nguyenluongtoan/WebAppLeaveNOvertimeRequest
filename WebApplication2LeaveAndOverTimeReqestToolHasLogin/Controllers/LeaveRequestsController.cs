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

namespace WebApplication2LeaveAndOverTimeReqestToolHasLogin.Controllers
{
    
    public class LeaveRequestsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        [Authorize]
        // GET: LeaveRequests
        public async Task<ActionResult> Index(string sortOrder, string searchString)
        {
            // add col sort link
            //      ViewBag.NameSortParm
            // end add col sort link
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "account_desc" : "";
            ViewBag.DateSortParm = sortOrder == "LeaveDate" ? "date_desc" : "Date";
            var leaveRequests = from s in db.LeaveRequests select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                leaveRequests = leaveRequests.Where(s => s.Account.Contains(searchString) || s.EmailAddress.Contains(searchString)
                                       || s.LeaderAccount.Contains(searchString) || s.LeaderEmailAddress.Contains(searchString)
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
            return View(await leaveRequests.ToListAsync());
        }

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
        public ActionResult Create()
        {
            ViewBag.AllLeaders = Utils.Csv.GetLeaderAccount();
            ViewBag.AllLeaders2 = Utils.Csv.GetLeaderInfo();
            ViewBag.Type = new int[3] { 1, 2, 3 };
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
            ViewBag.Type = new int[3] { 1, 2, 3 };
            leaveRequest.TimeStamp = DateTime.Now;
            leaveRequest.Status = Constants.OPEN;
            leaveRequest.Month = 0;
            leaveRequest.LastEditedByAccount = leaveRequest.Account;

            if (ModelState.IsValid)
            {
                db.LeaveRequests.Add(leaveRequest);
                await db.SaveChangesAsync();
                await Mail.SendCreatedReqMail2Member(leaveRequest);
                await Mail.SendCreatedReqMail2Leader(leaveRequest, Constants.LEADER);
                //await Mail.SendCreatedReqMail2Leader(leaveRequest, Constants.HR);
                return RedirectToAction("Index");
            }

            return View(leaveRequest);
        }
        [Authorize]
        // GET: LeaveRequests/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LeaveRequest leaveRequest = await db.LeaveRequests.FindAsync(id);
            ViewBag.AllLeaders = Utils.Csv.GetLeaderAccount();
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
        [Authorize]
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
                await Mail.SendNotice(leaveRequest1, Constants.LEADER, Constants.REJECTED);
                //await Mail.SendNotice(leaveRequest, Constants.HR, Constants.APPROVED);
                return RedirectToAction("Index");
            }
            return View(leaveRequest);
        }
        [Authorize]
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
            await Mail.SendNotice(leaveRequest, Constants.LEADER, Constants.APPROVED);
            //await Mail.SendNotice(leaveRequest, Constants.HR, Constants.APPROVED);
            return RedirectToAction("Index");
        }
        //unused, using ApproveOrReject HttpPost
        [Authorize]
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
            await Mail.SendNotice(leaveRequest, Constants.LEADER, Constants.REJECTED);
            //await Mail.SendNotice(leaveRequest, Constants.HR, Constants.REJECTED);
            return RedirectToAction("Index");
        }
        // GET: LeaveRequests/Delete/5
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
    }
}
