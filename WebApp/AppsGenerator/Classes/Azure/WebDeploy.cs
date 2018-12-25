using Microsoft.Build.Evaluation;
using Microsoft.Build.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppsGenerator.Classes.Azure
{
    public class WebDeploy
    {
        public string BuildApp(string ApplicationFile, string PublishXmlProfile)
        {
            string projectFileName = ApplicationFile;
            ProjectCollection pc = new ProjectCollection();

            Dictionary<string, string> GlobalProperty = new Dictionary<string, string>();

            GlobalProperty.Add("DeployOnBuild", "true");
            GlobalProperty.Add("VisualStudioVersion", "12.0");
            GlobalProperty.Add("PublishProfile", PublishXmlProfile);

            BuildRequestData BuildRequest = new BuildRequestData(projectFileName, GlobalProperty, null, new string[] { "Build" }, null);

            BuildResult buildResult = BuildManager.DefaultBuildManager.Build(new BuildParameters(pc), BuildRequest);
            if (buildResult.Exception != null)
                return buildResult.Exception.Message;
            return buildResult.OverallResult.ToString();
        }
    }
}