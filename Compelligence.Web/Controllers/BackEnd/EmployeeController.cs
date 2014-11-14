using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Compelligence.Domain.Entity;
using Compelligence.BusinessLogic.Interface;
using Resources;
using Compelligence.Util.Validation;
using Compelligence.Web.Models.Web;
using Compelligence.Domain.Entity.Resource;
using Compelligence.Util.Type;

namespace Compelligence.Web.Controllers
{
    public class EmployeeController : BackEndAsyncFormController<Employee, decimal>
    {
        #region Public Properties

        public IEmployeeService EmployeeService
        {
            get { return (IEmployeeService)_genericService; }
            set { _genericService = value; }
        }

        #endregion

        #region Action Methods

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult EditPassword(decimal id)
        {
            Employee employee = EmployeeService.GetById(id);

            SetDefaultRequestParametersToForm(ActionMethod.EditPassword, OperationStatus.Initiated);

            SetFormData();

            return View(employee);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditPassword(decimal id, Employee employeeForm, FormCollection collection)
        {
            try
            {
                OperationStatus operationStatus = OperationStatus.Initiated;
                Employee employeeResult = employeeForm;
                Employee employee = EmployeeService.GetById(id);

                if (ValidateEditFormPasswordData(employeeForm))
                {
                    SetFormDataToEntity(employee, collection);

                    SetDefaultDataForUpdate(employee);

                    EmployeeService.UpdatePassword(employee);

                    employeeResult = employee;

                    operationStatus = OperationStatus.Successful;

                    return RedirectToAction("Edit", new { id = employee.Id, operationStatus = operationStatus.ToString(), Scope = Request["Scope"], BrowseId = Request["BrowseId"], IsDetail = Request["IsDetail"], HeaderType = Request["HeaderType"], DetailFilter = Request["DetailFilter"] });
                }

                SetDefaultRequestParametersToForm(ActionMethod.EditPassword, operationStatus);

                SetFormData();

                return View(employeeResult);
            }
            catch
            {
                return View();
            }
        }

        #endregion

        #region Validation Methods

        protected override bool ValidateFormData(Employee employee, FormCollection formCollection)
        {
            string headerType = StringUtility.CheckNull(Request["HeaderType"]);
            string clientCompany = StringUtility.CheckNull((string)Session["ClientCompany"]);

            if (Validator.IsBlankOrNull(employee.FirstName))
            {
                ValidationDictionary.AddError("FirstName", LabelResource.EmployeeFirstNameRequiredError);
            }

            if (Validator.IsBlankOrNull(employee.LastName))
            {
                ValidationDictionary.AddError("LastName", LabelResource.EmployeeLastNameRequiredError);
            }

            //if (Validator.IsBlankOrNull(employee.Email))
            //{
            //    ValidationDictionary.AddError("Email", LabelResource.EmployeeEmailRequiredError);
            //}
            if (!Validator.IsBlankOrNull(employee.Email))
            {
                if (!Validator.IsEmail(employee.Email))
                {
                    ValidationDictionary.AddError("Email", LabelResource.EmployeeEmailFormatError);
                }
                else if (headerType.Equals(DomainObjectType.CompetitorPartner) && (!EmployeeService.IsValidEmailToEmployee(employee.Id, employee.Email, clientCompany)))
                {
                    ValidationDictionary.AddError("Email", LabelResource.EmployeeEmailExistsError);
                }
            }
            if (!Validator.NumberFaxAndPhone(employee.PhoneNumber))
            {
                ValidationDictionary.AddError("PhoneNumber", LabelResource.ValidateTextPhone);
            }
            if (!Validator.NumberFaxAndPhone(employee.CellNumber))
            {
                ValidationDictionary.AddError("CellNumber", LabelResource.ValidateTextPhone);
            }
            if (!Validator.NumberFaxAndPhone(employee.Fax))
            {
                ValidationDictionary.AddError("Fax", LabelResource.ValidateTextFax);
            }
            if (headerType.Equals(DomainObjectType.Partner))
            {
                if (Validator.IsBlankOrNull(employee.Password))
                {
                    ValidationDictionary.AddError("Password", LabelResource.EmployeePasswordRequiredError);
                }
                else if (!Validator.MatchRegexp(employee.Password, employee.RePassword))
                {
                    ValidationDictionary.AddError("RePassword", LabelResource.EmployeeRePasswordMatchError);
                }
            }

            return ValidationDictionary.IsValid;
        }

        protected override bool ValidateEditFormData(Employee employee, FormCollection formCollection)
        {
            string headerType = StringUtility.CheckNull(Request["HeaderType"]);
            string clientCompany = StringUtility.CheckNull((string)Session["ClientCompany"]);

            if (Validator.IsBlankOrNull(employee.FirstName))
            {
                ValidationDictionary.AddError("FirstName", LabelResource.EmployeeFirstNameRequiredError);
            }

            if (Validator.IsBlankOrNull(employee.LastName))
            {
                ValidationDictionary.AddError("LastName", LabelResource.EmployeeLastNameRequiredError);
            }

            if (Validator.IsBlankOrNull(employee.Email))
            {
                ValidationDictionary.AddError("Email", LabelResource.EmployeeEmailRequiredError);
            }

            else if (!Validator.IsEmail(employee.Email))
            {
                ValidationDictionary.AddError("Email", LabelResource.EmployeeEmailFormatError);
            }
            else if (headerType.Equals(DomainObjectType.CompetitorPartner) && (!EmployeeService.IsValidEmailToEmployee(employee.Id, employee.Email, clientCompany)))
            {
                ValidationDictionary.AddError("Email", LabelResource.EmployeeEmailExistsError);
            }

            return ValidationDictionary.IsValid;
        }

        private bool ValidateEditFormPasswordData(Employee employee)
        {
            string clientCompany = StringUtility.CheckNull((string)Session["ClientCompany"]);

            if (Validator.IsBlankOrNull(employee.CurrentPassword))
            {
                ValidationDictionary.AddError("CurrentPassword", LabelResource.EmployeeCurrentPasswordRequiredError);
            }
            else if (!EmployeeService.IsValidCurrentPasswordToEmployee(employee.Id, employee.CurrentPassword))
            {
                ValidationDictionary.AddError("CurrentPassword", LabelResource.EmployeeCurrentPasswordExistsError);
            }

            if (Validator.IsBlankOrNull(employee.Password))
            {
                ValidationDictionary.AddError("Password", LabelResource.EmployeeNewPasswordRequiredError);
            }
            else if (!Validator.Equals(employee.Password, employee.RePassword))
            {
                ValidationDictionary.AddError("RePassword", LabelResource.EmployeeRePasswordMatchError);
            }

            return ValidationDictionary.IsValid;
        }

        #endregion
    }
}
