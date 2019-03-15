using CachingTestTask.Api;
using CachingTestTask.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using NUnit.Framework;

namespace CachingTestTask.Test {
	[TestFixture]
	public class CachingControllerTests {
		private readonly CachingController _controller;

		private readonly Mock<IMemoryCache> _cache;

		public CachingControllerTests() {
			_cache = new Mock<IMemoryCache>();
			_controller = new CachingController(_cache.Object);
		}

		[SetUp]
		public void SetUp() {
			_cache.Setup(c => c.CreateEntry(It.IsAny<object>())).Returns(new CacheEntryStub());
		}

		[Test]
		public void CanAddItemToCache() {
			var key = new CacheEntryKey(1, 1);
			object value = null;
			_cache.Setup(c => c.TryGetValue(key, out value)).Returns(false);

			var actual = _controller.AddToCache(1, 1, null, new object());
			Assert.IsInstanceOf<OkObjectResult>(actual);
			Assert.AreEqual("Successfully added object to cache", (actual as OkObjectResult).Value);
		}

		[Test]
		public void CanAddSameItemToCache() {
			var key = new CacheEntryKey(1, 1);
			object value = null;
			_cache.Setup(c => c.TryGetValue(key, out value)).Returns(true);

			var actual = _controller.AddToCache(1, 1, null, new object());
			Assert.IsInstanceOf<OkObjectResult>(actual);
			Assert.AreEqual("Object with the same key already exists!", (actual as OkObjectResult).Value);
		}

		[Test]
		public void CanHeadExistingItem() {
			var key = new CacheEntryKey(1, 1);
			object value = null;
			_cache.Setup(c => c.TryGetValue(key, out value)).Returns(true);

			var actual = _controller.HeadCache(1, 1);
			Assert.IsInstanceOf<OkResult>(actual);
		}

		[Test]
		public void CanHeadUnexistedItem() {
			var key = new CacheEntryKey(1, 1);
			object value = null;
			_cache.Setup(c => c.TryGetValue(key, out value)).Returns(false);

			var actual = _controller.HeadCache(1, 1);
			Assert.IsInstanceOf<NotFoundResult>(actual);
		}

		[Test]
		public void CanGetExistingItem() {
			var key = new CacheEntryKey(1, 1);
			object value = "Retrieved item";
			_cache.Setup(c => c.TryGetValue(key, out value)).Returns(true);

			var actual = _controller.GetCache(1, 1);
			Assert.IsInstanceOf<OkObjectResult>(actual);
			Assert.AreEqual("Retrieved item", (actual as OkObjectResult).Value);
		}

		[Test]
		public void CanGetUnexistedItem() {
			var key = new CacheEntryKey(1, 1);
			object value = null;
			_cache.Setup(c => c.TryGetValue(key, out value)).Returns(false);

			var actual = _controller.GetCache(1, 1);
			Assert.IsInstanceOf<NotFoundObjectResult>(actual);
			Assert.AreEqual("Cached item not found!", (actual as NotFoundObjectResult).Value);
		}

		[Test]
		public void CanDeleteExistingItem() {
			var key = new CacheEntryKey(1, 1);
			object value = null;
			_cache.Setup(c => c.TryGetValue(key, out value)).Returns(true);

			var actual = _controller.DeleteCache(1, 1);
			Assert.IsInstanceOf<OkObjectResult>(actual);
			Assert.AreEqual("Item was successfully deleted", (actual as OkObjectResult).Value);
		}

		[Test]
		public void CanDeleteUnexistedItem() {
			var key = new CacheEntryKey(1, 1);
			object value = null;
			_cache.Setup(c => c.TryGetValue(key, out value)).Returns(false);

			var actual = _controller.DeleteCache(1, 1);
			Assert.IsInstanceOf<NotFoundObjectResult>(actual);
			Assert.AreEqual("Item to delete was not found!", (actual as NotFoundObjectResult).Value);
		}
	}
}