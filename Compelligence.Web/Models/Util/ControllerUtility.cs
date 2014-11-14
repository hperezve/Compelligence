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
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Compelligence.Util.Type;

namespace Compelligence.Web.Models.Util
{
    public static class ControllerUtility
    {
        public static JsonResult GetSelectOptionsFromGenericList<T>(IList<T> objectList, string value, string text)
        {
            return GetSelectOptionsFromGenericList<T>(objectList, value, text, true);
        }

        public static JsonResult GetSelectOptionsFromGenericList<T>(IList<T> objectList, string value, string text, bool firstblank)
        {
            Type type = typeof(T);
            JsonResult jsonResult = new JsonResult();
            var objectResult = new object[objectList.Count + 1];
            int index = 0;
            if (firstblank)
            {
                objectResult[index++] = new
                {
                    Value = string.Empty,
                    Text = string.Empty
                };
            }

            foreach (T item in objectList)
            {
                PropertyInfo propertyValue = type.GetProperty(value, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
                PropertyInfo propertyText = type.GetProperty(text, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);

                objectResult[index++] = new
                {
                    Value = propertyValue.GetGetMethod().Invoke(item, null).ToString(),
                    Text = propertyText.GetValue(item, null).ToString()
                };
            }

            jsonResult.Data = objectResult;

            return jsonResult;
        }



        public static JsonResult GetSelectOptionsFromGenericListOnly<T>(IList<T> objectList)
        {
            Type type = typeof(T);
            JsonResult jsonResult = new JsonResult();

            var objectResult = new object[objectList.Count];
            int index = 0;

            foreach (T item in objectList)
            {
                 objectResult[index++] = new
                {
                    Value = objectList[index-1],
                    Text = objectList[index-1]
                };
                
            }

            jsonResult.Data = objectResult;

            return jsonResult;
        }

        public static JsonResult GetSelectOptionsFromGenericListOnlyObject<T>(IList<T> objectList)
        {
            Type type = typeof(T);
            JsonResult jsonResult = new JsonResult();

            var objectResult = new object[objectList.Count];
            int index = 0;

            foreach (T item in objectList)
            {
                objectResult[index++] = new
                {
                    Value = ((object[])(object)item)[0].ToString(),
                    Text = ((object[])(object)item)[1]
                };

            }

            jsonResult.Data = objectResult;

            return jsonResult;
        }




        /// <summary>
        /// Get JSonp from Generic List
        /// </summary>
        public static JsonpResult GetJSonpList<T>(IList<T> objectList, string value, string text)
        {
            return GetJSonpList<T>(objectList, value, text, true);
        }

        /// <summary>
        /// Get JSonp from Generic List
        /// </summary>
        public static JsonpResult GetJSonpList<T>(IList<T> objectList, string value, string text, bool firstblank)
        {
            Type type = typeof(T);
            JsonpResult jsonResult = new JsonpResult();
            var objectResult = new object[objectList.Count + 1];
            int index = 0;
            if (firstblank)
            {
                objectResult[index++] = new
                {
                    Value = string.Empty,
                    Text = string.Empty
                };
            }

            foreach (T item in objectList)
            {
                PropertyInfo propertyValue = type.GetProperty(value, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
                PropertyInfo propertyText = type.GetProperty(text, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);

                objectResult[index++] = new
                {
                    Value = propertyValue.GetGetMethod().Invoke(item, null).ToString(),
                    Text = propertyText.GetValue(item, null).ToString()
                };
            }

            jsonResult.Data = objectResult;

            return jsonResult;
        }

        public static JsonResult GetSelectOptionsEnabledFromGenericList<T>(IList<T> objectList, string value, string text, bool firstblank, string disabled)
        {
            Type type = typeof(T);
            JsonResult jsonResult = new JsonResult();
            var objectResult = new object[objectList.Count + 1];
            int index = 0;
            if (firstblank)
            {
                objectResult[index++] = new
                {
                    Value = string.Empty,
                    Text = string.Empty,
                    Disabled = string.Empty
                };
            }

            foreach (T item in objectList)
            {
                PropertyInfo propertyValue = type.GetProperty(value, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
                PropertyInfo propertyText = type.GetProperty(text, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
                PropertyInfo propertyDisabled = type.GetProperty(disabled, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);

                objectResult[index++] = new
                {
                    Value = propertyValue.GetGetMethod().Invoke(item, null).ToString(),
                    Text = propertyText.GetValue(item, null).ToString(),
                    Disabled = propertyDisabled.GetValue(item, null).ToString()
                };
            }

            jsonResult.Data = objectResult;

            return jsonResult;
        }
    }
}
