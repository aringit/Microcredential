using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace a2cMessages
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async System.Threading.Tasks.Task RunAsync([ServiceBusTrigger("a2cmessage", Connection = "ServiceBusConnection")]string myQueueItem, ILogger log)
        {
            // log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
            a2cSendMessage _clsRes = JsonConvert.DeserializeObject<a2cSendMessage>(myQueueItem);
            string con = "Server=tcp:microauclidb.database.windows.net,1433;Initial Catalog=ClientDB;Persist Security Info=False;User ID=cog;Password=arin@123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();
                var text = "INSERT INTO [dbo].[MessageDetails](CommentDetails,CID,AUID,CommentedBy,TSKID) VALUES('" + _clsRes.CommentDetails.ToString() + "'," + _clsRes.Cid + ",'" + _clsRes.Auid + "','" + _clsRes.CommentedBy + "'," + _clsRes.Tskid + ")";


                using (SqlCommand cmd = new SqlCommand(text, conn))
                {
                    // Execute the command and log the # rows affected.
                    await cmd.ExecuteNonQueryAsync();
                    //log.LogInformation($"{rows} rows were updated");
                }
            }
        }
        private class a2cSendMessage
        {

            public long Mid { get; set; }          
            public string CommentDetails { get; set; }           
            public long Cid { get; set; }          
            public string Auid { get; set; }           
            public string CommentedBy { get; set; }           
            public long Tskid { get; set; }
        }
    }
}
