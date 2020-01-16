namespace WebApplication2LeaveAndOverTimeReqestToolHasLogin.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_leave : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LeaveRequests",
                c => new
                    {
                        LeaveRequestID = c.Int(nullable: false, identity: true),
                        TimeStamp = c.DateTime(nullable: false),
                        Account = c.String(),
                        EmailAddress = c.String(),
                        LeaderAccount = c.String(),
                        LeaderEmailAddress = c.String(),
                        LeaveDate = c.DateTime(nullable: false),
                        NoDayOff = c.Double(nullable: false),
                        FullAmPm = c.Int(nullable: false),
                        TypeOfLeave = c.String(),
                        ReasonForLeave = c.String(),
                        Status = c.Int(nullable: false),
                        LeaderComment = c.String(),
                        Month = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.LeaveRequestID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.LeaveRequests");
        }
    }
}
