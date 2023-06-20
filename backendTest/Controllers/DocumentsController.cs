using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using backendTest.Models;
using backendTest.Services;

namespace backendTest.Controllers
{
    [Route("api/documents")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        private readonly IDocumentService documentService;

        public DocumentsController(IDocumentService documentService)
        {
            this.documentService = documentService;
        }

        [HttpPost]
        public async Task<ActionResult<string>> UploadDocument([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file provided.");

            var documentId = await documentService.ProcessAndStoreDocumentAsync(file);
            return Ok(documentId);
        }

        [HttpGet("current")]
        public ActionResult<Document> GetCurrentDocument()
        {
            var currentDocument = documentService.GetCurrentDocument();
            if (currentDocument == null)
                return NotFound("No document is currently selected.");

            return Ok(currentDocument);
        }

        [HttpGet("documents/{documentId}")]
        public ActionResult<Document> GetDocumentById(string documentId)
        {
            var document = documentService.GetDocumentById(documentId);
            if (document == null)
                return NotFound("Document not found.");

            return Ok(document);
        }

        [HttpGet("users/{userId}")]
        public ActionResult<IEnumerable<Document>> GetDocumentsByUserId(string userId, string documentNumber = null)
        {
            var documents = documentService.GetDocumentsByUserId(userId, documentNumber);
            if (documents.Count == 0)
                return NotFound("No documents found for the specified user and document number.");

            return Ok(documents);
        }

        [HttpGet("contracts/{contractNumber}")]
        public ActionResult<IEnumerable<Document>> GetDocumentsByContractNumber(string contractNumber, string documentNumber = null)
        {
            var documents = documentService.GetDocumentsByContractNumber(contractNumber, documentNumber);
            if (documents.Count == 0)
                return NotFound("No documents found for the specified contract and document number.");

            return Ok(documents);
        }

        [HttpGet("session/{userId}/{contractNumber}")]
        public ActionResult<IEnumerable<Document>> GetDocumentsByUserAndContract(string userId, string contractNumber)
        {
            var documents = documentService.GetDocumentsByUserAndContract(userId, contractNumber);
            if (documents.Count == 0)
                return NotFound("No documents found for the specified user and contract number.");

            return Ok(documents);
        }

        [HttpDelete("contracts/{contractNumber}")]
        public ActionResult<string> DeleteDocumentsByContractNumber(string contractNumber, string documentNumber = null)
        {
            var deleteCount = documentService.DeleteDocumentsByContractNumber(contractNumber, documentNumber);
            if (deleteCount == 0)
                return NotFound("No documents found for the specified contract and document number.");

            return Ok($"Deleted {deleteCount} document(s).");
        }
    }
}
