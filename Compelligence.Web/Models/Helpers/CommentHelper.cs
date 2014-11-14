using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Compelligence.Domain.Entity;
using Compelligence.Domain.Entity.Resource;
using System.Web.Mvc;

namespace Compelligence.Web.Models.Helpers
{
    public static class CommentHelper
    {
        private static string MakeCommentLibrary(String UrlAction,String Caption)
        {
            StringBuilder Result=new StringBuilder();
            Result.AppendLine("<a class='commentfile' href='javascript: void(0);' onclick='return downloadFile(\""+UrlAction+"\");' >"+Caption+"</a>");
            return Result.ToString();
        }

        private static string MakeCommentItemChild(UrlHelper url, ForumResponse fr, int order, decimal ForumId, string UserSecurityAccessValue,string Controller)
        { 
            StringBuilder Result=new StringBuilder();

            String CommentLibraries = string.Empty;
            if (fr.Libraries.Count > 0)
            {
                foreach (Library library in fr.Libraries)
                {
                    File file = library.File;
                    //<%= Url.Action("Download", "DealSupport") + "/" + library.Id %>
                    CommentLibraries += MakeCommentLibrary(url.Action("Download", Controller, new { Id = library.Id }), url.Encode(library.FileName));
                    CommentLibraries += "<br>";
                }
            }
			Result.AppendLine("<ol class='children'>");
			Result.AppendLine("	<li class='comment' id='"+fr.Id+"'>");
            Result.AppendLine("  <div class='wrap " + (order % 2 == 0 ? string.Empty : "altt") + "'>");
			Result.AppendLine("  <div class='leftarea'>");
            Result.AppendLine("	  <img height=40 width=40 src='../../Content/Images/Styles/undefperson.jpg' class='avatar avatar-40 photo'></div>");
            Result.AppendLine("	   <div class='rightarea'>");
            Result.AppendLine("   	<div class='numero'><a href='#"+fr.Id+"'>"+order+"</a></div>");
            Result.AppendLine("   	<div class='autorcomentario'>");
			Result.AppendLine("		  <strong><a href='javascript:void(0)' rel='external nofollow' class='url'>"+fr.CreatedByName+"</a></strong>");
            Result.AppendLine("   	</div>");
            Result.AppendLine("   	<p>"+fr.Response+"</p>");
            Result.AppendLine("	    <p>" + CommentLibraries + "</p><br>");
            Result.AppendLine("   	<small class='commentmetadata'><a href='#"+fr.Id+"' title='Comment link'>"+fr.CreatedDate+"</a>");
            Result.AppendLine("   	</small>");

            if (!string.IsNullOrEmpty(UserSecurityAccessValue) && UserSecurityAccessValue.Equals(UserSecurityAccess.Edit))
                Result.AppendLine("	    <button class='shortButton'  onclick='location.href=\"" + url.Action("RemoveComments", Controller, new { id = ForumId, forumresponseid = fr.Id }) + "\"' style='float:right'>Remove</button> ");

			Result.AppendLine("	   </div>");
			Result.AppendLine("   </div>	");
			Result.AppendLine("  </li>");
			Result.AppendLine("</ol>");
            return Result.ToString();
        }

        private static string MakeCommentItem(string Action, string Controller, UrlHelper url, ForumResponse fr, int order, decimal ForumId, string UserSecurityAccessValue)
        { 
            StringBuilder Result=new StringBuilder();

            String CommentLibraries = string.Empty;
            if (fr.Libraries.Count > 0)
            {
                foreach (Library library in fr.Libraries)
                {
                    File file = library.File;
                    CommentLibraries += MakeCommentLibrary(url.Action(Action,Controller, new { Id = library.Id }), url.Encode(library.FileName));
                    CommentLibraries += "<br>";
                }
            }

            Result.AppendLine("<li id='"+fr.Id +"' class='comment'>");

            Result.AppendLine("<div class='wrap " + (order % 2 == 0 ? string.Empty : "altt" )+ "'>");
			Result.AppendLine("      <div class='leftarea'>");
            Result.AppendLine("      <img height=40 width=40 class='avatar avatar-40 photo' src='../../Content/Images/Styles/undefperson.jpg' >");
			Result.AppendLine("      </div>");
			Result.AppendLine("     <div class='rightarea'>");
			Result.AppendLine("	    <div class='numero'><a href='#"+fr.Id+"'>"+order+"</a></div>");
			Result.AppendLine("	    <div class='autorcomentario'>");
			Result.AppendLine("	      <strong><a class='url' rel='external nofollow' href='javascript:void(0)'>"+fr.CreatedByName+"</a></strong>");
			Result.AppendLine("	    </div>");
			Result.AppendLine("	    <p>"+fr.Response+"</p>");
			Result.AppendLine("	    <p>"+CommentLibraries+"</p><br>");
			Result.AppendLine("	    <small class='commentmetadata'><a title='this comment link' href='#"+fr.Id+"'>"+fr.CreatedDate+"</a>");
			Result.AppendLine("	    </small>");
            //Result.AppendLine("	    <a onclick='AddFormComment(\"" + fr.Id + "\");' href='javascript:void(0)' class='comment-reply-link' rel='nofollow'>Reply</a> ");
            Result.AppendLine("	    <button class='shortButton' onclick='AddFormComment(\"" + fr.Id + "\");' style='float:right'>Reply</button>");
            if (!string.IsNullOrEmpty(UserSecurityAccessValue) && UserSecurityAccessValue.Equals(UserSecurityAccess.Edit))
                Result.AppendLine("	    <button class='shortButton'  onclick='location.href=\"" + url.Action("RemoveComments", Controller, new { id = ForumId, forumresponseid = fr.Id }) + "\"' style='float:right'>Remove</button> ");
            Result.AppendLine("      </div>");
		    Result.AppendLine("</div>	");
            Result.AppendLine("</li>");
            //<a href="javascript: void(0)" onclick="javascript: setResponseId('<%= fr.Id %>');">  Reply </a>                                

            return Result.ToString();
        }

        public static string CommentControl(this HtmlHelper helper, string Action,string Controller,decimal ForumId, List<ForumResponse> Comments, string UserSecurityAccess)
        {
            UrlHelper url = new UrlHelper(helper.ViewContext.RequestContext);
            StringBuilder Result = new StringBuilder();
            if (Comments.Count == 0)
                return string.Empty;

            Result.AppendLine("<div id='comentarios'>");
            Result.AppendLine("    <div class='commentp'><a name='comments'>"+Comments.Count+"</a></div>");
            Result.AppendLine("    <h3>Comments</h3>");
	        Result.AppendLine("    <ol class='lista_comentarios'>");

            for (int i=0;i< Comments.Count ; i++)
            {
                 ForumResponse fr=Comments[i];

                 if (fr.ParentResponseId != null && fr.ParentResponseId > 0)
                 {
                     Result.AppendLine(MakeCommentItemChild(url, fr, i + 1,ForumId,UserSecurityAccess,Controller));
                 }
                 else
                   Result.AppendLine(MakeCommentItem(Action,Controller,url,fr,i+1,ForumId, UserSecurityAccess));

            }
	        Result.AppendLine("    </ol>");
            Result.AppendLine("</div>	");


            return Result.ToString();

        }

        public static string CommentThreds(this HtmlHelper helper, string Action, string Controller, decimal ForumId, List<ForumResponse> Comments, string UserSecurityAccessValue)
        {
            UrlHelper url = new UrlHelper(helper.ViewContext.RequestContext);
            StringBuilder Result = new StringBuilder();
            if (Comments.Count == 0)
                return string.Empty;

            Result.AppendLine("<div id='comentarios'>");
            Result.AppendLine("    <div class='commentp'><a name='comments'>" + Comments.Count + "</a></div>");
            Result.AppendLine("    <h3>Comments</h3>");
            Result.AppendLine("    <ol class='lista_comentarios'>");

            int i = 0;
            while(i < Comments.Count)
            {
                ForumResponse fr = Comments[i];

                Result.AppendLine("<li id='" + fr.Id + "'class='comment'>");
                //Result.AppendLine("<div>");
                //  Result.AppendLine(""+fr.Response);
                //Result.AppendLine("</div>");

                String CommentLibraries = string.Empty;
                if (fr.Libraries.Count > 0)
                {
                    foreach (Library library in fr.Libraries)
                    {
                        File file = library.File;
                        CommentLibraries += MakeCommentLibrary(url.Action(Action, Controller, new { Id = library.Id }), url.Encode(library.FileName));
                        CommentLibraries += "<br>";
                    }
                }
                Result.AppendLine("<div class='wrap " + (i % 2 == 0 ? string.Empty : "altt") + "'>");
                Result.AppendLine("      <div class='leftarea'>");
                Result.AppendLine("      <img height=40 width=40 class='avatar avatar-40 photo' src='../../Content/Images/Styles/undefperson.jpg' >");
                Result.AppendLine("      </div>");
                Result.AppendLine("     <div class='rightarea'>");
                Result.AppendLine("	    <div class='numero'>" + (i+1) + "</div>");
                Result.AppendLine("	    <div class='autorcomentario'>");
                Result.AppendLine("	      <strong class='commentname'>" + fr.CreatedByName + "</strong>");
                Result.AppendLine("	    </div>");
                Result.AppendLine("	    <p>" + fr.Response + "</p>");
                Result.AppendLine("	    <p>" + CommentLibraries+ "</p><br>");
                Result.AppendLine("	    <small class='commentmetadata'>" + fr.CreatedDate + "</small>");
                //Result.AppendLine("	    <a onclick='AddFormComment(\"" + fr.Id + "\");' href='javascript:void(0)' class='comment-reply-link' rel='nofollow'>Reply</a> ");
                Result.AppendLine("	    <button class='shortButton' onclick='AddFormComment(\"" + fr.Id + "\");' style='float:right'>Reply</button>");
                if (!string.IsNullOrEmpty(UserSecurityAccessValue) && UserSecurityAccessValue.Equals(UserSecurityAccess.Edit))
                    Result.AppendLine("	    <button class='shortButton'  onclick='location.href=\"" + url.Action("RemoveComments", Controller, new { id = ForumId, forumresponseid = fr.Id }) + "\"' style='float:right'>Remove</button> ");
                Result.AppendLine("      </div>");
                Result.AppendLine("</div>	");

                addItem(Comments, Result, ref i,ForumId,UserSecurityAccessValue,url,Controller,Action );
                Result.AppendLine("</li>");
                i++;
            }
            Result.AppendLine("    </ol>");
            Result.AppendLine("</div>	");


            return Result.ToString();

        }
        public static void addItem(IList<ForumResponse> Comments, StringBuilder Result, ref int i, decimal ForumId, string UserSecurityAccessValue, UrlHelper url,string Controller,string Action)
        {
            int j = i + 1;
           if (j< Comments.Count && Comments[i+1].ParentResponseId==Comments[i].Id) //have childs
           {
               Result.Append("<ol class='children'>");
               while (j < Comments.Count && Comments[j].ParentResponseId == Comments[i].Id)
               {
                   ForumResponse fr = Comments[j];
                   Result.Append("<li id='"+fr.Id+"' class='comment'>");
                   
                   //Result.AppendLine("<div>");
                   //Result.AppendLine("" + Comments[j].Response);
                   //Result.AppendLine("</div>");

                   String CommentLibraries = string.Empty;
                   if (fr.Libraries.Count > 0)
                   {
                       foreach (Library library in fr.Libraries)
                       {
                           File file = library.File;
                           CommentLibraries += MakeCommentLibrary(url.Action(Action, Controller, new { Id = library.Id }), url.Encode(library.FileName));
                           CommentLibraries += "<br>";
                       }
                   }

                   Result.AppendLine("  <div class='wrap " + (j % 2 == 0 ? string.Empty : "altt") + "'>");
                   Result.AppendLine("  <div class='leftarea'>");
                   Result.AppendLine("	  <img height=40 width=40 src='../../Content/Images/Styles/undefperson.jpg' class='avatar avatar-40 photo'></div>");
                   Result.AppendLine("	   <div class='rightarea'>");
                   Result.AppendLine("   	<div class='numero'>" + (j+1) + "</div>");
                   Result.AppendLine("   	<div class='autorcomentario'>");
                   Result.AppendLine("		  <strong class='commentname'>" + fr.CreatedByName + "</strong>");
                   Result.AppendLine("   	</div>");
                   Result.AppendLine("   	<p>" + fr.Response + "</p>");
                   Result.AppendLine("	    <p>" + CommentLibraries + "</p><br>");
                   Result.AppendLine("   	<small class='commentmetadata'>" + fr.CreatedDate + "</small>");
                   Result.AppendLine("	    <button class='shortButton' onclick='AddFormComment(\"" + fr.Id + "\");' style='float:right'>Reply</button>");
                   if (!string.IsNullOrEmpty(UserSecurityAccessValue) && UserSecurityAccessValue.Equals(UserSecurityAccess.Edit))
                       Result.AppendLine("	    <button class='shortButton'  onclick='location.href=\"" + url.Action("RemoveComments", Controller, new { id = ForumId, forumresponseid = fr.Id }) + "\"' style='float:right'>Remove</button> ");

                   Result.AppendLine("	   </div>");
                   Result.AppendLine("   </div>	");

                   addItem(Comments, Result, ref j,ForumId,UserSecurityAccessValue,url,Controller,Action);
                   j++;
                   Result.Append("</li>");
               }
               Result.Append("</ol>");
               i = j-1;
           }


        }

    }
}
