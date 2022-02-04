﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpotifyNewMusic.Models
{
    public class LoginModel<T> where T: BrandPrimitive
    {
        public string MemberID { get; set; }
        public string AuthrizeUrl { get; set; }
        public bool IsLogin { get; set; }
        public bool IsMember { get; set; }
        public ApplicationUser<T> User { get; set; }
    }
    public enum IPBlandType 
    {
        Microsoft,
        Facebook,
        Twitter,
        Google,
        Amazon,
        GitHub,
        Instagram,
        Spotify
    }
    public class ApplicationUser<T>
    {
        public bool IsAADUser
        {
            get
            {
                return (BrandName == IPBlandType.Microsoft);
            }
        }
        public IPBlandType BrandName { get; set; }
        public T UserCore { get; set; }
        public List<AccessHistory> AccessList { get; set; }
        public string GlobalName { get; set; }
    }
    public class BrandPrimitive 
    {
        public string id { get; set; }
    }
    public class MSGraphUser: BrandPrimitive
    {
        public string displayName { get; set; }
        public string surname { get; set; }
        public string givenName { get; set; }
        public string userPrincipalName { get; set; }
        public List<string> businessPhones { get; set; }
        public string jobTitle { get; set; }
        public string mail { get; set; }
        public string mobilePhone { get; set; }
        public string officeLocation { get; set; }
        public string preferredLanguage { get; set; }
    }
    public class FBGraphUser: BrandPrimitive
    {
        public string name { get; set; }
    }
    public class SPGraphUser : BrandPrimitive
    {
        public string country { get; set; }
        public string display_name { get; set; }
        public string email { get; set; }
        public Explicit_content explicit_content { get; set; }
        public SpotifyUrl external_urls { get; set; }
        public FollowersProperties followers { get; set; }
        public string href { get; set; }
        public List<SpotityImage> images { get; set; }
        public string product { get; set; }
        public string type { get; set; }
        public string uri { get; set; }
    }

    public class Explicit_content
    {
        public bool filter_enabled { get; set; }

        public bool filter_locked { get; set; }
    }

    public class AccessHistory
    {
        public string AADEndPoint { get; set; }
        public string TenantID { get; set; }
        public string ClientId { get; set; }
        public string Secret { get; set; }
        public Uri Redirect { get; set; }
        public string AuthCode { get; set; }
        public string Resource { get; set; }
        public string Scope { get; set; }
        public string GrantType { get; set; }
        public string ResponseType { get; set; }
        public OAuthTokenModel AuthTokens { get; set; }
    }
    public class OAuthTokenModel
    {
        public string token_type { get; set; }
        public string scope { get; set; }
        public int expires_in { get; set; }
        public int ext_expires_in { get; set; }
        public string access_token { get; set; }
        public string refresh_token { get; set; }
        public string id_token { get; set; }
    }
}
