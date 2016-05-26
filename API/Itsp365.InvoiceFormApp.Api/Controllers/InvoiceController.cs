using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Itsp365.InvoiceFormApp.Api.Models.Entities;
using Itsp365.InvoiceFormApp.Api.Repositories;

namespace Itsp365.InvoiceFormApp.Api.Controllers
{
    [Authorize()]
    public class InvoiceController : ApiController
    {
        private InvoiceRepository _invoiceRepository = null;

        public InvoiceController()
            : base()
        {

        }

        private async Task Initialise()
        {
            _invoiceRepository = await InvoiceRepository.GetCurrent();
        }

        public async Task<IList<InvoiceForm>> Get()
        {
            IList<InvoiceForm> listOfForms = new List<InvoiceForm>();
            try
            {
                await Initialise();
                listOfForms = _invoiceRepository.GetAll();
            }
            catch (Exception ex)
            {
                throw;
            }

            return listOfForms;

        }

        [HttpGet]
        // GET api/values/5
        public async Task<InvoiceForm> Get(string reference)
        {
            InvoiceForm invoiceForm = new InvoiceNotFoundForm();
            try
            {
                await Initialise();
                invoiceForm = _invoiceRepository.Get(reference);

            }
            catch (Exception ex)
            {
                throw;
            }

            return invoiceForm;
        }

        [HttpPost]
        [Route("api/invoice/add")]
        // POST api/values
        public async Task Post([FromBody]InvoiceForm invoice)
        {
            try
            {
                await Initialise();
                await _invoiceRepository.Add(invoice);
            }
            catch (Exception ex)
            {
                throw;
            }

        }


        [HttpPut]
        // PUT api/values/5
        public async Task Put(string reference, [FromBody]InvoiceForm invoice)
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        // DELETE api/values/5
        public async Task Delete(int reference)
        {
            try
            {
                await Initialise();
                await _invoiceRepository.Remove(reference);
            }
            catch (Exception ex)
            {
                throw;
            }

        }

    }

}