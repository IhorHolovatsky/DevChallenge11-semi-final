using NewsMonitoringSystem.API.Filters;
using NewsMonitoringSystem.API.Models;
using NewsMonitoringSystem.Managers.Intefaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace NewsMonitoringSystem.API.Controllers
{
    [RoutePrefix("Admin")]
    public class AdminController : ApiController
    {
        private IDocumentImportManager _documentImportManager;

        public AdminController(IDocumentImportManager documentImportManager)
        {
            _documentImportManager = documentImportManager;
        }

        [Route("SyncDocuments")]
        [ValidateAdminToken]
        [HttpGet]
        public ResponseWrapper<bool> SyncDocuments(DateTime? publishStartDate, DateTime? publishEndDate)
        {
            var result = new ResponseWrapper<bool>();

            try
            {
                if (publishStartDate.HasValue
                    && publishEndDate.HasValue)
                {
                    _documentImportManager.SyncAllDocuments(publishStartDate.Value.Date, 
                                                            publishEndDate.Value.Date);
                }
                else if (publishStartDate.HasValue)
                {
                    _documentImportManager.SyncAllDocuments(publishStartDate.Value, 
                                                            DateTime.Now);
                }
                else
                {
                    _documentImportManager.SyncAllDocuments();
                }

                result.IsSuccess = true;
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.Message;
                result.IsSuccess = false;
            }           


            return result;
        }
    }
}
