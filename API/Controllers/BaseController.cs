using Core;
using Enterprise;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API.Controllers
{
    public class BaseController : ApiController
    {
        //Create object for Unit of Work
        protected IUnitOfWork UnitOfWork = new UnitOfWork();

        /// <summary>
        /// To dispose
        /// </summary>
        /// <param name="disposing">Get the bool as input</param>
        protected override void Dispose(bool disposing)
        {
            UnitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}
