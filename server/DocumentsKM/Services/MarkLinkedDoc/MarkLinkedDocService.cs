using System.Collections.Generic;
using DocumentsKM.Models;
using DocumentsKM.Data;
using System;

namespace DocumentsKM.Services
{
    public class MarkLinkedDocService : IMarkLinkedDocService
    {
        private IMarkLinkedDocRepo _repository;
        private readonly IMarkRepo _markRepo;
        private readonly ILinkedDocRepo _linkedDocRepo;

        public MarkLinkedDocService(
            IMarkLinkedDocRepo markLinkedDocRepo,
            IMarkRepo markRepo,
            ILinkedDocRepo linkedDocRepo)
        {
            _repository = markLinkedDocRepo;
            _markRepo = markRepo;
            _linkedDocRepo = linkedDocRepo;
        }

        public IEnumerable<LinkedDoc> GetAllLinkedDocsByMarkId(int markId)
        {
            var markLinkedDocs = _repository.GetAllByMarkId(markId);
            var linkedDocs = new List<LinkedDoc>{};
            foreach (var mld in markLinkedDocs)
            {
                linkedDocs.Add(mld.LinkedDoc);
            }
            return linkedDocs;
        }

        public void Update(
            int markId,
            List<int> linkedDocIds)
        {
            var foundMark = _markRepo.GetById(markId);
            if (foundMark == null)
                throw new ArgumentNullException(nameof(foundMark));
            var linkedDocs = new List<LinkedDoc>{};

            foreach (var id in linkedDocIds)
            {
                var linkedDoc = _linkedDocRepo.GetById(id);
                if (linkedDoc == null)
                    throw new ArgumentNullException(nameof(linkedDoc));
                linkedDocs.Add(linkedDoc);
            }

            var markLinkedDocs = _repository.GetAllByMarkId(markId);
            var currentLinkedDocsIds = new List<int>{};
            foreach (var mld in markLinkedDocs)
            {
                if (!linkedDocIds.Contains(mld.LinkedDoc.Id))
                    _repository.Delete(mld);
                currentLinkedDocsIds.Add(mld.LinkedDoc.Id);
            }

            foreach (var (id, i) in linkedDocIds.WithIndex())
                if (!currentLinkedDocsIds.Contains(id))
                    _repository.Add(
                        new MarkLinkedDoc
                        {
                            Mark=foundMark,
                            LinkedDoc=linkedDocs[i],
                        });
        }
    }
}
