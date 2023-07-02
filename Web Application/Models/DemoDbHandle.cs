using System.Data;
using System.Data.SqlClient;
using System.Text;
using Web_Application.ModelViews;

namespace Web_Application.Models
{
    public class DemoDbHandle
    {
        private SqlConnection con;
        private void connection()
        {
            string constring = "Data Source=DESKTOP-3P1U2GV\\OMSSERVER;Initial Catalog=SupportDB;Integrated Security=True;Pooling=False";
            con = new SqlConnection(constring);
        }

        public List<DemoMV> GetDemoDetail()
        {
            connection();
            List<DemoMV> list =new List<DemoMV>();
            SqlCommand cmd = new SqlCommand("select d.DemoId,d.QuatationPrice,d.SoftwareType,d.TotalUser,d.NoOfBranch,d.NoOfCompany,d.SaleStage,d.ClientFeedBack,MarketedBy,d.implementedBy,FORMAT(d.FollowUpDate, 'yyyy-MM-dd') as FollowUpDate,c.CompanyId, c.CompanyName from Demo d join CompanyInfo c on c.CompanyId = d.CompanyId", con);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable st = new DataTable();
            con.Open();
            adapter.Fill(st);
            con.Close();
            foreach (DataRow dr in st.Rows)
            {
                list.Add(
                    new DemoMV
                    {
                        Id = Convert.ToInt32(dr["DemoId"]),
                        CompanyName = Convert.ToString(dr["CompanyName"]),
                        CompanyId = Convert.ToInt32(dr["CompanyId"]),
                        QuatationPrice = Convert.ToInt32(dr["QuatationPrice"]),
                        SoftwareType = Convert.ToString(dr["SoftwareType"]),
                        TotalUser = Convert.ToInt32(dr["TotalUser"]),
                        NoOfBranch = Convert.ToInt32(dr["NoOfBranch"]),
                        NoOfCompany = Convert.ToInt32(dr["NoOfCompany"]),
                        SaleStage = Convert.ToString(dr["SaleStage"]),
                        FeedBack = Convert.ToString(dr["ClientFeedBack"]),
                        MarketedBy = Convert.ToString(dr["MarketedBy"]),
                        ImplementedBy = Convert.ToString(dr["ImplementedBy"]),
                        FollowUpDate = Convert.ToString(dr["FollowUpDate"]),
                    }
                );
            }
            return list;
        }

        public bool CreateDemo(DemoMV vm, List<string> ac)
        {
            connection();
            StringBuilder str = new StringBuilder();
            str.Append(" declare @did bigint \n");
            str.Append("declare @aid int \n");
            str.Append(" set @did = (select isnull(max(demoid), 0) + 1 from Demo) \n");
            if (vm.Id != null)
            {
                if (vm.FollowUpDate != null)
                {
                    str.Append("update Demo set  QuatationPrice=" + vm.QuatationPrice + ", SoftwareType='" + vm.SoftwareType + "', TotalUser=" + vm.TotalUser + ", NoOfBranch=" + vm.NoOfBranch + ", NoOfCompany = " + vm.NoOfCompany + ", SaleStage = '" + vm.SaleStage + "', ClientFeedBack = '" + vm.FeedBack + "', MarketedBy = '" + vm.MarketedBy + "', ImplementedBy='" + vm.ImplementedBy + "', FollowUpDate='" + vm.FollowUpDate + "' where DemoId =" + vm.Id + " \n");

                }
                else
                {
                    str.Append("update Demo set  QuatationPrice=" + vm.QuatationPrice + ", SoftwareType='" + vm.SoftwareType + "', TotalUser=" + vm.TotalUser + ", NoOfBranch=" + vm.NoOfBranch + ", NoOfCompany = " + vm.NoOfCompany + ", SaleStage = '" + vm.SaleStage + "', ClientFeedBack = '" + vm.FeedBack + "', MarketedBy = '" + vm.MarketedBy + "', ImplementedBy='" + vm.ImplementedBy + "' where DemoId =" + vm.Id + " \n");

                }
            }
            else
            {
                if(vm.FollowUpDate != null)
                {
                    str.Append("insert into Demo(DemoId, QuatationPrice, SoftwareType, TotalUser, NoOfBranch, NoOfCompany, SaleStage, ClientFeedBack,MarketedBy, ImplementedBy, FollowUpDate, CompanyId) values(@did, " + vm.QuatationPrice + ",'" + vm.SoftwareType + "'," + vm.TotalUser + "," + vm.NoOfBranch + "," + vm.NoOfCompany + ",'" + vm.SaleStage + "','" + vm.FeedBack + "','" + vm.MarketedBy + "','" + vm.ImplementedBy + "','" + vm.FollowUpDate + "'," + vm.CompanyId + ")\n");
                }
                else
                {
                    str.Append("insert into Demo(DemoId, QuatationPrice, SoftwareType, TotalUser, NoOfBranch, NoOfCompany, SaleStage, ClientFeedBack,MarketedBy, ImplementedBy, CompanyId) values(@did, " + vm.QuatationPrice + ",'" + vm.SoftwareType + "'," + vm.TotalUser + "," + vm.NoOfBranch + "," + vm.NoOfCompany + ",'" + vm.SaleStage + "','" + vm.FeedBack + "','" + vm.MarketedBy + "','" + vm.ImplementedBy + "'," + vm.CompanyId + ")\n");

                }
            }
            foreach(var data in ac)
            {
                str.Append("set @aid = (select isnull(max(AttachmentId), 0) + 1 from Attachments) \n");
                str.Append("insert into Attachments(AttachmentId, AttachmentName, DemoId) values(@aid, '" + data + "', @did) \n");
            }
            SqlCommand cmd = new SqlCommand(str.ToString(),con);
            con.Open();
            var i = cmd.ExecuteNonQuery();
            con.Close();
            if (i >= 1)
                return true;
            else
                return false;
        }
        public List<AttachmentVM> GetAttachments(int id)
        {
            connection();
            List<AttachmentVM> list = new List<AttachmentVM>();
            SqlCommand cmd = new SqlCommand("select * from Attachments where DemoId=" + id, con);
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            ad.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                list.Add(
                    new AttachmentVM()
                    {
                        Id = Convert.ToInt32(dr["AttachmentId"]),
                        AttachmentName = Convert.ToString(dr["AttachmentName"]),
                    });
            }
            return list;
        }
    }
}
