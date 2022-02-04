using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Extensions;
using Microsoft.OpenApi.Readers;
using SpotifyNewMusic.Models;

namespace SpotifyNewMusic.Controllers
{
    public class HomeController : Controller
    {
        private readonly IStringLocalizer<HomeController> _localizer;

        private readonly ILogger<HomeController> _logger;
        string clientId { get; set; }
        string redirectUri { get; set; }
        string endpoint { get; set; }
        string clientSecret { get; set; }
        string profileScope { get; set; }
        string profileResource { get; set; }
        string tokenSTS { get; set; }
        string key { get; set; }

        public HomeController(ILogger<HomeController> logger, IStringLocalizer<HomeController> localizer)
        {
            _localizer = localizer;
            _logger = logger;
            redirectUri = Environment.GetEnvironmentVariable("RedirectLocal");
            switch (Environment.GetEnvironmentVariable("TargetBrand"))
            {
                case "microsoft":
                    key = "MicrosoftIdentity";
                    break;
                case "facebook":
                    key = "FacebookIdentity";
                    break;
                case "spotify":
                    key = "SpotifyIdentity";
                    break;
                default:
                    key = "MicrosoftIdentity";
                    break;
            }
            endpoint = Environment.GetEnvironmentVariable(key + "_Endpoint");
            clientId = Environment.GetEnvironmentVariable(key + "_ClientId");
            clientSecret = Environment.GetEnvironmentVariable(key + "_ClientSecret");
            profileScope = Environment.GetEnvironmentVariable(key + "_ProfileScope");
            profileResource = Environment.GetEnvironmentVariable(key + "_ProfileResource");
            tokenSTS = Environment.GetEnvironmentVariable(key + "_TokenSTS");
        }

        public IActionResult Index()
        {
            var artists = new List<Artist>();
            var albums = new List<Album>();
            var messages = new List<MessageModel>();
            #region Get Authorization code
            var query = Request.Query;
            var referer = Request.Headers["Referer"].ToString();
            var code = string.Empty;
            if (query.ContainsKey("code")) code = query["code"];//if (query.ContainsKey("session_state")) state = query["session_state"];
            #endregion
            var isSignin = !string.IsNullOrEmpty(code);
            var brandsList = new List<KeyValuePair<string, bool>>();
            var brands = Environment.GetEnvironmentVariable("Brands");
            if (string.IsNullOrEmpty(brands))
            {
                var pair = new KeyValuePair<string, bool>();
                foreach (var i in Environment.GetEnvironmentVariables())
                {
                    if (i.GetType() == typeof(DictionaryEntry))
                    {
                        var key = string.Empty;
                        if (((DictionaryEntry)i).Key.ToString().Contains("MicrosoftIdentity"))
                        {
                            key = "microsoft";
                        }
                        if (((DictionaryEntry)i).Key.ToString().Contains("FacebookIdentity"))
                        {
                            key = "facebook";
                        }
                        if (((DictionaryEntry)i).Key.ToString().Contains("TwitterIdentity"))
                        {
                            key = "twitter";
                        }
                        if (((DictionaryEntry)i).Key.ToString().Contains("SpotifyIdentity"))
                        {
                            key = "spotify";
                        }
                        if (!string.IsNullOrEmpty(key))
                        {
                            pair = new KeyValuePair<string, bool>(key, false);
                            if (brandsList.Where(d => d.Key == key).Count() == 0) brandsList.Add(pair);
                        }
                    }
                }
                brands = JsonSerializer.Serialize(brandsList);
            }
            brandsList = JsonSerializer.Deserialize<List<KeyValuePair<string, bool>>>(brands);
            if (isSignin)
            {
                var brand = Environment.GetEnvironmentVariable("TargetBrand");
                using (var httpClient = new HttpClient())
                {
                    #region Get Access token
                    var properties = "client_id=" + clientId + "&client_secret=" + clientSecret;
                    properties += "&redirect_uri=" + redirectUri;
                    properties += "&scope=" + profileScope;
                    properties += "&code=" + code + "&grant_type=authorization_code";
                    var seed = Environment.GetEnvironmentVariable("ChallengeSeed");
                    //challenge length is 43-128 characters
                    var challengeLength = int.Parse(Environment.GetEnvironmentVariable("ChallengeLength"));
                    properties += "&code_verifier=" + new Encryptor().CreateChallengeText(seed, challengeLength);
                    HttpResponseMessage res = null;
                    if (brand == "microsoft")
                    {
                        var content = new StringContent(properties, Encoding.UTF8, "application/x-www-form-urlencoded");
                        res = httpClient.PostAsync(tokenSTS, content).Result;
                    }
                    if (Environment.GetEnvironmentVariable("TargetBrand") == "facebook")
                    {
                        res = httpClient.GetAsync(tokenSTS + "?" + properties).Result;
                    }
                    if (Environment.GetEnvironmentVariable("TargetBrand") == "spotify")
                    {
                        var bearerToken = Environment.GetEnvironmentVariable(key + "_BearerToken");
                        httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + bearerToken);
                        properties = string.Join("&", properties.Split('&').Where(r => r.StartsWith("redirect_uri") || r.StartsWith("grant_type") || r.StartsWith("code")));
                        var content = new StringContent(properties, Encoding.UTF8, "application/x-www-form-urlencoded");

                        res = httpClient.PostAsync(tokenSTS, content).Result;
                    }
                    string resultJson = res.Content.ReadAsStringAsync().Result;
                    var accessResult = JsonSerializer.Deserialize<OAuthTokenModel>(resultJson);
                    var tempElement = new JsonElement();
                    #endregion
                    var token = string.Empty;
                    if (res.IsSuccessStatusCode)
                    {
                        var doc = JsonDocument.Parse(resultJson).RootElement;
                        if (doc.TryGetProperty("access_token", out tempElement))
                        {
                            token = tempElement.GetString();
                        }

                        using (var httpClient2 = new HttpClient())
                        {
                            httpClient2.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                            var res2 = httpClient2.GetAsync(profileResource).Result;
                            string resultJson2 = res2.Content.ReadAsStringAsync().Result;
                            if (res2.IsSuccessStatusCode)
                            {
                                BrandPrimitive user = null;
                                var saveResult = string.Empty;
                                if (brand == "microsoft")
                                {
                                    user = JsonSerializer.Deserialize<MSGraphUser>(resultJson2);
                                    ViewBag.AccountName = (user as MSGraphUser).displayName;
                                    var appUser = new ApplicationUser<MSGraphUser>() { UserCore = (user as MSGraphUser), BrandName = IPBlandType.Microsoft, AccessList = new List<AccessHistory>() };
                                    appUser.AccessList.Add(new AccessHistory() { AuthTokens = accessResult, AADEndPoint = tokenSTS, AuthCode = code, ClientId = clientId, GrantType = "authorization_code", Redirect = new Uri(redirectUri), Scope = profileScope });
                                    var loginState = new LoginModel<MSGraphUser>() { AuthrizeUrl = tokenSTS, IsLogin = true, User = appUser };
                                    saveResult = new DataAccessLayer().SetStateManagement<MSGraphUser>(loginState);
                                }
                                if (brand == "facebook")
                                {
                                    user = JsonSerializer.Deserialize<FBGraphUser>(resultJson2);
                                    ViewBag.AccountName = (user as FBGraphUser).name;
                                    var appUser = new ApplicationUser<FBGraphUser>() { UserCore = (user as FBGraphUser), BrandName = IPBlandType.Facebook };
                                    var loginState = new LoginModel<FBGraphUser>() { AuthrizeUrl = tokenSTS, IsLogin = true, User = appUser };
                                    saveResult = new DataAccessLayer().SetStateManagement<FBGraphUser>(loginState);
                                }
                                if (brand == "spotify")
                                {
                                    user = JsonSerializer.Deserialize<SPGraphUser>(resultJson2);
                                    ViewBag.AccountName = (user as SPGraphUser).display_name;
                                    var appUser = new ApplicationUser<SPGraphUser>() { UserCore = (user as SPGraphUser), BrandName = IPBlandType.Spotify };
                                    var loginState = new LoginModel<SPGraphUser>() { AuthrizeUrl = tokenSTS, IsLogin = true, User = appUser };
                                    saveResult = new DataAccessLayer().SetStateManagement<SPGraphUser>(loginState);
                                    messages = GetContents(ref artists, ref albums, "new-releases-link", "");
                                    var spotifyMessages = GetContents(ref artists,ref albums, "new-releases", token);
                                }
                                var target = brandsList.Where(b => b.Key == brand).FirstOrDefault();
                                brandsList.Remove(target);
                                brandsList.Add(new KeyValuePair<string, bool>(brand, true));
                                if (saveResult.Contains(","))
                                {
                                    foreach (var r in saveResult.Split(','))
                                    {
                                        if (string.IsNullOrEmpty(r)) continue;
                                        target = brandsList.Where(b => b.Key == r).FirstOrDefault();
                                        brandsList.Remove(target);
                                        brandsList.Add(new KeyValuePair<string, bool>(r, true));
                                    }
                                }
                            }
                        }
                    }
                }
            }
            ViewBag.DefaultBrand = Environment.GetEnvironmentVariable("DefaultBrand");
            if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DefaultBrand")))
            {
                var firstBrand = brandsList.Where(b => b.Key == Environment.GetEnvironmentVariable("DefaultBrand")).FirstOrDefault();
                if (!string.IsNullOrEmpty(firstBrand.Key))
                {
                    brandsList.Remove(firstBrand);
                    brandsList.Insert(0, firstBrand);
                }
            }
            ViewBag.Brands = brandsList;
            ViewBag.IsSignin = isSignin;
            if (messages.FirstOrDefault() == null)
            {
                ViewBag.Messages = new List<MessageModel>();
                ViewBag.Messages.Add(new MessageModel() { EventMessage = DateTime.Now.ToLongDateString(), Type = MessageType.Info });
            }
            else 
            {
                ViewBag.Messages = messages;
            }
            ViewBag.Artists = artists;
            ViewBag.Albums = albums;
            return View();
        }

        List<MessageModel> GetContents(ref List<Artist> artists, ref List<Album> albums, string contentsType,string bearerToken)
        {
            var messages = new List<MessageModel>();
            switch (contentsType)
            {
                case "yamlVarify":
                    using (var client = new HttpClient())
                    {
                        var url = "https://raw.githubusercontent.com/APIs-guru/openapi-directory/master/APIs/spotify.com/v1/swagger.yaml";
                        var stream = client.GetStreamAsync(url).Result;
                        var openApiDocument = new OpenApiStreamReader().Read(stream, out var diagnostic);
                        var json = openApiDocument.SerializeAsJson(OpenApiSpecVersion.OpenApi2_0);
                    }
                    break;
                case "artist":
                    using (var client = new HttpClient())
                    {
                        var url = "https://api.spotify.com/v1/search?q=VAN%20Halen&type=artist";
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
                        var response = client.GetAsync(url).Result;
                        var result = response.Content.ReadAsStringAsync().Result;
                        var items = new JsonElement();
                        if (response.IsSuccessStatusCode)
                        {
                            if (JsonDocument.Parse(result).RootElement.TryGetProperty("artists", out items))
                            {
                                artists = JsonSerializer.Deserialize<List<Artist>>(items.GetProperty("items").ToString());
                            }
                        }
                        else
                        {
                            if (JsonDocument.Parse(result).RootElement.TryGetProperty("error", out items))
                            {
                                var message = items.GetProperty("message").ToString();
                                var httpcode = items.GetProperty("status").ToString();
                                var log = new MessageModel() { EventMessage = result, EventOccours = DateTime.Now, EventSource = url, Memo = client.DefaultRequestHeaders.Authorization.Parameter, Error = new ErrorModel() { ErrorMessage = message, ErrorSource = "" }, Type = MessageType.DisplayError };
                                messages.Add(log);
                            }
                        }
                    }
                    break;
                case "new-releases":
                    using (var client = new HttpClient())
                    {
                        var url = "https://api.spotify.com/v1/browse/new-releases?country=" + Environment.GetEnvironmentVariable("SpotifyIdentity_ISO3166-1");
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
                        var response = client.GetAsync(url).Result;
                        var result = response.Content.ReadAsStringAsync().Result;
                        var items = new JsonElement();
                        if (response.IsSuccessStatusCode)
                        {
                            if (JsonDocument.Parse(result).RootElement.TryGetProperty("albums", out items))
                            {
                                albums = JsonSerializer.Deserialize<List<Album>>(items.GetProperty("items").ToString());
                            }
                        }
                        else
                        {
                            if (JsonDocument.Parse(result).RootElement.TryGetProperty("error", out items))
                            {
                                var message = items.GetProperty("message").ToString();
                                var httpcode = items.GetProperty("status").ToString();
                                var log = new MessageModel() { EventMessage = result, EventOccours = DateTime.Now, EventSource = url, Memo = client.DefaultRequestHeaders.Authorization.Parameter, Error = new ErrorModel() { ErrorMessage = message, ErrorSource = "" }, Type = MessageType.DisplayError };
                                messages.Add(log);
                            }
                        }
                    }
                    break;
                case "new-releases-link":
                    messages.Add(new MessageModel() { EventMessage = "Spotify - " + _localizer["New release"], EventSource = "https://open.spotify.com/playlist/3dEjWfgB5jC6zn6tLoy9yy", Type = MessageType.ExternalLink | MessageType.Display });
                    break;
            }
            return messages;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
