using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using backendTest.Models;
using backendTest.Storage;

namespace backendTest.Services
{
    public interface IDocumentService
    {
        Task<string> ProcessAndStoreDocumentAsync(IFormFile file);
        Document GetCurrentDocument();
        Document GetDocumentById(string documentId);
        List<Document> GetDocumentsByUserId(string userId, string documentNumber = null);
        List<Document> GetDocumentsByContractNumber(string contractNumber, string documentNumber = null);
        List<Document> GetDocumentsByUserAndContract(string userId, string contractNumber);
        int DeleteDocumentsByContractNumber(string contractNumber, string documentNumber = null);
    }

    public class DocumentService : IDocumentService
    {
        private readonly IDocumentStorage documentStorage;
        private Document currentDocument;

        public DocumentService(IDocumentStorage documentStorage)
        {
            this.documentStorage = documentStorage;
            currentDocument = null;
        }

        public async Task<string> ProcessAndStoreDocumentAsync(IFormFile file)
        {
            var document = await DocumentProcessor.ProcessAsync(file);

            if (documentStorage.Contains(document.DocumentName))
                documentStorage.Update(document.DocumentName, document);
            else
                documentStorage.Add(document.DocumentName, document);

            currentDocument = document;
            return document.Id;
        }

        public Document GetCurrentDocument()
        {
            return currentDocument;
        }

        public Document GetDocumentById(string documentId)
        {
            return documentStorage.GetDocumentById(documentId);
        }

        public List<Document> GetDocumentsByUserId(string userId, string documentNumber = null)
        {
            return documentStorage.GetDocumentsByUserId(userId, documentNumber);
        }

        public List<Document> GetDocumentsByContractNumber(string contractNumber, string documentNumber = null)
        {
            return documentStorage.GetDocumentsByContractNumber(contractNumber, documentNumber);
        }

        public List<Document> GetDocumentsByUserAndContract(string userId, string contractNumber)
        {
            return documentStorage.GetDocumentsByUserAndContract(userId, contractNumber);
        }

        public int DeleteDocumentsByContractNumber(string contractNumber, string documentNumber = null)
        {
            return documentStorage.DeleteDocumentsByContractNumber(contractNumber, documentNumber);
        }
    }

}