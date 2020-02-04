using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using WebApplication2LeaveAndOverTimeReqestToolHasLogin.Models;

namespace WebApplication2LeaveAndOverTimeReqestToolHasLogin.Utils
{
    public class Csv
    {

        public static List<Leader> GetLeader()
        {
            List<Leader> leaders = new List<Leader>();
            string path = HostingEnvironment.MapPath("~/App_Data/LqaLeader.csv");
            List<string> csvfile1Text = System.IO.File.ReadAllLines(path).ToList();
            foreach(string aline in csvfile1Text)
            {
                string[] line = aline.Split(',');
                string account = line[0];
                string email = line[1];
                leaders.Add(new Leader() {Account = account, EmailAddress = email });
            }
            return leaders;
        }
        public static List<string> GetLeaderAccount()
        {
            string path = HostingEnvironment.MapPath("~/App_Data/LqaLeader.csv");
            List<string> csvfile1Text = System.IO.File.ReadAllLines(path).ToList();
            List<string> result = new List<string>();
            foreach (string aline in csvfile1Text)
            {
                string[] line = aline.Split(',');
                string account = line[0];
                result.Add(account);
            }
            return result;
        }
        public static List<Leader> GetLeaderInfo()
        {
            string path = HostingEnvironment.MapPath("~/App_Data/LqaLeader.csv");
            List<string> csvfile1Text = System.IO.File.ReadAllLines(path).ToList();
            List<Leader> leaders = new List<Leader>();
            foreach (string aline in csvfile1Text)
            {
                string[] line = aline.Split(',');
                string account = line[0];
                string email = line[1];
                leaders.Add(new Leader() { Account = account, EmailAddress = email });
            }
            return leaders;
        }

        public static List<string> GetMemberAccount()
        {
            string path = HostingEnvironment.MapPath("~/App_Data/Member list 200120.csv");
            List<string> FileReadAllLinesToList = System.IO.File.ReadAllLines(path).ToList();
            List<string> result = new List<string>();
            foreach (string aline in FileReadAllLinesToList)
            {
                string[] alineSplit = aline.Split(',');
                string account = alineSplit[1];
                result.Add(account);
            }
            return result;
        }
        public static List<KeyValuePair<string, int>> GetMemberLeaveDays()
        {
            string path = HostingEnvironment.MapPath("~/App_Data/nghiPhep2020.csv");
            List<string> FileReadAllLinesToList = System.IO.File.ReadAllLines(path).ToList();
            List<KeyValuePair<string, int>> MemberOtHour = new List<KeyValuePair<string, int>>();
            foreach (string aline in FileReadAllLinesToList)
            {
                string[] alineSplit = aline.Split(',');
                string account = alineSplit[0];
                int noHour = int.Parse(alineSplit[1]);
                MemberOtHour.Add(new KeyValuePair<string, int>(account, noHour));
            }
            return MemberOtHour;
        }
    }
}