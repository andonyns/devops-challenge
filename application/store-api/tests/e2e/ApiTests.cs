using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;

namespace PlaywrightTests
{
    [TestFixture]
    public class ApiTests : PlaywrightTest
    {
        private const string BaseUrl = "http://127.0.0.1:3000";
        private IAPIRequestContext? _requestContext;

        // Using a static field to share the created item's ID across test methods,
        // which are executed sequentially thanks to the [Order] attribute.
        private static int _createdItemId;

        [SetUp]
        public async Task SetUpAPITesting()
        {
            // Create a new API request context for each test.
            _requestContext = await Playwright.APIRequest.NewContextAsync(new()
            {
                BaseURL = BaseUrl
            });
        }

        [TearDown]
        public async Task TearDownAPITesting()
        {
            await _requestContext.DisposeAsync();
        }

        [Test, Order(1)]
        public async Task ShouldGetEmptyListOfItemsInitially()
        {
            var response = await _requestContext.GetAsync("/items");
            await Assertions.Expect(response).ToBeOKAsync();
            
            var json = await response.JsonAsync();
            Assert.That(json.Value.ToString(), Is.EqualTo("[]"));
        }

        [Test, Order(2)]
        public async Task ShouldCreateNewItem()
        {
            var itemData = "test1";
            var response = await _requestContext.PostAsync("/items", new()
            {
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } },
                Data = JsonSerializer.Serialize(itemData)
            });

            await Assertions.Expect(response).ToBeOKAsync();
            
            // Assuming the API returns the created item's name directly
            var responseText = await response.TextAsync();
            Assert.That(JsonSerializer.Deserialize<string>(responseText), Is.EqualTo(itemData));

            // Based on the API behavior, the first item created gets ID 1.
            _createdItemId = 1;
        }

        [Test, Order(3)]
        public async Task ShouldGetCreatedItemById()
        {
            Assert.That(_createdItemId, Is.GreaterThan(0), "Item must be created before it can be retrieved.");

            var response = await _requestContext.GetAsync($"/items/{_createdItemId}");
            await Assertions.Expect(response).ToBeOKAsync();
            
            var responseText = await response.TextAsync();
            Assert.That(JsonSerializer.Deserialize<string>(responseText), Is.EqualTo("test1"));
        }

        [Test, Order(4)]
        public async Task ShouldUpdateExistingItem()
        {
            Assert.That(_createdItemId, Is.GreaterThan(0), "Item must be created before it can be updated.");
            var updatedData = "updated item 1";

            var response = await _requestContext.PutAsync($"/items/{_createdItemId}", new()
            {
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } },
                Data = JsonSerializer.Serialize(updatedData)
            });

            await Assertions.Expect(response).ToBeOKAsync();
            var responseText = await response.TextAsync();
            Assert.That(JsonSerializer.Deserialize<string>(responseText), Is.EqualTo(updatedData));

            // Verify the update persisted
            var getResponse = await _requestContext.GetAsync($"/items/{_createdItemId}");
            await Assertions.Expect(getResponse).ToBeOKAsync();
            var getResponseText = await getResponse.TextAsync();
            Assert.That(JsonSerializer.Deserialize<string>(getResponseText), Is.EqualTo(updatedData));
        }

        [Test, Order(5)]
        public async Task ShouldDeleteItem()
        {
            Assert.That(_createdItemId, Is.GreaterThan(0), "Item must be created before it can be deleted.");

            var response = await _requestcontext.DeleteAsync($"/items/{_createdItemId}");
            await Assertions.Expect(response).ToBeOKAsync();

            // Verify the item was deleted by trying to fetch it again
            var getResponse = await _requestContext.GetAsync($"/items/{_createdItemId}");
            await Assertions.Expect(getResponse).Not.ToBeOKAsync();
            Assert.That(getResponse.Status, Is.EqualTo(404));
        }
    }
}
