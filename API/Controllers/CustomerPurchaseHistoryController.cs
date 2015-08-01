using Entity.POCO;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API.Controllers
{
    [RoutePrefix("api/CustomerPurchaseHistory")]
    public class CustomerPurchaseHistoryController : BaseController
    {

        [Route("GetAllPurchaseList")]
        public HttpResponseMessage GetAllPurchasedCourseList(Int64 userId)
        {
            var outCourse = from course in UnitOfWork.CourseRepository.Get()
                            join purchaseHistory in UnitOfWork.UserPurchaseHistoryRepository.Get(m => m.ItemType == ItemType.Course && m.UserId == userId)
                            on course.Id equals purchaseHistory.ItemId
                            select new
                            {
                                Id = course.Id,
                                Name = course.CourseName,
                                Price = course.Price.ToString(),
                                Currency = course.CurrencyType.ToString(),
                            };

            var outToolkit = from toolkit in UnitOfWork.ToolkitRepository.Get()
                             join purchaseHistory in UnitOfWork.UserPurchaseHistoryRepository.Get(m => m.ItemType == ItemType.Toolkit && m.UserId == userId)
                            on toolkit.Id equals purchaseHistory.ItemId
                            select new
                            {
                                Id = toolkit.Id,
                                Name = toolkit.ToolkitName,
                                Price = toolkit.Price.ToString(),
                                Currency = toolkit.CurrencyType.ToString(),
                            };

            var outData = outCourse.Concat(outToolkit);
            return this.Request.CreateResponse(HttpStatusCode.OK, new { Course = outData });
        }


        [Route("GetPurchasedItemdetail")]
        public HttpResponseMessage GetPurchasedItemdetail(Int64 userId, Int64 itemId, ItemType itemType)
        {

            if (itemType == ItemType.Course)
            {
                var outCourse = from course in UnitOfWork.CourseRepository.Get()
                                join purchaseHistory in UnitOfWork.UserPurchaseHistoryRepository.Get(m => m.ItemType == ItemType.Course && m.UserId == userId 
                                                && m.ItemId == itemId)
                                on course.Id equals purchaseHistory.ItemId
                                select new
                                {
                                    Id = course.Id,
                                    Name = course.CourseName,
                                    Price = course.Price.ToString(),
                                    Currency = course.CurrencyType.ToString(),
                                    StartDate = course.StartDate,
                                    EndDate = course.EndDate,
                                    ItemCount = purchaseHistory.ItemCount,
                                    AmountPaid = purchaseHistory.AmountPaid,
                                    TransactionDate = purchaseHistory.TransactionDate
                                };
                return this.Request.CreateResponse(HttpStatusCode.OK, new { data = outCourse });
            }          
       
            var outToolkit = from toolkit in UnitOfWork.ToolkitRepository.Get()
                             join purchaseHistory in UnitOfWork.UserPurchaseHistoryRepository.Get(m => m.ItemType == ItemType.Toolkit && m.UserId == userId
                                                && m.ItemId == itemId)
                            on toolkit.Id equals purchaseHistory.ItemId
                             select new
                             {
                                 Id = toolkit.Id,
                                 Name = toolkit.ToolkitName,
                                 Price = toolkit.Price.ToString(),
                                 Currency = toolkit.CurrencyType.ToString(),
                                 ItemCount = purchaseHistory.ItemCount,
                                 AmountPaid = purchaseHistory.AmountPaid,
                                 TransactionDate = purchaseHistory.TransactionDate
                             };

            return this.Request.CreateResponse(HttpStatusCode.OK, new { data = outToolkit });
        }



    }
}
