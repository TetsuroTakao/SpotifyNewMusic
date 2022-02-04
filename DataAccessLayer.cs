using Microsoft.Extensions.FileProviders;
using SpotifyNewMusic.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace SpotifyNewMusic
{
    public class DataAccessLayer
    {
        public IFileInfo StateManagementFile { get; set; }
        public List<IFileInfo> BrandSVGFiles { get; set; }
        public DataAccessLayer() 
        {
            var provider = new PhysicalFileProvider(Directory.GetCurrentDirectory());
            var filePath = Path.Combine("wwwroot", "contents", "loginstate.json");
            StateManagementFile = provider.GetFileInfo(filePath);
            var _fileFilter = Path.Combine("wwwroot", "contents", "Assets");
            var svgFiles = provider.GetDirectoryContents(_fileFilter);
            if (BrandSVGFiles == null) BrandSVGFiles = new List<IFileInfo>();
            foreach (var i in svgFiles) 
            {
                if (!i.IsDirectory && i.Name.EndsWith(".svg")) BrandSVGFiles.Add(i);
            }
            #region change tracking
            //var contents = provider.GetDirectoryContents(string.Empty);
            //var _fileFilter = Path.Combine("TextFiles", "*.txt");
            //IChangeToken token = provider.Watch(_fileFilter);
            //var tcs = new TaskCompletionSource<object>();
            //token.RegisterChangeCallback(state =>((TaskCompletionSource<object>)state).TrySetResult(null), tcs);
            //await tcs.Task.ConfigureAwait(false);
            #endregion
        }
        public string SetStateManagement<T>(LoginModel<T> loginstate) where T : BrandPrimitive
        {
            string result = string.Empty;
            try
            {
                var loginStates = new List<LoginModel<T>>();
                var hasMemberId = false;
                if (typeof(T) == typeof(FBGraphUser)) if(!string.IsNullOrEmpty((loginstate as LoginModel<FBGraphUser>).MemberID)) hasMemberId = true;
                if (typeof(T) == typeof(SPGraphUser)) if (!string.IsNullOrEmpty((loginstate as LoginModel<SPGraphUser>).MemberID)) hasMemberId = true;
                if (typeof(T) == typeof(MSGraphUser)) if (!string.IsNullOrEmpty((loginstate as LoginModel<MSGraphUser>).MemberID)) hasMemberId = true;
                if (!hasMemberId) 
                {
                    var id = Guid.NewGuid().ToString();
                    if (typeof(T) == typeof(FBGraphUser)) (loginstate as LoginModel<FBGraphUser>).MemberID = id;
                    if (typeof(T) == typeof(SPGraphUser)) (loginstate as LoginModel<SPGraphUser>).MemberID = id;
                    if (typeof(T) == typeof(MSGraphUser)) (loginstate as LoginModel<MSGraphUser>).MemberID = id;
                }
                loginStates.Add(loginstate);
                string jsonstring = string.Empty;
                var f = new FileInfo(StateManagementFile.PhysicalPath);
                if (!f.Directory.Exists) return f.Directory.FullName;
                if (f.Exists) jsonstring = File.ReadAllText(f.FullName);
                if (!string.IsNullOrEmpty(jsonstring))
                {
                    var list = JsonSerializer.Deserialize<List<LoginModel<T>>>(jsonstring);
                    if (typeof(T) == typeof(LoginModel<MSGraphUser>)) 
                    {
                        LoginModel<MSGraphUser>  exist = (list as List<LoginModel<MSGraphUser>>).Where(l => l.User.UserCore.id == (loginstate as LoginModel<MSGraphUser>).User.UserCore.id).FirstOrDefault();
                        if (exist != null) 
                        {
                            var fbUser = (list as List<LoginModel<FBGraphUser>>).Where(l => l.MemberID == exist.MemberID).FirstOrDefault();
                            if (fbUser != null) result += ",facebook";
                            var spUser = (list as List<LoginModel<SPGraphUser>>).Where(l => l.MemberID == exist.MemberID).FirstOrDefault();
                            if (spUser != null) result += ",spotify";

                            (list as List<LoginModel<MSGraphUser>>).Remove(exist);

                        }
                    }
                    if (typeof(T) == typeof(LoginModel<FBGraphUser>))
                    {
                        LoginModel<FBGraphUser> exist = (list as List<LoginModel<FBGraphUser>>).Where(l => l.User.UserCore.id == (loginstate as LoginModel<FBGraphUser>).User.UserCore.id).FirstOrDefault();
                        var msUser = (list as List<LoginModel<MSGraphUser>>).Where(l => l.MemberID == exist.MemberID).FirstOrDefault();
                        if (msUser != null) result += ",microsoft";
                        var spUser = (list as List<LoginModel<SPGraphUser>>).Where(l => l.MemberID == exist.MemberID).FirstOrDefault();
                        if (spUser != null) result += ",spotify";
                        if (exist != null) (list as List<LoginModel<FBGraphUser>>).Remove(exist);
                    }
                    if (typeof(T) == typeof(LoginModel<SPGraphUser>))
                    {
                        LoginModel<SPGraphUser> exist = (list as List<LoginModel<SPGraphUser>>).Where(l => l.User.UserCore.id == (loginstate as LoginModel<SPGraphUser>).User.UserCore.id).FirstOrDefault();
                        var msUser = (list as List<LoginModel<MSGraphUser>>).Where(l => l.MemberID == exist.MemberID).FirstOrDefault();
                        if (msUser != null) result += ",microsoft";
                        var fbUser = (list as List<LoginModel<FBGraphUser>>).Where(l => l.MemberID == exist.MemberID).FirstOrDefault();
                        if (fbUser != null) result += ",facebook";
                        if (exist != null) (list as List<LoginModel<SPGraphUser>>).Remove(exist);
                    }
                    loginStates.AddRange(list);
                }
                using (StreamWriter sw = File.CreateText(f.FullName))
                {
                    using (Utf8JsonWriter writer = new Utf8JsonWriter(sw.BaseStream))
                    {
                        JsonSerializer.Serialize(writer, loginStates, typeof(List<T>));
                    }
                }
            }
            catch (Exception ex) 
            {
                result = ex.Message;
            }
            return result;
        }
        public T GetStateManagement<T>(string id) where T : LoginModel<BrandPrimitive>, new()
        {
            var result = new T();
            if (StateManagementFile.Exists)
            {
                byte[] b = new byte[0];
                using (var st = StateManagementFile.CreateReadStream())
                {
                    var len = st.ReadAsync(b, 0, (int)st.Length).Result;
                    var json = UTF8Encoding.UTF8.GetString(b);
                    if (!string.IsNullOrEmpty(json))
                    {
                        foreach (var e in JsonDocument.Parse(json).RootElement.EnumerateArray())
                        {
                            var item = JsonSerializer.Deserialize<T>(e.GetString());
                            if (item != null)
                            {
                                if (item.User.UserCore.id == id) 
                                {
                                    result = item;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }
        public List<Tuple<string,string>> GetBrandSVG() 
        {
            var result = new List<Tuple<string,string>>();
            foreach (var svgfile in BrandSVGFiles) 
            {
                var st = File.ReadAllText(svgfile.PhysicalPath);
                result.Add(new Tuple<string, string>(svgfile.Name.Split('.').FirstOrDefault(), st));
            }
            return result;
        }
        public string ReadJwt(string jwt, string payloadKey = "") 
        {
            var result = string.Empty;
            if (jwt.Split('.').Count() != 3) return result;
            var tokenType = Base64UrlEncoder.Decode(jwt.Split('.').FirstOrDefault());
            var typeElement = new JsonElement();
            if(JsonDocument.Parse(tokenType).RootElement.TryGetProperty("typ",out typeElement))
            {
                if (typeElement.GetString() == "JWT")
                {
                    var token = new JwtSecurityTokenHandler().ReadJwtToken(jwt);
                    if (string.IsNullOrEmpty(payloadKey))
                    {
                        if (token.Payload.Iss.Contains("sts.windows.net"))
                        {
                            result = "microsoft";
                        }
                    }
                    else
                    {
                        result = token.Payload[payloadKey].ToString();
                    }
                }
            }
            return result;
        }
    }
}
