using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Container = Microsoft.Azure.Cosmos.Container;

namespace FitnessTracker.Infrastructure
{
    public class CosmosDbService
    {
        private readonly Container _container;
        public CosmosDbService(CosmosClient client)
        {
            _container = client.GetContainer("FitnessDb", "Exercises");
        }

        public Task UpsertAsync(string containerName, string id, object doc)
        {
            return _container.UpsertItemAsync(doc, new PartitionKey(id));
        }
    }
}
