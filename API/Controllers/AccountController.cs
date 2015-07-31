using Data;
using Data.Dto;
using Data.Enum;
using Enterprise.Repository;
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
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private AuthRepository _repo = null;
        public AccountController()
        {
            _repo = new AuthRepository();
        }

        // POST api/Account/Register
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


        // POST api/Account/Register
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
