using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Compelligence.Common.Browse;
using Compelligence.Common.Utility;
using Compelligence.Domain.Entity;
using Compelligence.Domain.Entity.Resource;
using Compelligence.Util.Type;
using Compelligence.Web.Models.Web;

namespace Compelligence.Web.Controllers
{
    public abstract class BackEndAsyncFormController<T, IdT> : BackEndFormController<T, IdT>
    {

        [AcceptVerbs(HttpVerbs.Post)]
        public override ActionResult Index()
        {
            ViewData["Scope"] = Request["Scope"];
            ViewData["Container"] = Request["Container"];
            SetGeneralHelpDataToBackEnd();
            LoadFormData();
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public virtual ActionResult GetDetails(IdT id)
        {
            string detailTypeParam = Request["DetailType"];
            ActionResult actionResult = new EmptyResult();
            string scope = Request["Scope"];

            if (!string.IsNullOrEmpty(detailTypeParam))
            {
                T entity = GenericService.GetById(id);
                DetailType detailType = (DetailType)Enum.ToObject(typeof(DetailType), Convert.ToInt32(detailTypeParam));
                StringBuilder detailFilter = new StringBuilder();
                StringBuilder browseDetailFilter = new StringBuilder();
                string browseDetailName = string.Empty;
                string controller = SetDetailFilters(id, detailType, detailFilter, browseDetailFilter);
                string detailAction = "DetailList";

                if (controller.IndexOf(':') > 0)
                {
                    string[] components = controller.Split(':');
                    controller = components[0];
                    browseDetailName = components[1];

                    if (components.Length > 2)
                    {
                        detailAction = components[2];
                    }
                }

                SetUserSecurityAccess(entity);

                SetEntityLocking(entity);

                if (!(string.IsNullOrEmpty(controller) || string.IsNullOrEmpty(detailAction)))
                {
                    actionResult = RedirectToAction(detailAction, controller, new { Scope = scope, UserSecurityAccess = ViewData["UserSecurityAccess"], EntityLocked = ViewData["EntityLocked"], Container = Request["Container"], HeaderType = ViewData["HeaderType"], DetailFilter = detailFilter.ToString(), BrowseDetailName = browseDetailName, BrowseDetailFilter = browseDetailFilter.ToString() });
                }
            }

            return actionResult;
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public virtual ActionResult DetailList()
        {
            ViewData["Scope"] = Request["Scope"];
            ViewData["Container"] = Request["Container"];
            ViewData["HeaderType"] = Request["HeaderType"];
            ViewData["DetailFilter"] = Request["DetailFilter"];
            ViewData["BrowseDetailName"] = Request["BrowseDetailName"];
            ViewData["BrowseDetailFilter"] = Request["BrowseDetailFilter"];
            ViewData["UserSecurityAccess"] = Request["UserSecurityAccess"];
            ViewData["EntityLocked"] = Request["EntityLocked"];

            SetDetailFormData();

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public virtual ActionResult CreateDetail()
        {
            string[] ids = StringUtility.CheckNull(Request["Id"]).Split(':');
            string detailTypeParam = Request["DetailCreateType"];
            IList<T> newEntities = new List<T>();
            DetailCreateType detailType = DetailCreateType.Override;
            //string NameObject = collection["Name"]; 

            if (!string.IsNullOrEmpty(detailTypeParam))
            {
                detailType = (DetailCreateType)Enum.ToObject(typeof(DetailCreateType), Convert.ToInt32(detailTypeParam));
            }

            foreach (object id in ids)
            {
                if (!string.IsNullOrEmpty(id as string))
                {
                    T entity = GenericService.GetById((IdT)Convert.ChangeType(id, typeof(IdT)));

                    if (entity != null)
                    {
                        switch (detailType)
                        {
                            case DetailCreateType.Clone:

                                T entityClone = GenericService.GetEntityClone(entity);

                                SetDefaultDataFromRequest(entityClone);

                                SetDetailFilterData(entityClone);

                                SetDefaultDataForSave(entityClone);

                                newEntities.Add(entityClone);

                                break;

                            default:

                                SetDefaultDataFromRequest(entity);

                                SetDetailFilterData(entity);

                                SetDefaultDataForUpdate(entity);

                                newEntities.Add(entity);

                                //DomainObject<IdT> domainObject = (DomainObject<IdT>)((object)entity);

                                //string DomainT = (entity.GetType().Name).ToLower();
                                //string NameObject = DomainT.GetType().Name;

                                //string domainObjectEntity = domainObject.HeaderType;
                                //string ObjectEntityDetail = entity.GetType().Name;
                                //decimal IdsEntity = Convert.ToDecimal(domainObject.Id);
                                //GetActionHistoryOfFormData(entity, null, EntityAction.AddDetail);
                                GetActionHistoryAddDetail(entity);
                                //ActionHistoryService.BackEndActionHistory(IdsEntity, EntityAction.AddDetail, domainObjectEntity, ObjectEntityDetail, NameObject, CurrentUser, CurrentCompany);

                                break;
                        }
                    }
                }
            }

            switch (detailType)
            {
                case DetailCreateType.Clone:
                    GenericService.SaveCollection(newEntities);
                    break;
                default:
                    GenericService.UpdateCollection(newEntities);
                    break;
            }

            return null;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public override ActionResult Create(T entity, FormCollection collection)
        {
            string groupIdUser = string.Empty;
            OperationStatus operationStatus = OperationStatus.Initiated;
            string domainObjectEntity = entity.GetType().Name;
            string NameObject = collection["Name"];
            int i = 0;

            while (NameObject == string.Empty || NameObject == null)
            {
                i++;
                switch (i)
                {

                    case 1:
                        NameObject = ReflectionTool.GetProperty(entity, "Title");
                        break;

                    case 2:
                        NameObject = ReflectionTool.GetProperty(entity, "EventName");
                        break;

                    case 3:
                        NameObject = ReflectionTool.GetProperty(entity, "Task");
                        break;

                    case 4:
                        NameObject = ReflectionTool.GetProperty(entity, "FirstName") + " " + ReflectionTool.GetProperty(entity, "LastName");
                        break;
                    case 5:
                        NameObject = ReflectionTool.GetProperty(entity, "Source") + " " + ReflectionTool.GetProperty(entity, "Source");
                        break;    

                    default:
                        NameObject = "Error";
                        break;
                }

            }

                       

            if (ValidateFormData(entity, collection))
            {
                DomainObject<IdT> domainObject = (DomainObject<IdT>)((object)entity);                

                groupIdUser = SetActionUser(entity, Convert.ToDecimal(UniqueKeyGenerator.GetInstance().GetUniqueKey()), Convert.ToString(ActionMethod.Create));

                SetDetailFilterData(entity);

                GetFormData(entity, collection);
                                
                SetDefaultEntityDataForSave(entity);

                SetDefaultDataForSave(entity);

                GenericService.Save(entity);
               
                decimal IdsEntity = Convert.ToDecimal(domainObject.Id);
                //ActionHistoryService.BackEndActionHistory(IdsEntity, EntityAction.Created, domainObjectEntity, domainObjectEntity, NameObject, CurrentUser, CurrentCompany);
                //GetActionHistoryOfFormData(entity, collection, EntityAction.Created);
                GetActionHistoryCreated(entity, collection);

                operationStatus = OperationStatus.Successful;

                return GetActionResultForCreate(domainObject, operationStatus);             
                
            }

            SetDefaultRequestParametersToForm(ActionMethod.Create, operationStatus);

            SetFormData();

            SetFormEntityDataToForm(entity);

            SetUserSecurityAccess();

            return View("Edit", entity);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public virtual ActionResult Delete()
        {
            string groupIdUser = string.Empty;
            string[] ids = StringUtility.CheckNull(Request["Id"]).Split(',');
            string userId = (string)Session["UserId"];
            StringBuilder returnMessage = new StringBuilder();
            StringBuilder errorMessage = new StringBuilder();
            var result = new JsonResult();
            
            

            foreach (object identifier in ids)
            {
                T entity = GenericService.GetById((IdT)Convert.ChangeType(identifier, typeof(IdT)));
                //DomainObject<IdT> domainObject = (DomainObject<IdT>)((object)entity);
                //int i = 0;

                if (ValidateDeleteData(entity, errorMessage))
                {
                    SetDefaultDataFromRequest(entity);
                    groupIdUser += SetActionUser(entity, Convert.ToDecimal(UniqueKeyGenerator.GetInstance().GetUniqueKey()), Convert.ToString(ActionMethod.Delete)) + ',';
                    GenericService.Delete(entity, userId);

                    //string NameObject = ReflectionTool.GetProperty(entity,"Name");
                    

                    //while (NameObject == string.Empty || NameObject == null)
                    //{
                    //    i++;
                    //    switch (i)
                    //    {

                    //        case 1:
                    //            NameObject = ReflectionTool.GetProperty(entity, "Title");
                    //            break;

                    //        case 2:
                    //            NameObject = ReflectionTool.GetProperty(entity, "EventName");
                    //            break;

                    //        case 3:
                    //            NameObject = ReflectionTool.GetProperty(entity, "Task");
                    //            break;

                    //        case 4:
                    //            NameObject = ReflectionTool.GetProperty(entity, "FirstName") + " " + ReflectionTool.GetProperty(entity, "LastName");
                    //            break;

                            
                    //        case 5:
                    //            NameObject = ReflectionTool.GetProperty(entity, "Source") + " " + ReflectionTool.GetProperty(entity, "Source");
                    //            break;
                    //        case 6:
                    //            NameObject = ReflectionTool.GetProperty(entity, "TimePeriod");
                    //            break;
                    //        default:
                    //            NameObject = "Error";
                    //            break;
                    //    }

                    //}


                   // string domainObjectEntity = entity.GetType().Name;
                    //decimal IdsEntity = Convert.ToDecimal(domainObject.Id);
                    //GetActionHistoryOfFormData(entity, null, EntityAction.Deleted);
                    GetActionHistoryDeleted(entity);
                        //ActionHistoryService.BackEndActionHistory(IdsEntity, EntityAction.Deleted, domainObjectEntity, domainObjectEntity, NameObject, CurrentUser, CurrentCompany);
                }
            }

            if (errorMessage.Length > 0)
            {
                string[] messages = errorMessage.ToString().Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                returnMessage.Append("<p><ul>");

                foreach (string message in messages)
                {
                    returnMessage.Append("<div style='padding-left: 23px;'><li>" + Server.HtmlEncode(message) + "</li></div>");
                }

                returnMessage.Append("</ul></p>");
                
            }
            
            result.Data = new { GroupIdUser = groupIdUser, ReturnMessage = returnMessage.ToString() };
            return result;            
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public virtual ActionResult DeleteDetail()
        {
            string[] ids = StringUtility.CheckNull(Request["Id"]).Split(',');


            foreach (object identifier in ids)
            {
                T entity = GenericService.GetById((IdT)Convert.ChangeType(identifier, typeof(IdT)));

                SetDefaultDataFromRequest(entity);

                SetDetailFilterData(entity);

                SetDefaultDataForUpdate(entity);

                GenericService.DeleteRelations(entity);

            }


            return null;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public virtual ActionResult Duplicate()
        {
            string[] ids = StringUtility.CheckNull(Request["Id"]).Split(',');
            string groupIdUser = string.Empty;
            OperationStatus operationStatus = OperationStatus.Initiated;
            DomainObject<IdT> domainObject;
            IdT tempId = (IdT)Convert.ChangeType(ids[0], typeof(IdT));
            foreach (object identifier in ids)
            {
                T entity = GenericService.GetById((IdT)Convert.ChangeType(identifier, typeof(IdT)));

                SetDefaultDataFromRequest(entity);                

                SetDefaultDataForSave(entity);                

                domainObject = (DomainObject<IdT>)((object)GenericService.Duplicate(entity));                
               
                tempId = domainObject.Id;

                groupIdUser += SetActionUser(GenericService.GetById(tempId), Convert.ToDecimal(UniqueKeyGenerator.GetInstance().GetUniqueKey()), Convert.ToString(ActionMethod.Create)) + ',';
            }

            operationStatus = OperationStatus.Successful;
            return RedirectToAction("Edit", new { id = tempId, operationStatus = operationStatus.ToString(), Scope = Request["Scope"], BrowseId = Request["BrowseId"], IsDetail = Request["IsDetail"], HeaderType = Request["HeaderType"], DetailFilter = Request["DetailFilter"], Container = Request["Container"], GroupIdUser = groupIdUser});
        }

        protected override void SetDefaultRequestParametersToForm(ActionMethod actionMethod, OperationStatus operationStatus)
        {
            string formHeaderMessage = (string)HttpContext.GetGlobalResourceObject("LabelResource", "ActionMethod" + actionMethod.ToString());

            if (string.IsNullOrEmpty(formHeaderMessage))
            {
                formHeaderMessage = actionMethod.ToString();
            }
            string groupIdUser = Request["GroupIdUser"];
            ViewData["ActionMethod"] = actionMethod.ToString();
            ViewData["ActionMethodValue"] = actionMethod;
            ViewData["FormHeaderMessage"] = formHeaderMessage;
            ViewData["Scope"] = Request["Scope"];
            ViewData["BrowseId"] = Request["BrowseId"];
            ViewData["HeaderType"] = Request["HeaderType"];
            ViewData["IsDetail"] = Request["IsDetail"];
            ViewData["Container"] = Request["Container"];
            ViewData["DetailFilter"] = Request["DetailFilter"];
            ViewData["OperationStatus"] = operationStatus;
            if (!string.IsNullOrEmpty(groupIdUser))
            {
                ViewData["GroupIdUser"] = Request["GroupIdUser"];
            }            
        }

        protected virtual void SetDetailContent()
        {
            ViewData["TypeObject"] = Request["TypeObject"];
        }

        /// <summary>
        /// Get detail filter value, according to a parameter
        /// </summary>
        /// <param name="parameter"></param>
        protected virtual string GetDetailFilterValue(string parameter)
        {
            string detailFilterValue = default(string);

            string detailFilter = Request["DetailFilter"];

            if (!string.IsNullOrEmpty(detailFilter))
            {
                string[] filters = detailFilter.Split(':');

                foreach (string filter in filters)
                {
                    string[] filterComponents = filter.Split('_');

                    if (filterComponents.Length == 3)
                    {
                        if (parameter.Equals(filterComponents[0]))
                        {
                            detailFilterValue = filterComponents[2];
                        }
                    }
                }
            }

            return detailFilterValue;
        }

        protected virtual void SetDetailFilterData(T entity)
        {
            String paramIsDetail = Request["IsDetail"];
            bool isDetail = Convert.ToBoolean(String.IsNullOrEmpty(paramIsDetail) ? "False": paramIsDetail);

            if (isDetail)
            {
                string detailFilter = Request["DetailFilter"];
                Type entityType = typeof(T);
                object entityObject = (object)entity;

                if (!string.IsNullOrEmpty(detailFilter))
                {
                    string[] filters = detailFilter.Split(':');

                    foreach (string filter in filters)
                    {
                        string[] filterComponents = filter.Split('_');

                        if (filterComponents.Length == 3)
                        {
                            string entityName = entityType.Name;
                            string column = filterComponents[0];

                            if (column.IndexOf('.') > 0)
                            {
                                string[] columnComponents = column.Split('.');

                                entityName = columnComponents[0];
                                column = columnComponents[1];
                            }

                            if (entityType.Name.Equals(entityName))
                            {
                                TypeUtility.SetPropertyValue(entityType, entityObject, column, filterComponents[2]);
                            }
                        }
                    }
                }
            }
        }

        protected void AddFilter(StringBuilder filters, string property, string value)
        {
            AddFilter(filters, property, BrowseFilter.Operator.Eq, value);
        }

        protected void AddFilter(StringBuilder filters, string property, BrowseFilter.Operator filterOperator, string value)
        {
            StringBuilder newFilter = new StringBuilder();

            newFilter.Append(property);
            newFilter.Append("_");
            newFilter.Append(filterOperator.ToString());
            newFilter.Append("_");
            newFilter.Append(value);

            if (!string.IsNullOrEmpty(filters.ToString()))
            {
                filters.Append(":");
            }

            filters.Append(newFilter);
        }

        protected void SetDefaultDataFromRequest(T entityObject)
        {
            ((DomainObject<IdT>)((object)entityObject)).HeaderType = StringUtility.CheckNull(Request["HeaderType"]);
        }

        /// <summary>
        /// Initialize detail form data
        /// </summary>
        protected virtual void SetDetailFormData() { }

        protected virtual string SetDetailFilters(IdT parentId, DetailType detailType, StringBuilder detailFilter, StringBuilder browseDetailFilter) { return string.Empty; }

        protected bool IsDetailForm()
        {
            bool isDetailForm = false;
            string isDetail = Request["IsDetail"];

            if (!string.IsNullOrEmpty(isDetail))
            {
                isDetailForm = Convert.ToBoolean(isDetail);
            }

            return isDetailForm;
        }

    }
}
