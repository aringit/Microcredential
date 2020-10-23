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

namespace ClientUI
{
    public partial class login : System.Web.UI.Page
    {
        public LoginModel _LoginModel = new LoginModel();
        public string stringJWT = string.Empty;
        public IList<AspNetUsers> _lstUsers { get; set; }


        protected async void Page_Load(object sender, EventArgs e)
        {
            lblLoginMessage.Text = string.Empty;
             stringJWT = string.Empty;
            Application["token"] = string.Empty;
            Application["username"] = string.Empty;
        }

        protected async void btnLogin_Click(object sender, EventArgs e)
        {
            lblLoginMessage.Text = string.Empty;
            _LoginModel.Username = txtUsername.Text.ToString();
            _LoginModel.Password = txtPassword.Text.ToString();
            HttpClient _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
            string stringData = JsonConvert.SerializeObject(_LoginModel);
            var contentData = new StringContent(stringData, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PostAsync("http://clientapi.azurewebsites.net/api/authenticate/login", contentData);
            if (response.IsSuccessStatusCode)
            {
                stringJWT = response.Content.ReadAsStringAsync().Result;
                if (!string.IsNullOrEmpty(stringJWT))
                {
                    JWT jwt = JsonConvert.DeserializeObject<JWT>(stringJWT);
                    Application["token"] = jwt.Token;
                    Response.Redirect("index.aspx");
                    GetUserID(txtUsername.Text.ToString(), jwt.Token);
                }

                //Session to store JWT Token               
                // HttpContext.Session.SetString("token", jwt.Token);
                // Logout
                //HttpContext.Session.Remove("token");
                //Message = "User logged out successfully!";
            }
            //Session to store JWT Token



            // HttpContext.Session.SetString("token", jwt.Token);

            // Logout
            //HttpContext.Session.Remove("token");
            //Message = "User logged out successfully!";
        
            else
                lblLoginMessage.Text = "Invalid Username or Password";
        }

        private async void GetUserID(string username, string token)
        {
            HttpClient _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            //HttpResponseMessage response = await _client.GetAsync("http://localhost:54213/api/AuditorPortfolios/GetUserDetails/"+username.ToString());           
            HttpResponseMessage response = await _client.GetAsync("http://clientapi.azurewebsites.net/api/ClientDetails/GetUserDetails/" + username.ToString());
            string apiResponse = response.Content.ReadAsStringAsync().Result;
            if (response.IsSuccessStatusCode)
            {
                _lstUsers = JsonConvert.DeserializeObject<List<AspNetUsers>>(apiResponse).ToList();
                Application["userID"] = _lstUsers[0].Id.ToString();
                Application["UserName"] = _lstUsers[0].NormalizedUserName.ToString();

            }



        }
    }
}