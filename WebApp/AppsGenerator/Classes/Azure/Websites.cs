using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Management.WebSites;
using Microsoft.WindowsAzure.Management.WebSites.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;

namespace AppsGenerator.Classes.Azure
{
    public class Websites
    {
        private string WebSpace;
        private PublishSettings _Settings;

        public Websites()
        {
            WebSpace = "";
            _Settings = new PublishSettings();

        }

        private WebSiteManagementClient GetWebSiteManagementClient()
        {
            var client = CloudContext.Clients.CreateWebSiteManagementClient(_Settings.GetCredentials(), _Settings.ServiceUrl);
            return client;
        }

        public async Task<bool> IsAvailableName(string WebSiteName)
        {
            try
            {
                using(var client = GetWebSiteManagementClient())
                {
                    //Check available hostName for website
                    var s = await client.WebSites.IsHostnameAvailableAsync(WebSiteName);
                    client.Dispose();
                    return s.IsAvailable;
                }
               
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public async Task<WebSiteGetPublishProfileResponse> CreateWebsite(string WebSiteName)
        {
            string Location = "SouthCentralUSwebspace";
            using(var client = GetWebSiteManagementClient())
            {
                string WebSpaceName = Location;

                var parameters = new WebSiteCreateParameters
                {
                    Name = WebSiteName,
                    ServerFarm = ""
                };
                try
                {
                    var result = await client.WebSites.CreateAsync(WebSpaceName, parameters);
                    //(result.WebSite.State == "Running") ? 
                    
                    return await GetPublishProfile(client,WebSiteName);
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        private async Task<WebSiteGetPublishProfileResponse> GetPublishProfile(WebSiteManagementClient client, string SiteName)
        {
            string Location = "SouthCentralUSwebspace";
            WebSiteGetPublishProfileResponse res = await client.WebSites.GetPublishProfileAsync(Location, SiteName);
            return res;
        }

        public async Task<string> ChangeAppState(string websiteName,string state)
        {
            using (var client = GetWebSiteManagementClient())
            {
                WebSiteUpdateParameters _params = new WebSiteUpdateParameters();
                //_params.State = "";
                //WebSiteUpdateResponse resp = await client.WebSites.UpdateAsync("SouthCentralUSwebspace", websiteName, _params);
                return "";
            }
        }

        public async Task<string> RestartWebSite(string websiteName)
        {
            using (var client = GetWebSiteManagementClient())
            {
                WebSiteUpdateParameters _params = new WebSiteUpdateParameters();
                //_params.State = "";
                OperationResponse resp = await client.WebSites.RestartAsync("SouthCentralUSwebspace", websiteName);
                //WebSiteUpdateResponse resp = await client.WebSites.UpdateAsync("SouthCentralUSwebspace", websiteName, _params);
                return resp.RequestId;
            }
        }

        public string DeleteWebsite(string WebSiteName)
        {
            try
            {
                using (var client = GetWebSiteManagementClient())
                {
                    //var 
                    var deleteParams = new WebSiteDeleteParameters();
                    var resp = client.WebSites.Delete(WebSpace, WebSiteName, deleteParams);
                    return resp.RequestId;
                }
                
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}