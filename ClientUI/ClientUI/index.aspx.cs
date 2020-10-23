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
using System.Text;
using System.Data;
using System.IO;

namespace ClientUI
{
    public partial class index : System.Web.UI.Page
    {
        public List<ClientDetails> _lstclientDetails { get; set; }
        public IList<TaskDetails> _lstTaskDetails { get; set; }
        public IList<MessageDetails> _lstMessageDetails { get; set; }
        public IList<TaskDocument> _lstDocumentDetails { get; set; }

        bool _isTokenValid = false;
        string _userID = string.Empty;

        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
              
                if (ValidateToken())
                {
                    _userID = Application["userID"].ToString();
                    lblClientname.Text = Application["UserName"].ToString();
                }
                GetClientDetails(_userID);
                GetDefaultGrid();


            }
        }
        protected  void chkRow_CheckedChanged(object sender, EventArgs e)
        {
            GridViewRow row = ((GridViewRow)((CheckBox)sender).NamingContainer);
            int index = row.RowIndex;
            CheckBox cb1 = (CheckBox)grdTask.Rows[index].FindControl("chkRow");
            if (cb1.Checked)
            {
                long TSKID = Convert.ToInt64((row.Cells[2].FindControl("lblTSKID") as Label).Text);              
                GetMessageDetails(TSKID);
            }

        }

        private async void GetClientDetails(string id)
        {
            if (ValidateToken())
            {
                string token = Application["token"].ToString();
                HttpClient _client = new HttpClient();
                var contentType = new MediaTypeWithQualityHeaderValue("application/json");
                _client.DefaultRequestHeaders.Accept.Add(contentType);
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage response = await _client.GetAsync("http://clientapi.azurewebsites.net/api/ClientDetails/GetClientDetailsbyId/" + id);
                string apiResponse = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    _lstclientDetails = JsonConvert.DeserializeObject<List<ClientDetails>>(apiResponse).ToList();
                    GetTaskDetails(_lstclientDetails[0].Cid);
                    lblClientname.Text = _lstclientDetails[0].ClientName.ToString();
                   
                }
            }

        }

        private async void GetTaskDetails(long CID)
        {
            if (ValidateToken())
            {
                string token = Application["token"].ToString();
                HttpClient _client = new HttpClient();
                var contentType = new MediaTypeWithQualityHeaderValue("application/json");
                _client.DefaultRequestHeaders.Accept.Add(contentType);
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage response = await _client.GetAsync("http://clientapi.azurewebsites.net/api/ClientDetails/GetTaskDetailsByCID/" + CID);
                string apiResponse = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    _lstTaskDetails = JsonConvert.DeserializeObject<List<TaskDetails>>(apiResponse).ToList();
                }
                grdTask.DataSource = _lstTaskDetails;
                grdTask.DataBind();
                lblTaskCount.Text = _lstTaskDetails.Count.ToString();
            }

        }
        private async void GetMessageDetails(long TSKID)
        {
            if (ValidateToken())
            {
                string token = Application["token"].ToString();
                HttpClient _client = new HttpClient();
                var contentType = new MediaTypeWithQualityHeaderValue("application/json");
                _client.DefaultRequestHeaders.Accept.Add(contentType);
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage response = await _client.GetAsync("http://clientapi.azurewebsites.net/api/ClientDetails/GetMessagebyTSKID/" + TSKID);
                string apiResponse = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                   _lstMessageDetails = JsonConvert.DeserializeObject<List<MessageDetails>>(apiResponse).ToList();
                    grdMsg.DataSource = _lstMessageDetails;
                    grdMsg.DataBind();

                }
            }

        }
        private async void GetDocumentDetails(long TSKID)
        {
            if (ValidateToken())
            {
                string token = Application["token"].ToString();
                HttpClient _client = new HttpClient();
                var contentType = new MediaTypeWithQualityHeaderValue("application/json");
                _client.DefaultRequestHeaders.Accept.Add(contentType);
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage response = await _client.GetAsync("http://clientapi.azurewebsites.net/api/ClientDetails/GetDocumentsByTaskID/" + TSKID);
                string apiResponse = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    _lstDocumentDetails = JsonConvert.DeserializeObject<List<TaskDocument>>(apiResponse).ToList();
                    grdDocument.DataSource = _lstDocumentDetails;
                    grdDocument.DataBind();

                }
            }
        }
        private async void UploadDocument(long TSKID, byte[] filedata, string filename, string filetype, long CID)
        {
            if (ValidateToken())
            {
                string token = Application["token"].ToString();
                string _userID = Application["userID"].ToString();

                TaskDocument _tskDocu = new TaskDocument();
                _tskDocu.Cid = CID;
                string documentname = Path.GetFileName(filename);
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
                HttpResponseMessage response = await _client.PostAsync("http://clientapi.azurewebsites.net/api/ClientDetails/TaskDocumentUpload/", data);

                string apiResponse = await response.Content.ReadAsStringAsync();
                string result = response.Content.ReadAsStringAsync().Result;
                GetDocumentDetails(TSKID);

            }
        }
        private async void SendMessage(long TSKID, long CID)
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
                msgDetails.CommentedBy = "Client";
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
                HttpResponseMessage response = await _client.PostAsync("http://clientapi.azurewebsites.net/api/ClientDetails/PostMessageDetails/", data);

                string apiResponse = await response.Content.ReadAsStringAsync();
                string result = response.Content.ReadAsStringAsync().Result;
                GetMessageDetails(TSKID);

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

        private void GetDefaultGrid()
        {
            DataTable dtDefaultDocument = new DataTable();
            DataTable dtDefaultMsg = new DataTable();
            grdDocument.DataSource = dtDefaultDocument;
            grdDocument.DataBind();
            grdMsg.DataSource = dtDefaultMsg;
            grdMsg.DataBind();
        }

        protected void btnSendMsg_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in grdTask.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox c = (CheckBox)row.FindControl("chkRow");
                    if (c.Checked)
                    {
                        long TSKID = Convert.ToInt64((row.Cells[2].FindControl("lblTSKID") as Label).Text);
                        long CID = Convert.ToInt64((row.Cells[3].FindControl("lblCID") as Label).Text);
                        SendMessage(TSKID, CID);
                        //GetMessageDetails(TSKID);
                    }
                    else
                        GetDefaultGrid();
                }
            }
        }

        protected void chkDoc_CheckedChanged(object sender, EventArgs e)
        {
            GridViewRow row = ((GridViewRow)((CheckBox)sender).NamingContainer);
            int index = row.RowIndex;
            CheckBox cb1 = (CheckBox)grdTask.Rows[index].FindControl("chkDoc");
            if (cb1.Checked)
            {
                long TSKID = Convert.ToInt64((row.Cells[2].FindControl("lblTSKID") as Label).Text);
                long CID = Convert.ToInt64((row.Cells[3].FindControl("lblCID") as Label).Text);
                GetDocumentDetails(TSKID);
            }
            else
                GetDefaultGrid();
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (ValidateToken())
            {
               
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
                    foreach (GridViewRow row in grdTask.Rows)
                    {
                        if (row.RowType == DataControlRowType.DataRow)
                        {
                            CheckBox c = (CheckBox)row.FindControl("chkDoc");
                            if (c.Checked)
                            {
                                long TSKID = Convert.ToInt64((row.Cells[2].FindControl("lblTSKID") as Label).Text);
                                long CID = Convert.ToInt64((row.Cells[3].FindControl("lblCID") as Label).Text);

                                UploadDocument(TSKID, fileData, filename, _strmimeType, CID);
                            }
                        }
                    }

                }
            }
        }

      
    }
}