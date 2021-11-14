using BlueValueAssessment.Core.Configs;
using BlueValueAssessment.Core.Documents;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlueValueAssessment.Data
{
    public class MongoDbContext
    {
        private const string DATABASE_NAME = "movies";
        private const string REQUEST_COLLECTION = "requests";
        private readonly IMongoDatabase _mongoDatabase;
        private readonly string _conn;

        public MongoDbContext(string conn)
        {
            _conn = conn;
            _mongoDatabase = GetDatabase();
           
        }

        public IMongoCollection<RequestDocument> Requests
        {
            get
            {
                return _mongoDatabase.GetCollection<RequestDocument>(REQUEST_COLLECTION);
            }
        }

        public IMongoDatabase GetDatabase()
        {
            MongoClient mongoClient = new MongoClient(_conn);

            BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
            BsonDefaults.GuidRepresentationMode = GuidRepresentationMode.V3;

            return mongoClient.GetDatabase(DATABASE_NAME);
        }
    }
}
