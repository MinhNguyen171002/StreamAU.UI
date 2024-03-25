using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using UserUI.Data.Model;
using static UserUI.Data.Model.UserPlayList;
using static UserUI.Data.Model.YoutubeVideo;

namespace UserUI.Data.Service
{
    public class Service
    {
        public List<UserPlayList> playLists = new List<UserPlayList>();
        public List<YoutubeVideo> details = new List<YoutubeVideo>();
        private HttpClient client;
        private ILocalStorageService _localStorageService;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        public List<YoutubeVideos> Videos = new List<YoutubeVideos>();
        public YoutubeVideos videos = new YoutubeVideos();
        public SpeechtoText speech;
        public string RecognitionText;
        private string httpadd = "https://earlyshinyshop2.conveyor.cloud/";
        public Service(HttpClient client, AuthenticationStateProvider _authenticationStateProvider, ILocalStorageService _localStorageService, SpeechtoText speech)
        {
            this.client = client;
            this._authenticationStateProvider = _authenticationStateProvider;
            this._localStorageService = _localStorageService;
            this.speech = speech;
        }
        public async Task<List<YoutubeVideos>> SearchAsync(string key)
        {
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Get, httpadd + "API/YoutubeAPI/search");

                request.RequestUri = new Uri(httpadd + $"API/YoutubeAPI/search?key={key}");
                var response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Root videos = JsonConvert.DeserializeObject<Root>(content);
                    Videos = videos.videos;
                }

            }
            return Videos;
        }

        public async Task<List<YoutubeVideos>> RemcomAsync(string value)
        {
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Get, httpadd + "API/YoutubeAPI/recommend");
                request.RequestUri = new Uri(httpadd + $"API/YoutubeAPI/recommend?reg={value}");
                var response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Root videos = JsonConvert.DeserializeObject<Root>(content);
                    Videos = videos.videos;
                }
            }
            return Videos;
        }

        public async Task<LoginResponse> UserLogin(LoginModel login)
        {
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Post, httpadd + "API/UserAPI/login");
                var json = JsonConvert.SerializeObject(login);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.SendAsync(request);
                var loginResult = JsonConvert.DeserializeObject<LoginResponse>(await response.Content.ReadAsStringAsync());
                if (!response.IsSuccessStatusCode)
                {
                    return loginResult;
                }
                await _localStorageService.SetItemAsync("authToken", loginResult.Token);
                ((CustomAuthProvider)_authenticationStateProvider).MarkUserAsAuthenticated(login.Username);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", loginResult.Token);

                return loginResult;
            }
        }
        public async Task<Response> UserRegister(RegisterModel model)
        {
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Post, httpadd + "API/UserAPI/register");
                var json = JsonConvert.SerializeObject(model);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var resposne = JsonConvert.DeserializeObject<Response>(await response.Content.ReadAsStringAsync());
                    return resposne;
                }
                else
                {
                    throw new Exception($"Failed to sign in: {response.StatusCode}");
                }
            }
        }

        public async Task<Response> AddplayList(string username, string plname)
        {
            using (var client = new HttpClient())
            {

                var request = new HttpRequestMessage(HttpMethod.Post, httpadd + "API/UserAPI/addplaylist");
                request.RequestUri = new Uri(httpadd + $"API/UserAPI/addplaylist?username={username}" + $"&plname={plname}");
                var response = await client.SendAsync(request);
                var content = JsonConvert.DeserializeObject<Response>(await response.Content.ReadAsStringAsync());
                if (!response.IsSuccessStatusCode)
                {
                    return content;
                }
                return content;
            }
        }

        public async Task<List<UserPlayList>> GetUserList(string username)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, httpadd + "API/YoutubeAPI/list");

            request.RequestUri = new Uri(httpadd + $"API/YoutubeAPI/list?username={username}");
            var response = await client.SendAsync(request);
            using (var client = new HttpClient())
            {
                var content = await response.Content.ReadAsStringAsync();
                RootUser playlist = JsonConvert.DeserializeObject<RootUser>(content);
                playLists = playlist.pl;
            }
            return playLists;

        }

        public async Task<List<YoutubeVideo>> GetVideoDetail(Guid? id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, httpadd + "API/YoutubeAPI/details");

            request.RequestUri = new Uri(httpadd + $"API/YoutubeAPI/details?id={id}");
            var response = await client.SendAsync(request);
            using (var client = new HttpClient())
            {
                var content = await response.Content.ReadAsStringAsync();
                RootVideo detail = JsonConvert.DeserializeObject<RootVideo>(content);
                details = detail.details;
            }
            return details;
        }


        public async Task<Response> AddVideo(string name,string id,string plname)
        {
            using (var client = new HttpClient())
            {

                var request = new HttpRequestMessage(HttpMethod.Post, httpadd + "API/UserAPI/addvideo");
                request.RequestUri = new Uri(httpadd + $"API/UserAPI/addvideo?username={name}" + $"&Id={id}" + $"&plname={plname}");
                var response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var resposne = JsonConvert.DeserializeObject<Response>(await response.Content.ReadAsStringAsync());
                    return resposne;
                }
                else
                {
                    throw new Exception($"Failed to sign in: {response.StatusCode}");
                }
            }
        }
        /*public async Task ShazamAPI()
        {
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri("https://shazam.p.rapidapi.com/songs/v2/detect"),
                    Headers =
                    {
                        { "X-RapidAPI-Key", "971c9da308msh677bf1067a80634p1175e5jsn50eebc73a2f6" },
                        { "X-RapidAPI-Host", "shazam.p.rapidapi.com" },
                    },
                    Content = new StringContent(speech.base64Str)
                    {
                        Headers =
                    {
                        ContentType = new MediaTypeHeaderValue("text/plain")
                    }
                    }
                };
                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    var body = await response.Content.ReadAsStringAsync();
                }
            }
        }*/
        /*public async Task GoogleSpeech(byte[] data)
        {
            using (var client = new HttpClient())
            {

                var request = new HttpRequestMessage(HttpMethod.Post, httpadd + "API/UserAPI/speechtotext");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/bson"));

                var response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var resposne = JsonConvert.DeserializeObject<Response>(await response.Content.ReadAsStringAsync());
                    return resposne;
                }
                else
                {
                    throw new Exception($"Failed to sign in: {response.StatusCode}");
                }
            }
        }*/
        public void StartListen()
        {
             speech.StartListen();
        }
        public void StopListen()
        {
            speech.StopListen();
        }
        public async Task Logout()
        {
            await _localStorageService.RemoveItemAsync("authToken");
            ((CustomAuthProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
            client.DefaultRequestHeaders.Authorization = null;
        }
    }
}