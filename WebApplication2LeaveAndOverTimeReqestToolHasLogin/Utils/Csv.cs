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
    }
}