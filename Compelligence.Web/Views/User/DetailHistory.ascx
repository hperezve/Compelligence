<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<% IList<HistoryField> listHistoryField = (IList<HistoryField>)ViewData["ListHistoryField"];
   UserActionField userActionField = (UserActionField)ViewData["UserActionField"]; %>
<style type="text/css">
    td p
    {
        width:25em;
        word-wrap:break-word;
        margin-bottom : 0px;
    }
    #tableComment td p
    {
        width:16em;
        word-wrap:break-word;
        margin-bottom : 0px;
    }
</style>
<div>
    <table>
        <thead>
            <tr style="border:1px solid #AAAAAA;">
                <th colspan="2">User: <%= userActionField.UserName%></th>                
            </tr>
        </thead>
        <tbody>
            <tr>
                <td style="width:50%">
                    Date
                </td>
                <td>
                    <p><%= userActionField.Date%></p>
                </td>
            </tr>
            <tr>
                <td>
                    Action
                </td>
                <td>
                    <p><%= userActionField.Action%></p>
                </td>
            </tr>
            <tr>
                <td>
                    By Whom
                </td>
                <td>
                   <p><%= userActionField.ByWhom%></p>
                </td>
            </tr>
            <tr>
                <td>
                    Notes
                </td>
                <td>
                    <p><%= userActionField.Notes%></p>          
                </td>
            </tr>
        </tbody>
    </table>
    <br />
    <br />
    <br />
    <br />
    <table id="tableComment">
        <thead>
            <tr style="border:1px solid #AAAAAA;">
                <th>Field Changed</th>
                <th>Old Value</th>
                <th>New Value</th>
            </tr>
        </thead>
        <tbody>
            <%foreach (HistoryField historyField in listHistoryField)
              {%>
                 <tr>
                    <td>
                        <p><%= historyField.FieldName%></p>
                    </td>
                    <td>
                        <p><%= historyField.OldValue%></p>
                    </td>
                    <td>
                        <p><%= historyField.Value%></p>
                    </td>
                 </tr>   
            <%}%>
        </tbody>
    </table>
</div>