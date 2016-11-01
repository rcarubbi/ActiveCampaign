﻿using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Helpers;

namespace ActiveCampaign
{
    public abstract class Connector
    {
        internal readonly string ACTIVE_CAMPAIGN_URL;
        internal readonly string API_KEY;
        private string _apiUrl;

        public Connector(string apiUrl, string apiKey)
        {

            if (string.IsNullOrEmpty(apiUrl))
                throw new ArgumentException("The reseller or customer API URL was not specified", "apiUrl");
            if (string.IsNullOrEmpty(apiKey))
                throw new ArgumentException("The API key was not specified", "apiKey");

            API_KEY = apiKey;
            ACTIVE_CAMPAIGN_URL = apiUrl;

            _apiUrl = CreateBaseUrl(apiUrl, apiKey);
        }

        internal dynamic Post(ActiveCampaignMethods method, string parameters)
        {
            var request = (HttpWebRequest)WebRequest.Create(_apiUrl + $"&api_action={method}");
            request.ServicePoint.Expect100Continue = false;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Expect = "application/json";

            ASCIIEncoding encoder = new ASCIIEncoding();
            byte[] data = encoder.GetBytes(parameters);

            request.ContentLength = data.Length;
            request.GetRequestStream().Write(data, 0, data.Length);

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
            {
                var returnedObject = Json.Decode(sr.ReadToEnd());
                return returnedObject;
            }
        }

        protected dynamic Get(ActiveCampaignMethods method)
        {
            var request = (HttpWebRequest)WebRequest.Create(_apiUrl + $"&api_action={method}");
            request.ServicePoint.Expect100Continue = false;
            request.Method = "GET";

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
            {
                var returnedObject = Json.Decode(sr.ReadToEnd());
                return returnedObject;
            }
        }


        private string CreateBaseUrl(string apiUrl, string apiKey)
        {
            string cleanedUrl = Regex.IsMatch(apiUrl, "/$") ? apiUrl.Substring(0, apiUrl.Length - 1) : apiUrl;

            if (Regex.IsMatch(apiUrl, "https://www.activecampaign.com/"))
                return $"{cleanedUrl}/api.php?api_output=json&api_key={apiKey};";
            else
                return $"{cleanedUrl}/admin/api.php?api_output=json&api_key={apiKey}";
        }

        /// <summary>
        ///  Tests the connection to the ActiveCampaign server.
        /// </summary>
        /// <remarks>This will throw an exception if the connection failed, rather than returning a value.</remarks>
        protected void TestConnection()
        {
            var result = Get(ActiveCampaignMethods.user_me);

            if (null == result.result_code)
                throw new Exception("The XML returned was not a valid ActiveCampaign response");

            if (1 != result.result_code)
                throw new Exception("Connection failed");

        }
    }
}
