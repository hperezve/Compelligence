using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Compelligence.Security.Filters;
using Compelligence.Common.Utility.Parser;
using Compelligence.Domain.Entity;
using Resources;
using Compelligence.Util.Type;
using Compelligence.Common.Utility;

namespace Compelligence.Web.Controllers
{
    /*[AuthenticationFilter]*/ //it's dangerous
    // It was disabled because if logged from SFDC there redirect to login
    // then every child from GenericFrontEndController will set individually.
  
    public abstract class GenericFrontEndController : GenericController
    {
        protected string GetFormatDate()
        {
            return "MM/dd/yyyy";
        }

        /// <summary>
        /// Register ActionHistory Need 
        /// </summary>
        protected override void ActionHistory(decimal Id, string entityAction, string domainObjectType)
        {
            string actionFrom = "FrontEnd";
            ActionHistory action = new ActionHistory(Id, entityAction, domainObjectType, actionFrom);
            string RealObjectType = this.GetType().ToString();


            action.CreatedBy = CurrentUser;
            action.ClientCompany = CurrentCompany;
            action.Description = string.Format(MessagesResource.ActionHistorySmall, CurrentUser, action.EntityAction, action.EntityType);
            ActionHistoryService.Register(action);
        }

        /// <summary>
        /// Get Request decode parameter
        /// </summary>
        public string GetDecodeParam(string paramname)
        {
            string paramencode = StringUtility.CheckNull(Request[paramname]); //if not exist return string empty
            return string.IsNullOrEmpty(paramencode) ? string.Empty : Encryptor.Decode(paramencode); //if valid return decode
        }

        /// <summary>
        /// Get Form decode parameter
        /// </summary>
        public string GetDecodeParam(FormCollection form, string paramname)
        {
            string paramencode = StringUtility.CheckNull(form[paramname]); //if not exist return string empty
            return string.IsNullOrEmpty(paramencode) ? string.Empty : Encryptor.Decode(paramencode); //if valid return decode
        }

        public void SetDefaultDataForSave<T>(DomainObject<T> entity)
        {
            entity.CreatedBy = CurrentUser;
            entity.LastChangedBy = CurrentUser;
            entity.CreatedDate = DateTime.Today;
            entity.LastChangedDate = DateTime.Today;
            entity.ClientCompany = CurrentCompany;
        }
        public void SetDefaultDataForSave<T>(DomainObject<T> entity, string CurrentCompany, string CurrentUser)
        {
            entity.CreatedBy = CurrentUser;
            entity.LastChangedBy = CurrentUser;
            entity.CreatedDate = DateTime.Today;
            entity.LastChangedDate = DateTime.Today;
            entity.ClientCompany = CurrentCompany;
        }

        public void SetDefaultDataForUpdate<T>(DomainObject<T> entity)
        {
            entity.LastChangedBy = CurrentUser;
            entity.LastChangedDate = DateTime.Today;
        }

        public void SetDefaultDataForUpdate<T>(DomainObject<T> entity,string CurrentUser)
        {
            entity.LastChangedBy = CurrentUser;
            entity.LastChangedDate = DateTime.Today;
        }
        /// <summary>
        /// This Method update the Audit Properties - last changed
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        public void SetDefaultDataForUpdateEntity<T>(DomainObject<T> entity)
        {
            entity.LastChangedBy = CurrentUser;
            entity.LastChangedDate = DateTime.Now;
        }
        /// <summary>
        /// This Method update the Audit Properties - last changed with Parameter User when the User no exist in Session
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="user"></param>
        public void SetDefaultDataForUpdateEntity<T>(DomainObject<T> entity, string user)
        {
            entity.LastChangedBy = user;
            entity.LastChangedDate = DateTime.Now;
        }
        /// <summary>
        /// This Method update the Audit Properties - Created
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        public void SetDefaultDataForSaveEntity<T>(DomainObject<T> entity)
        {
            entity.CreatedBy = CurrentUser;
            entity.CreatedDate = DateTime.Now;
            entity.ClientCompany = CurrentCompany;
            SetDefaultDataForUpdateEntity(entity);//To Audit Properties - last changed
        }
        /// <summary>
        /// This Method update the Audit Properties - created with Parameter User and Company when no exist in Session
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="user"></param>
        /// <param name="company"></param>
        public void SetDefaultDataForSaveEntity<T>(DomainObject<T> entity, string user, string company)
        {
            entity.CreatedBy = user;
            entity.CreatedDate = DateTime.Now;
            entity.ClientCompany = company;
            SetDefaultDataForUpdateEntity(entity,  user);
        }
        /// <summary>
        /// This method the set the ImageURL in the case no exist
        /// </summary>
        /// <param name="product"></param>
        /// <param name="competitorImageUrl"></param>
        /// <param name="clientCompanyImageUrl"></param>
        public void SetImageUrlToProduct(Product product, string competitorImageUrl, string clientCompanyImageUrl)
        {
            if (string.IsNullOrEmpty(product.ImageUrl))
            {
                if (!string.IsNullOrEmpty(competitorImageUrl))
                {
                    product.ImageUrl = competitorImageUrl;
                }
                else
                {
                    if (product.IsClientCompetitor.Equals("Y")&&!string.IsNullOrEmpty(clientCompanyImageUrl))
                    {
                        product.ImageUrl = clientCompanyImageUrl;
                    }
                }
            }
        }

        /// <summary>
        /// This method will set in ViewData the values , by defualt session and action
        /// and use the SetDefaultDataByPage Method to set the other values
        /// this method should be use in all Index
        /// </summary>
        protected virtual void SetDefaultDataToLoadPage()
        {
            SetDataSectionToHelp(Compelligence.Domain.Entity.Resource.ActionFrom.FrontEnd);
            SetDefaultDataByPage();
        }
        /// <summary>
        /// THis method will use SetDataToHelp method to set values
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="titleHelp"></param>
        protected virtual void SetDefatultEntityToLoadPage(string entity, string titleHelp)
        {
            SetDataToHelp( Compelligence.Domain.Entity.Resource.ActionFrom.FrontEnd,entity, titleHelp);
        }
    }
}
