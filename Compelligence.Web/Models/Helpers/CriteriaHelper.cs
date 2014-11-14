using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Web.Mvc;
using System.Text;
using Compelligence.DataTransfer.Comparinator;
using Compelligence.Domain.Entity.Resource;
using Compelligence.Domain.Entity;
using Compelligence.Util.Type;
using System.Reflection;
using Compelligence.Common.Utility.Comparinator;


namespace Compelligence.Web.Models.Helpers
{
    public static class CriteriaHelper
    {
        public static string CriteriaRow(this HtmlHelper helper, ComparinatorCriteria oRowThree, UserProfile user, IList<Product> Titles, String IndustryId, bool pubcomments, string c, string u)
        {
            return CriteriaRow(helper, oRowThree, user, Titles, IndustryId, pubcomments, "false", "false", "false", c, u); //require enable  pubcomments and next three paramertes
        }

        public static string CriteriaRow(this HtmlHelper helper, ComparinatorCriteria oRowThree, UserProfile user, IList<Product> Titles, String IndustryId,bool pubcomments, string indstandard, string benefit, string cost, string c, string u)
        {
            UrlHelper url = new UrlHelper(helper.ViewContext.RequestContext);
            string path = ConfigurationSettings.AppSettings["ContentFilePath"].Substring(1);
            string cComp_crb = string.Empty;
            StringBuilder result = new StringBuilder();

            string classeq = oRowThree.IsEqual ? "comp_eq" : "comp_neq";

            if (string.IsNullOrEmpty(c) || string.IsNullOrEmpty(u))
            {
                cComp_crb = "comp_crb";
            }
            else
            {
                cComp_crb = "comp_crb_sfcd";
            }
            if (String.IsNullOrEmpty(oRowThree.Criteria.Relevancy))
                oRowThree.Criteria.Relevancy = CriteriaRelevancy.High;

            result.Append("<tr id='C" + oRowThree.Criteria.Id.ToString() + "' class='" + classeq + "' relevant='" + oRowThree.Criteria.Relevancy + "'>");

            result.Append("<td class='comp_td'>");

            result.Append("<div>");
            result.Append("<span>" + oRowThree.Criteria.Name + "</span>");
            result.Append("</div>");

            if (user.SecurityGroupId != "ENDUSER")
            {
                //next line it's for colorize corner icon
                //result.Append("<div class='divImg' onMouseOver =\"CheckColorAll('loadrelevancy" + oRowThree.Criteria.Id + "','popRelevancy" + oRowThree.Criteria.Id + "')\">");
                string relevancycss = "";
                if (oRowThree.Criteria.Relevancy.Equals(CriteriaRelevancy.High))
                {
                    relevancycss = "comp_crbg";
                }
                else if (oRowThree.Criteria.Relevancy.Equals(CriteriaRelevancy.Low))
                {
                    relevancycss = "comp_crbr";
                }
                result.Append("<div class='comp_crb relevancy " + relevancycss + "'  rel='/'>");
                result.Append("</div>");
            }
            result.Append("</td>");
            string cssindstandard = string.Empty;
            if (indstandard.Equals("false"))
                cssindstandard = "style='display:none'";

            if (string.IsNullOrEmpty(oRowThree.Criteria.IndustryStandard))
                oRowThree.Criteria.IndustryStandard = "&nbsp;";
            result.Append("<td id='Ind" + oRowThree.Criteria.Id + "' " + cssindstandard + "><div><span>" + oRowThree.Criteria.IndustryStandard + "</span></div>");
            result.Append("<div class='comp_crb indstd'  rel='" + url.Action("CellIndustryStandard", "Comparinator") + "'>&nbsp;</div>");
            result.Append("</td>");
            string cssbenefit = string.Empty;
            string csscost = string.Empty;

            foreach (var value in oRowThree.Values)
            {
                string productid = "" + value.Key; //diff=0,1,2,...
                string cellValue = string.Empty;
                ProductCriteriaCell cell = (ProductCriteriaCell)value.Value;
                if (cell == null)
                  cell = new ProductCriteriaCell(string.Empty, string.Empty, 0, 0, 0, 0, 0, 0);
                bool isChanged = cell.LNotes > 0 || cell.LLinks > 0 || cell.CComments > 0;
                string cellvalue = TypeUtility.GetAnonymous(value.Value, "value");
                string cellfeature = TypeUtility.GetAnonymous(value.Value, "feature");
                bool isnotNumeric = true;
                //
                string autofeature = "NF";
                string autofeatureall = "NF";
                
                if ( oRowThree.Criteria.Type.Equals(CriteriaType.Numeric) )
                {
                    isnotNumeric = false;
                    string valueDecimal = TypeUtility.GetAnonymous(value.Value, "valuedecimal");
                    cellvalue = valueDecimal;
                    if (cellvalue.IndexOf(',') != -1) cellvalue = cellvalue.Replace(",", ".");
                    if (!string.IsNullOrEmpty(cellvalue) && DecimalUtility.IsDecimal(cellvalue))
                    {
                        //for selected
                        autofeature = "WOR";
                        if (oRowThree.HighValueSelectedProducts != oRowThree.LowValueSelectedProduct)//if range between productus selected is diferent to 0 , WOR = WithOut Range
                        {
                            autofeature = AutoFeature.GetNumericFeature(decimal.Parse(valueDecimal), oRowThree.Criteria.MostDesiredValue, (decimal)oRowThree.HighValueSelectedProducts, (decimal)oRowThree.LowValueSelectedProduct);
                        }

                        //for all products
                        autofeatureall = "WOR";
                        if (oRowThree.Criteria.HighValueAllProducts != oRowThree.Criteria.LowValueAllProducts)//if range between productus selected is diferent to 0 , WOR = WithOut Range
                        {
                            autofeatureall = AutoFeature.GetNumericFeature(decimal.Parse(valueDecimal), oRowThree.Criteria.MostDesiredValue, (decimal)oRowThree.Criteria.HighValueAllProducts, (decimal)oRowThree.Criteria.LowValueAllProducts);
                        }
                    }
                    else //the values range of values to products selects is 0, 
                    {
                        autofeature = "NF";
                        autofeatureall = "NF";
                    }
                    cell.Feature = autofeature; //trying update feature in current session
                    string cellValueStr = TypeUtility.GetAnonymous(value.Value, "value");
                    if (!string.IsNullOrEmpty(cellValueStr))
                    {

                        if (cellValueStr.ToUpper().Equals("N/A"))
                        {
                            cellValue = TypeUtility.GetAnonymous(value.Value, "value");
                        }
                        else
                        {
                            cellValue = string.Empty;
                            string valuedecimal = TypeUtility.GetAnonymous(value.Value, "valuedecimal");
                            
                            if (DecimalUtility.IsDecimal(valuedecimal))
                            {
                                //cellValue = Convert.ToDecimal(valuedecimal).ToString("N2", System.Globalization.CultureInfo.InvariantCulture);
                                cellValue = String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:#,0.########}", Convert.ToDecimal(valuedecimal));
                            }
                            //else
                            //{
                            //    cellValue = string.Empty;
                            //}
                        }
                    }    
                }
                else if (oRowThree.Criteria.Type.Equals(CriteriaType.Boolean))
                {
                    //for selected product
                    if (oRowThree.HighValueSelectedProducts == null || oRowThree.LowValueSelectedProduct == null)
                    {
                        autofeature = "WOR";
                    }
                    else
                        autofeature = AutoFeature.GetBooleanFeature(TypeUtility.GetAnonymous(value.Value, "value"), oRowThree.Criteria.MostDesiredValue);

                    //for all products
                    if (oRowThree.Criteria.HighValueAllProducts == null || oRowThree.Criteria.LowValueAllProducts == null)
                    {
                        autofeatureall = "WOR";
                    }
                    else
                        autofeatureall = AutoFeature.GetBooleanFeature(TypeUtility.GetAnonymous(value.Value, "value"), oRowThree.Criteria.MostDesiredValue);

                    cell.Feature = autofeature; //trying update feature in current session
                }
                else
                //at this time, equal to list
                {
                    autofeature = cell.Feature;
                    autofeatureall = cell.Feature;
                }

                if (string.IsNullOrEmpty(cellValue) && isnotNumeric)
                {
                    cellValue = TypeUtility.GetAnonymous(value.Value, "value");
                }
                result.Append("<td id='P" + productid + "' class='" + autofeature + "'   A='" + autofeatureall + "'  S='" + autofeature + "'>"); //NF=Not Feature
                result.Append("<div >");
                result.Append("<span>" + cellValue + "</span>");
                result.Append("</div>");

                //next lines it's for colorize corner icon
                result.Append("<div class='comp_crb cp " + (isChanged ? "comp_crbr" : "") + "'  rel='/'>&nbsp;</div>");

                result.Append("</td>");
            }

            //Benefit
            if (benefit.Equals("false"))
                cssbenefit = "style='display:none'";

            result.Append("<td id='B" + oRowThree.Criteria.Id + "'align='center' " + cssbenefit + ">");
            result.Append("<div>");
            result.Append("<span class='wrd_wrap'>" + oRowThree.Criteria.Benefit + "</span>");
            result.Append("</div>");
            result.Append("<div class='"+ cComp_crb + " benefit' rel='" + url.Action("CellBenefit", "Comparinator") + "' />");
            result.Append("</td>");
            //**************************
            if (cost.Equals("false"))
                csscost = "style='display:none'";

            //Cost
            result.Append("<td id='O" + oRowThree.Criteria.Id + "' " + csscost + ">");
            result.Append("<div>");
            result.Append("<span class='wrd_wrap'>" + oRowThree.Criteria.Cost + "</span>");
            result.Append("</div>");
            result.Append("<div class='"+ cComp_crb + " cost' rel='" + url.Action("CellCost", "Comparinator") + "' />");
            result.Append("</td>");
            //**************************
            result.Append("</tr>");
            return result.ToString();
        }



        private static string GetCheckedByRelevancy(string value, string key)
        {
            string result = string.Empty;
            if (value.Equals(key))
            {
                result = "checked='checked'";
            }
            return result;
        }
    }
}
