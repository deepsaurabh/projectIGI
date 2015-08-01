using Entity.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
                         select new {
                             Id =documentCourse.Id,
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
        public HttpResponseMessage Post(Course model)
        {
            if (!ModelState.IsValid)
                return this.Request.CreateResponse(HttpStatusCode.BadRequest);
            
            model.CurrencyType = Currency.INR;
            UnitOfWork.CourseRepository.Insert(model);
            UnitOfWork.SaveChange();
            return this.Request.CreateResponse(HttpStatusCode.Created);
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
                course.CourseDocument = null;
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

    }
}
