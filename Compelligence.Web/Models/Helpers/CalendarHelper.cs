using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using Compelligence.Domain.Entity;
using System.Web.Mvc;

namespace Compelligence.Web.Models.Helpers
{
    public static class CalendarHelper
    {
        public static string MakeCalendar(this HtmlHelper helper, string objectName,IList<Calendar> calendars)
        {
            StringBuilder result = new StringBuilder();

            result.AppendLine("<script type='text/javascript'>");
	        result.AppendLine("$(document).ready(function() {");
		
		    result.AppendLine("$('"+objectName+"').fullCalendar({");
          
        result.AppendLine("theme: true,");
        result.AppendLine("header: {");
        result.AppendLine("left: 'prev,next today',");
        result.AppendLine("center: 'title'");
        result.AppendLine("},");
            										
		result.AppendLine("	editable: false,");
		result.AppendLine("	events: [");
        int calcount = calendars.Count;
        int icount = 0;

        String subresult = " ";

        foreach(Calendar calendar in calendars)
        {
            if (calendar.Date!=null)
            {
                subresult+=("	{");
                subresult += ("		title: '" + calendar.Name.Replace("'", "`") + "',");
                subresult += ("		start: new Date(" + calendar.Date.Value.Year + "," + (calendar.Date.Value.Month -1) + ", " + calendar.Date.Value.Day + "),");
                subresult += ("		end: new Date(" + calendar.Date.Value.Year + "," + (calendar.Date.Value.Month -1) + ", " + calendar.Date.Value.Day + ")");
                subresult += ("	},");
            }
					//id: 999,
					//allDay: false
        }
        subresult = subresult.Substring(0, subresult.Length - 1);
        result.AppendLine(subresult);
		result.AppendLine("	]");
		result.AppendLine("});");
    	result.AppendLine("});");
        result.AppendLine("</script>");
        return result.ToString();
        }

        public static string MakeCalendar(this HtmlHelper helper, string objectName, IList<Event> calendars)
        {
            StringBuilder result = new StringBuilder();

            result.AppendLine("<script type='text/javascript'>");
            result.AppendLine("$(document).ready(function() {");
            
            result.AppendLine("$('" + objectName + "').fullCalendar({");
          
            result.AppendLine("theme: true,");
            result.AppendLine("header: {");
            result.AppendLine("left: 'prev,next today',");
            result.AppendLine("center: 'title'");
            result.AppendLine("},");           										
            result.AppendLine("	editable: false,");
            result.AppendLine("	events: [");
            int calcount = calendars.Count;
            int icount = 0;

            String subresult = "";

            foreach (Event calendar in calendars)
            {
                if (calendar.StartDate != null)
                {
                    subresult += ("	{");
                    subresult += ("		title: '" + calendar.EventName.Replace("'", "`") + "',");
                    subresult += ("		start: new Date(" + calendar.StartDate.Value.Year + "," + (calendar.StartDate.Value.Month - 1) + ", " + calendar.StartDate.Value.Day + "),");

                    if (calendar.EndDate != null)
                    {
                        subresult += ("		end: new Date(" + calendar.EndDate.Value.Year + "," + (calendar.EndDate.Value.Month - 1) + ", " + calendar.EndDate.Value.Day + ")");
                    }
                    else
                    {
                        subresult += ("		end: new Date(" + calendar.StartDate.Value.Year + "," + (calendar.StartDate.Value.Month - 1) + ", " + calendar.StartDate.Value.Day + ")");
                    }
                                                            
                    subresult += ("	},");
                }
             
            }
            subresult = subresult.Substring(0, subresult.Length - 1);
            result.AppendLine(subresult);
            result.AppendLine("	]");
            result.AppendLine("});");
            result.AppendLine("});");
            result.AppendLine("</script>");
            return result.ToString();
        }

        public static string MakeCalendar(this HtmlHelper helper, string objectName, IList<Newsletter> calendars)
        {
            StringBuilder result = new StringBuilder();

            result.AppendLine("<script type='text/javascript'>");
            result.AppendLine("$(document).ready(function() {");

            result.AppendLine("$('" + objectName + "').fullCalendar({");

            result.AppendLine("theme: true,");
            result.AppendLine("header: {");
            result.AppendLine("left: 'prev,next today',");
            result.AppendLine("center: 'title'");
            result.AppendLine("},");
            result.AppendLine("	editable: false,");
            result.AppendLine("	events: [");
            int calcount = calendars.Count;
            int icount = 0;

            String subresult = "";

            foreach (Newsletter calendar in calendars)
            {
                if (calendar.CreatedDate != null)
                {
                    subresult += ("	{");
                    subresult += ("		title: '" + calendar.Title.Replace("'", "`") + "',");
                    subresult += ("		start: new Date(" + calendar.CreatedDate.Year + "," + (calendar.CreatedDate.Month - 1) + ", " + calendar.CreatedDate.Day + "),");

                    if (calendar.CreatedDate != null)
                    {
                        subresult += ("		end: new Date(" + calendar.CreatedDate.Year + "," + (calendar.CreatedDate.Month - 1) + ", " + calendar.CreatedDate.Day + ")");
                    }
                    else
                    {
                        subresult += ("		end: new Date(" + calendar.CreatedDate.Year + "," + (calendar.CreatedDate.Month - 1) + ", " + calendar.CreatedDate.Day + ")");
                    }

                    subresult += ("	},");
                }

            }
            if (subresult.Length > 1)
            {
                subresult = subresult.Substring(0, subresult.Length - 1);
            }
            result.AppendLine(subresult);
            result.AppendLine("	]");
            result.AppendLine("});");
            result.AppendLine("});");
            result.AppendLine("</script>");
            return result.ToString();
        }
    }
}
