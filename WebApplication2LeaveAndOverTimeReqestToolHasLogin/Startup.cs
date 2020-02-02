using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using System.Collections.Generic;
using WebApplication2LeaveAndOverTimeReqestToolHasLogin.Models;

[assembly: OwinStartupAttribute(typeof(WebApplication2LeaveAndOverTimeReqestToolHasLogin.Startup))]
namespace WebApplication2LeaveAndOverTimeReqestToolHasLogin
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            //CreateRolesandUsers();
        }
        private void CreateRolesandUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));


            // In Startup iam creating first Admin Role and creating a default Admin User     
            if (!roleManager.RoleExists("Admin"))
            {

                // first we create Admin rool    
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);

                //Here we create a Admin super user who will maintain the website                   

                //var user = new ApplicationUser();
                //user.UserName = "toannl";
                //user.Email = "toannl@lqa.com.vn";

                //string userPWD = "zzzZ2@";

                //var chkUser = UserManager.Create(user, userPWD);

                ////Add default User to Role Admin    
                //if (chkUser.Succeeded)
                //{
                //    var result1 = UserManager.AddToRole(user.Id, "Admin");

                //}
            }

            // creating Creating Manager role     
            if (!roleManager.RoleExists("Manager"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Manager";
                roleManager.Create(role);

            }

            // creating Creating Employee role     
            if (!roleManager.RoleExists("Employee"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Employee";
                roleManager.Create(role);

            }
            bool flag = false;
            if (flag)
            {
                List<string> emails = new List<string>()
                { "Xuan","Hai","Thuy","Thuy.Nguyen","Tien.Vu","LanTT","Dung.Phan","Bach.Nguyen","Trong.Le","Khiem.Nguyen","TrangTQ","AnhNTN","GiangDT","HungKM","HienNTM","DuongDN","QuynhDTH",
                    "AnhNTT","AnTD","YenTT","QuynhHT","GiangNT","HuyenHT","VietPT","LongTD","NganNK",
                    //"ToanNL",
                    "HangTT","VanDT","HueNT","LinhLTD","NganPTT","ThuyTN","NhanDT","HaoPA","PhongDT",
                    "TramTTN","ThinhNT","HoaNT","DungNT","ChauVM","HuongTTT","ThamVT","LeTTN","MaiNDC","Hanh.Do","HanhNT","ThanhNV","NamNH","TrangVT","NamTH","TanNC","ThanhNH","TienPD","HoaNTT",
                    "VanTTT","NgocVH","TienTV","BaoNQ","NganLTK","MaiNH","MinhNT","GiangNT1","DungMNT","LyNT","NgaNTD","DuongTV","HoangNN","HueBT","NgocTTB","TrangKTT","DuongLT","QuynhNN","ThuyCV",
                    "NhungTT","DuyetPT","DuyNT","PhuongNT1","DungTTT",
                };
                List<string> leaderAccounts = Utils.Csv.GetLeaderAccount();

                foreach (string email in emails)
                {
                    var user = new ApplicationUser();
                    user.UserName = email + "@lqa.com.vn";
                    user.Email = user.UserName;

                    string userPWD = "zzzZ2@";
                    ApplicationUser existUser = null;
                    existUser = UserManager.FindByName(user.UserName);
                    if (existUser != null)
                        continue;

                    existUser = UserManager.FindByEmail(user.Email);
                    if (existUser != null)
                        continue;
                    var chkUser = UserManager.Create(user, userPWD);
                    //Add default User to Role Admin    
                    if (chkUser.Succeeded)
                    {
                        var result1 = UserManager.AddToRole(user.Id, "Employee");

                    }
                    else
                    {
                        var r = 0;
                    }
                }

            }
        }
        
    }
}
