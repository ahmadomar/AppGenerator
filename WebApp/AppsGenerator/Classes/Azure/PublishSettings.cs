using Microsoft.WindowsAzure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Configuration;

namespace AppsGenerator.Classes.Azure
{
    public class PublishSettings
    {
        public string Id
        {
            get
            {
                return WebConfigurationManager.AppSettings["AzureSubscriptionId"];
            }
        }
        public string Name { get; set; }
        public Uri ServiceUrl
        {
            get
            {
                return new Uri(WebConfigurationManager.AppSettings["AzureServiceManagementUrl"]);
            }
        }
        public X509Certificate2 Certificate
        {
            get
            {
                return GetCertificate();
            }
        }

        public SubscriptionCloudCredentials GetCredentials()
        {
            return new CertificateCloudCredentials(Id, Certificate);
        }

        public X509Certificate2 GetCertificate()
        {
            string file = HttpContext.Current.Server.MapPath(WebConfigurationManager.AppSettings["AzureCertPath"]);
            return new X509Certificate2(file, WebConfigurationManager.AppSettings["AzureCertPassword"]);
        }
    }
}