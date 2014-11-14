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
    public class LibraryTypeController : BackEndAsyncFormController<LibraryType, decimal>
    {

        #region Public Properties

        public ILibraryTypeService LibraryTypeService
        {
            get { return (ILibraryTypeService)_genericService; }
            set { _genericService = value; }
        }

        public IResourceService ResourceService { get; set; }

        public IUserProfileService UserProfileService { get; set; }

        #endregion

        #region Validation Methods

        protected override bool ValidateFormData(LibraryType LibraryType, FormCollection formCollection)
        {
            if (Validator.IsBlankOrNull(LibraryType.Name))
            {
                ValidationDictionary.AddError("Name", LabelResource.LibraryTypeNameRequiredError);
            }
            if (Validator.IsBlankOrNull(LibraryType.DeletionUnit))
            {
                ValidationDictionary.AddError("DeletionUnit", LabelResource.LibraryTypeDeletionUnitRequiredError);
            }
            if (Validator.IsBlankOrNull(LibraryType.DeletionPeriod))
            {
                ValidationDictionary.AddError("DeletionPeriod", LabelResource.LibraryTypeDeletionPeriodRequiredError);
            }

            return ValidationDictionary.IsValid;
        }

        protected override bool ValidateDeleteData(LibraryType libraryType, StringBuilder errorMessage)
        {
            bool validDelete = true;

            if (libraryType.Permanent.Equals(EntityPermanent.Yes))
            {
                errorMessage.AppendLine(libraryType.Name + " is permanent");

                validDelete = false;
            }

            return validDelete;
        }

        #endregion

        #region Override Methods

        protected override void SetFormData()
        {
            string clientCompany = (string)Session["ClientCompany"];
            //IList<UserProfile> userList = UserProfileService.GetAdministratorUsers(clientCompany);
            IList<ResourceObject> libraryTypeList = ResourceService.GetAll<LibraryTypePeriod>();
            //IList<Library> libraryTypeList = LibraryService.GetAllByClientCompany(clientCompany);

            ViewData["DeletionPeriodList"] = new SelectList(libraryTypeList, "Id", "Value");
            //ViewData["AssignedToList"] = new SelectList(userList, "Id", "Name");

        }

        #endregion

    }
}
