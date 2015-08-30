using API.Security;
using Data.Enum;
using Data.ViewModel;
using Entity.POCO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Validation;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace API.Controllers
{
    [RoutePrefix("api/Course")]
    public class CourseController : BaseController
    {
        [AuthorizeAppRole(AppRole.admin)]
        [Route("GetAllCourse")]
        public HttpResponseMessage GetAllCourse()
        {
            var outCourse = UnitOfWork.CourseRepository.Get(ch => ch.IsDeleted == false);

            return this.Request.CreateResponse(HttpStatusCode.OK, new { Course = outCourse });
        }

        [AuthorizeAppRole(AppRole.customer,AppRole.admin)]
        [Route("GetAllPublicCourse")]
        public HttpResponseMessage GetAllPublicCourse()
        {
            var outCourse = UnitOfWork.CourseRepository.Get(ch => ch.IsDeleted == false);

            return this.Request.CreateResponse(HttpStatusCode.OK, new { Course = outCourse });
        }

        [Route("GetAllFreeCourse")]
        public HttpResponseMessage GetAllFreeCourse()
        {
            var outCourse = UnitOfWork.CourseRepository.Get(ch => ch.IsDeleted == false);

            return this.Request.CreateResponse(HttpStatusCode.OK, new { Course = outCourse });
        }


        private CourseViewModel CreateCourseInstance(Course item, DocumentScope scope)
        {
            var courseDocumentList = UnitOfWork.CourseAttachmentRepository.Get(ch => ch.courseId == item.Id);
            var courseModel = new CourseViewModel()
            {
                courseID = item.Id,
                courseName = item.CourseName,
                startDate = item.StartDate,
                endDate = item.EndDate,
                price = item.Price,
                currencyType = item.CurrencyType,
            };

            if (scope == DocumentScope.allDocument || scope == DocumentScope.freeDocument)
            {
                courseModel.freeContent = new Content()
                {
                    description = item.CourseFreeContent
                };
            }
            if (scope == DocumentScope.allDocument || scope == DocumentScope.paidDocument)
            {
                courseModel.paidContent = new Content()
                {
                    description = item.CoursePaidContent
                };
            }
            if (scope == DocumentScope.allDocument || scope == DocumentScope.publicDocument)
            {
                courseModel.publicContent = new Content()
                {
                    description = item.CoursePublicContent
                };
            }

            foreach (var courseDocument in courseDocumentList)
            {
                AttachedDocument document = null;
                if (scope == DocumentScope.allDocument)
                    document = UnitOfWork.AttachedDocumentRepository.Get(ch => ch.Id == courseDocument.courseDocumentID && !ch.IsDeleted).FirstOrDefault();
                else
                    document = UnitOfWork.AttachedDocumentRepository.Get(ch => ch.Id == courseDocument.courseDocumentID && !ch.IsDeleted && ch.Scope == scope).FirstOrDefault();
                if (document != null)
                {
                    if (document.Scope == DocumentScope.freeDocument && courseModel.freeContent != null && courseModel.freeContent.fileAttachment == null)
                        courseModel.freeContent.fileAttachment = new List<DocumentAttached>();
                    else if (document.Scope == DocumentScope.publicDocument && courseModel.publicContent != null && courseModel.publicContent.fileAttachment == null)
                        courseModel.publicContent.fileAttachment = new List<DocumentAttached>();
                    else if (document.Scope == DocumentScope.paidDocument && courseModel.paidContent != null && courseModel.paidContent.fileAttachment == null)
                        courseModel.paidContent.fileAttachment = new List<DocumentAttached>();
                    var base64String = Convert.ToBase64String(document.FileData, 0, document.FileData.Length);
                    var attachedDocument = new DocumentAttached()
                        {
                            attachmentID = document.Id,
                            imageURL = String.Format("data:{0};base64,{1}", document.FileType, base64String),
                            documentScope = document.Scope,
                            documentName = document.DocumentName
                        };
                    if (document.Scope == DocumentScope.freeDocument)
                        courseModel.freeContent.fileAttachment.Add(attachedDocument);
                    else if (document.Scope == DocumentScope.paidDocument)
                        courseModel.paidContent.fileAttachment.Add(attachedDocument);
                    else if (document.Scope == DocumentScope.publicDocument)
                        courseModel.publicContent.fileAttachment.Add(attachedDocument);
                }
            }
            return courseModel;
        }

        [AuthorizeAppRole(AppRole.admin,AppRole.customer)]
        [Route("GetAllPaidCourse")]
        public HttpResponseMessage GetAllPaidCourse()
        {
            var outCourse = UnitOfWork.CourseRepository.Get(ch => ch.IsDeleted == false);

            return this.Request.CreateResponse(HttpStatusCode.OK, new { Course = outCourse });
        }


        [Route("GetAllCoursebyId")]
        public HttpResponseMessage GetAllCoursebyId(Int64 id, DocumentScope scope)
        {

            var documentCourse = UnitOfWork.CourseRepository.GetById(id);

            var course = CreateCourseInstance(documentCourse, scope);

            return this.Request.CreateResponse(HttpStatusCode.OK, new { Course = course });

        }

        [AuthorizeAppRole(AppRole.admin, AppRole.customer)]
        [Route("GetPublicCourseById")]
        public HttpResponseMessage GetPublicCourseById(Int64 id)
        {
            var documentCourse = UnitOfWork.CourseRepository.GetById(id);
            var outCourse = new
                            {
                                Id = documentCourse.Id,
                                Name = documentCourse.CourseName,
                                Content = documentCourse.CoursePublicContent,
                                Price = documentCourse.Price.ToString(),
                                Currency = documentCourse.CurrencyType.ToString(),
                                StartDate = documentCourse.StartDate,
                                EndDate = documentCourse.EndDate
                            };


            return this.Request.CreateResponse(HttpStatusCode.OK, new { Course = outCourse });
        }

        [Route("GetFreeCourseById")]
        public HttpResponseMessage GetFreeCourseById(Int64 id, DocumentScope scope)
        {
            var documentCourse = UnitOfWork.CourseRepository.GetById(id);

            var course = CreateCourseInstance(documentCourse, scope);

            return this.Request.CreateResponse(HttpStatusCode.OK, new { Course = course });
        }

        [AuthorizeAppRole(AppRole.admin, AppRole.customer)]
        [Route("GetPaidCourseById")]
        public HttpResponseMessage GetPaidCourseById(Int64 id)
        {
            var documentCourse = UnitOfWork.CourseRepository.GetById(id);
            var outCourse = new
            {
                Id = documentCourse.Id,
                Name = documentCourse.CourseName,
                Content = documentCourse.CoursePaidContent,
                Price = documentCourse.Price.ToString(),
                Currency = documentCourse.CurrencyType.ToString(),
                StartDate = documentCourse.StartDate,
                EndDate = documentCourse.EndDate
            };


            return this.Request.CreateResponse(HttpStatusCode.OK, new { Course = outCourse });
        }

        [AuthorizeAppRole(AppRole.admin)]
        [Route("Post")]
        public HttpResponseMessage Post(CourseViewModel viewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest);

                Course model = new Course()
                {
                    CourseFreeContent = viewModel.freeContent.description,
                    CourseName = viewModel.courseName,
                    CoursePaidContent = viewModel.paidContent.description,
                    CoursePublicContent = viewModel.publicContent.description,
                    Price = viewModel.price,
                    StartDate = viewModel.startDate,
                    EndDate = viewModel.endDate,
                    CurrencyType = viewModel.currencyType
                };
                if (viewModel.courseID > 0)
                {
                    model.Id = viewModel.courseID;
                    model.CreatedDate = DateTime.UtcNow;
                    UnitOfWork.CourseRepository.Update(model);
                }
                else
                {
                    UnitOfWork.CourseRepository.Insert(model);
                }
                UnitOfWork.SaveChange();
                var getAllMapping = UnitOfWork.CourseAttachmentRepository.Get(ch => ch.courseId == model.Id && !ch.IsDeleted);

                if (viewModel.freeContent != null && viewModel.freeContent.fileAttachment != null)
                {
                    foreach (var item in viewModel.freeContent.fileAttachment)
                    {
                        var itemExist = getAllMapping.Where(ch => ch.courseDocumentID == item.attachmentID).FirstOrDefault();
                        if (itemExist == null)
                        {
                            UnitOfWork.CourseAttachmentRepository.Insert(new CourseAttachmentMapping() { courseId = model.Id, courseDocumentID = item.attachmentID });
                        }
                        else if (item.isDeleted)
                        {
                            itemExist.IsDeleted = true;
                            UnitOfWork.CourseAttachmentRepository.Update(itemExist);
                        }
                    }
                }

                if (viewModel.publicContent != null && viewModel.publicContent.fileAttachment != null)
                {
                    foreach (var item in viewModel.publicContent.fileAttachment)
                    {
                        var itemExist = getAllMapping.Where(ch => ch.courseDocumentID == item.attachmentID).FirstOrDefault();
                        if (itemExist == null)
                        {
                            UnitOfWork.CourseAttachmentRepository.Insert(new CourseAttachmentMapping() { courseId = model.Id, courseDocumentID = item.attachmentID });
                        }
                        else if (item.isDeleted)
                        {
                            itemExist.IsDeleted = true;
                            UnitOfWork.CourseAttachmentRepository.Update(itemExist);
                        }
                    }
                }

                if (viewModel.paidContent != null && viewModel.paidContent.fileAttachment != null)
                {
                    foreach (var item in viewModel.paidContent.fileAttachment)
                    {
                        var itemExist = getAllMapping.Where(ch => ch.courseDocumentID == item.attachmentID).FirstOrDefault();
                        if (itemExist == null)
                        {
                            UnitOfWork.CourseAttachmentRepository.Insert(new CourseAttachmentMapping() { courseId = model.Id, courseDocumentID = item.attachmentID });
                        }
                        else if (item.isDeleted)
                        {
                            itemExist.IsDeleted = true;
                            UnitOfWork.CourseAttachmentRepository.Update(itemExist);
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
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }

        }

        [AuthorizeAppRole(AppRole.admin)]
        [Route("Put")]
        public HttpResponseMessage Put(Course model)
        {
            if (!ModelState.IsValid)
                return this.Request.CreateResponse(HttpStatusCode.BadRequest);

            var course = UnitOfWork.CourseRepository.GetById(model.Id);
            if (course != null)
            {
                course.CourseName = model.CourseName;
                course.CourseFreeContent = model.CourseFreeContent;
                course.CoursePaidContent = model.CoursePaidContent;
                course.CoursePublicContent = model.CoursePublicContent;
                course.Price = model.Price;
                course.StartDate = model.StartDate;
                course.EndDate = model.EndDate;

                UnitOfWork.CourseRepository.Update(course);
                UnitOfWork.SaveChange();
            }
            return this.Request.CreateResponse(HttpStatusCode.OK);
        }

        [AuthorizeAppRole(AppRole.admin)]
        [Route("Delete")]
        public HttpResponseMessage Delete(int id)
        {
            UnitOfWork.CourseRepository.SoftDelete(id);
            UnitOfWork.SaveChange();

            return this.Request.CreateResponse(HttpStatusCode.OK);
        }        
    }
}
