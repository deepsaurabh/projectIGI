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
    [RoutePrefix("api/CheckoutAddress")]
    public class CheckoutAddressController : BaseController
    {

        [AuthorizeAppRole(AppRole.customer)]
        [Route("GetMyAddress")]
        public HttpResponseMessage GetMyAddress(string userName)
        {
            var outAddress = UnitOfWork.CheckOutAddressRepository.Get(ct => !ct.IsDeleted && 
                            ct.UserName.Equals(userName)).FirstOrDefault();

            return this.Request.CreateResponse(HttpStatusCode.OK, new { address = outAddress });
        }
        [AuthorizeAppRole(AppRole.customer)]
        [Route("Post")]
        public HttpResponseMessage Post(CheckOutAddress viewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest);

               
                if (viewModel.Id > 0)
                {
                    var model = UnitOfWork.CheckOutAddressRepository.GetById(viewModel.Id);
                    model.UpdatedDate = DateTime.UtcNow;
                    model.Name = viewModel.Name;
                    model.CompleteAddress = viewModel.CompleteAddress;
                    model.City = viewModel.City;
                    model.PhoneNumber = viewModel.PhoneNumber;
                    model.PinCode = viewModel.PinCode;
                    model.State = viewModel.State;
                    model.EmailAddress = viewModel.EmailAddress;

                    UnitOfWork.CheckOutAddressRepository.Update(model);
                }
                else
                {
                    viewModel.CreatedDate = DateTime.UtcNow;
                    UnitOfWork.CheckOutAddressRepository.Insert(viewModel);
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
            UnitOfWork.CheckOutAddressRepository.SoftDelete(id);
            UnitOfWork.SaveChange();

            return this.Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
