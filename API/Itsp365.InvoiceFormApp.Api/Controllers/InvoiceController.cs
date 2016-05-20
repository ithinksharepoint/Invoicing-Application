using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Itsp365.InvoiceFormApp.Api.Models.Entities;
using Itsp365.InvoiceFormApp.Api.Repositories;

namespace Itsp365.InvoiceFormApp.Api.Controllers
{
    [Authorize()]
    public class InvoiceController : ApiController
    {
        private InvoiceRepository _invoiceRepository = InvoiceRepository.GetCurrent();


        // GET api/values
        public IEnumerable<InvoiceForm> Get()
        {
            return _invoiceRepository.GetAll();
        }

        // GET api/values/5
        public InvoiceForm Get(int id)
        {
            var invoiceForm = Get().FirstOrDefault(i => i.Id == id);
            if (invoiceForm == null)
            {
                invoiceForm = new InvoiceNotFoundForm();
            }
            return invoiceForm;
        }

        [HttpGet]
        // GET api/values/5
        public InvoiceForm Get(string reference)
        {
            var invoiceForm = Get().FirstOrDefault(i => i.Reference == reference);
            if (invoiceForm == null)
            {
                invoiceForm = new InvoiceNotFoundForm();
            }
            return invoiceForm;
        }

        [HttpPost]
        [Route("api/invoice/add")]
        // POST api/values
        public void Post([FromBody]InvoiceForm invoice)
        {
            _invoiceRepository.Add(invoice);
        }

        [HttpPut]
        // PUT api/values/5
        public void Put(string reference, [FromBody]InvoiceForm invoice)
        {
            _invoiceRepository.Update(invoice);
        }

        [HttpDelete]
        // DELETE api/values/5
        public void Delete(int id)
        {
            _invoiceRepository.Remove(id);
        }

        public HttpResponseMessage DownloadFile()
        {
            var responseMessage = new HttpResponseMessage();
            var localFilePath = System.Web.HttpContext.Current.Server.MapPath("~/test.pdf");
            if (!System.IO.File.Exists(localFilePath))
            {
                responseMessage = Request.CreateResponse(HttpStatusCode.Gone);
            }
            else
            {
                responseMessage = Request.CreateResponse(HttpStatusCode.OK);
                responseMessage.Content = new StreamContent(new FileStream(localFilePath, FileMode.Open, FileAccess.Read));
                responseMessage.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                responseMessage.Content.Headers.ContentDisposition.FileName = System.IO.Path.GetFileName(localFilePath);
            }
            return responseMessage;
        }
    }


}