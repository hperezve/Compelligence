using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Resources;
using Compelligence.Domain.Entity;
using Compelligence.BusinessLogic.Interface;
using Compelligence.Util.Validation;
using Compelligence.Domain.Entity.Resource;
using System.Text;
using Compelligence.Web.Models.Web;

namespace Compelligence.Web.Controllers
{
    public class CriteriaGroupController : BackEndAsyncFormController<CriteriaGroup, decimal>
    {

        #region Public Properties

        public ICriteriaGroupService CriteriaGroupService
        {
            get { return (ICriteriaGroupService)_genericService; }
            set { _genericService = value; }
        }

        public IResourceService ResourceService { get; set; }

        #endregion

        #region Validation Methods

        protected override bool ValidateFormData(CriteriaGroup criteriagroup, FormCollection formCollection)
        {

            if (Validator.IsBlankOrNull(criteriagroup.Name))
            {
                ValidationDictionary.AddError("Name", LabelResource.CriteriaGroupNameRequiredError);
            }

            return ValidationDictionary.IsValid;
        }

        #endregion

        #region Override Methods

        protected override void SetFormData()
        {
            string clientCompany = (string)Session["ClientCompany"];
            IList<ResourceObject> criteriaGroupTypeList = ResourceService.GetAll<CriteriaGroupType>();
            ViewData["TypeList"] = new SelectList(criteriaGroupTypeList, "Id", "Value");
        }

        protected override string SetDetailFilters(decimal parentId, DetailType detailType, StringBuilder detailFilter, StringBuilder browseDetailFilter)
        {
            string childController = string.Empty;
            ViewData["HeaderType"] = DomainObjectType.CriteriaGroup;

            switch (detailType)
            {
                case DetailType.Industry:
                    AddFilter(detailFilter, "Industry.CriteriaGroupId", parentId.ToString());
                    AddFilter(browseDetailFilter, "IndustryCriteriaGroupDetailView.CriteriaGroupId", parentId.ToString());
                    childController = "Industry:IndustryCriteriaGroupDetail";
                    break;
            }

            return childController;
        }

        #endregion
    }
}
