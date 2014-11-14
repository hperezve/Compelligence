using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Compelligence.Domain.Entity;
using Compelligence.BusinessLogic.Interface;
using Resources;
using Compelligence.BusinessLogic.Implementation;
using Compelligence.Util.Validation;
using Compelligence.Domain.Entity.Resource;

namespace Compelligence.Web.Controllers
{
    public class LocationController : BackEndAsyncFormController<Location, decimal>
    {
        #region Public Properties

        public ILocationService LocationService
        {
            get { return (ILocationService)_genericService; }
            set { _genericService = value; }
        }

        #endregion

        #region Validation Methods

        protected override bool ValidateFormData(Location location, FormCollection formCollection)
        {
            if (Validator.IsBlankOrNull(location.CountryCode))
            {
                ValidationDictionary.AddError("CountryCode", LabelResource.LocationCountryCodeRequiredError);
            }
            if (!(Validator.IsBlankOrNull(location.CountryCode)) && (location.CountryCode.Length > 15))
            {
                ValidationDictionary.AddError("CountryCode", LabelResource.LocationCountryCodeLengthError);
            }
            if (Validator.IsBlankOrNull(location.AddressLine1))
            {
                ValidationDictionary.AddError("AddressLine1", LabelResource.LocationAddressLine1RequiredError);
            }
            if (Validator.IsBlankOrNull(location.City))
            {
                ValidationDictionary.AddError("City", LabelResource.LocationCityRequiredError);
            }

            return ValidationDictionary.IsValid;

        }

        #endregion
    }
}
