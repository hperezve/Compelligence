using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Compelligence.BusinessLogic.Interface;
using Compelligence.Domain.Entity;
using Resources;
using Compelligence.Util.Validation;
using Compelligence.Util.Type;
using Compelligence.Domain.Entity.Resource;
using System.Text;


namespace Compelligence.Web.Controllers
{
    public class CustomFieldController : BackEndAsyncFormController<CustomField, decimal>
    {

        #region Public Properties

        public ICustomFieldService CustomFieldService
        {
            get { return (ICustomFieldService)_genericService; }
            set { _genericService = value; }
        }

        public IResourceService ResourceService { get; set; }

        public IUserProfileService UserProfileService { get; set; }

        #endregion

        #region Validation Methods

        protected override bool ValidateFormData(CustomField CustomField, FormCollection formCollection)
        {
            string clientCompany = (string)Session["ClientCompany"];

            if (Validator.IsBlankOrNull(CustomField.Name))
            {
                ValidationDictionary.AddError("Name", LabelResource.CustomFieldNameRequiredError);
            }
            if (Validator.IsBlankOrNull(CustomField.Type))
            {
                ValidationDictionary.AddError("Type", LabelResource.CustomFieldTypeRequiredError);
            }
            if (Validator.IsBlankOrNull(CustomField.Size))
            {
                ValidationDictionary.AddError("Size", LabelResource.CustomFieldSizeRequiredError);
            }
            else
            {
                if (CustomField.Size > 255)
                {
                    ValidationDictionary.AddError("Size", LabelResource.CustomFieldSizeMaxSize);
                }
            }
            if (Validator.IsBlankOrNull(CustomField.EntityType))
            {
                ValidationDictionary.AddError("EntityType", LabelResource.CustomFieldEntityTypeRequiredError);
            }
            if (ValidationDictionary.IsValid && CountByTypeAndCompany(CustomField.EntityType, clientCompany) > 4)
            {
                string type = ResourceService.GetName<DomainObjectType>(CustomField.EntityType);
                ValidationDictionary.AddError("Limit Exceeded", LabelResource.LimitExceededByEntity + " " + type);
            }
            return ValidationDictionary.IsValid;
        }

        #endregion
        
        #region Override Methods

        protected override void SetFormData()
        {
            IList<ResourceObject> entityTypeList = ResourceService.GetAll<CustomFieldEntityType>();
            IList<ResourceObject> typeList = ResourceService.GetAll<CustomFieldType>();

            ViewData["EntityTypeList"] = new SelectList(entityTypeList, "Id", "Value");
            ViewData["TypeList"] = new SelectList(typeList, "Id", "Value");
        }
        protected override void SetFormEntityDataToForm(CustomField formEntity)
        {
            base.SetFormEntityDataToForm(formEntity);
        }
        protected override void SetFormDataToEntity(CustomField entity, FormCollection collection)
        {
            base.SetFormDataToEntity(entity, collection);
        }
        protected override void SetEntityDataToForm(CustomField entity)
        {
            base.SetEntityDataToForm(entity);
        }
        protected override void GetFormData(CustomField entityObject, FormCollection collection)
        {
            //if don't have PhisicalName assign free
            if (String.IsNullOrEmpty(collection["PhysicalName"]))
            {
                IList<CustomField> fields=CustomFieldService.GetByEntityTypeId(entityObject.EntityType, CurrentCompany);
                String physicalname = string.Empty;
                for (int i = 1; i < 6; i++)
                {
                    physicalname = "CustomField"+i;
                    bool found = false;
                    foreach (CustomField field in fields)
                    {
                        if (physicalname.Equals(field.PhysicalName))
                        {
                            found = true;
                            break;
                        }
                    }
                    if (!found) break;
                }
                entityObject.PhysicalName = physicalname;
            }
        }
        #endregion
        #region Private Methods
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DuplicateCustomField()
        {
            string[] ids = StringUtility.CheckNull(Request["Id"]).Split(':');
            string userId = (string)Session["UserId"];
            StringBuilder returnMessage = new StringBuilder();
            StringBuilder errorMessage = new StringBuilder();
            //string nameEntitiesChangeSuccessful = string.Empty;
            string nameEntitiesDuplicateUnsuccessful = string.Empty;
            foreach (string identifier in ids)
            {
                CustomField customField = CustomFieldService.GetById(decimal.Parse(identifier));
                if (CountByTypeAndCompany(customField.EntityType, CurrentCompany) > 4)
                {
                    string type = ResourceService.GetName<DomainObjectType>(customField.EntityType);
                    string result = LabelResource.LimitExceededByEntity + " " + type;
                    if (!string.IsNullOrEmpty(nameEntitiesDuplicateUnsuccessful))
                    {
                        nameEntitiesDuplicateUnsuccessful += ":";
                    }
                    nameEntitiesDuplicateUnsuccessful += customField.Name;
                }
                else
                {
                    SetDefaultDataFromRequest(customField);
                    SetDefaultDataForSave(customField);

                    CustomFieldService.Duplicate(customField);
                }
            }

            if (!string.IsNullOrEmpty(nameEntitiesDuplicateUnsuccessful))
            {
                string[] namesEntityUnSuccessful = nameEntitiesDuplicateUnsuccessful.Split(':');
                returnMessage.Append("<br/><div>");
                returnMessage.Append(LabelResource.LimitExceededByEntity+":");
                returnMessage.Append("<p><ul class=LiEnableDisc>");
                foreach (string neus in namesEntityUnSuccessful)
                {
                    returnMessage.Append("<li>" + neus + "</li>");
                }
                returnMessage.Append("</ul></p>");
                returnMessage.Append("</div>");
                return Content(returnMessage.ToString());
            }
            else
            {
                return null;
            }
        }
        #endregion
        #region Private Methods
        private long CountByTypeAndCompany(string entityType, string company)
        {
            long totalCustomFields = CustomFieldService.CountByEntityTypeAndCompany(entityType, company);
            return totalCustomFields;
        }
        #endregion
    }
}
