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

using Compelligence.Domain.Entity;
using System.Collections.Generic;
using Compelligence.DataTransfer.FrontEnd;
using System.Text;
using System.Web.Mvc;


namespace Compelligence.Web.Models.Helpers
{
    public static class MultiUploadHelper
    {
        public static string MultiUploadControl(this HtmlHelper helper)
        { 
           string _Result=string.Empty;
           TagBuilder divContainer = new TagBuilder("div");
           divContainer.GenerateId("multiuploadbox");           
           _Result="</br><div id='uploadfilebox'>";
           _Result+="</div>";
           _Result += "<input type='button' class='shortButton' id='uploadfilebox_attachafile' value='Attach a file (max. 2 MB)' onclick='addUploads()' />";
           _Result += "<input type='button' class='shortButton' id='uploadfilebox_attachanotherfile' style='display: none' value='Attach another file (max. 2 MB)' onclick='addUploads()' />";



          divContainer.InnerHtml = _Result;

          return divContainer.ToString();
          

        }
        public static string MultiUploadControlSurvey(this HtmlHelper helper)
        {
            string _Result = string.Empty;
            TagBuilder divContainer = new TagBuilder("div");
            divContainer.GenerateId("multiuploadbox");
            _Result = "<div id='uploadfilebox'>";
            _Result += "<span style='display: none;'>";
            _Result += "<input id='uploadfile' name='uploadfile' class='inputFile' type='file' size=43  />";

            _Result += " <a href='javascript:void(0)' style='text-decoration:none' onclick='removeUploadField(this);' class='estilo2'><input class='button, buttonRemove' type='button' value='Remove' /></a><br />";

            _Result += "</span>";
            _Result += "<noscript>";
            _Result += " <span>";
            _Result += " <input id='uploadfile' name='uploadfile' class='inputFile' type='file' size=43 />";

            _Result += " <a href='javascript:void(0)' style='text-decoration:none' onclick='removeUploadField(this);' class='estilo2'><input class='button, buttonRemove' type='button' value='Remove' /></a><br />";

            _Result += " </span>";
            _Result += "</noscript>";
            _Result += "</div>";
            _Result += "<input id='uploadfilebox_attachafile' type='button' class='button, buttonSurvey' value='Attach a file (max. 2 MB)' onclick='addUploadFields()' />";
            _Result += "<input id='uploadfilebox_attachanotherfile' class='button, buttonSurvey' style='display: none' value='Attach another file (max. 2 MB)' type='button' onclick='addUploadFields()' />";

            divContainer.InnerHtml = _Result;

            return divContainer.ToString();


        }

        public static string MultiUploadControlPersonalized(this HtmlHelper helper)
        {
            string _Result = string.Empty;
            TagBuilder divContainer = new TagBuilder("div");
            divContainer.GenerateId("multiuploadbox");
            //_Result = "<div id='uploadfilebox'>";
            //_Result += "<span style='display: none;'>";
            //_Result += "<input id='uploadfile' name='uploadfile' class='inputFile' type='file' size=43  />";
            //_Result += " <a href='javascript:void(0)' onclick='removeUploadField(this);' class='estilo2'><input class='button, buttonRemove' type='button' value='Remove' /></a><br />";
            //_Result += "</span>";
            //_Result += "<noscript>";
            //_Result += " <span>";
            //_Result += " <input id='uploadfile' name='uploadfile' class='inputFile' type='file' size=43 />";

            //_Result += " <input class='button, buttonRemove' type='button' value='Remove'  onclick='removeUploadField(this);' /><br />";

            //_Result += " </span>";
            //_Result += "</noscript>";
            //_Result += "</div>";
            //_Result += "<input id='uploadfilebox_attachafile' type='button' class='button' value='Attach a file (max. 2 MB)' onclick='addUploadFields()' />";
            //_Result += "<input id='uploadfilebox_attachanotherfile' class='button' style='display: none' value='Attach another file (max. 2 MB)' type='button' onclick='addUploadFields()' />";

            _Result = "<div id='uploadfileuploadbox'>";
            _Result += "</div>";
            _Result += "<input id='btnaddnewfile' class='button' type='button' value='Attach a file (max. 2 MB)'  onclick='AddNewInputFile(this);' />";
            divContainer.InnerHtml = _Result;

            return divContainer.ToString();


        }
    }
}
