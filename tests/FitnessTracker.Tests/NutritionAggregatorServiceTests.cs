using System.Net;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using FitnessTracker.API.Services;
using FitnessTracker.Core.Models;
using Microsoft.Extensions.Configuration;
using Moq;

namespace FitnessTracker.Tests;

public class NutritionAggregatorServiceTests
{
    private IConfiguration BuildConfig() => new ConfigurationBuilder()
        .AddInMemoryCollection(new Dictionary<string, string?>
        {
            {"NutritionApis:NutritionixAppId", "id1"},
            {"NutritionApis:NutritionixAppKey", "key1"},
            {"NutritionApis:EdamamAppId", "id2"},
            {"NutritionApis:EdamamAppKey", "key2"}
        })
        .Build();

    private class HandlerStub : HttpMessageHandler
    {
        private readonly Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> _func;
        public HandlerStub(Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> func) => _func = func;
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) => _func(request, cancellationToken);
    }

    [Fact]
    public async Task Returns_primary_result_when_first_call_succeeds()
    {
        var entry = new FoodEntry { Protein = 20, Carbs = 10, Fat = 5 };
        var primaryHandler = new HandlerStub((req, ct) =>
        {
            var json = JsonConvert.SerializeObject(entry);
            var resp = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
            return Task.FromResult(resp);
        });

        var fallbackHandler = new HandlerStub((req, ct) => throw new Exception("fallback should not be called"));

        var factory = new Mock<IHttpClientFactory>();
        var call = 0;
        factory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(() =>
        {
            call++;
            return call == 1 ? new HttpClient(primaryHandler) : new HttpClient(fallbackHandler);
        });

        var svc = new NutritionAggregatorService(factory.Object, BuildConfig());
        var userId = Guid.NewGuid();

        var result = await svc.GetMacrosForBarcodeAsync("123", userId);

        Assert.Equal(userId, result.UserId);
        Assert.Equal(entry.Protein, result.Protein);
        Assert.Equal(entry.Carbs, result.Carbs);
        Assert.Equal(entry.Fat, result.Fat);
        var expectedCals = entry.Protein * 4 + entry.Carbs * 4 + entry.Fat * 9;
        Assert.Equal(expectedCals, result.Calories);
    }

    [Fact]
    public async Task Fallback_called_when_primary_fails()
    {
        var primaryHandler = new HandlerStub((req, ct) =>
            Task.FromResult(new HttpResponseMessage(HttpStatusCode.InternalServerError)));

        var fallbackEntry = new FoodEntry { Protein = 4, Carbs = 5, Fat = 1 };
        bool fallbackCalled = false;
        var fallbackHandler = new HandlerStub((req, ct) =>
        {
            fallbackCalled = true;
            var json = JsonConvert.SerializeObject(fallbackEntry);
            var resp = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
            return Task.FromResult(resp);
        });

        var factory = new Mock<IHttpClientFactory>();
        var call = 0;
        factory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(() =>
        {
            call++;
            return call == 1 ? new HttpClient(primaryHandler) : new HttpClient(fallbackHandler);
        });

        var svc = new NutritionAggregatorService(factory.Object, BuildConfig());
        var userId = Guid.NewGuid();

        var result = await svc.GetMacrosForBarcodeAsync("123", userId);

        Assert.True(fallbackCalled);
        Assert.Equal(userId, result.UserId);
        Assert.Equal(fallbackEntry.Protein, result.Protein);
        Assert.Equal(fallbackEntry.Carbs, result.Carbs);
        Assert.Equal(fallbackEntry.Fat, result.Fat);
    }
}
