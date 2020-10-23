using System;
using System.Data.SqlClient;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace a2cUT
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async System.Threading.Tasks.Task RunAsync([ServiceBusTrigger("a2cut", Connection = "ServiceBusConnection")]string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
            
            UpdateTaskToClient _clsRes = JsonConvert.DeserializeObject<UpdateTaskToClient>(myQueueItem);
          
            string con = "Server=tcp:microauclidb.database.windows.net,1433;Initial Catalog=ClientDB;Persist Security Info=False;User ID=cog;Password=arin@123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();
                var text = "Update TaskDetails SET [Status]="+ "'" + _clsRes.Status + "'" +" where TSKID= "+  _clsRes.Tskid +" and CID= "+ _clsRes.Cid ;
                   
                //"VALUES(" + _clsRes.Tskid + ",'" + _clsRes.TaskDescription.ToString() + "'," + _clsRes.Cid + "," + _clsRes.Pid + ",'" + _clsRes.Id + "','" + _clsRes.Status + "')";

                using (SqlCommand cmd = new SqlCommand(text, conn))
                {
                    // Execute the command and log the # rows affected.
                    await cmd.ExecuteNonQueryAsync();
                    //log.LogInformation($"{rows} rows were updated");
                }
            }
            // log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
        }

        private class UpdateTaskToClient
        {

            public long Tskid { get; set; }
            public string TaskDescription { get; set; }
            public long Cid { get; set; }
            public long Pid { get; set; }
            public string Id { get; set; }
            public string Status { get; set; }
        }
    }
 }

