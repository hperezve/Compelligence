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

using System.Collections.Generic;
using Compelligence.DataTransfer.FrontEnd;
using System.Text;
using System.Web.Mvc;

namespace Compelligence.Web.Models.Helpers
{
    public static class RatingHelper
    {
        public static string RatingStarts(this HtmlHelper helper,decimal? Id,decimal? Rating,decimal? RatingCounter ,string UrlUpdate, bool RatingAllowed)
        {
            double RatingWidth = Math.Truncate((double)Rating * 84 / 100);
            string EventControl = RatingAllowed ? " onmousedown=\"mouseUpdate(event,this,'" + RatingCounter + "','" + UrlUpdate + "');\"  onmousemove='mouseMove(event,this)' onmouseout='mouseOut(event,this)'" : " onmousemove='toggle_visibility(this,true)' onmouseout='toggle_visibility(this,false)' ";
            string ClassControl = RatingAllowed ? "" : "tooltip";
            string SpanControl = RatingAllowed ? "" : "<span>Rating already established.</span>";

            string _Result = 
            //"<div id='RatingBox' style='width:200px'>" + 
            "<div id='Ra"+Id+"' class='ratingBox'>" +
            "  <div class='rating-average' style='float:left;'>" +
            "    <div class='rating-foreground' style='width:" + RatingWidth + "px;'> &nbsp; </div>" +
            "  </div>      "+
            "  <div class='rating-percent' > "+Rating+"%  </div>"+
            "</div> "+
            "<div id='Rc"+Id+"' class='ratingBox' style='background-color:transparent'>" +
            "  <div id='" + Id + "' class='" + ClassControl + " rating' " + EventControl + " >" + SpanControl +
            "    <div class='rating-foreground' style='width: 0px;'></div>" +
            "  </div>      "+
            "  <div class='rating-percent' > 0% </div>" +
            "</div> "/*+
            "</div>"*/;
            return _Result;
        }

        public static string FirstRatingStarts(this HtmlHelper helper, decimal? Id, decimal? Rating, decimal? RatingCounter, string UrlUpdate, bool RatingAllowed)
        {
            double RatingWidth = Math.Truncate((double)Rating * 84 / 100);
            string EventControl = RatingAllowed ? " onmousedown=\"mouseUpdate(event,this,'" + RatingCounter + "','" + UrlUpdate + "');\"  onmousemove='mouseMove(event,this)' onmouseout='mouseOut(event,this)'" : string.Empty;
            string _Result =
            "<div id='Ra" + Id + "' class='ratingBox' style='display:none;'>" +
            "  <div class='rating-average' style='float:left;'>" +
            "    <div class='rating-foreground' style='width:" + RatingWidth + "px;'> &nbsp; </div>" +
            "  </div>      " +
            "  <div class='rating-percent' > " + Rating + "%  </div>" +
            "</div> " +
            "<div id='Rc" + Id + "' class='ratingBox' style='background-color:transparent'>" +
            "  <div id='" + Id + "' class='rating' " + EventControl + " >" +
            "    <div class='rating-foreground' style='width: 0px;'></div>" +
            "  </div>      " +
            "  <div class='rating-percent' > 0% </div>" +
            "</div> ";
            return _Result;
        }
    }
}
