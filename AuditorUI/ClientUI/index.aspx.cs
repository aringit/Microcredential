using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ClientUI.Repository;
using System.Globalization;
using System.Text;
using System.IO;

using Microsoft.Extensions.Hosting;
using System.Data;

namespace ClientUI
{
    public partial class index : System.Web.UI.Page
    {
        #region "Variables"

        public List<AuditorPortfolio> _lstAuditorPortfolio { get; set; }
        public List<ClientDetails> _lstclientDetails { get; set; }
        public List<TaskDetails> _lsttaskDetails { get; set; }

        public List<AuditorCommentDetails> _lstAuditorCommentDetails { get; set; }
        public List<MessageDetails> _lstMessageDetails { get; set; }

        public TaskDetails TskDetails { get; set; }

        public TaskDocument TskDocument { get; set; }
        public List<TaskDocument> _lstTaskDocument { get; set; }

        bool _isTokenValid = false;
        string _userID = string.Empty;

        #endregion

        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (ValidateToken())
                {
                    _userID = Application["userID"].ToString();
                    GetDefaultData(Application["UserName"].ToString());

                }
                GetPortfolio();
               
                GetDefaultGrid();
              //  Page.Form.Attributes.Add("enctype", "multipart/form-data");

                // pnlDocument.Visible = false;

            }
        }
        private void GetDefaultGrid()
        {
            DataTable dtDefaultDocument = new DataTable();
            DataTable dtDefaultMsg = new DataTable();
            DataTable dtDefaultTask = new DataTable();
            grvDocument.DataSource = dtDefaultDocument;
            grvDocument.DataBind();
            grdViewComment.DataSource = dtDefaultMsg;
            grdViewComment.DataBind();
            grdViewTask.DataSource = dtDefaultTask;
            grdViewTask.DataBind();
        }
        private void GetDefaultData(string username)
        {
            if(username=="sarkar")
            lblAuditorname.Text = "AuditorMC";
        }
        private async void GetPortfolio()
        {

            if (ValidateToken())
            {
                string token = Application["token"].ToString();
                HttpClient _client = new HttpClient();
                var contentType = new MediaTypeWithQualityHeaderValue("application/json");
                _client.DefaultRequestHeaders.Accept.Add(contentType);
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                //HttpResponseMessage response = await _client.GetAsync("http://localhost:54213/api/AuditorPortfolios/GetAuditorPortfolio");
                HttpResponseMessage response = await _client.GetAsync("https://auditorapi.azurewebsites.net/api/AuditorPortfolios/GetAuditorPortfolio");
                string apiResponse = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    _lstAuditorPortfolio = JsonConvert.DeserializeObject<List<AuditorPortfolio>>(apiResponse).ToList();

                }
                Dictionary<long, string> dPortfolio = new Dictionary<long, string>();
                foreach (var items in _lstAuditorPortfolio)
                {
                    dPortfolio.Add(items.Pid, items.PortfolioName);
                }
                ddlPortfolio.DataTextField = "Value";
                ddlPortfolio.DataValueField = "Key";
                ddlPortfolio.DataSource = dPortfolio;
                ddlPortfolio.DataBind();
            }


        }

       

        private bool ValidateToken()
        {

            if (!string.IsNullOrEmpty(Application["token"].ToString()))
            {
                _isTokenValid = true;
            }
            else
            {
                _isTokenValid = false;
                Response.Redirect("login.aspx");
            }

            return _isTokenValid;

        }
        protected async void ddlPortfolio_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ValidateToken())
            {
               
                HttpClient _client = new HttpClient();
                var contentType = new MediaTypeWithQualityHeaderValue("application/json");
                _client.DefaultRequestHeaders.Accept.Add(contentType);
                string token = Application["token"].ToString();
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                //HttpResponseMessage response = await _client.GetAsync("http://localhost:54213/api/AuditorPortfolios/GetClientsByPortfolio/" + Convert.ToInt64(ddlPortfolio.SelectedValue.ToString()));
                HttpResponseMessage response = await _client.GetAsync("https://auditorapi.azurewebsites.net/api/AuditorPortfolios/GetClientsByPortfolio/" + Convert.ToInt64(ddlPortfolio.SelectedValue.ToString()));
                string apiResponse = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    _lstclientDetails = JsonConvert.DeserializeObject<List<ClientDetails>>(apiResponse).ToList();

                }
                Dictionary<long, string> dClients = new Dictionary<long, string>();
                foreach (var items in _lstclientDetails)
                {
                    dClients.Add(items.Cid, items.ClientName);
                }
                ddlClient.DataTextField = "Value";
                ddlClient.DataValueField = "Key";
                ddlClient.DataSource = dClients;
                ddlClient.DataBind();
            }

        }

       
        public async void GetTaskDetails()
        {
            if (ValidateToken())
            {
                HttpClient _client = new HttpClient();
                var contentType = new MediaTypeWithQualityHeaderValue("application/json");
                _client.DefaultRequestHeaders.Accept.Add(contentType);
                string token = Application["token"].ToString();
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                //HttpResponseMessage response = await _client.GetAsync("http://localhost:54213/api/AuditorPortfolios/GetTaskDetailsByClient/" + Convert.ToInt64(ddlClient.SelectedValue.ToString()));
               
                HttpResponseMessage response = await _client.GetAsync("https://auditorapi.azurewebsites.net/api/AuditorPortfolios/GetTaskDetailsByClient/" + Convert.ToInt64(ddlClient.SelectedValue.ToString()));
                string apiResponse = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    _lsttaskDetails = JsonConvert.DeserializeObject<List<TaskDetails>>(apiResponse).ToList();
                    grdViewTask.DataSource = _lsttaskDetails;
                    grdViewTask.DataBind();

                }
            }
        }

        public async void GetCommentDetails(long TSKID)
        {
            if (ValidateToken())
            {
                HttpClient _client = new HttpClient();
                var contentType = new MediaTypeWithQualityHeaderValue("application/json");
                _client.DefaultRequestHeaders.Accept.Add(contentType);
                string token = Application["token"].ToString();
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                //HttpResponseMessage response = await _client.GetAsync("http://localhost:54213/api/AuditorPortfolios/GetCommentsByTaskID/" + TSKID);
                HttpResponseMessage response = await _client.GetAsync("https://auditorapi.azurewebsites.net/api/AuditorPortfolios/GetCommentsByTaskID/" + TSKID);
                string apiResponse = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    _lstAuditorCommentDetails = JsonConvert.DeserializeObject<List<AuditorCommentDetails>>(apiResponse).ToList();
                    grdViewComment.DataSource = _lstAuditorCommentDetails;
                    grdViewComment.DataBind();

                }
            }
        }
        public async void GetMessageDetails(long TSKID)
        {
            if (ValidateToken())
            {
                HttpClient _client = new HttpClient();
                var contentType = new MediaTypeWithQualityHeaderValue("application/json");
                _client.DefaultRequestHeaders.Accept.Add(contentType);
                string token = Application["token"].ToString();
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                //HttpResponseMessage response = await _client.GetAsync("http://localhost:54213/api/AuditorPortfolios/GetCommentsByTaskID/" + TSKID);
                HttpResponseMessage response = await _client.GetAsync("https://auditorapi.azurewebsites.net/api/AuditorPortfolios/GetMessagesByTaskID/" + TSKID);
                string apiResponse = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    _lstMessageDetails = JsonConvert.DeserializeObject<List<MessageDetails>>(apiResponse).ToList();
                    grdViewComment.DataSource = _lstMessageDetails;
                    grdViewComment.DataBind();

                }
            }
        }

        private async void CreateTask()
        {
            if (ValidateToken())
            {
                string token = Application["token"].ToString();
                string _userID = Application["userID"].ToString();

                TaskDetails _TKD = new TaskDetails();
                _TKD.TaskDescription = txtTask.Text.ToString();
                _TKD.Pid = Convert.ToInt64(ddlPortfolio.SelectedValue);
                _TKD.Cid = Convert.ToInt64(ddlClient.SelectedValue);
                _TKD.Id= Application["userID"].ToString();
                _TKD.Status = "Active";

                HttpClient _client = new HttpClient();
                var contentType = new MediaTypeWithQualityHeaderValue("application/json");
                _client.DefaultRequestHeaders.Accept.Add(contentType);
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var json = JsonConvert.SerializeObject(_TKD);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                // HttpResponseMessage response = await _client.PostAsync("http://localhost:54213/api/AuditorPortfolios/PostTaskDetails/", data);
                HttpResponseMessage response = await _client.PostAsync("https://auditorapi.azurewebsites.net/api/AuditorPortfolios/PostTaskDetails/", data);

                string apiResponse = await response.Content.ReadAsStringAsync();
                string result = response.Content.ReadAsStringAsync().Result;
                txtTask.Text = string.Empty;
                GetTaskDetails();


            }
        }
        private async void SendMessage(long TSKID,long CID)
        {
            if (ValidateToken())
            {
                string token = Application["token"].ToString();
                string _userID = Application["userID"].ToString();

               // AuditorCommentDetails _SyncAuditorComments = new AuditorCommentDetails();
                MessageDetails msgDetails = new MessageDetails();
                msgDetails.Auid = Application["userID"].ToString();
                //msgDetails.Cid =Convert.ToInt64(Session["CID"]) ;
                msgDetails.Cid = CID;
                msgDetails.CommentDetails = txtSendComment.Text.ToString();
                msgDetails.CommentedBy = "Auditor";
                //msgDetails.Tskid= Convert.ToInt64(Session["TSKID"]);
                msgDetails.Tskid = TSKID;
                //  _SyncAuditorComments.Pid =;
                HttpClient _client = new HttpClient();
                var contentType = new MediaTypeWithQualityHeaderValue("application/json");
                _client.DefaultRequestHeaders.Accept.Add(contentType);
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var json = JsonConvert.SerializeObject(msgDetails);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                // HttpResponseMessage response = await _client.PostAsync("http://localhost:54213/api/AuditorPortfolios/PostTaskDetails/", data);
                HttpResponseMessage response = await _client.PostAsync("https://auditorapi.azurewebsites.net/api/AuditorPortfolios/PostMessageDetails/", data);

                string apiResponse = await response.Content.ReadAsStringAsync();
                string result = response.Content.ReadAsStringAsync().Result;
                txtSendComment.Text = string.Empty;


            }
        }


        public async void GetDocumentDetails(long TSKID)
        {
            if (ValidateToken())
            {
                HttpClient _client = new HttpClient();
                var contentType = new MediaTypeWithQualityHeaderValue("application/json");
                _client.DefaultRequestHeaders.Accept.Add(contentType);
                string token = Application["token"].ToString();
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                //HttpResponseMessage response = await _client.GetAsync("http://localhost:54213/api/AuditorPortfolios/GetDocumentsByTaskID/" + TSKID);
                HttpResponseMessage response = await _client.GetAsync("https://auditorapi.azurewebsites.net/api/AuditorPortfolios/GetDocumentsByTaskID/" + TSKID);
                string apiResponse = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    _lstTaskDocument = JsonConvert.DeserializeObject<List<TaskDocument>>(apiResponse).ToList();
                    grvDocument.DataSource = _lstTaskDocument;
                    grvDocument.DataBind();

                }
            }
        }
        public async void UploadDocument(long TSKID, byte[] filedata, string filename, string filetype, long CID)
        {

            if (ValidateToken())
            {
                string token = Application["token"].ToString();
                string _userID = Application["userID"].ToString();

                TaskDocument _tskDocu = new TaskDocument();
                _tskDocu.Cid = CID;
                string documentname= Path.GetFileName(filename);
                _tskDocu.DocumentName = documentname;
                string bytearrData = Convert.ToBase64String(filedata);
                byte[] tstArry = { 0, 1 };
                _tskDocu.FileData = tstArry;
                _tskDocu.Filestream = bytearrData;
                _tskDocu.FileType = filetype;
                _tskDocu.Tskid = TSKID;
                _tskDocu.UplodedBy = _userID;

                HttpClient _client = new HttpClient();
                var contentType = new MediaTypeWithQualityHeaderValue("application/json");
                _client.DefaultRequestHeaders.Accept.Add(contentType);
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var json = JsonConvert.SerializeObject(_tskDocu);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                //HttpResponseMessage response = await _client.PostAsync("http://localhost:54213/api/AuditorPortfolios/TaskDocumentUpload/", data);
                HttpResponseMessage response = await _client.PostAsync("https://auditorapi.azurewebsites.net/api/AuditorPortfolios/TaskDocumentUpload/", data);

                string apiResponse = await response.Content.ReadAsStringAsync();
                string result = response.Content.ReadAsStringAsync().Result;
                GetDocumentDetails(TSKID);

            }

        }
        protected void dtnCreateTask_Click(object sender, EventArgs e)
        {
            CreateTask();
        }
        //protected void btnReset_Click(object sender, EventArgs e)
        //{
        //    ddlPortfolio.SelectedIndex = 0;
        //    ddlClient.SelectedIndex = 0;
        //    pnlDocument.Visible = false;
        //}
        protected void dtnSearch_Click(object sender, EventArgs e)
        {
            GetTaskDetails();
        }

        protected void chkRowComments_OnCheckedChanged(object sender, EventArgs e)
        {

            GridViewRow row = ((GridViewRow)((CheckBox)sender).NamingContainer);
            int index = row.RowIndex;
            CheckBox cb1 = (CheckBox)grdViewTask.Rows[index].FindControl("chkRowComments");
            if (cb1.Checked)
            {
                long TSKID = Convert.ToInt64((row.Cells[2].FindControl("lblTaskid") as Label).Text);
                long CID = Convert.ToInt64((row.Cells[3].FindControl("lblTaskid") as Label).Text);
                GetMessageDetails(TSKID);
            }


        }

        protected void btnTs_Click(object sender, EventArgs e)
        {
          
        }

        protected void btnSendMsg_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in grdViewTask.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox c = (CheckBox)row.FindControl("chkRowComments");
                    if (c.Checked)
                    {
                        long TSKID = Convert.ToInt64((row.Cells[2].FindControl("lblTaskid") as Label).Text);
                        long CID = Convert.ToInt64((row.Cells[3].FindControl("lblCID") as Label).Text);
                        SendMessage(TSKID, CID);
                        GetMessageDetails(TSKID);
                    }
                }
            }
        }


    
        protected void chkRowDoc_CheckedChanged(object sender, EventArgs e)
        {
            pnlDocument.Visible = true;
            GridViewRow row = ((GridViewRow)((CheckBox)sender).NamingContainer);
            int index = row.RowIndex;
            CheckBox cb2 = (CheckBox)grdViewTask.Rows[index].FindControl("chkRowDoc");
            if (cb2.Checked)
            {
                long TSKID = Convert.ToInt64((row.Cells[2].FindControl("lblTaskid") as Label).Text);
                long CID = Convert.ToInt64((row.Cells[3].FindControl("lblCID") as Label).Text);
                GetDocumentDetails(TSKID);
              
            }
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (ValidateToken())
            {
                pnlDocument.Visible = true;
                string token = Application["token"].ToString();
                string _userID = Application["userID"].ToString();
                if (FileUploadDoc.HasFile)
                {
                    string filename = Server.MapPath(FileUploadDoc.PostedFile.FileName);
                    byte[] fileData;
                   
                    string TempfileLocation = @"E:\upload\";

                    string FullPath = System.IO.Path.Combine(TempfileLocation, filename);

                    FileUploadDoc.SaveAs(FullPath);

                    Response.Write(FullPath);
                    string _strmimeType;
                    using (FileStream fileStream = File.Open(FullPath, FileMode.Open))
                    {
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            // set the memory stream position to starting
                            memoryStream.Position = 0;

                            // copy file content to memory stream
                            fileStream.CopyTo(memoryStream);
                            fileData = memoryStream.ToArray();
                            memoryStream.Seek(0, SeekOrigin.Begin);

                            // get the mime type of the file
                            _strmimeType = "application /unknown";
                            string ext = (filename.Contains(".")) ?
                                        System.IO.Path.GetExtension(filename).ToLower() : "." + filename;
                            Microsoft.Win32.RegistryKey regKey =
                                        Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
                            if (regKey != null && regKey.GetValue("Content Type") != null)
                                _strmimeType = regKey.GetValue("Content Type").ToString();
                        }
                    }
                    foreach (GridViewRow row in grdViewTask.Rows)
                    {
                        if (row.RowType == DataControlRowType.DataRow)
                        {
                            CheckBox c = (CheckBox)row.FindControl("chkRowDoc");
                            if (c.Checked)
                            {
                                long TSKID = Convert.ToInt64((row.Cells[2].FindControl("lblTaskid") as Label).Text);
                                long CID = Convert.ToInt64((row.Cells[3].FindControl("lblCID") as Label).Text);
                               
                                UploadDocument(TSKID, fileData, filename, _strmimeType, CID);
                            }
                        }
                    }
                   
                }
            }
        }

        protected void chkRowClose_CheckedChanged(object sender, EventArgs e)
        {
            GridViewRow row = ((GridViewRow)((CheckBox)sender).NamingContainer);
            int index = row.RowIndex;
            CheckBox cb2 = (CheckBox)grdViewTask.Rows[index].FindControl("chkRowClose");
            if (cb2.Checked)
            {
                long TSKID = Convert.ToInt64((row.Cells[2].FindControl("lblTaskid") as Label).Text);
                long CID = Convert.ToInt64((row.Cells[3].FindControl("lblCID") as Label).Text);
                string taskDescription = Convert.ToString((row.Cells[1].FindControl("lblTask") as Label).Text);
                long PID = Convert.ToInt64((row.Cells[4].FindControl("lblPID") as Label).Text);
                CompleteTaskDetails(TSKID,CID,PID,taskDescription);
                RefreshTaskDetails(CID);

            }
        }

        private async void RefreshTaskDetails(long CID)
        {

            if (ValidateToken())
            {
                HttpClient _client = new HttpClient();
                var contentType = new MediaTypeWithQualityHeaderValue("application/json");
                _client.DefaultRequestHeaders.Accept.Add(contentType);
                string token = Application["token"].ToString();
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage response = await _client.GetAsync("https://auditorapi.azurewebsites.net/api/AuditorPortfolios/GetTaskDetailsByClient/" + CID);
                string apiResponse = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    _lsttaskDetails = JsonConvert.DeserializeObject<List<TaskDetails>>(apiResponse).ToList();
                    grdViewTask.DataSource = _lsttaskDetails;
                    grdViewTask.DataBind();

                }
            }
        }

        private async void CompleteTaskDetails(long TSKID,long CID,long PID,string taskDescription)
        {
            if (ValidateToken())
            {
                string token = Application["token"].ToString();
                string _userID = Application["userID"].ToString();

                TaskDetails _TKD = new TaskDetails();
                _TKD.TaskDescription = taskDescription;
                _TKD.Pid = PID;
                _TKD.Cid = CID;
                _TKD.Id = Application["userID"].ToString();
                _TKD.Status = "In-Active";
                _TKD.Tskid = TSKID;

                HttpClient _client = new HttpClient();
                var contentType = new MediaTypeWithQualityHeaderValue("application/json");
                _client.DefaultRequestHeaders.Accept.Add(contentType);
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var json = JsonConvert.SerializeObject(_TKD);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                // HttpResponseMessage response = await _client.PostAsync("http://localhost:54213/api/AuditorPortfolios/PostTaskDetails/", data);
                HttpResponseMessage response = await _client.PutAsync("https://auditorapi.azurewebsites.net/api/AuditorPortfolios/CompleteTaskDetails/" + TSKID, data);

                string apiResponse = await response.Content.ReadAsStringAsync();
                string result = response.Content.ReadAsStringAsync().Result;
              
               


            }
        }

        protected void lnkDocumentName_Click(object sender, EventArgs e)
        {
            GridViewRow row = ((GridViewRow)((LinkButton)sender).NamingContainer);
            int index = row.RowIndex;
            LinkButton lb = (LinkButton)grvDocument.Rows[index].FindControl("lnkDocumentName");
          
                long TSKID = Convert.ToInt64((row.Cells[1].FindControl("lblTaskid") as Label).Text);
                long CID = Convert.ToInt64((row.Cells[2].FindControl("lblCID") as Label).Text);
                string documentName = Convert.ToString((row.Cells[3].FindControl("lnkDocumentName") as LinkButton).Text);               
                DownloadDocument(TSKID, CID, documentName);
               
        }

        private async void DownloadDocument(long TSKID, long CID, string documentName)
        {
            if (ValidateToken())
            {
                string token = Application["token"].ToString();
                string _userID = Application["userID"].ToString();

                TaskDocument _tskDocu = new TaskDocument();
                _tskDocu.Cid = CID;              
                _tskDocu.DocumentName = documentName;
                string bytearrData = null;
                byte[] tstArry = { 0, 1 };
                _tskDocu.FileData = tstArry;
                _tskDocu.Filestream = bytearrData;
                _tskDocu.FileType = null;
                _tskDocu.Tskid = TSKID;
                _tskDocu.UplodedBy = _userID;

                HttpClient _client = new HttpClient();
                var contentType = new MediaTypeWithQualityHeaderValue("application/json");
                _client.DefaultRequestHeaders.Accept.Add(contentType);
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var json = JsonConvert.SerializeObject(_tskDocu);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _client.PostAsync("http://localhost:54213/api/AuditorPortfolios/TaskDocumentDownload/", data);

                //HttpResponseMessage response = await _client.PostAsync("https://auditorapi.azurewebsites.net/api/AuditorPortfolios/TaskDocumentDownload/", data);

                Stream apiResponse = await response.Content.ReadAsStreamAsync();
                string TempfileLocation = @"E:\uploads";

                string FullPath = System.IO.Path.Combine(TempfileLocation,_tskDocu.DocumentName);

                FileStream fileStream = new FileStream(FullPath, FileMode.Create, FileAccess.Write);
                apiResponse.CopyTo(fileStream);
                fileStream.Dispose();             
                GetDocumentDetails(TSKID);

            }
        }

        protected void chkDelete_CheckedChanged(object sender, EventArgs e)
        {
            GridViewRow row = ((GridViewRow)((CheckBox)sender).NamingContainer);
            int index = row.RowIndex;
            CheckBox cb2 = (CheckBox)grvDocument.Rows[index].FindControl("chkDelete");
            if (cb2.Checked)
            {
                long TSKID = Convert.ToInt64((row.Cells[1].FindControl("lblTaskid") as Label).Text);
                long CID = Convert.ToInt64((row.Cells[2].FindControl("lblCID") as Label).Text);
                long DOCUID = Convert.ToInt64((row.Cells[3].FindControl("lblDOCUID") as Label).Text);
                string documentname = Convert.ToString((row.Cells[4].FindControl("lnkDocumentName") as LinkButton).Text);
                DeleteTaskDocument(TSKID, CID, documentname, DOCUID);
                GetDocumentDetails(TSKID);

            }
        }

        private async void DeleteTaskDocument(long TSKID, long CID, string documentname,long DOCUID)
        {
            if (ValidateToken())
            {
                string token = Application["token"].ToString();
                string _userID = Application["userID"].ToString();

                TaskDocument _tskDocu = new TaskDocument();
                _tskDocu.Cid = CID;
                _tskDocu.DocumentName = documentname;
                string bytearrData = null;
                byte[] tstArry = { 0, 1 };
                _tskDocu.FileData = tstArry;
                _tskDocu.Filestream = bytearrData;
                _tskDocu.FileType = null;
                _tskDocu.Tskid = TSKID;
                _tskDocu.UplodedBy = _userID;
                _tskDocu.Docuid = DOCUID;

                HttpClient _client = new HttpClient();
                var contentType = new MediaTypeWithQualityHeaderValue("application/json");
                _client.DefaultRequestHeaders.Accept.Add(contentType);
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var json = JsonConvert.SerializeObject(_tskDocu);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                //HttpResponseMessage response = await _client.DeleteAsync("http://localhost:54213/api/AuditorPortfolios/DeleteTaskDocument/"+DOCUID);

                HttpResponseMessage response = await _client.DeleteAsync("https://auditorapi.azurewebsites.net/api/AuditorPortfolios/DeleteTaskDocument/"+DOCUID);


                GetDocumentDetails(TSKID);

            }
        }
    }
}
