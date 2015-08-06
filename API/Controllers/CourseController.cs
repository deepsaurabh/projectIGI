using Data.Enum;
using Data.ViewModel;
using Entity.POCO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        [Route("GetAllCourse")]
        public HttpResponseMessage GetAllCourse()
        {
            var outCourse = from documentCourse in UnitOfWork.CourseRepository.Get()
                            select new
                            {
                                Id = documentCourse.Id,
                                Name = documentCourse.CourseName,
                                Price = documentCourse.Price.ToString(),
                                Currency = documentCourse.CurrencyType.ToString(),
                            };


            return this.Request.CreateResponse(HttpStatusCode.OK, new { Course = outCourse });
        }

        [Route("GetAllPublicCourse")]
        public HttpResponseMessage GetAllPublicCourse()
        {
            var outCourse = from documentCourse in UnitOfWork.CourseRepository.Get()
                            select new
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


        [Route("GetAllFreeCourse")]
        public HttpResponseMessage GetAllFreeCourse()
        {
            var outCourse = UnitOfWork.CourseRepository.Get(ch => ch.IsDeleted == false);
            List<CourseViewModel> viewModel = new List<CourseViewModel>();

            foreach (var item in outCourse)
            {
                var courseDocumentList = UnitOfWork.CourseAttachmentRepository.Get(ch => ch.courseId == item.Id);
                foreach (var courseDocument in courseDocumentList)
                {
                    var document = UnitOfWork.CourseDocumentRepository.Get(ch => ch.Id == courseDocument.courseDocumentID && ch.Scope == DocumentScope.freeDocument).FirstOrDefault();
                    if (document != null)
                    {
                        string base64String = Convert.ToBase64String(document.FileData, 0, document.FileData.Length);
                        viewModel.Add(new CourseViewModel()
                        {
                            CourseID = item.Id,
                            CourseName = item.CourseName,
                            CourseFreeContent = item.CourseFreeContent,
                            StartDate = item.StartDate,
                            EndDate = item.EndDate,
                            Price = item.Price,
                            CurrencyType = item.CurrencyType,
                            ImageURL = String.Format("data:{0};base64,{1}", document.FileType, base64String)
                        });
                    }
                }
            }


            return this.Request.CreateResponse(HttpStatusCode.OK, new { Course = viewModel });
        }

        [Route("GetAllPaidCourse")]
        public HttpResponseMessage GetAllPaidCourse()
        {
            var outCourse = from documentCourse in UnitOfWork.CourseRepository.Get()
                            select new
                            {
                                Id = documentCourse.Id,
                                Name = documentCourse.CourseName,
                                Content = documentCourse.CourseFreeContent,
                                Price = documentCourse.Price.ToString(),
                                Currency = documentCourse.CurrencyType.ToString(),
                                StartDate = documentCourse.StartDate,
                                EndDate = documentCourse.EndDate
                            };


            return this.Request.CreateResponse(HttpStatusCode.OK, new { Course = outCourse });
        }

        [Route("GetAllCoursebyId")]
        public HttpResponseMessage GetAllCoursebyId(Int64 id)
        {

            var documentCourse = UnitOfWork.CourseRepository.GetById(id);

            return this.Request.CreateResponse(HttpStatusCode.OK, new { Course = documentCourse });

        }

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
        public HttpResponseMessage GetFreeCourseById(Int64 id)
        {
            var documentCourse = UnitOfWork.CourseRepository.GetById(id);
            var outCourse = new
            {
                Id = documentCourse.Id,
                Name = documentCourse.CourseName,
                Content = documentCourse.CourseFreeContent,
                Price = documentCourse.Price.ToString(),
                Currency = documentCourse.CurrencyType.ToString(),
                StartDate = documentCourse.StartDate,
                EndDate = documentCourse.EndDate
            };


            return this.Request.CreateResponse(HttpStatusCode.OK, new { Course = outCourse });
        }

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

        [Route("Post")]
        public HttpResponseMessage Post(CourseViewModel viewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest);

                Course model = new Course()
                {
                    CourseFreeContent = viewModel.CourseFreeContent,
                    CourseName = viewModel.CourseName,
                    CoursePaidContent = viewModel.CoursePaidContent,
                    CoursePublicContent = viewModel.CoursePublicContent,
                    Price = viewModel.Price,
                    StartDate = viewModel.StartDate,
                    EndDate = viewModel.EndDate,
                    CurrencyType = viewModel.CurrencyType
                };
                UnitOfWork.CourseRepository.Insert(model);
                UnitOfWork.SaveChange();

                UnitOfWork.CourseAttachmentRepository.Insert(new CourseAttachmentMapping() { courseId = model.Id, courseDocumentID = viewModel.FreeContentImageId });
                UnitOfWork.CourseAttachmentRepository.Insert(new CourseAttachmentMapping() { courseId = model.Id, courseDocumentID = viewModel.PublicContentImageId });
                UnitOfWork.CourseAttachmentRepository.Insert(new CourseAttachmentMapping() { courseId = model.Id, courseDocumentID = viewModel.PaidContentImageId });
                UnitOfWork.SaveChange();

                return this.Request.CreateResponse(HttpStatusCode.Created, model);
            }
            catch (Exception ex)
            {
                return this.Request.CreateResponse(HttpStatusCode.BadRequest, ex);
            }
        }

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

        [Route("Delete")]
        public HttpResponseMessage Delete(int id)
        {
            UnitOfWork.CourseRepository.SoftDelete(id);
            UnitOfWork.SaveChange();

            return this.Request.CreateResponse(HttpStatusCode.OK);
        }
        public HttpResponseMessage PostUploadAttachment(DocumentScope scope)
        {
            HttpContext context = HttpContext.Current;
            List<CourseDocument> attachedFiles = new List<CourseDocument>();
            try
            {
                string erroredAtchmnts = String.Empty;

                //Loop through the multiple files.
                for (int i = 0; i < context.Request.Files.Count; i++)
                {
                    var file = context.Request.Files.Get(i);

                    try
                    {
                        CourseDocument fileAttachment = new CourseDocument()
                        {
                            //Path = savePath,
                            FileSize = file.ContentLength,
                            FileType = file.ContentType,
                            DocumentName = file.FileName,
                            Scope = scope
                        };

                        using (var binaryReader = new BinaryReader(file.InputStream))
                        {
                            fileAttachment.FileData = binaryReader.ReadBytes(file.ContentLength);
                        }

                        UnitOfWork.CourseDocumentRepository.Insert(fileAttachment);
                        UnitOfWork.SaveChange();
                        attachedFiles.Add(fileAttachment);
                    }
                    catch (Exception)
                    {
                        if (String.IsNullOrEmpty(erroredAtchmnts)) { erroredAtchmnts = "Attachments that could not be saved due to some error: " + file.FileName; }
                        else { erroredAtchmnts += ", " + file.FileName; }
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, new { erroredAttachments = erroredAtchmnts, attachedFiles = attachedFiles });
            }
            catch (UnauthorizedAccessException uex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, uex);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }
    }
}
