using DataAnnotationsExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication2LeaveAndOverTimeReqestToolHasLogin.Models
{
    public class LeaveRequest
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LeaveRequestID { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Account { get; set; }
        [DataType(DataType.EmailAddress)]
        [Email]
        public string EmailAddress { get; set; }
        [Display(Name = "Leader Account")]
        public string LeaderAccount { get; set; }
        [DataType(DataType.EmailAddress)]
        [Email]
        [Display(Name = "Leader Email Address")]
        public string LeaderEmailAddress { get; set; }
        [Display(Name = "Leave Date")]
        [DataType(DataType.Date)]
        public DateTime LeaveDate { get; set; }
        [Display(Name = "No Day Off")]
        public double NoDayOff { get; set; }
        [Display(Name = "Time Slot")]
        public int FullAmPm { get; set; }
        [Display(Name = "Type Of Leave")]
        public string TypeOfLeave { get; set; }
        [Display(Name = "Reason For Leave")]
        public string ReasonForLeave { get; set; }
        [Display(Name = "Status")]
        public int Status { get; set; }
        [Display(Name = "Leader Comment")] 
        public string LeaderComment { get; set; }
        public string LastEditedByAccount { get; set; }
        public int Month { get; set; }
        
    }
}