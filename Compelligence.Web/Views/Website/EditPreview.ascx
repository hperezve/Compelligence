<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="Compelligence.Domain.Entity" %>

<script type="text/javascript">
      $(document).ready(function() {
      $("#PreviewAccordion").accordion();
      });
</script>

<div class="indexTwo">
    <div id="PreviewAccordion" class="contentFormEdit">
        <% string[] titles = new string[] { "Root", "Industry", "Competitor", "Product" };
           for (int i = 0; i < 4; i++)
           {%>
        <h3>
            <a href="#">
                <%=titles[i] %></a></h3>
        <div>
            <fieldset>
                <legend>Edit Preview </legend>
                <div>
                    <%List<WebsiteDetail> Details = (List<WebsiteDetail>)ViewData["CMSDetails" + i];
                      string align = "left";
                      foreach (WebsiteDetail Detail in Details)
                      {
                          if (Detail.Displayable.Equals("Y"))
                          {
                              if (Detail.Ajust.Equals("S"))
                              {
                                  if (align.CompareTo("left") == 0)
                                  {%>
                    <div class="borderPreviewL">
                        <div class="textBoldPreview">
                            <%=Detail.ContentType.Name%></div>
                        <div class="borderDouble50">
                        </div>
                    </div>
                    <%
                        align = "right";
                                  }
                                  else
                                  {%>
                    <div class="borderPreviewR">
                        <div class="textBoldPreview">
                            <%=Detail.ContentType.Name%></div>
                        <div class="borderDouble50">
                        </div>
                    </div>
                    <%
                        align = "left";
                                  }
                              }
                              else
                              {%>
                    <div class="borderPreviewLR">
                        <div class="textBoldPreview">
                            <%=Detail.ContentType.Name%>
                        </div>
                        <div class="borderDouble100">
                            
                        </div>
                    </div>
                    <%
                        align = "left";
                              }
                          }
                      } %>
                </div>
            </fieldset>
        </div>
        <% } %>
    </div>
</div>