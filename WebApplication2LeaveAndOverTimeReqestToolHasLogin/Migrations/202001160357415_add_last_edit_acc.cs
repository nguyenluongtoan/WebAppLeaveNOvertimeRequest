namespace WebApplication2LeaveAndOverTimeReqestToolHasLogin.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_last_edit_acc : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LeaveRequests", "LastEditedByAccount", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.LeaveRequests", "LastEditedByAccount");
        }
    }
}
