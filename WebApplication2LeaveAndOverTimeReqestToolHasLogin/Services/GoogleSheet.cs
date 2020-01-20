using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Hosting;

namespace WebApplication2LeaveAndOverTimeReqestToolHasLogin.Services
{
    //https://www.youtube.com/watch?v=afTiNU6EoA8 
    public class GoogleSheet
    {
        public GoogleSheet()
        {
            
        }
        public string Url { get; set; }
        public string SheetId{ get; set; }
        public string TabName { get; set; }
        //static string[] Scopes = { SheetsService.Scope.SpreadsheetsReadonly };
        static string[] Scopes = { SheetsService.Scope.Spreadsheets };
        static string ApplicationName = "Google Sheets API .NET Quickstart";
        public string SpreadSheetId { get; set; }
        //public string SpreadSheetId = "1lYBNEs-YhKCHwYCFeHQNZXM1wW1NnJH6LUsr8aix0-U";
        public string Range { get; set; }
        public void Init()
        {
            FirstTime = true;
            SetSpreadSheetIdFromUrl();
        }
        public bool FirstTime { get; set; }
        public void SetSpreadSheetIdFromUrl()
        {
            string[] seg = new Uri(Url).Segments;
            SpreadSheetId = seg[3];
            //1lYBNEs-YhKCHwYCFeHQNZXM1wW1NnJH6LUsr8aix0-U
        }
        IList<IList<Object>> IListIListObject;
        public IList<IList<Object>> ResponseValues()
        {
            Init();
            if (FirstTime)
            {
                FirstTime = false;
                UserCredential credential;
                string path = HostingEnvironment.MapPath("~/App_Data/credentials.json");
                using (var stream =
                    new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    string credPath = HostingEnvironment.MapPath("~/App_Data/token.json");
                    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.Load(stream).Secrets,
                        Scopes,
                        "user",
                        CancellationToken.None,
                        new FileDataStore(credPath, true)).Result;
                }

                //GoogleCredential credential;
                //string path = HostingEnvironment.MapPath("~/App_Data/My Project 67047-0ff6009988f1.json");
                //using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
                //{
                //    credential = GoogleCredential.FromStream(stream).CreateScoped(Scopes);
                //}
                var service = new SheetsService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName,
                });

                string range = TabName + "!" + Range;
                SpreadsheetsResource.ValuesResource.GetRequest request =
                        service.Spreadsheets.Values.Get(SpreadSheetId, range);
                ValueRange response = request.Execute();
                IListIListObject = response.Values;

            }
            return IListIListObject;
        }
        
    }
}