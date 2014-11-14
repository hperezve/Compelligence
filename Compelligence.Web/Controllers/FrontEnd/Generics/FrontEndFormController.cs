using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Compelligence.BusinessLogic.Interface;
using Compelligence.Web.Models.Web;
using Compelligence.Domain.Entity;
using Compelligence.Domain.Entity.Resource;
using Compelligence.Security.Filters;

namespace Compelligence.Web.Controllers
{
    public abstract class FrontEndFormController<T, IdT> : GenericFrontEndController
    {
        protected IGenericService<T, IdT> _genericService;

        public IGenericService<T, IdT> GenericService
        {
            get { return _genericService; }
        }
     
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)] //it's dangerous, enabledonly for Library and Newsletter
        public virtual ActionResult Index()
        {
            SetDefaultDataToLoadPage();
            return View();
        }
       
      
        [AcceptVerbs(HttpVerbs.Get), AuthenticationFilter]
        [ValidateInput(false)] //it's dangerous, enabledonly for Library and Newsletter
        public virtual ActionResult Create()
        {
            SetDefaultRequestParametersToForm(ActionMethod.Create, OperationStatus.Initiated);

            SetFormData();

            SetUserSecurityAccess();

            return View("Edit");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)] //it's dangerous, enabledonly for Library and Newsletter
        public virtual ActionResult Create(T entity, FormCollection collection)
        {
            OperationStatus operationStatus = OperationStatus.Initiated;

            if (ValidateFormData(entity, collection))
            {
                DomainObject<IdT> domainObject = (DomainObject<IdT>)((object)entity);

                GetFormData(entity, collection);

                SetDefaultEntityDataForSave(entity);

                SetDefaultDataForSave(entity);

                GenericService.Save(entity);

                operationStatus = OperationStatus.Successful;

                ExecuteActionsAfterToSave(entity);

                return GetActionResultForCreate(domainObject, operationStatus);
            }

            SetDefaultRequestParametersToForm(ActionMethod.Create, operationStatus);

            SetFormData();

            SetFormEntityDataToForm(entity);

            SetUserSecurityAccess();

            return View("Edit", entity);
        }
       
        [ValidateInput(false)] //it's dangerous, enabledonly for Library and Newsletter
        [AcceptVerbs(HttpVerbs.Get), AuthenticationFilter]
        public virtual ActionResult Edit(IdT id, string operationStatus)
        {
            T entityObject = GenericService.GetById(id);

            OperationStatus operationStatusParam = OperationStatus.Initiated;

            if (!string.IsNullOrEmpty(operationStatus))
            {
                operationStatusParam = (OperationStatus)Enum.Parse(typeof(OperationStatus), operationStatus);
            }

            SetDefaultRequestParametersToForm(ActionMethod.Edit, operationStatusParam);

            SetFormData();

            SetEntityDataToForm(entityObject);

            SetUserSecurityAccess(entityObject);

            SetEntityLocking(entityObject);

            return View(entityObject);
        }
       
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)] //it's dangerous, enabledonly for Library and Newsletter
        public virtual ActionResult Edit(IdT id, T formEntity, FormCollection collection)
        {
            T entityResult = formEntity;
            T entity = GenericService.GetById(id);

            OperationStatus operationStatus = OperationStatus.Initiated;

            if (ValidateEditFormData(formEntity, collection))
            {
                SetFormDataToEntity(entity, collection);

                GetFormData(entity, collection);

                SetDefaultDataForUpdate(entity);

                GenericService.Update(entity);

                entityResult = entity;

                ExecuteActionsAfterToUpdate(entity);

                operationStatus = OperationStatus.Successful;
            }

            SetDefaultRequestParametersToForm(ActionMethod.Edit, operationStatus);

            SetFormData();

            SetFormEntityDataToForm(entityResult);

            SetUserSecurityAccess(entity);

            SetEntityLocking(entity);

            return GetActionResultForEdit(entity, operationStatus);
        }

        protected void SetDefaultRequestParametersToForm(OperationStatus operationStatus)
        {
            SetDefaultRequestParametersToForm(ActionMethod.Unknown, operationStatus);
        }

        protected virtual void SetDefaultRequestParametersToForm(ActionMethod actionMethod, OperationStatus operationStatus)
        {
            ViewData["OperationStatus"] = operationStatus;
        }

        /// <summary>
        /// Validate data form binding in entity object
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected virtual bool ValidateFormData(T entity, FormCollection collection)
        {
            return true;
        }

        /// <summary>
        /// Validate data for Edit form binding in entity object
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected virtual bool ValidateEditFormData(T entity, FormCollection collection)
        {
            return ValidateFormData(entity, collection);
        }

        protected virtual ActionResult GetActionResultForCreate(DomainObject<IdT> domainObject, OperationStatus operationStatus)
        {
            return RedirectToAction("Edit", new { id = domainObject.Id, operationStatus = operationStatus.ToString(), Scope = Request["Scope"], BrowseId = Request["BrowseId"], IsDetail = Request["IsDetail"], Container = Request["Container"], HeaderType = Request["HeaderType"], DetailFilter = Request["DetailFilter"] });
        }

        protected virtual ActionResult GetActionResultForEdit(T entityObject, OperationStatus operationStatus)
        {
            return View("Edit", entityObject);
        }

        /// <summary>
        /// Get additional data from form, and set it in an entity object
        /// </summary>
        /// <param name="entityObject"></param>
        /// <param name="collection"></param>
        protected virtual void GetFormData(T entityObject, FormCollection collection) { }

        /// <summary>
        /// Set default entity data for save in a entity object
        /// </summary>
        /// <param name="entity"></param>
        protected virtual void SetDefaultEntityDataForSave(T entity) { }

        /// <summary>
        /// Set the default data for save action in a entity object
        /// </summary>
        /// <param name="entity"></param>
        protected virtual void SetDefaultDataForSave(object entity)
        {
            DomainObject<IdT> domainObject = (DomainObject<IdT>)entity;

            domainObject.CreatedDate = DateTime.Now;

            domainObject.ClientCompany = CurrentCompany;

            domainObject.CreatedBy = CurrentUser;

            SetDefaultDataForUpdate(entity);
        }

        /// <summary>
        /// Set the default data for update action in a entity object
        /// </summary>
        /// <param name="entity"></param>
        protected virtual void SetDefaultDataForUpdate(object entity)
        {
            DomainObject<IdT> domainObject = (DomainObject<IdT>)entity;

            domainObject.LastChangedBy = CurrentUser;

            domainObject.LastChangedDate = DateTime.Now;

        }

        /// <summary>
        /// Initialize the form data, as selects, lists or another web types 
        /// </summary>
        protected virtual void SetFormData() { }

        protected virtual void SetFormEntityDataToForm(T formEntity) { }

        protected virtual void SetEntityDataToForm(T entity) { }

        protected virtual void SetUserSecurityAccess()
        {
            ViewData["UserSecurityAccess"] = UserSecurityAccess.Edit;
        }

        protected virtual void SetUserSecurityAccess(T entity)
        {
            SetUserSecurityAccess();
        }

        protected virtual void SetEntityLocking(T entity)
        {
            ViewData["EntityLocked"] = GenericService.IsEntityLocked(entity).ToString();
        }

        protected virtual void ExecuteActionsAfterToSave(T entity) { }

        protected virtual void ExecuteActionsAfterToUpdate(T entity) { }
        
    }
}
