using Data.Enum;
using Entity.POCO;
using System;
using System.Collections.Generic;
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
            var outToolkit = from documentToolkit in UnitOfWork.ToolkitRepository.Get()
                            select new
                            {
                                Id = documentToolkit.Id,
                                Name = documentToolkit.ToolkitName,
                                Price = documentToolkit.Price.ToString(),
                                Currency = documentToolkit.CurrencyType.ToString(),
                            };


            return this.Request.CreateResponse(HttpStatusCode.OK, new { Toolkit = outToolkit });
        }

        [Route("GetAllPublicToolkit")]
        public HttpResponseMessage GetAllPublicToolkit()
        {
            var outToolkit = from documentToolkit in UnitOfWork.ToolkitRepository.Get()
                         select new {
                             Id =documentToolkit.Id,
                             Name = documentToolkit.ToolkitName,
                             Content = documentToolkit.ToolkitPublicContent,
                             Price = documentToolkit.Price.ToString(),
                             Currency = documentToolkit.CurrencyType.ToString(),
                         };


            return this.Request.CreateResponse(HttpStatusCode.OK, new { Toolkit = outToolkit });
        }


        [Route("GetAllFreeToolkit")]
        public HttpResponseMessage GetAllFreeToolkit()
        {
            var outToolkit = from documentToolkit in UnitOfWork.ToolkitRepository.Get()
                            select new
                            {
                                Id = documentToolkit.Id,
                                Name = documentToolkit.ToolkitName,
                                Content = documentToolkit.ToolkitFreeContent,
                                Price = documentToolkit.Price.ToString(),
                                Currency = documentToolkit.CurrencyType.ToString(),
                            };


            return this.Request.CreateResponse(HttpStatusCode.OK, new { Toolkit = outToolkit });
        }

        [Route("GetAllPaidToolkit")]
        public HttpResponseMessage GetAllPaidToolkit()
        {
            var outToolkit = from documentToolkit in UnitOfWork.ToolkitRepository.Get()
                            select new
                            {
                                Id = documentToolkit.Id,
                                Name = documentToolkit.ToolkitName,
                                Content = documentToolkit.ToolkitFreeContent,
                                Price = documentToolkit.Price.ToString(),
                                Currency = documentToolkit.CurrencyType.ToString(),
                            };


            return this.Request.CreateResponse(HttpStatusCode.OK, new { Toolkit = outToolkit });
        }

        [Route("GetAllToolkitbyId")]
        public HttpResponseMessage GetAllToolkitbyId(Int64 id)
        {
            var documentToolkit = UnitOfWork.ToolkitRepository.GetById(id);
            return this.Request.CreateResponse(HttpStatusCode.OK, new { Toolkit = documentToolkit });

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

        [Route("Post")]
        public HttpResponseMessage Post(Toolkit model)
        {
            if (!ModelState.IsValid)
                return this.Request.CreateResponse(HttpStatusCode.BadRequest);

            model.CurrencyType = Currency.INR;
            UnitOfWork.ToolkitRepository.Insert(model);
            UnitOfWork.SaveChange();
            return this.Request.CreateResponse(HttpStatusCode.Created);
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
                Toolkit.ToolkitDocument = null;
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
