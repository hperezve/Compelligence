using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Compelligence.BusinessLogic.Interface;
using Compelligence.Web.Models.Web;
using Compelligence.Domain.Entity;
using Compelligence.Domain.Entity.Resource;
using Compelligence.Util.Type;
using System.Text;
using Compelligence.Common.Utility;


namespace Compelligence.Web.Controllers
{
    public abstract class BackEndFormController<T, IdT> : GenericBackEndController
    {
        protected IGenericService<T, IdT> _genericService;

        public IGenericService<T, IdT> GenericService
        {
            get { return _genericService; }
        }
                    
        [AcceptVerbs(HttpVerbs.Post)]
        public virtual ActionResult Index()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
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
            //string domainObjectEntity = entity.GetType().Name;
            //string NameObject = collection["Name"];

            //int i = 0;

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
            //        default:
            //            NameObject = "Error";
            //            break;
            //    }

            //}
            
            
            if (ValidateFormData(entity, collection))
            {
                DomainObject<IdT> domainObject = (DomainObject<IdT>)((object)entity);
                
                GetFormData(entity, collection);

                SetDefaultEntityDataForSave(entity);

                SetDefaultDataForSave(entity);

                GenericService.Save(entity);

                //decimal IdsEntity = Convert.ToDecimal(domainObject.Id);
                GetActionHistoryCreated(entity, collection);
                //ActionHistoryService.BackEndActionHistory(IdsEntity, EntityAction.Created, domainObjectEntity, domainObjectEntity, NameObject, CurrentUser, CurrentCompany);
                
                operationStatus = OperationStatus.Successful;

                return GetActionResultForCreate(domainObject, operationStatus);
            }

            SetDefaultRequestParametersToForm(ActionMethod.Create, operationStatus);

            SetFormData();

            SetFormEntityDataToForm(entity);

            SetUserSecurityAccess();
                       

            return View("Edit", entity);
        }

        [AcceptVerbs(HttpVerbs.Get)]
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
            //string domainObjectEntity = entity.GetType().Name;
            //decimal IdsEntity = Convert.ToDecimal(collection["Id"]);
            //string NameObject = collection["Name"];
            //int i = 0;

            //while (NameObject == string.Empty || NameObject == null )
            //{
            //    i++;
            //    switch  (i) {

            //    case 1:
            //     NameObject = ReflectionTool.GetProperty(entity, "Title");
            //     break; 

            //    case 2:
            //     NameObject = ReflectionTool.GetProperty(entity, "EventName");
            //     break; 

            //    case 3:
            //     NameObject = ReflectionTool.GetProperty(entity, "Task");
            //     break;

            //    case 4:
            //     NameObject = ReflectionTool.GetProperty(entity, "FirstName") + " " + ReflectionTool.GetProperty(entity, "LastName");
            //     break;
            //    case 5:
            //     NameObject = ReflectionTool.GetProperty(entity, "Source") + " " + ReflectionTool.GetProperty(entity, "Source");
            //     break;
            //    default:
            //     NameObject = "Error";
            //     break;
                
            //    } 
              


            //}

            OperationStatus operationStatus = OperationStatus.Initiated;

            if (ValidateEditFormData(formEntity, collection))
            {
                CompareForm(entity, formEntity, collection,ActionMethod.Edit);

                SetFormDataToEntity(entity, collection);                

                GetFormData(entity, collection);

                SetDefaultDataForUpdate(entity);

                GenericService.Update(entity);
               
                //ActionHistoryService.BackEndActionHistory(IdsEntity, EntityAction.Updated, domainObjectEntity, domainObjectEntity, NameObject, CurrentUser, CurrentCompany);
                //GetActionHistoryOfFormData(entity, collection, EntityAction.Updated);
                GetActionHistoryUpdated(entity, collection);

                entityResult = entity;

                operationStatus = OperationStatus.Successful;

            }           

            SetDefaultRequestParametersToForm(ActionMethod.Edit, operationStatus);

            SetFormData();

            SetFormEntityDataToForm(entityResult);

            SetUserSecurityAccess(entity);

            SetEntityLocking(entity);

            return GetActionResultForEdit(entity, operationStatus);
        }

        protected virtual void SetDefaultRequestParametersToForm(OperationStatus operationStatus)
        {
            SetDefaultRequestParametersToForm(ActionMethod.Unknown, operationStatus);
        }

        protected virtual void SetDefaultRequestParametersToForm(ActionMethod actionMethod, OperationStatus operationStatus)
        {
        }
        public virtual string SetActionUser(T entity, decimal GroupFieldId, string Action)
        {
            return string.Empty;
        }
        public virtual void CompareForm(T entity, T formEntity, FormCollection collection, ActionMethod actionMethod)
        {

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

        /// <summary>
        /// Validate data for delete
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        protected virtual bool ValidateDeleteData(T entity, StringBuilder errorMessage)
        {
            return true;
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

        protected virtual void SetDefaultDataForSaveHistory(object entity)
        {
            DomainObject<decimal> domainObject = (DomainObject<decimal>)entity;

            domainObject.CreatedDate = DateTime.Now;

            domainObject.ClientCompany = CurrentCompany;

            domainObject.CreatedBy = CurrentUser;

            SetDefaultDataForUpdateHistory(entity);
        }

        protected virtual void SetDefaultDataForUpdateHistory(object entity)
        {
            DomainObject<decimal> domainObject = (DomainObject<decimal>)entity;

            domainObject.LastChangedBy = CurrentUser;

            domainObject.LastChangedDate = DateTime.Now;

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

        /// <summary>
        /// Set in the new entity object data does not considered in web form
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="collection"></param>
        protected virtual void SetFormDataToEntity(T entity, FormCollection collection)
        {
            string[] formKeys = collection.AllKeys;
            Type entityType = typeof(T);
            object entityObject = (object)entity;

            foreach (string formKey in formKeys)
            {
                if (!formKey.Equals("Id", StringComparison.OrdinalIgnoreCase))
                {
                    TypeUtility.SetPropertyValue(entityType, entityObject, formKey, collection[formKey]);
                }
            }

        }

        protected virtual void HistoryUser(T entity, FormCollection collection)
        {
                       
            string[] formKeys = collection.AllKeys;
                    
            Type entityType = typeof(T);
            object entityObject = (object)entity;

            string domainObjectEntity = collection["BrowseId"];                 
            decimal IdsEntity = Convert.ToDecimal(collection["Id"]);  
  
           
            ActionHistoryService.RecordActionHistory(IdsEntity, EntityAction.Updated, domainObjectEntity, ActionFrom.BackEnd, CurrentUser, CurrentCompany);
   
        }

        protected virtual void GetActionHistoryCreated(T entityObject, FormCollection collection)
        {
            GetActionHistoryOfFormData(entityObject, collection, EntityAction.Created);
        }
        protected virtual void GetActionHistoryDeleted(T entityObject)
        {
            GetActionHistoryOfFormData(entityObject, null, EntityAction.Deleted);
        }
        protected virtual void GetActionHistoryUpdated(T entityObject, FormCollection collection)
        {
            GetActionHistoryOfFormData(entityObject, collection, EntityAction.Updated);
        }
        protected virtual void GetActionHistoryAddDetail(T entityObject)
        {
            GetActionHistoryOfFormData(entityObject, null, EntityAction.AddDetail);
        }

        protected virtual void GetActionHistoryOfFormData(T entityObject, FormCollection collection, string entityAction) {

            DomainObject<IdT> domainObject = (DomainObject<IdT>)((object)entityObject);
            decimal IdsEntity = Convert.ToDecimal(domainObject.Id);
            string domainObjectHeaderType = domainObject.HeaderType;
            string domainObjectEntity = entityObject.GetType().Name;
            
            string NameObject = string.Empty;
            NameObject = ReflectionTool.GetProperty(entityObject, "Name");
            if (string.IsNullOrEmpty(NameObject) && collection != null) 
            {
                NameObject = collection["Name"];
            }
            int i = 0;

            while (NameObject == string.Empty || NameObject == null || NameObject == " ")
            {
                i++;
                switch (i)
                {
                    case 1:
                        NameObject = ReflectionTool.GetProperty(entityObject, "Title");
                        break;
                    case 2:
                        NameObject = ReflectionTool.GetProperty(entityObject, "EventName");
                        break;
                    case 3:
                        NameObject = ReflectionTool.GetProperty(entityObject, "Task");
                        break;
                    case 4:
                        NameObject = ReflectionTool.GetProperty(entityObject, "FirstName") + " " + ReflectionTool.GetProperty(entityObject, "LastName");
                        break;
                    case 5:
                        NameObject = ReflectionTool.GetProperty(entityObject, "Source");
                        break;
                    case 6:
                        NameObject = ReflectionTool.GetProperty(entityObject, "TimePeriod");
                        break;
                    default:
                        NameObject = "Error";
                        break;
                }

            }
            //ActionHistoryService.RecordActionHistory(IdsEntity, EntityAction.Updated, domainObjectEntity, ActionFrom.BackEnd, CurrentUser, CurrentCompany);
            if (entityAction.Equals(EntityAction.AddDetail))
            {
                string DomainT = (entityObject.GetType().Name).ToLower();
                //NameObject = DomainT.GetType().Name;
                ActionHistoryService.BackEndActionHistory(IdsEntity, entityAction, domainObjectHeaderType, domainObjectEntity, NameObject, CurrentUser, CurrentCompany);
            }
            else
            {
                ActionHistoryService.BackEndActionHistory(IdsEntity, entityAction, domainObjectEntity, domainObjectEntity, NameObject, CurrentUser, CurrentCompany);
            }
        }

        protected virtual void LoadFormData() { }


        /// <summary>
        /// This method will get new URL to image, the new URL is the location of file in the application web and no user url address 
        /// </summary>
        /// <param name="urlImage"></param>
        /// <param name="contextFilePath"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        protected virtual string GetNewUrlToImage(string urlImage, string contextFilePath, Compelligence.Common.Utility.Upload.FileUploadType type)
        {

            if (!string.IsNullOrEmpty(urlImage))
            {
                if (urlImage.IndexOf("./FilesRepository/Images/") == -1)
                {
                    byte[] imageData = ResizeImage.GetBytesFromUrl(urlImage);
                    if (imageData != null)
                    {
                        System.IO.MemoryStream stream = new System.IO.MemoryStream(imageData);
                        try
                        {
                            System.Drawing.Image fullsizeImage = System.Drawing.Image.FromStream(stream);
                            stream.Close();

                            string[] urlObjects = urlImage.Split('/');

                            int newWidth = 170;
                            int newHeight = 170;

                            fullsizeImage.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone);
                            fullsizeImage.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone);
                            if (fullsizeImage.Width > newWidth)
                            {
                                newWidth = fullsizeImage.Width;
                            }
                            int resizeHeight = fullsizeImage.Height * newWidth / fullsizeImage.Width;
                            if (resizeHeight > newHeight)
                            {
                                newWidth = fullsizeImage.Width * newHeight / fullsizeImage.Height;
                                resizeHeight = newHeight;
                            }
                            System.Drawing.Image newImage = ResizeImage.DownloadImageFromUrl(urlImage.Trim());

                            fullsizeImage.Dispose();

                            Compelligence.Domain.Entity.File newFileImage = new Compelligence.Domain.Entity.File();

                            if (urlObjects.Length > 0)
                            {
                                newFileImage.FileName = System.IO.Path.GetFileName(urlObjects[urlObjects.Length - 1]);
                                if (newFileImage.FileName.IndexOf("%20") != -1)
                                {
                                    newFileImage.FileName = newFileImage.FileName.Replace("%20", "-");
                                }
                                if (newFileImage.FileName.IndexOf("?") != -1)
                                {
                                    string[] parameterBegin = newFileImage.FileName.Split('?');
                                    if (parameterBegin.Length > 1)
                                    {
                                        newFileImage.FileName = parameterBegin[0];
                                    }
                                }
                                if (newFileImage.FileName.IndexOf("&") != -1)
                                {
                                    string[] parameterOther = newFileImage.FileName.Split('?');
                                    if (parameterOther.Length > 1)
                                    {
                                        newFileImage.FileName = parameterOther[0];
                                    }
                                }
                                if (newFileImage.FileName.IndexOf("=") != -1)
                                {
                                    string[] parameterAssignment = newFileImage.FileName.Split('?');
                                    if (parameterAssignment.Length > 1)
                                    {
                                        newFileImage.FileName = parameterAssignment[0];
                                    }
                                }
                            }
                            if (newFileImage.FileName.LastIndexOf('.') != -1)
                            {
                                newFileImage.FileFormat = newFileImage.FileName.Substring(newFileImage.FileName.LastIndexOf('.') + 1);//Errir
                            }
                            if (newFileImage.FileFormat.Equals("ashx"))
                            {
                                newFileImage.FileFormat = "jpg";
                                newFileImage.FileName = newFileImage.FileName.Substring(0, newFileImage.FileName.LastIndexOf('.')) + "." + newFileImage.FileFormat;
                            }
                            decimal genericid = (decimal)UniqueKeyGenerator.GetInstance().GetUniqueKey();
                            string newPhysicalName = string.Empty + genericid + "_" + newFileImage.FileName;
                            string fileNameResult = string.Empty;

                            SetDefaultDataForSave(newFileImage);
                            newFileImage.PhysicalName = newPhysicalName;

                            SaveFile(newFileImage);

                            fileNameResult = System.IO.Path.Combine(contextFilePath + GetFilePath(type), newFileImage.PhysicalName);
                            fileNameResult = fileNameResult.Replace("\\\\", "\\");
                            newImage.Save(fileNameResult);
                            urlImage = "." + System.IO.Path.Combine(GetFilePath(type), newFileImage.PhysicalName).Replace("\\", "/");
                        }
                        catch
                        {
                            bool ErrorImageS = true;
                            ReturnResult(ErrorImageS);
                        }
                    }
                }
            }
            return urlImage;
        }

        /// <summary>
        /// This Method was created to save the file, the file can not save in generic , this should be save in ENTITY controller
        /// </summary>
        /// <param name="file"></param>
        protected virtual void SaveFile(Compelligence.Domain.Entity.File file)
        {
            ///save
        }
        /// <summary>
        /// This Method is to return a Message in the case no is correct the way to build the file
        /// </summary>
        /// <param name="error"></param>
        protected virtual void ReturnResult(bool error)
        { }
    }
}
