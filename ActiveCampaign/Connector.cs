using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Xml;

namespace ActiveCampaign
{
    public class Connector
    {
        private readonly string _apiUrl;

        /// <summary>
        ///  Creates a new ActiveCampaign Connector using an API key.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        public Connector(string apiUrl, string apiKey)
        {
            if (string.IsNullOrEmpty(apiUrl))
                throw new ArgumentException("The reseller or customer API URL was not specified", "apiUrl");
            if (string.IsNullOrEmpty(apiKey))
                throw new ArgumentException("The API key was not specified", "apiKey");

            _apiUrl = CreateBaseUrl(apiUrl) + "&api_key=" + apiKey;
        }

        /// <summary>
        ///  Creates a new ActiveCampaign Connector using a username and password.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        public Connector(string apiUrl, string apiUser, string apiPassword)
        {
            if (string.IsNullOrEmpty(apiUrl))
                throw new ArgumentException("The reseller or customer API URL was not specified", "apiUrl");
            if (string.IsNullOrEmpty(apiUser))
                throw new ArgumentException("The API username was not specified", "apiUser");
            if (string.IsNullOrEmpty(apiPassword))
                throw new ArgumentException("The API password was not specified", "apiPassword");

            _apiUrl = CreateBaseUrl(apiUrl) + "&api_user=" + apiUser + "&api_pass=" + apiPassword;
        }

        private string CreateBaseUrl(string apiUrl)
        {
            string cleanedUrl = Regex.IsMatch(apiUrl, "/$") ? apiUrl.Substring(0, apiUrl.Length - 1) : apiUrl;

            if (Regex.IsMatch(apiUrl, "https://www.activecampaign.com/"))
                return cleanedUrl + "/api.php?";

            return cleanedUrl + "/admin/api.php?api_output=xml";
        }

        /// <summary>
        ///  Tests the connection to the ActiveCampaign server.
        /// </summary>
        /// <remarks>This will throw an exception if the connection failed, rather than returning a value.</remarks>
        public void TestConnection()
        {
            var request = (HttpWebRequest)WebRequest.Create(_apiUrl + "&api_action=user_me");
            request.ServicePoint.Expect100Continue = false;
            request.Method = "GET";

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                XmlDocument responseXml = new XmlDocument();
                responseXml.Load(response.GetResponseStream());

                if (null == responseXml.SelectSingleNode("//result_code"))
                    throw new Exception("The XML returned was not a valid ActiveCampaign response");
            }
        }
    }
}
