using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RoadsOfRussiaAPI.Controllers.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace RoadsOfRussiaDLL.Document
{
    public class DocumentController
    {
        private string jwtToken { get; set; }

        public async Task<string> Authorization(string name, string password)
        {
            try
            {
                var authorizationData = new AuthorizationModel() { name = name, password = password };
                var jsonData = JsonConvert.SerializeObject(authorizationData);

                using (var client = new HttpClient())
                {
                    var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PutAsync(@"http://localhost:5246/api/v1/SignIn", content);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseData = await response.Content.ReadAsStringAsync();
                        var tokenResponse = JsonConvert.DeserializeObject(responseData);

                        jwtToken = tokenResponse.ToString();
                        return tokenResponse.ToString();
                    }
                    else
                    {
                        string errorData = await response.Content.ReadAsStringAsync();
                        return errorData.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<List<DocumentsModel>> GetDocuments()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

                    HttpResponseMessage response = await client.GetAsync(@"http://localhost:5246/api/v1/Documents");

                    if (response.IsSuccessStatusCode)
                    {
                        string responseData = await response.Content.ReadAsStringAsync();
                        var documents = JsonConvert.DeserializeObject<List<DocumentsModel>>(responseData);

                        return documents;
                    }
                    else
                    {
                        //string errorData = await response.Content.ReadAsStringAsync();
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<CommentModel>> GetComments(int documentId)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

                    HttpResponseMessage response = await client.GetAsync($@"http://localhost:5246/api/v1/Documents/{documentId}/Comments");

                    if (response.IsSuccessStatusCode)
                    {
                        string responseData = await response.Content.ReadAsStringAsync();
                        var comments = JsonConvert.DeserializeObject<List<CommentModel>>(responseData);

                        return comments;
                    }
                    else
                    {
                        //string errorData = await response.Content.ReadAsStringAsync();
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<string> SetComment(int documentID, string text, int authorId)
        {
            try
            {
                var comment = new CommentContextModel() { text = text, autor_id = authorId };

                string json = JsonConvert.SerializeObject(comment);

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync($@"http://localhost:5246/api/v1/Documents/{documentID}/Comment", content);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseData = await response.Content.ReadAsStringAsync();
                        return responseData;
                    }
                    else
                    {
                        string errorData = await response.Content.ReadAsStringAsync();
                        return errorData;
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
