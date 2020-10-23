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
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;

namespace ClientAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]

    public class ClientDetailsController : ControllerBase
    {
        private readonly ClientDBContext _context;
        private static IQueueClient queueClient;

        public ClientDetailsController(ClientDBContext context)
        {
            _context = context;
        }

        // GET: api/ClientDetails
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
        [HttpGet]
        [Authorize]
        [ActionName("GetClientDetails")]
        public async Task<ActionResult<IEnumerable<ClientDetails>>> GetClientDetails()
        {
            return await _context.ClientDetails.ToListAsync();
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
        [ActionName("GetTaskDocumentDetails")]
        public async Task<ActionResult<IEnumerable<TaskDocument>>> GetTaskDocumentDetails()
        {
            return await _context.TaskDocument.ToListAsync();
        }
        [HttpGet]
        [Authorize]
        [ActionName("GetClientPortfolioDetails")]
        public async Task<ActionResult<IEnumerable<ClientPortfolio>>> GetClientPortfolioDetails()
        {
            return await _context.ClientPortfolio.ToListAsync();
        }

        // GET: api/ClientDetails/5
        [HttpGet("{id}")]
        [Authorize]
        [ActionName("GetClientDetailsbyPortfolio")]
        public async Task<ActionResult<ClientDetails>> GetClientDetailsbyPortfolio(long id)
        {
           var clientDetails = await _context.ClientDetails.FindAsync(id);
          

            if (clientDetails == null)
            {
                return NotFound();
            }
          
            return clientDetails;
        }
        [HttpGet("{id}")]
        [Authorize]
        [ActionName("GetCommentsbyClient")]
        public async Task<IList<AuditorClientComments>> GetCommentsbyClient(long id)
        {
            var commentsDetails =  from g in _context.AuditorClientComments where g.Cid == id select g;
            //var commentsDetails =await  _context.AuditorClientComments.Select(x => x.Cid == id).ToListAsync();

            //if (commentsDetails == null)
            //{
            //    return NotFound();
            //}
            return await commentsDetails.ToListAsync();
            //return await Ok(commentsDetails);
        }
        [HttpGet("{id}")]
        [Authorize]
        [ActionName("GetCommentsbyID")]
        public async Task<ActionResult<AuditorClientComments>> GetCommentsbyID(long id)
        {
            var messageDetails = await _context.AuditorClientComments.FindAsync(id);


            if (messageDetails == null)
            {
                return NotFound();
            }

            return messageDetails;
        }
        [HttpGet("{id}")]
        [Authorize]
        [ActionName("GetMessagebyTSKID")]
        public async Task<IList<MessageDetails>> GetMessagebyTSKID(long id)
        {
            var messageDetails = from g in _context.MessageDetails where g.Tskid == id select g;
            //var commentsDetails =await  _context.AuditorClientComments.Select(x => x.Cid == id).ToListAsync();

            //if (commentsDetails == null)
            //{
            //    return NotFound();
            //}
            return await messageDetails.ToListAsync();          
        }
        [HttpGet("{id}")]
        [Authorize]
        [ActionName("GetTaskDetailsByCID")]
        public async Task<IList<TaskDetails>> GetTaskDetailsByCID(long id)
        {
            var taskDetails = from g in _context.TaskDetails where g.Cid == id && g.Status == "Active" select g;


            //if (taskDetails == null)
            //{
            //    return NotFound();
            //}

            return await taskDetails.ToListAsync();
        }
        [HttpGet("{id}")]
        [Authorize]
        [ActionName("GetClientDetailsbyId")]
        public async Task<IList<ClientDetails>> GetCIDbyId(string id)
        {
            var clientDetails =  from g in _context.ClientDetails where g.Id == id select g;


            //if (clientDetails == null)
            //{
            //    return NotFound();
            //}

            return await clientDetails.ToListAsync();
        }

        [HttpGet("{TSKID}")]
        [Authorize]
        [ActionName("GetDocumentsByTaskID")]
        public async Task<ActionResult<IList<TaskDocument>>> GetDocumentsByTaskID(long TSKID)
        {

            var taskDocumentDetails = from g in _context.TaskDocument where g.Tskid == TSKID  select g;
            if (taskDocumentDetails == null)
            {
                return NotFound();
            }
            return await taskDocumentDetails.ToListAsync();

        }

        // PUT: api/ClientDetails/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutClientDetails(long id, ClientDetails clientDetails)
        {
            if (id != clientDetails.Cid)
            {
                return BadRequest();
            }

            _context.Entry(clientDetails).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientDetailsExists(id))
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

        // POST: api/ClientDetails
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [Authorize]
        [ActionName("PostClientDetails")]
        public async Task<ActionResult<ClientDetails>> PostClientDetails(ClientDetails clientDetails)
        {
            _context.ClientDetails.Add(clientDetails);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetClientDetails", new { id = clientDetails.Cid }, clientDetails);
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
            const string QueueName = "c2amessage";
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
            CreatedAtAction("GetTaskDocumentDetails", new { id = _taskDocument.Tskid }, _taskDocument);
           
            
            #region "Blob Storage Upload"

            try
            {
                CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=mccog;AccountKey=oan1RKJMPe82KxZDhfHqZArWCSHzSukEdwl8VSQgSP+w3pxIvLkHEObL6iUwMbiSM9Kx6mcmx5ZYsj3lBQ29GQ==;EndpointSuffix=core.windows.net");
                Microsoft.Azure.Storage.Blob.CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                string strContainerName = "taskdocuments";
                Microsoft.Azure.Storage.Blob.CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(strContainerName);

                if (await cloudBlobContainer.CreateIfNotExistsAsync())
                {
                    await cloudBlobContainer.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
                }

                if (_taskDocument.DocumentName != null && bytesFile != null)
                {
                    CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(_taskDocument.DocumentName);

                    cloudBlockBlob.Properties.ContentType = _taskDocument.FileType;
                    await cloudBlockBlob.UploadFromByteArrayAsync(bytesFile, 0, bytesFile.Length);
                    string URI = cloudBlockBlob.Uri.AbsoluteUri;
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
            const string QueueName = "c2adoc";
            queueClient = new QueueClient(ServiceBusConnectionString, QueueName);
            string ShareMessage = JsonConvert.SerializeObject(taskDocumentServiceBus);
            var message = new Message(Encoding.UTF8.GetBytes(ShareMessage));
            await queueClient.SendAsync(message);

            #endregion

            return NoContent();


        }

        // DELETE: api/ClientDetails/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ClientDetails>> DeleteClientDetails(long id)
        {
            var clientDetails = await _context.ClientDetails.FindAsync(id);
            if (clientDetails == null)
            {
                return NotFound();
            }

            _context.ClientDetails.Remove(clientDetails);
            await _context.SaveChangesAsync();

            return clientDetails;
        }

        private bool ClientDetailsExists(long id)
        {
            return _context.ClientDetails.Any(e => e.Cid == id);
        }
    }
}
