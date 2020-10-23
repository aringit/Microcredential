using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace c2aDoc
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async System.Threading.Tasks.Task RunAsync([ServiceBusTrigger("c2adoc", Connection = "ServiceBusConnection")]string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
            TaskDocument _clsRes = JsonConvert.DeserializeObject<TaskDocument>(myQueueItem);
            string con = "Server=tcp:microauclidb.database.windows.net,1433;Initial Catalog=AuditorDB;Persist Security Info=False;User ID=cog;Password=arin@123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();
                var text = "INSERT INTO [dbo].[TaskDocument](DocumentName,CID,TSKID,UplodedBy,FileType) VALUES('" + _clsRes.DocumentName.ToString() + "'," + _clsRes.Cid + "," + _clsRes.Tskid + ",'" + _clsRes.UplodedBy + "','" + _clsRes.FileType + "')";


                using (SqlCommand cmd = new SqlCommand(text, conn))
                {
                    // Execute the command and log the # rows affected.
                    await cmd.ExecuteNonQueryAsync();
                    //log.LogInformation($"{rows} rows were updated");
                }
            }
        }
        public  class TaskDocument
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
