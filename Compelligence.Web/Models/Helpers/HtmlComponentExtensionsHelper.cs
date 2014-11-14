using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Text;
using System.Collections;

namespace Compelligence.Web.Models.Helpers
{
    public static class HtmlComponentExtensionsHelper
    {
        public static string CascadingParentDropDownList(this HtmlHelper helper, string name, SelectList selectList, string optionLabel, string urlAction, string targetComponent)
        {
            return CascadingParentDropDownList(helper, name, selectList, optionLabel, false, urlAction, new string[] { }, string.Empty, targetComponent, new string[] { },null);
        }

        public static string CascadingParentDropDownList(this HtmlHelper helper, string name, SelectList selectList, string optionLabel, string urlAction, string[] urlParameters, string targetComponent)
        {
            return CascadingParentDropDownList(helper, name, selectList, optionLabel, false, urlAction, urlParameters, string.Empty, targetComponent, new string[] { },null);
        }

        public static string CascadingParentDropDownList(this HtmlHelper helper, string name, SelectList selectList, string optionLabel, string urlAction, string formId, string targetComponent)
        {
            return CascadingParentDropDownList(helper, name, selectList, optionLabel, false, urlAction, new string[] { }, formId, targetComponent, new string[] { },null);
        }

        public static string CascadingParentDropDownList(this HtmlHelper helper, string name, SelectList selectList, string optionLabel, string urlAction, string[] urlParameters, string formId, string targetComponent)
        {
            return CascadingParentDropDownList(helper, name, selectList, optionLabel, false, urlAction, urlParameters, formId, targetComponent, new string[] { },null);
        }

        public static string CascadingParentDropDownList(this HtmlHelper helper, string name, SelectList selectList, string optionLabel, string urlAction, string targetComponent, string[] otherChildComponents)
        {
            return CascadingParentDropDownList(helper, name, selectList, optionLabel, false, urlAction, new string[] { }, string.Empty, targetComponent, otherChildComponents,null);
        }

        public static string CascadingParentDropDownList(this HtmlHelper helper, string name, SelectList selectList, string optionLabel, string urlAction, string[] urlParameters, string targetComponent, string[] otherChildComponents)
        {
            return CascadingParentDropDownList(helper, name, selectList, optionLabel, false, urlAction, urlParameters, string.Empty, targetComponent, otherChildComponents,null);
        }

        public static string CascadingParentDropDownList(this HtmlHelper helper, string name, SelectList selectList, string optionLabel, string urlAction, string formId, string targetComponent, string[] otherChildComponents)
        {
            return CascadingParentDropDownList(helper, name, selectList, optionLabel, false, urlAction, new string[] { }, formId, targetComponent, otherChildComponents,null);
        }
        /// <summary>
        /// with onblur
        /// </summary>
        /// <returns>string</returns>
        public static string CascadingParentDropDownList(this HtmlHelper helper, string name, SelectList selectList, string optionLabel, string urlAction, string formId, string targetComponent, string[] otherChildComponents,string onblur)
        {
            return CascadingParentDropDownList(helper, name, selectList, optionLabel, false, urlAction, new string[] { }, formId, targetComponent, otherChildComponents, onblur);
        }

        public static string CascadingParentDropDownList(this HtmlHelper helper, string name, SelectList selectList, string optionLabel, string urlAction, string[] urlParameters, string formId, string targetComponent, string[] otherChildComponents)
        {
            return CascadingParentDropDownList(helper, name, selectList, optionLabel, false, urlAction, urlParameters, formId, targetComponent, otherChildComponents,null);
        }

        public static string CascadingParentDropDownList(this HtmlHelper helper, string name, SelectList selectList, string optionLabel, bool isChildElement, string urlAction, string targetComponent)
        {
            return CascadingParentDropDownList(helper, name, selectList, optionLabel, isChildElement, urlAction, new string[] { }, string.Empty, targetComponent, new string[] { },null);
        }

        public static string CascadingParentDropDownList(this HtmlHelper helper, string name, SelectList selectList, string optionLabel, bool isChildElement, string urlAction, string[] urlParameters, string targetComponent)
        {
            return CascadingParentDropDownList(helper, name, selectList, optionLabel, isChildElement, urlAction, urlParameters, string.Empty, targetComponent, new string[] { },null);
        }

        public static string CascadingParentDropDownList(this HtmlHelper helper, string name, SelectList selectList, string optionLabel, bool isChildElement, string urlAction, string formId, string targetComponent)
        {
            return CascadingParentDropDownList(helper, name, selectList, optionLabel, isChildElement, urlAction, new string[] { }, formId, targetComponent, new string[] { },null);
        }
        /// <summary>
        /// with onblur
        /// </summary>
        public static string CascadingParentDropDownList(this HtmlHelper helper, string name, SelectList selectList, string optionLabel, bool isChildElement, string urlAction, string formId, string targetComponent,string onblur)
        {
            return CascadingParentDropDownList(helper, name, selectList, optionLabel, isChildElement, urlAction, new string[] { }, formId, targetComponent, new string[] { }, onblur);
        }

        public static string CascadingParentDropDownList(this HtmlHelper helper, string name, SelectList selectList, string optionLabel, bool isChildElement, string urlAction, string[] urlParameters, string formId, string targetComponent)
        {
            return CascadingParentDropDownList(helper, name, selectList, optionLabel, isChildElement, urlAction, urlParameters, formId, targetComponent, new string[] { },null);
        }

        public static string CascadingParentDropDownList(this HtmlHelper helper, string name, SelectList selectList, string optionLabel, bool isChildElement, string urlAction, string targetComponent, string[] otherChildComponents)
        {
            return CascadingParentDropDownList(helper, name, selectList, optionLabel, isChildElement, urlAction, new string[] { }, string.Empty, targetComponent, otherChildComponents,null);
        }

        public static string CascadingParentDropDownList(this HtmlHelper helper, string name, SelectList selectList, string optionLabel, bool isChildElement, string urlAction, string[] urlParameters, string targetComponent, string[] otherChildComponents)
        {
            return CascadingParentDropDownList(helper, name, selectList, optionLabel, isChildElement, urlAction, urlParameters, string.Empty, targetComponent, otherChildComponents,null);
        }

        public static string CascadingParentDropDownList(this HtmlHelper helper, string name, SelectList selectList, string optionLabel, bool isChildElement, string urlAction, string formId, string targetComponent, string[] otherChildComponents)
        {
            return CascadingParentDropDownList(helper, name, selectList, optionLabel, isChildElement, urlAction, new string[] { }, string.Empty, targetComponent, otherChildComponents,null);
        }
        /// <summary>
        ///  Cascading 
        ///  otherChildComponents,it's for clean value
        /// </summary>
        public static string CascadingParentDropDownList(this HtmlHelper helper, string name, SelectList selectList, string optionLabel, bool isChildElement, string urlAction, string[] urlParameters, string formId, string targetComponent, string[] otherChildComponents,string onblur)
        {
            UrlHelper url = new UrlHelper(helper.ViewContext.RequestContext);
            StringBuilder cascadingFunction = new StringBuilder("javascript: ");
            string selectorFormat = "#{0}";
            string targetComponentSelector;
            string currentComponentSelector;
            string[] childComponentsSelector = new string[otherChildComponents.Length];
            string[] urlParametersSelector = new string[urlParameters.Length];
            string loaderComponent = targetComponent;
            string loadingElement = string.Empty;

            if (isChildElement)
            {
                loadingElement = GetLoadingElement(url, formId, name);
            }

            // Get selectors for components by "formId" and "fieldName" or only "fieldId"
            if (!string.IsNullOrEmpty(formId))
            {
                selectorFormat = "#{0} [name={1}]";
                loaderComponent = formId + targetComponent;

                currentComponentSelector = string.Format(selectorFormat, formId, name);
                targetComponentSelector = string.Format(selectorFormat, formId, targetComponent);

                for (int i = 0; i < otherChildComponents.Length; i++)
                {
                    childComponentsSelector[i] = string.Format(selectorFormat, formId, otherChildComponents[i]);
                }

                for (int i = 0; i < urlParameters.Length; i++)
                {
                    urlParametersSelector[i] = string.Format(selectorFormat, formId, urlParameters[i]);
                }

            }
            else
            {
                currentComponentSelector = string.Format(selectorFormat, name);
                targetComponentSelector = string.Format(selectorFormat, targetComponent);

                for (int i = 0; i < otherChildComponents.Length; i++)
                {
                    childComponentsSelector[i] = string.Format(selectorFormat, otherChildComponents[i]);
                }

                for (int i = 0; i < urlParameters.Length; i++)
                {
                    urlParametersSelector[i] = string.Format(selectorFormat, urlParameters[i]);
                }
            }

            if (selectList == null)
            {
                selectList = new SelectList(new ArrayList());
            }

            cascadingFunction.Append(" getCascadeObjects(");
            cascadingFunction.Append("'" + urlAction + "',");
            cascadingFunction.Append("'" + currentComponentSelector + "',");
            cascadingFunction.Append("'" + targetComponentSelector + "',");
            cascadingFunction.Append("'#" + loaderComponent + "Loader'");
            cascadingFunction.Append(GetElementsAsArray(childComponentsSelector));
            cascadingFunction.Append(GetElementsAsArray(urlParametersSelector));
            cascadingFunction.Append(");");
            
            if ( !string.IsNullOrEmpty(onblur) )
                 return SelectExtensions.DropDownList(helper, name, selectList, optionLabel, new { id = formId + name, onChange = cascadingFunction.ToString(), onBlur = onblur }) + loadingElement;

            return SelectExtensions.DropDownList(helper, name, selectList, optionLabel, new { id = formId + name, onChange = cascadingFunction.ToString() }) + loadingElement;
        }

        public static string CascadingChildDropDownList(this HtmlHelper helper, string name, SelectList selectList, string optionLabel)
        {
            return CascadingChildDropDownList(helper, name, selectList, optionLabel, string.Empty);
        }

        public static string CascadingChildDropDownList(this HtmlHelper helper, string name, SelectList selectList, string optionLabel, string formId)
        {
            UrlHelper url = new UrlHelper(helper.ViewContext.RequestContext);
            string loadingElement = GetLoadingElement(url, formId, name);

            if (selectList == null)
            {
                selectList = new SelectList(new ArrayList());
            }

            return SelectExtensions.DropDownList(helper, name, selectList, optionLabel, new { id = formId + name }) + loadingElement;
        }

        private static string GetLoadingElement(UrlHelper url, string formId, string name)
        {
            string loaderComponent = name;
            TagBuilder spanTag = new TagBuilder("span");
            TagBuilder imgTag = new TagBuilder("img");

            if (!string.IsNullOrEmpty(formId))
            {
                loaderComponent = formId + name;
            }

            spanTag.GenerateId(loaderComponent + "Loader");
            spanTag.MergeAttribute("style", "display: none; float: left;");

            imgTag.MergeAttribute("src", url.Content("~/Content/Images/Ajax/loadingkit.gif"));
            imgTag.MergeAttribute("alt", "Loading ...");

            spanTag.InnerHtml = imgTag.ToString();

            return spanTag.ToString();
        }

        private static string GetElementsAsArray(string[] elements)
        {
            StringBuilder arrayElements = new StringBuilder();

            arrayElements.Append(", [");

            for (int i = 0; i < elements.Length; i++)
            {
                if (i > 0)
                {
                    arrayElements.Append(", ");
                }

                arrayElements.Append("'" + elements[i] + "'");
            }

            arrayElements.Append("]");

            return arrayElements.ToString();
        }

        public static string CascadingParentDropDownListWP(this HtmlHelper helper, string name, SelectList selectList, string optionLabel, bool isChildElement, string urlAction, string[] urlParameters, string formId, string targetComponent, string[] otherChildComponents, string onblur, string onclick, string styleForSelect)
        {
            UrlHelper url = new UrlHelper(helper.ViewContext.RequestContext);
            StringBuilder cascadingFunction = new StringBuilder("javascript: ");
            string selectorFormat = "#{0}";
            string targetComponentSelector;
            string currentComponentSelector;
            string[] childComponentsSelector = new string[otherChildComponents.Length];
            string[] urlParametersSelector = new string[urlParameters.Length];
            string loaderComponent = targetComponent;
            string loadingElement = string.Empty;

            if (isChildElement)
            {
                loadingElement = GetLoadingElement(url, formId, name);
            }

            // Get selectors for components by "formId" and "fieldName" or only "fieldId"
            if (!string.IsNullOrEmpty(formId))
            {
                selectorFormat = "#{0} [name={1}]";
                loaderComponent = formId + targetComponent;

                currentComponentSelector = string.Format(selectorFormat, formId, name);
                targetComponentSelector = string.Format(selectorFormat, formId, targetComponent);

                for (int i = 0; i < otherChildComponents.Length; i++)
                {
                    childComponentsSelector[i] = string.Format(selectorFormat, formId, otherChildComponents[i]);
                }

                for (int i = 0; i < urlParameters.Length; i++)
                {
                    urlParametersSelector[i] = string.Format(selectorFormat, formId, urlParameters[i]);
                }

            }
            else
            {
                currentComponentSelector = string.Format(selectorFormat, name);
                targetComponentSelector = string.Format(selectorFormat, targetComponent);

                for (int i = 0; i < otherChildComponents.Length; i++)
                {
                    childComponentsSelector[i] = string.Format(selectorFormat, otherChildComponents[i]);
                }

                for (int i = 0; i < urlParameters.Length; i++)
                {
                    urlParametersSelector[i] = string.Format(selectorFormat, urlParameters[i]);
                }
            }

            if (selectList == null)
            {
                selectList = new SelectList(new ArrayList());
            }

            cascadingFunction.Append(" getCascadeObjects(");
            cascadingFunction.Append("'" + urlAction + "',");
            cascadingFunction.Append("'" + currentComponentSelector + "',");
            cascadingFunction.Append("'" + targetComponentSelector + "',");
            cascadingFunction.Append("'#" + loaderComponent + "Loader'");
            cascadingFunction.Append(GetElementsAsArray(childComponentsSelector));
            cascadingFunction.Append(GetElementsAsArray(urlParametersSelector));
            cascadingFunction.Append(");");
            if (!string.IsNullOrEmpty(onclick))
            {
                cascadingFunction.Append(onclick);
            }

            if (!string.IsNullOrEmpty(onblur))
            {
                if (!string.IsNullOrEmpty(styleForSelect))
                {
                    return SelectExtensions.DropDownList(helper, name, selectList, optionLabel, new { id = formId + name, onChange = cascadingFunction.ToString(), onBlur = onblur, style = styleForSelect }) + loadingElement;
                }
                else
                return SelectExtensions.DropDownList(helper, name, selectList, optionLabel, new { id = formId + name, onChange = cascadingFunction.ToString(), onBlur = onblur }) + loadingElement;
            }
            else if (!string.IsNullOrEmpty(styleForSelect))
            {
                return SelectExtensions.DropDownList(helper, name, selectList, optionLabel, new { id = formId + name, onChange = cascadingFunction.ToString(), style = styleForSelect }) + loadingElement;
            }
            else
            return SelectExtensions.DropDownList(helper, name, selectList, optionLabel, new { id = formId + name, onChange = cascadingFunction.ToString() }) + loadingElement;
        }

        public static string CascadingChildDropDownListWP(this HtmlHelper helper, string name, SelectList selectList, string optionLabel, string formId, string onclick, string styleForSelect)
        {
            UrlHelper url = new UrlHelper(helper.ViewContext.RequestContext);
            string loadingElement = GetLoadingElement(url, formId, name);

            if (selectList == null)
            {
                selectList = new SelectList(new ArrayList());
            }

            return SelectExtensions.DropDownList(helper, name, selectList, optionLabel, new { id = formId + name, onchange = onclick, style = styleForSelect }) + loadingElement;
        }
    }
}
