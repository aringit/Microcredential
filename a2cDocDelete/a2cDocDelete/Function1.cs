using System;
using System.Data.SqlClient;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace a2cDocDelete
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async System.Threading.Tasks.Task RunAsync([ServiceBusTrigger("a2cdocdelete", Connection = "ServiceBusConnection")]string myQueueItem, ILogger log)
        {
            
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
            TaskDocument _clsRes = JsonConvert.DeserializeObject<TaskDocument>(myQueueItem);
            string con = "Server=tcp:microauclidb.database.windows.net,1433;Initial Catalog=ClientDB;Persist Security Info=False;User ID=cog;Password=arin@123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();
                var text = "DELETE [dbo].[TaskDocument] WHERE DocumentName=" + "'" + _clsRes.DocumentName.ToString() + "'" ;
                // "// + "AND CID=" + _clsRes.Cid + "," + _clsRes.Tskid + ",'" + _clsRes.UplodedBy + "';
                //SqlCommand SqlComm = new SqlCommand();
                //SqlComm.Connection = conn;
                //SqlComm = new SqlCommand("DELETE [dbo].[TaskDocument] WHERE DocumentName=='@Documentname' and CID='@CID' and TSKID='@TSKID' and UplodedBy='@Uploadedby", conn);
                //SqlComm.Parameters.AddWithValue("@Documentname", _clsRes.DocumentName.ToString());
                //SqlComm.Parameters.AddWithValue("@CID", _clsRes.Cid);
                //SqlComm.Parameters.AddWithValue("@TSKID", _clsRes.Tskid);
                //SqlComm.Parameters.AddWithValue("@Uploadedby", _clsRes.UplodedBy.ToString());
                //await SqlComm.ExecuteNonQueryAsync();

                using (SqlCommand cmd = new SqlCommand(text, conn))
                {
                    // Execute the command and log the # rows affected.
                    await cmd.ExecuteNonQueryAsync();
                    //log.LogInformation($"{rows} rows were updated");
                }
            }
        }
        public class TaskDocument
        {

            public long Docuid { get; set; }
            public string DocumentName { get; set; }
            public long Cid { get; set; }
            public long Tskid { get; set; }
            public string UplodedBy { get; set; }
            public byte[] FileData { get; set; }
            public string FileType { get; set; }
            public string Filestream { get; set; }
        }
    }
}
