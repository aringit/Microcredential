using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JWTAuthentication.Models;
using System.Text;
using System.Threading;
//using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Azure.ServiceBus;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;

namespace JWTAuthentication.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]

    public class AuditorPortfoliosController : ControllerBase
    {
        private readonly AuditorDBContext _context;
        private QueueClient queueClient;

        public AuditorPortfoliosController(AuditorDBContext context)
        {
            _context = context;
        }

        // GET: api/AuditorPortfolios
        [HttpGet]
        [Authorize]
        [ActionName("GetAuditorPortfolio")]
        public async Task<ActionResult<IEnumerable<AuditorPortfolio>>> GetAuditorPortfolio()
        {
            return await _context.AuditorPortfolio.ToListAsync();
        }

        [HttpGet]
        [Authorize]
        [ActionName("GetMessageDetails")]
        public async Task<ActionResult<IEnumerable<MessageDetails>>> GetMessageDetails()
        {
            return await _context.MessageDetails.ToListAsync();
        }

        [HttpGet]
        [Authorize]
        [ActionName("GetAuditorDetails")]
        public async Task<ActionResult<IEnumerable<AspNetUsers>>> GetAuditorDetails()
        {
            return await _context.AspNetUsers.ToListAsync();
        }

        [HttpGet]
        [Authorize]
        [ActionName("GetAspNetUsers")]
        public async Task<ActionResult<IEnumerable<AspNetUsers>>> GetAspNetUsers()
        {
            return await _context.AspNetUsers.ToListAsync();
        }

        [HttpGet("{username}")]
        [Authorize]
        [ActionName("GetUserDetails")]
        public async Task<ActionResult<IEnumerable<AspNetUsers>>> GetUserDetails(string username)
        {

            var userDetails = from g in _context.AspNetUsers where g.UserName == username select g;
            if (userDetails == null)
            {
                return NotFound();
            }
            return await userDetails.ToListAsync();

        }

        [HttpGet("{id}")]
        [Authorize]
        [ActionName("GetTaskDetailsByClient")]
        public async Task<ActionResult<IEnumerable<TaskDetails>>> GetTaskDetailsByClient(long id)
        {

            var TaskDetails = from g in _context.TaskDetails where g.Cid == id && g.Status=="Active" select g;
            if (TaskDetails == null)
            {
                return NotFound();
            }
            return await TaskDetails.ToListAsync();
        }


        [HttpGet("{id}")]
        [Authorize]
        [ActionName("GetTaskDetailsByPortfolio")]
        public async Task<ActionResult<IEnumerable<TaskDetails>>> GetTaskDetailsByPortfolio(long id)
        {

            var TaskDetails = from g in _context.TaskDetails where g.Pid == id select g;
            if (TaskDetails == null)
            {
                return NotFound();
            }
            return await TaskDetails.ToListAsync();

        }
        [HttpGet("{id}")]
        [Authorize]
        [ActionName("GetClientsByPortfolio")]
        public async Task<ActionResult<IEnumerable<ClientDetails>>> GetClientsByPortfolio(long id)
        {

            var clientDetails = from g in _context.ClientDetails where g.Pid == id select g;
            if (clientDetails == null)
            {
                return NotFound();
            }
            return await clientDetails.ToListAsync();

        }
        [HttpGet("{id}")]
        [Authorize]
        [ActionName("GetCommentsByClient")]
        public async Task<ActionResult<IEnumerable<AuditorCommentDetails>>> GetCommentsByClient(long id,string AUID)
        {

            var auditorCommentsDetails = from g in _context.AuditorCommentDetails where g.Cid == id && g.Auid==AUID  select g;
            if (auditorCommentsDetails == null)
            {
                return NotFound();
            }
            return await auditorCommentsDetails.ToListAsync();

        }
        [HttpGet("{id}")]
        [Authorize]
        [ActionName("GetCommentsByTaskID")]
        public async Task<ActionResult<IEnumerable<AuditorCommentDetails>>> GetCommentsByTaskID(long id)
        {

            var auditorCommentsDetails = from g in _context.AuditorCommentDetails where g.Tskid == id select g;
            if (auditorCommentsDetails == null)
            {
                return NotFound();
            }
            return await auditorCommentsDetails.ToListAsync();

        }

        [HttpGet("{id}")]
        [Authorize]
        [ActionName("GetMessagesByTaskID")]
        public async Task<ActionResult<IEnumerable<MessageDetails>>> GetMessagesByTaskID(long id)
        {

            var auditorMessageDetails = from g in _context.MessageDetails where g.Tskid == id select g;
            //if (auditorCommentsDetails == null)
            //{
            //    return NotFound();
            //}
            return await auditorMessageDetails.ToListAsync();

        }


        [HttpGet("{id}")]
        [Authorize]
        [ActionName("GetDocumentsByTaskID")]
        public async Task<ActionResult<IEnumerable<TaskDocument>>> GetDocumentsByTaskID(long id)
        {

            var taskDocumentDetails = from g in _context.TaskDocument where g.Tskid == id select g;
            if (taskDocumentDetails == null)
            {
                return NotFound();
            }
            return await taskDocumentDetails.ToListAsync();

        }

        // GET: api/AuditorPortfolios/5
        [HttpGet("{id}")]
        [Authorize]
        [ActionName("GetAuditorPortfolio")]
        public async Task<ActionResult<AuditorPortfolio>> GetAuditorPortfolio(long id)
        {
            var auditorPortfolio = await _context.AuditorPortfolio.FindAsync(id);

            if (auditorPortfolio == null)
            {
                return NotFound();
            }

            return auditorPortfolio;
        }

        [HttpGet("{id}")]
        [Authorize]
        [ActionName("GetMessagebyTSKID")]
        public async Task<IList<MessageDetails>> GetMessagebyTSKID(long id)
        {
            var messageDetails = from g in _context.MessageDetails where g.Cid == id select g;
            //var commentsDetails =await  _context.AuditorClientComments.Select(x => x.Cid == id).ToListAsync();

            //if (commentsDetails == null)
            //{
            //    return NotFound();
            //}
            return await messageDetails.ToListAsync();
        }

        // POST: api/AuditorPortfolios
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [Authorize]
        [ActionName("PostAuditorPortfolio")]
        public async Task<ActionResult<AuditorPortfolio>> PostAuditorPortfolio(AuditorPortfolio auditorPortfolio)
        {
            _context.AuditorPortfolio.Add(auditorPortfolio);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAuditorPortfolio", new { id = auditorPortfolio.Pid }, auditorPortfolio);
        }

     
        [HttpPost]
        [Authorize]
        [ActionName("PostTaskDetails")]
        public async Task<ActionResult<TaskDetails>> PostTaskDetails(TaskDetails _taskDetails)
        {
            _context.TaskDetails.Add(_taskDetails);
            await _context.SaveChangesAsync();

           #region "Service Bus Entry"

            const string ServiceBusConnectionString = "Endpoint=sb://auditor2client.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=BJtC7Tu9IhAH62XqaazRBI24PgeRj43cwbXAy0zbFLE=";                                                                           //access policy 
            const string QueueName = "a2c";
            queueClient = new QueueClient(ServiceBusConnectionString, QueueName);
            string clientRespondMessage = JsonConvert.SerializeObject(_taskDetails);
            var message = new Message(Encoding.UTF8.GetBytes(clientRespondMessage));
            await queueClient.SendAsync(message);

           #endregion

            return CreatedAtAction("GetAuditorPortfolio", new { id = _taskDetails.Tskid }, _taskDetails);
        }

        [HttpPost]
        [Authorize]
        [ActionName("PostMessageDetails")]
        public async Task<ActionResult<MessageDetails>> PostMessageDetails(MessageDetails _MessageDetails)
        {
            _context.MessageDetails.Add(_MessageDetails);
            await _context.SaveChangesAsync();

            #region "Service Bus Entry"

            const string ServiceBusConnectionString = "Endpoint=sb://auditor2client.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=BJtC7Tu9IhAH62XqaazRBI24PgeRj43cwbXAy0zbFLE=";                                                                           //access policy 
            const string QueueName = "a2cmessage";
            queueClient = new QueueClient(ServiceBusConnectionString, QueueName);
            string ShareMessage = JsonConvert.SerializeObject(_MessageDetails);
            var message = new Message(Encoding.UTF8.GetBytes(ShareMessage));
            await queueClient.SendAsync(message);

            #endregion

            return CreatedAtAction("GetMessageDetails", new { id = _MessageDetails.Mid }, _MessageDetails);
        }


        [HttpPost]
        [Authorize]
        [ActionName("TaskDocumentUpload")]
        public async Task<ActionResult<TaskDocument>> TaskDocumentUpload(TaskDocument _taskDocument)
        {
            TaskDocument taskDocument = new TaskDocument();
            taskDocument.Cid = _taskDocument.Cid;
            taskDocument.DocumentName = _taskDocument.DocumentName;
            taskDocument.Tskid = _taskDocument.Tskid;
            taskDocument.UplodedBy = _taskDocument.UplodedBy;
            Byte[] bytesFile = Convert.FromBase64String(_taskDocument.Filestream);
            byte[] tstArry = { 0, 1 };
            taskDocument.FileData = tstArry;
            taskDocument.FileType = _taskDocument.FileType;
            taskDocument.Filestream = "test";          

            _context.TaskDocument.Add(taskDocument);
            await _context.SaveChangesAsync();
            CreatedAtAction("GetAuditorPortfolio", new { id = _taskDocument.Tskid }, _taskDocument);
            #region "Blob Storage Upload"

            try
            {
                CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=mccog;AccountKey=oan1RKJMPe82KxZDhfHqZArWCSHzSukEdwl8VSQgSP+w3pxIvLkHEObL6iUwMbiSM9Kx6mcmx5ZYsj3lBQ29GQ==;EndpointSuffix=core.windows.net");
                CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                string strContainerName = "taskdocuments";
                CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(strContainerName);
               
                if (await cloudBlobContainer.CreateIfNotExistsAsync())
                {
                    await cloudBlobContainer.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
                }

                if (_taskDocument.DocumentName != null && bytesFile != null)
                {
                    CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(_taskDocument.DocumentName);
              
                    cloudBlockBlob.Properties.ContentType = _taskDocument.FileType;
                    await cloudBlockBlob.UploadFromByteArrayAsync(bytesFile, 0, bytesFile.Length);
                    string URI= cloudBlockBlob.Uri.AbsoluteUri;
                }
                

            }
            catch (Exception ex)
            {
                throw (ex);
            }

            #endregion

            #region "Service Bus Entry"

            TaskDocument taskDocumentServiceBus = new TaskDocument();
            taskDocumentServiceBus.Cid = _taskDocument.Cid;
            taskDocumentServiceBus.DocumentName = _taskDocument.DocumentName;
            taskDocumentServiceBus.Tskid = _taskDocument.Tskid;
            taskDocumentServiceBus.UplodedBy = _taskDocument.UplodedBy;
            taskDocumentServiceBus.FileType = _taskDocument.FileType;


            const string ServiceBusConnectionString = "Endpoint=sb://auditor2client.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=BJtC7Tu9IhAH62XqaazRBI24PgeRj43cwbXAy0zbFLE=";                                                                           //access policy 
            const string QueueName = "a2cdoc";
            queueClient = new QueueClient(ServiceBusConnectionString, QueueName);
            string ShareMessage = JsonConvert.SerializeObject(taskDocumentServiceBus);
            var message = new Message(Encoding.UTF8.GetBytes(ShareMessage));
            await queueClient.SendAsync(message);

            #endregion
            return NoContent();
           

        }

        [HttpPost]
        [Authorize]
        [ActionName("TaskDocumentDownload")]
        public async Task<ActionResult> TaskDocumentDownload(TaskDocument _taskDocument)
        {
            TaskDocument taskDocument = new TaskDocument();
            taskDocument.Cid = _taskDocument.Cid;
            taskDocument.DocumentName = _taskDocument.DocumentName;
            taskDocument.Tskid = _taskDocument.Tskid;
            taskDocument.UplodedBy = _taskDocument.UplodedBy;
            Byte[] bytesFile = Convert.FromBase64String(_taskDocument.Filestream);
            byte[] tstArry = { 0, 1 };
            taskDocument.FileData = tstArry;
            taskDocument.FileType = _taskDocument.FileType;
            taskDocument.Filestream = "test";

            //_context.TaskDocument.Add(taskDocument);
            //await _context.SaveChangesAsync();
            //CreatedAtAction("GetAuditorPortfolio", new { id = _taskDocument.Tskid }, _taskDocument);
           
            #region "Blob Storage Download"

            try
            {
               
                MemoryStream ms = new MemoryStream();
                if (CloudStorageAccount.TryParse("DefaultEndpointsProtocol=https;AccountName=mccog;AccountKey=oan1RKJMPe82KxZDhfHqZArWCSHzSukEdwl8VSQgSP+w3pxIvLkHEObL6iUwMbiSM9Kx6mcmx5ZYsj3lBQ29GQ==;EndpointSuffix=core.windows.net", out CloudStorageAccount storageAccount))
                {
                    CloudBlobClient BlobClient = storageAccount.CreateCloudBlobClient();
                    string strContainerName = "taskdocuments";
                    CloudBlobContainer container = BlobClient.GetContainerReference(strContainerName);

                    if (await container.ExistsAsync())
                    {
                        CloudBlob file = container.GetBlobReference(_taskDocument.DocumentName);

                        if (await file.ExistsAsync())
                        {
                            await file.DownloadToStreamAsync(ms);
                            Stream blobStream = file.OpenReadAsync().Result;
                            return File(blobStream, file.Properties.ContentType, file.Name);
                        }
                        else
                        {
                            return Content("File does not exist");
                        }
                    }
                    else
                    {
                        return Content("Container does not exist");
                    }
                }
                else
                {
                    return Content("Error opening storage");
                }


            }
            catch (Exception ex)
            {
                throw (ex);
            }

            #endregion

            return NoContent();


        }
        // PUT: api/AuditorPortfolios/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        [Authorize]
        [ActionName("PutAuditorPortfolio")]
        public async Task<IActionResult> PutAuditorPortfolio(long id, AuditorPortfolio auditorPortfolio)
        {
            if (id != auditorPortfolio.Pid)
            {
                return BadRequest();
            }

            _context.Entry(auditorPortfolio).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AuditorPortfolioExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPut("{id}")]
        [Authorize]
        [ActionName("CompleteTaskDetails")]
        public async Task<IActionResult> CompleteTaskDetails(long id, TaskDetails taskDetails)
        {
            if (id != taskDetails.Tskid)
            {
                return BadRequest();
            }

            _context.Entry(taskDetails).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

                #region "Service Bus Entry"

                const string ServiceBusConnectionString = "Endpoint=sb://auditor2client.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=BJtC7Tu9IhAH62XqaazRBI24PgeRj43cwbXAy0zbFLE=";                                                                           //access policy 
                const string QueueName = "a2cut";
                queueClient = new QueueClient(ServiceBusConnectionString, QueueName);
                string clientRespondMessage = JsonConvert.SerializeObject(taskDetails);
                var message = new Message(Encoding.UTF8.GetBytes(clientRespondMessage));
                await queueClient.SendAsync(message);

                #endregion
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskDetailsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/AuditorPortfolios/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<AuditorPortfolio>> DeleteAuditorPortfolio(long id)
        {
            var auditorPortfolio = await _context.AuditorPortfolio.FindAsync(id);
            if (auditorPortfolio == null)
            {
                return NotFound();
            }

            _context.AuditorPortfolio.Remove(auditorPortfolio);
            await _context.SaveChangesAsync();

            return auditorPortfolio;
        }

        [HttpDelete("{DOCUID}")]
        [Authorize]
        [ActionName("DeleteTaskDocument")]
        public async Task<ActionResult<TaskDocument>> DeleteTaskDocument(long DOCUID)
        {
            var _taskDocument = await _context.TaskDocument.FindAsync(DOCUID);
            if (_taskDocument == null)
            {
                return NotFound();
            }
            _context.TaskDocument.Remove(_taskDocument);
            await _context.SaveChangesAsync();

            #region "Blob File Delete"

            //TaskDocument _deletetaskDocument = new TaskDocument();
            //_deletetaskDocument.Cid = _taskDocument.Cid;
            //_deletetaskDocument.DocumentName = _taskDocument.DocumentName;
            //_deletetaskDocument.Tskid = _taskDocument.Tskid;
            //_deletetaskDocument.UplodedBy = _taskDocument.UplodedBy;
            //// Byte[] bytesFile = Convert.FromBase64String(_taskDocument.Filestream);
            //byte[] tstArry = { 0, 1 };
            //_deletetaskDocument.FileData = tstArry;
            //_deletetaskDocument.FileType = _taskDocument.FileType;
            //_deletetaskDocument.Filestream = "test";

            try
            {

                MemoryStream ms = new MemoryStream();
                if (CloudStorageAccount.TryParse("DefaultEndpointsProtocol=https;AccountName=mccog;AccountKey=oan1RKJMPe82KxZDhfHqZArWCSHzSukEdwl8VSQgSP+w3pxIvLkHEObL6iUwMbiSM9Kx6mcmx5ZYsj3lBQ29GQ==;EndpointSuffix=core.windows.net", out CloudStorageAccount storageAccount))
                {
                    CloudBlobClient BlobClient = storageAccount.CreateCloudBlobClient();
                    string strContainerName = "taskdocuments";
                    CloudBlobContainer container = BlobClient.GetContainerReference(strContainerName);

                    if (await container.ExistsAsync())
                    {
                        CloudBlob file = container.GetBlobReference(_taskDocument.DocumentName);

                        if (await file.ExistsAsync())
                        {
                            await file.DeleteIfExistsAsync();

                        }
                        else
                        {
                            return Content("File does not exist");
                        }
                    }
                    else
                    {
                        return Content("Container does not exist");
                    }
                }
                else
                {
                    return Content("Error opening storage");
                }


            }
            catch (Exception ex)
            {
                throw (ex);
            }

            #endregion

            #region "ServiceBus Entry"
            const string ServiceBusConnectionString = "Endpoint=sb://auditor2client.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=BJtC7Tu9IhAH62XqaazRBI24PgeRj43cwbXAy0zbFLE=";                                                                           //access policy 
            const string QueueName = "a2cdocdelete";
            queueClient = new QueueClient(ServiceBusConnectionString, QueueName);
            string clientRespondMessage = JsonConvert.SerializeObject(_taskDocument);
            var message = new Message(Encoding.UTF8.GetBytes(clientRespondMessage));
            await queueClient.SendAsync(message);
            #endregion

            return _taskDocument;

        }

        private bool AuditorPortfolioExists(long id)
        {
            return _context.AuditorPortfolio.Any(e => e.Pid == id);
        }
        private bool TaskDetailsExists(long id)
        {
            return _context.TaskDetails.Any(e => e.Tskid == id);
        }
    }
}
