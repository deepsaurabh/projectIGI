using API.Security;
using Data.Enum;
using Data.ViewModel;
using Entity.POCO;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API.Controllers
{
    [RoutePrefix("api/Toolkit")]
    public class ToolkitController : BaseController
    {
        [Route("GetAllToolkit")]
        public HttpResponseMessage GetAllToolkit()
        {
            var outToolkit = UnitOfWork.ToolkitRepository.Get(ch => ch.IsDeleted == false);

            return this.Request.CreateResponse(HttpStatusCode.OK, new { toolkit = outToolkit });
        }

        [Route("GetAllPublicToolkit")]
        public HttpResponseMessage GetAllPublicToolkit()
        {
            var outToolkit = UnitOfWork.ToolkitRepository.Get(ch => ch.IsDeleted == false);

            return this.Request.CreateResponse(HttpStatusCode.OK, new { Toolkit = outToolkit });
        }


        [Route("GetAllFreeToolkit")]
        public HttpResponseMessage GetAllFreeToolkit()
        {
            var outToolkit = UnitOfWork.ToolkitRepository.Get(ch => ch.IsDeleted == false);

            return this.Request.CreateResponse(HttpStatusCode.OK, new { Toolkit = outToolkit });
        }

        [Route("GetAllPaidToolkit")]
        public HttpResponseMessage GetAllPaidToolkit()
        {
            var outToolkit = UnitOfWork.ToolkitRepository.Get(ch => ch.IsDeleted == false);

            return this.Request.CreateResponse(HttpStatusCode.OK, new { Toolkit = outToolkit });
        }

        [Route("GetAllToolkitbyId")]
        public HttpResponseMessage GetAllToolkitbyId(Int64 id, DocumentScope scope)
        {

            var documentCourse = UnitOfWork.ToolkitRepository.GetById(id);

            var course = CreateCourseInstance(documentCourse, scope);

            return this.Request.CreateResponse(HttpStatusCode.OK, new { Course = course });

        }

        private ToolkitViewModel CreateCourseInstance(Toolkit item, DocumentScope scope)
        {
            var documentList = UnitOfWork.ToolkitAttachmentRepository.Get(ch => ch.toolkitId == item.Id);
            var viewModel = new ToolkitViewModel()
            {
                toolkitID = item.Id,
                toolkitName = item.ToolkitName,
                //startDate = item.StartDate,
                //endDate = item.EndDate,
                price = item.Price,
                currencyType = item.CurrencyType,
            };

            if (scope == DocumentScope.allDocument || scope == DocumentScope.freeDocument)
            {
                viewModel.freeContent = new Content()
                {
                    description = item.ToolkitFreeContent
                };
            }
            if (scope == DocumentScope.allDocument || scope == DocumentScope.paidDocument)
            {
                viewModel.paidContent = new Content()
                {
                    description = item.ToolkitPaidContent
                };
            }
            if (scope == DocumentScope.allDocument || scope == DocumentScope.publicDocument)
            {
                viewModel.publicContent = new Content()
                {
                    description = item.ToolkitPublicContent
                };
            }

            foreach (var attDocument in documentList)
            {
                AttachedDocument document = null;
                if (scope == DocumentScope.allDocument)
                    document = UnitOfWork.AttachedDocumentRepository.Get(ch => ch.Id == attDocument.toolkitDocumentID && !ch.IsDeleted).FirstOrDefault();
                else
                    document = UnitOfWork.AttachedDocumentRepository.Get(ch => ch.Id == attDocument.toolkitDocumentID && !ch.IsDeleted && ch.Scope == scope).FirstOrDefault();
                if (document != null)
                {
                    if (document.Scope == DocumentScope.freeDocument && viewModel.freeContent != null && viewModel.freeContent.fileAttachment == null)
                        viewModel.freeContent.fileAttachment = new List<DocumentAttached>();
                    else if (document.Scope == DocumentScope.publicDocument && viewModel.publicContent != null && viewModel.publicContent.fileAttachment == null)
                        viewModel.publicContent.fileAttachment = new List<DocumentAttached>();
                    else if (document.Scope == DocumentScope.paidDocument && viewModel.paidContent != null && viewModel.paidContent.fileAttachment == null)
                        viewModel.paidContent.fileAttachment = new List<DocumentAttached>();
                    var base64String = Convert.ToBase64String(document.FileData, 0, document.FileData.Length);
                    var attachedDocument = new DocumentAttached()
                    {
                        attachmentID = document.Id,
                        imageURL = String.Format("data:{0};base64,{1}", document.FileType, base64String),
                        documentScope = document.Scope,
                        documentName = document.DocumentName
                    };
                    if (document.Scope == DocumentScope.freeDocument)
                        viewModel.freeContent.fileAttachment.Add(attachedDocument);
                    else if (document.Scope == DocumentScope.paidDocument)
                        viewModel.paidContent.fileAttachment.Add(attachedDocument);
                    else if (document.Scope == DocumentScope.publicDocument)
                        viewModel.publicContent.fileAttachment.Add(attachedDocument);
                }
            }
            return viewModel;
        }

        [Route("GetPublicToolkitById")]
        public HttpResponseMessage GetPublicToolkitById(Int64 id)
        {
            var documentToolkit = UnitOfWork.ToolkitRepository.GetById(id);
            var outToolkit = new
                            {
                                Id = documentToolkit.Id,
                                Name = documentToolkit.ToolkitName,
                                Content = documentToolkit.ToolkitPublicContent,
                                Price = documentToolkit.Price.ToString(),
                                Currency = documentToolkit.CurrencyType.ToString(),
                            };


            return this.Request.CreateResponse(HttpStatusCode.OK, new { Toolkit = outToolkit });
        }


        [Route("GetFreeToolkitById")]
        public HttpResponseMessage GetFreeToolkitById(Int64 id)
        {
            var documentToolkit = UnitOfWork.ToolkitRepository.GetById(id);
            var outToolkit = new
            {
                Id = documentToolkit.Id,
                Name = documentToolkit.ToolkitName,
                Content = documentToolkit.ToolkitFreeContent,
                Price = documentToolkit.Price.ToString(),
                Currency = documentToolkit.CurrencyType.ToString(),
            };

            return this.Request.CreateResponse(HttpStatusCode.OK, new { Toolkit = outToolkit });
        }

        [Route("GetPaidToolkitById")]
        public HttpResponseMessage GetPaidToolkitById(Int64 id)
        {
            var documentToolkit = UnitOfWork.ToolkitRepository.GetById(id);
            var outToolkit = new
            {
                Id = documentToolkit.Id,
                Name = documentToolkit.ToolkitName,
                Content = documentToolkit.ToolkitPaidContent,
                Price = documentToolkit.Price.ToString(),
                Currency = documentToolkit.CurrencyType.ToString(),
            };


            return this.Request.CreateResponse(HttpStatusCode.OK, new { Toolkit = outToolkit });
        }

        [AuthorizeAppRole(AppRole.admin)]

        [Route("Post")]
        public HttpResponseMessage Post(ToolkitViewModel viewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest);

                Toolkit model = new Toolkit()
                {
                    ToolkitFreeContent = viewModel.freeContent.description,
                    ToolkitName = viewModel.toolkitName,
                    ToolkitPaidContent = viewModel.paidContent.description,
                    ToolkitPublicContent = viewModel.publicContent.description,
                    Price = viewModel.price,
                    //StartDate = viewModel.startDate,
                    //EndDate = viewModel.endDate,
                    CurrencyType = viewModel.currencyType
                };
                if (viewModel.toolkitID > 0)
                {
                    model.Id = viewModel.toolkitID;
                    model.CreatedDate = DateTime.UtcNow;
                    UnitOfWork.ToolkitRepository.Update(model);
                }
                else
                {
                    UnitOfWork.ToolkitRepository.Insert(model);
                }
                UnitOfWork.SaveChange();
                var getAllMapping = UnitOfWork.ToolkitAttachmentRepository.Get(ch => ch.toolkitId == model.Id && !ch.IsDeleted);

                if (viewModel.freeContent != null && viewModel.freeContent.fileAttachment != null)
                {
                    foreach (var item in viewModel.freeContent.fileAttachment)
                    {
                        var itemExist = getAllMapping.Where(ch => ch.toolkitDocumentID == item.attachmentID).FirstOrDefault();
                        if (itemExist == null)
                        {
                            UnitOfWork.ToolkitAttachmentRepository.Insert(new ToolkitAttachmentMapping() { toolkitId = model.Id, toolkitDocumentID = item.attachmentID });
                        }
                        else if (item.isDeleted)
                        {
                            itemExist.IsDeleted = true;
                            UnitOfWork.ToolkitAttachmentRepository.Update(itemExist);
                        }
                    }
                }

                if (viewModel.publicContent != null && viewModel.publicContent.fileAttachment != null)
                {
                    foreach (var item in viewModel.publicContent.fileAttachment)
                    {
                        var itemExist = getAllMapping.Where(ch => ch.toolkitDocumentID == item.attachmentID).FirstOrDefault();
                        if (itemExist == null)
                        {
                            UnitOfWork.ToolkitAttachmentRepository.Insert(new ToolkitAttachmentMapping() { toolkitId = model.Id, toolkitDocumentID = item.attachmentID });
                        }
                        else if (item.isDeleted)
                        {
                            itemExist.IsDeleted = true;
                            UnitOfWork.ToolkitAttachmentRepository.Update(itemExist);
                        }
                    }
                }

                if (viewModel.paidContent != null && viewModel.paidContent.fileAttachment != null)
                {
                    foreach (var item in viewModel.paidContent.fileAttachment)
                    {
                        var itemExist = getAllMapping.Where(ch => ch.toolkitDocumentID == item.attachmentID).FirstOrDefault();
                        if (itemExist == null)
                        {
                            UnitOfWork.ToolkitAttachmentRepository.Insert(new ToolkitAttachmentMapping() { toolkitId = model.Id, toolkitDocumentID = item.attachmentID });
                        }
                        else if (item.isDeleted)
                        {
                            itemExist.IsDeleted = true;
                            UnitOfWork.ToolkitAttachmentRepository.Update(itemExist);
                        }
                    }
                }

                UnitOfWork.SaveChange();

                return this.Request.CreateResponse(HttpStatusCode.Created, model);
            }
            catch (DbEntityValidationException er)
            {
                return this.Request.CreateResponse(HttpStatusCode.BadRequest, er);
            }
            catch (Exception ex)
            {
                return this.Request.CreateResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [Route("Put")]
        public HttpResponseMessage Put(Toolkit model)
        {
            if (!ModelState.IsValid)
                return this.Request.CreateResponse(HttpStatusCode.BadRequest);

            var Toolkit = UnitOfWork.ToolkitRepository.GetById(model.Id);
            if (Toolkit != null)
            {
                Toolkit.ToolkitName = model.ToolkitName;
                Toolkit.ToolkitFreeContent = model.ToolkitFreeContent;
                Toolkit.ToolkitPaidContent = model.ToolkitPaidContent;
                Toolkit.ToolkitPublicContent = model.ToolkitPublicContent;
                Toolkit.Price = model.Price;

                UnitOfWork.ToolkitRepository.Update(Toolkit);
                UnitOfWork.SaveChange();
            }
            return this.Request.CreateResponse(HttpStatusCode.OK);
        }

        [Route("Delete")]
        public HttpResponseMessage Delete(int id)
        {
            UnitOfWork.ToolkitRepository.SoftDelete(id);
            UnitOfWork.SaveChange();

            return this.Request.CreateResponse(HttpStatusCode.OK);
        }

    }

}
