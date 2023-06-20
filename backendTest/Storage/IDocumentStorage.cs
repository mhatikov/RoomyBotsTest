using System.Collections.Generic;
using System.Linq;
using backendTest.Models;

namespace backendTest.Storage
{
    public interface IDocumentStorage
    {
        void Add(string documentName, Document document);
        void Update(string documentName, Document document);
        bool Contains(string documentName);
        Document GetDocumentById(string documentId);
        List<Document> GetDocumentsByUserId(string userId, string documentNumber = null);
        List<Document> GetDocumentsByContractNumber(string contractNumber, string documentNumber = null);
        List<Document> GetDocumentsByUserAndContract(string userId, string contractNumber);
        int DeleteDocumentsByContractNumber(string contractNumber, string documentNumber = null);
    }

    public class InMemoryDocumentStorage : IDocumentStorage
    {
        private readonly IDictionary<string, Document> documentStorage;

        public InMemoryDocumentStorage()
        {
            documentStorage = new Dictionary<string, Document>();
        }

        public void Add(string documentName, Document document)
        {
            documentStorage.Add(documentName, document);
        }

        public void Update(string documentName, Document document)
        {
            documentStorage[documentName] = document;
        }

        public bool Contains(string documentName)
        {
            return documentStorage.ContainsKey(documentName);
        }

        public Document GetDocumentById(string documentId)
        {
            return documentStorage.Values.FirstOrDefault(doc => doc.Id == documentId);
        }

        public List<Document> GetDocumentsByUserId(string userId, string documentNumber = null)
        {
            return documentStorage.Values
                .Where(doc => doc.UserId == userId && (documentNumber == null || doc.ContractNumber == documentNumber))
                .ToList();
        }

        public List<Document> GetDocumentsByContractNumber(string contractNumber, string documentNumber = null)
        {
            return documentStorage.Values
                .Where(doc => doc.ContractNumber == contractNumber && (documentNumber == null || doc.ContractNumber == documentNumber))
                .ToList();
        }

        public List<Document> GetDocumentsByUserAndContract(string userId, string contractNumber)
        {
            return documentStorage.Values
                .Where(doc => doc.UserId == userId && doc.ContractNumber == contractNumber)
                .ToList();
        }

        public int DeleteDocumentsByContractNumber(string contractNumber, string documentNumber = null)
        {
            var documentsToRemove = documentStorage.Values
                .Where(doc => doc.ContractNumber == contractNumber && (documentNumber == null || doc.ContractNumber == documentNumber))
                .ToList();

            foreach (var doc in documentsToRemove)
            {
                documentStorage.Remove(doc.ContractNumber);
            }

            return documentsToRemove.Count;
        }
    }
}