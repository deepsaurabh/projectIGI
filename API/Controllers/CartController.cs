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
    [RoutePrefix("api/Cart")]
    public class CartController : BaseController
    {

        [AuthorizeAppRole(AppRole.customer)]
        [Route("GetMyCart")]
        public HttpResponseMessage GetMyCart(string userName)
        {
            var outCart = UnitOfWork.CartRepository.Get(ct => !ct.IsDeleted && ct.UserName.Equals(userName));

            var outList = new List<MyCartModel>();
            foreach (var ocart in outCart)
            {
                if (ocart.type.Equals("toolkit", StringComparison.OrdinalIgnoreCase))
                {
                    //get toolkit
                    var toolkit = UnitOfWork.ToolkitRepository.GetById(ocart.ItemId);
                    if (toolkit != null)
                    {
                        var itemtoAdd = new MyCartModel()
                        {
                            id = ocart.Id,
                            name = toolkit.ToolkitName,
                            price = toolkit.Price,
                            quantity = ocart.quantity.ToString(),
                            type = ocart.type
                        };

                        outList.Add(itemtoAdd);
                    }

                }
                else
                {
                    //get Course

                    var course = UnitOfWork.CourseRepository.GetById(ocart.ItemId);
                    if (course != null)
                    {
                        var itemtoAdd = new MyCartModel()
                        {
                            id = ocart.Id,
                            name = course.CourseName,
                            price = course.Price,
                            quantity = ocart.quantity.ToString(),
                            type = ocart.type
                        };

                        outList.Add(itemtoAdd);
                    }
                }
            }

            return this.Request.CreateResponse(HttpStatusCode.OK, new { cart = outList });
        }
        [AuthorizeAppRole(AppRole.customer)]
        [Route("Post")]
        public HttpResponseMessage Post(Cart viewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest);

               
                if (viewModel.Id > 0)
                {
                    var model = UnitOfWork.CartRepository.GetById(viewModel.Id);
                    model.UpdatedDate = DateTime.UtcNow;
                    model.quantity = viewModel.quantity;
                    UnitOfWork.CartRepository.Update(model);
                }
                else
                {
                    viewModel.CreatedDate = DateTime.UtcNow;
                    UnitOfWork.CartRepository.Insert(viewModel);
                }
                UnitOfWork.SaveChange();
                
                return this.Request.CreateResponse(HttpStatusCode.OK);
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

        [AuthorizeAppRole(AppRole.customer)]
        [Route("Delete")]
        public HttpResponseMessage Delete(int id)
        {
            UnitOfWork.CartRepository.SoftDelete(id);
            UnitOfWork.SaveChange();

            return this.Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
