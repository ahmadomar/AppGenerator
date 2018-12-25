using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsGenerator.Classes.Utilities.Messages.Email
{
    public class EmailFormat
    {
        public static string PANEL_STYLE = "<style>"
                +".panel-primary {"
                +"    border-color: #2c3e50;"
                +" }"
                +" .panel {"
                +"     margin-bottom: 21px;"
                +"     background-color: #fff;"
                +"     border: 1px solid transparent;"
                +"     border-radius: 4px;"
                +"     -webkit-box-shadow: 0 1px 1px rgba(0,0,0,.05);"
                +"     box-shadow: 0 1px 1px rgba(0,0,0,.05);"
                +"  }"
                +"   .panel-heading {"
                +"     padding: 10px 15px;"
                +"     border-bottom: 1px solid transparent;"
                +"     border-top-left-radius: 3px;"
                +"     border-top-right-radius: 3px;"
                +"   }"
                +"    .panel-body {"
                +"      padding: 15px;"
                +"    }"
                +"</style>";
            
        public static string UserActivateMsg()
        {
            string html = "<html><head>" + PANEL_STYLE + "</head>"
                
                +"<body><div class=\"panel panel-primary\">"
                            + " <div class=\"panel-heading\">"
                            + "     <h4>Account activation</h4>"
                            + " </div>"
                            + " <div class=\"panel-body\">"
                            + "     <p class=\"col-md-10\" style=\"font-size: 20px\">"
                            + "         Click here to activate your account"
                            + "     </p>"
                            + "     <div class=\"pull-right\">"
                            + "     </div>"
                            + " </div>"
                            + "</div></body></html>";
            return html;
        }
    }
}
