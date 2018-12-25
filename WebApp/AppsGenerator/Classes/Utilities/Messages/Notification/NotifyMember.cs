using AppsGenerator.Classes.Utilities.Messages.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppsGenerator.Classes.Utilities.Messages.Notification
{
    public class NotifyMember
    {
        private static string APP_NAME="Apps Generator";

        private static EmailManagement emailManagement = new EmailManagement();

        public static string ChangePassword(string emailTo)
        {
            //Send email change password
            string subject = "Change password - " + APP_NAME;
            emailManagement.Subject = subject;
            emailManagement.Body = "<h1>The password was successfully changed</h1>";
            return emailManagement.SendEmail(emailTo,"Change password");
        }

        public static string LoginInformation(string emailTo,string username,string password)
        {
            string subject = "Login information - " + APP_NAME;
            string body = "<h3>Your account has been activated</h3><br />"
                             +"<b>Username : </b>" + username + "<br />"
                                     + "<b>Password : </b>" + password + "<br />";
            emailManagement.Subject = subject;
            emailManagement.Body = body;
            return emailManagement.SendEmail(emailTo, "Login information");
        }

        public static string AccountActivation(string emailTo, string activationUrl)
        {
            string subject = "Activate your account - " + APP_NAME;

            string body = "<h3>Click on this link to activate your account</h3><br />"
                + "<b><a href='" + activationUrl + "'>Activate your account</a></b>";

            emailManagement.Subject = subject;
            emailManagement.Body = body;
            return emailManagement.SendEmail(emailTo, "Activate your account");
        }

        public static string ResetPassword(string emailTo, string resetPasswordUrl)
        {
            string subject = "Recover password - " + APP_NAME;
            string body = "<h3>Click on this link to reset your password</h3><br />"
                + "<b>You can use this link during this day only</b><br />"
                + "<b><a href='" + resetPasswordUrl + "'>Retype Password</a></b>";

            emailManagement.Subject = subject;
            emailManagement.Body = body;

            return emailManagement.SendEmail(emailTo, "You have asked for the password recovery from Apps Generator");
        }
    }
}