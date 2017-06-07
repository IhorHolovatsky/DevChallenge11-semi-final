using NewsMonitoringSystem.API.Models;
using NewsMonitoringSystem.Data.Entities.Generated;
using NewsMonitoringSystem.Managers.Intefaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace NewsMonitoringSystem.API.Controllers
{
    [RoutePrefix("Document")]
    public class DocumentController : ApiControllerBase
    {
        private IDocumentManager _documentManager;

        public DocumentController(IDocumentManager documentManager)
        {
            _documentManager = documentManager;
        }
        
        [HttpGet]
        [Route("GetDocumentByIdRecentRevision")]
        public ResponseWrapper<Document> GetDocumentByIdRecentRevision(Guid? documentId)
        {
            var result = new ResponseWrapper<Document>();

            if (!documentId.HasValue)
            {
                result.IsSuccess = false;
                result.ErrorMessage = "Document Id was not provided.";
                return result;
            }

            result.ResponseData = _documentManager.GetDocumentByIdRecentRevision(documentId.Value);
            result.IsSuccess = true;

            return result;
        }

        [HttpGet]
        [Route("GetDocumentRevision")]
        public ResponseWrapper<Document> GetDocumentRevision(Guid? documentId, int? revision)
        {
            var result = new ResponseWrapper<Document>();

            if (!documentId.HasValue
                && !revision.HasValue)
            {
                result.IsSuccess = false;
                result.ErrorMessage = "Document Id or revision was not provided.";
                return result;
            }

            result.ResponseData = _documentManager.GetDocumentByIdAndRevision(documentId.Value, revision.Value);
            result.IsSuccess = true;

            return result;
        }

        [HttpGet]
        [Route("GetDocumentHistory")]
        public ResponseWrapper<IEnumerable<Document>> GetDocumentHistory(Guid? documentId)
        {
            var result = new ResponseWrapper<IEnumerable<Document>>();

            if (!documentId.HasValue
                || documentId == Guid.Empty)
            {
                result.IsSuccess = false;
                result.ErrorMessage = "Document Id was not provided.";
                return result;
            }

            result.ResponseData = _documentManager.GetDocumentsById(documentId.Value)
                                                  .OrderByDescending(d => d.Revision)
                                                  .ToList();
            result.IsSuccess = true;

            return result;
        }

        [HttpGet]
        [Route("GetDocuments")]
        public ResponseWrapper<IEnumerable<Document>> GetDocuments(DateTime? publishStartDate, DateTime? publishEndDate)
        {
            var result = new ResponseWrapper<IEnumerable<Document>>();
            var documents = Enumerable.Empty<Document>();

            if (publishStartDate.HasValue
                && publishEndDate.HasValue)
            {
                documents = _documentManager.GetDocumentsByDate(publishStartDate.Value, publishEndDate.Value);
            }
            else if (publishStartDate.HasValue)
            {
                documents = _documentManager.GetDocumentsByDate(publishStartDate.Value, DateTime.Now);
            }
            else
            {
                result.ErrorMessage = "Start Date was not provided.";
                result.IsSuccess = false;
                return result;
            }

            result.ResponseData = documents;
            result.IsSuccess = true;

            return result;
        }

        [HttpGet]
        [Route("GetDocumentByIdentifier")]
        public ResponseWrapper<IEnumerable<Document>> GetDocumentByIdentifier(string documentIdentifier)
        {
            var result = new ResponseWrapper<IEnumerable<Document>>();

            if (string.IsNullOrEmpty(documentIdentifier))
            {
                result.ErrorMessage = "Document Identifier was not provided.";
                result.IsSuccess = false;
                return result;
            }

            result.ResponseData = _documentManager.GetDocumentsByIdentifier(documentIdentifier);
            result.IsSuccess = true;

            return result;
        }
    }
}
