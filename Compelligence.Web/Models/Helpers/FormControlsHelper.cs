using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Xml.Linq;
using System.Web.Mvc;
using Compelligence.Web.Models.Web;
using System.Text;
using System.Reflection;
using Compelligence.Domain.Entity.Resource;

namespace Compelligence.Web.Models.Helpers
{
    public static class FormControlsHelper
    {
        public static string SecurityButton(this HtmlHelper helper, SecurityButtonType buttonType, SecurityButtonAction actionButton)
        {
            return SecurityButton(helper, buttonType, actionButton, null);
        }

        public static string SecurityButton(this HtmlHelper helper, SecurityButtonType buttonType, SecurityButtonAction actionButton, object htmlAttributes)
        {
            StringBuilder buttonBuilder = new StringBuilder();
            bool isEntityLocked = false;
            bool isVisible = false;
            string userAccess = UserSecurityAccess.Read;
            string value = string.Empty;

            if (helper.ViewData["UserSecurityAccess"] != null)
            {
                userAccess = helper.ViewData["UserSecurityAccess"].ToString();
            }

            if ((helper.ViewData["EntityLocked"] != null) && (!string.IsNullOrEmpty(helper.ViewData["EntityLocked"].ToString())))
            {
                isEntityLocked = Convert.ToBoolean(helper.ViewData["EntityLocked"]);
            }

            switch (actionButton)
            {
                case SecurityButtonAction.Save:
                    isVisible = (string.Compare(userAccess, UserSecurityAccess.Edit) == 0) && (!isEntityLocked);
                    value = "Save";
                    break;
                case SecurityButtonAction.Reset:
                    isVisible = (string.Compare(userAccess, UserSecurityAccess.Edit) == 0) && (!isEntityLocked);
                    value = "Reset";
                    break;
                case SecurityButtonAction.Cancel:
                    isVisible = (string.Compare(userAccess, UserSecurityAccess.Edit) == 0) && (!isEntityLocked);
                    value = "Cancel";
                    break;
                case SecurityButtonAction.NewDetail:
                    isVisible = (string.Compare(userAccess, UserSecurityAccess.Edit) == 0) && (!isEntityLocked);
                    value = "New";
                    break;
                case SecurityButtonAction.AddDetail:
                    isVisible = (string.Compare(userAccess, UserSecurityAccess.Edit) == 0) && (!isEntityLocked);
                    value = "Add";
                    break;
                case SecurityButtonAction.EditDetail:
                    isVisible = (string.Compare(userAccess, UserSecurityAccess.Edit) == 0) && (!isEntityLocked);
                    value = "Edit";
                    break;
                case SecurityButtonAction.DeleteDetail:
                    isVisible = (string.Compare(userAccess, UserSecurityAccess.Edit) == 0) && (!isEntityLocked);
                    value = "Delete";
                    break;
                case SecurityButtonAction.DuplicateDetail:
                    isVisible = (string.Compare(userAccess, UserSecurityAccess.Edit) == 0) && (!isEntityLocked);
                    value = "Duplicate";
                    break;
                case SecurityButtonAction.Sort:
                    isVisible = (string.Compare(userAccess, UserSecurityAccess.Edit) == 0) && (!isEntityLocked);
                    value = "Order";
                    break;
                case SecurityButtonAction.Execute:
                    isVisible = (string.Compare(userAccess, UserSecurityAccess.Edit) == 0) && (!isEntityLocked);
                    value = "Execute";
                    break;
                case SecurityButtonAction.GetData:
                    isVisible = (string.Compare(userAccess, UserSecurityAccess.Edit) == 0) && (!isEntityLocked);
                    value = "Get Data";
                    break;
                case SecurityButtonAction.ForceRead:
                    isVisible = (string.Compare(userAccess, UserSecurityAccess.Edit) == 0) && (!isEntityLocked);
                    value = "ForceRead";
                    break;
                case SecurityButtonAction.CancelDetail:
                    isVisible = ((string.Compare(userAccess, UserSecurityAccess.Edit) == 0) || (string.Compare(userAccess, UserSecurityAccess.Read) == 0)) && (!isEntityLocked);
                    value = "Cancel";
                    break;
            }

            if (isVisible)
            {
                buttonBuilder.Append("<input class='button' type=");

                switch (buttonType)
                {
                    case SecurityButtonType.Submit:
                        buttonBuilder.Append("\"submit\"");
                        break;
                    case SecurityButtonType.Button:
                        buttonBuilder.Append("\"button\"");
                        break;
                    case SecurityButtonType.Reset:
                        buttonBuilder.Append("\"reset\"");
                        break;
                    case SecurityButtonType.Image:
                        buttonBuilder.Append("\"image\"");
                        break;
                }

                buttonBuilder.Append(GetHtmlAttributes(htmlAttributes));

                if (buttonBuilder.ToString().IndexOf(" value") < 0)
                {
                    buttonBuilder.Append(" value=\"" + value + "\"");
                }

                buttonBuilder.Append(" />");
            }

            return buttonBuilder.ToString();
        }

        private static string GetHtmlAttributes(object htmlAttributes)
        {
            StringBuilder htmlStringAttributes = new StringBuilder();

            if (htmlAttributes != null)
            {
                Type type = htmlAttributes.GetType();

                htmlStringAttributes.Append(" ");

                foreach (PropertyInfo property in type.GetProperties())
                {
                    string name = property.Name.ToLower();
                    string value = (string)property.GetValue(htmlAttributes, null);

                    htmlStringAttributes.Append(name);
                    htmlStringAttributes.Append("=");
                    htmlStringAttributes.Append("\"");
                    htmlStringAttributes.Append(value);
                    htmlStringAttributes.Append("\"");
                }
            }

            return htmlStringAttributes.ToString();
        }
    }
}
