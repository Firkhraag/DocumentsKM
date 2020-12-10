using DocumentsKM.Helpers;
using DocumentsKM.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace DocumentsKM.Data
{
    public class NoSqlMarkGeneralDataRepo : IMarkGeneralDataRepo
    {
        private readonly IMongoCollection<MarkGeneralData> _markGeneralData;

        public NoSqlMarkGeneralDataRepo(IMongoSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _markGeneralData = database.GetCollection<MarkGeneralData>(settings.CollectionName);
        }

        public MarkGeneralData GetById(string id) =>
            _markGeneralData.Find<MarkGeneralData>(v => v.Id == id).FirstOrDefault();

        public MarkGeneralData GetByMarkId(int markId) =>
            _markGeneralData.Find<MarkGeneralData>(v => v.MarkId == markId).FirstOrDefault();

        public MarkGeneralData Add(MarkGeneralData markGeneralData)
        {
            _markGeneralData.InsertOne(markGeneralData);
            return markGeneralData;
        }

        public void Update(MarkGeneralData markGeneralDataIn) =>
            _markGeneralData.ReplaceOne(v => v.Id == markGeneralDataIn.Id, markGeneralDataIn);

        public void Delete(MarkGeneralData markGeneralDataIn) =>
            _markGeneralData.DeleteOne(v => v.Id == markGeneralDataIn.Id);
    }
}