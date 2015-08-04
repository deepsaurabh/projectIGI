using Data;
using Data.Dto;
using Data.Enum;
using Enterprise.Repository;
using Entity.POCO;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace API.Controllers
{
    /// <summary>
    /// basic controller to register customer and admin
    /// We will remove admin in future
    /// register the user with "RegisterCustomer"
    /// To login Please hit the url "http://localhost:51473/token" with data like "grant_type=password&username=testuser&password=123456789" with content type "application/x-www-form-urlencoded"
    /// </summary>
    [RoutePrefix("api/Account")]
    public class AccountController : BaseController
    {
        private AuthRepository _repo = null;
        public AccountController()
        {
            _repo = new AuthRepository();
        }


        //c
        //        {
        //    "UserName": "testuser",
        //    "Password": "123456789",
        //    "ConfirmPassword": "123456789",
        //    "EmailAddress": "test@test.test",
        //    "PhoneNumber": "09911270918",
        //    "FirstName": "test",
        //    "LastName": "user",
        //    "Gender": "1",
        //    "DateOfBirth": "2015-08-01"
        //}
        // POST api/Account/RegisterCustomer
        [HttpPost]
        [AllowAnonymous]
        [Route("RegisterCustomer")]
        public async Task<IHttpActionResult> RegisterCustomer(UserModel userModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = new IdentityResult();
            try
            {
                result = await _repo.RegisterCustomerAsync(userModel);

                var user = new User() 
                {
                   DateOfBirth = userModel.DateOfBirth,
                   FirstName = userModel.FirstName,
                   LastName = userModel.LastName,
                   Gender = (userModel.Gender == 1) ? Gender.male : Gender.female
                };
                UnitOfWork.UserRepository.Insert(user);
                UnitOfWork.SaveChange();
            }
            catch (Exception)
            {
                IHttpActionResult errorResult = GetErrorResult(result);

                if (errorResult != null)
                {
                    return errorResult;
                }
            }

            return Ok();
        }


        //http://localhost:51473/api/Account/RegisterAdmin
//        {
//    "UserName": "testuser",
//    "Password": "123456789",
//    "ConfirmPassword": "123456789",
//    "EmailAddress": "test@test.test",
//    "PhoneNumber": "09911270918",
//    "FirstName": "test",
//    "LastName": "user",
//    "Gender": "1",
//    "DateOfBirth": "2015-08-01"
//}
        // POST api/Account/RegisterAdmin
        [HttpPost]
        [AllowAnonymous]
        [Route("RegisterAdmin")]
        public async Task<IHttpActionResult> RegisterAdmin(UserModel userModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = new IdentityResult();
            try
            {
                result = await _repo.RegisterCustomerAsync(userModel, AppRole.admin);

                var user = new User()
                {
                    //DateOfBirth = userModel.DateOfBirth,
                    FirstName = userModel.FirstName,
                    LastName = userModel.LastName,
                    Gender = (userModel.Gender == 1) ? Gender.male : Gender.female
                };
                UnitOfWork.UserRepository.Insert(user);
                UnitOfWork.SaveChange();
            }
            catch (Exception)
            {
                IHttpActionResult errorResult = GetErrorResult(result);

                if (errorResult != null)
                {
                    return errorResult;
                }
            }

            return Ok();
        }

        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _repo.Dispose();
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// GetErrorResult
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }
    }
}
