using System;
using System.Data.SqlClient;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace a2cTask
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async System.Threading.Tasks.Task RunAsync([ServiceBusTrigger("a2c", Connection = "ServiceBusConnection")]string myQueueItem, ILogger log)
        {
            // log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
            //var _jsonObject= JsonConvert.DeserializeObject<RespondMessage>(myQueueItem);
            TaskToClient _clsRes = JsonConvert.DeserializeObject<TaskToClient>(myQueueItem);
            //_clsRes.Auid = _jsonObject[];
            //_clsRes.Cid =JsonArrayAttribute;
            //_clsRes.Comment =;
            //_clsRes.Docuid =;

            string con = "Server=tcp:microauclidb.database.windows.net,1433;Initial Catalog=ClientDB;Persist Security Info=False;User ID=cog;Password=arin@123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();
                var text = "INSERT INTO TaskDetails(TSKID,TaskDescription,CID,PID,Id,Status) VALUES(" + _clsRes.Tskid + ",'" + _clsRes.TaskDescription.ToString() + "'," + _clsRes.Cid + "," + _clsRes.Pid + ",'" + _clsRes.Id + "','" + _clsRes.Status + "')";


                using (SqlCommand cmd = new SqlCommand(text, conn))
                {
                    // Execute the command and log the # rows affected.
                    await cmd.ExecuteNonQueryAsync();
                    //log.LogInformation($"{rows} rows were updated");
                }
            }
            // log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
        }

        private class TaskToClient
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
