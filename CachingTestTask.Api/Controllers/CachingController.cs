using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CachingTestTask.Api.Controllers {
	public class CachingController : Controller {
		private readonly IMemoryCache _cache;

		public CachingController(IMemoryCache cache) {
			_cache = cache;
		}

		[HttpPut("api/add")]
		public IActionResult AddToCache(int userId, int objId, int? lifetime, [FromBody] object obj) {
			CacheEntryKey key = new CacheEntryKey(userId, objId);
			if (!_cache.TryGetValue(key, out _)) {
				MemoryCacheEntryOptions options = new MemoryCacheEntryOptions();
				if (lifetime.HasValue && lifetime.Value > 0)
					options.SetSlidingExpiration(TimeSpan.FromSeconds(lifetime.Value));
				else
					options.SetPriority(CacheItemPriority.NeverRemove);
				_cache.Set(key, obj, options);
				return Ok("Successfully added object to cache");
			}

			return Ok("Object with the same key already exists!");
		}

		[HttpHead("api/head")]
		public IActionResult HeadCache(int userId, int objId) {
			CacheEntryKey key = new CacheEntryKey(userId, objId);
			bool result = _cache.TryGetValue(key, out _);
			if (result)
				return Ok();
			return NotFound();
		}

		[HttpDelete("api/delete")]
		public IActionResult DeleteCache(int userId, int objId) {
			CacheEntryKey key = new CacheEntryKey(userId, objId);
			bool result = _cache.TryGetValue(key, out _);
			if (result) {
				_cache.Remove(key);
				return Ok("Item was successfully deleted");
			}

			return NotFound("Item to delete was not found!");
		}

		[HttpGet("api/get")]
		public IActionResult GetCache(int userId, int objId) {
			CacheEntryKey key = new CacheEntryKey(userId, objId);
			bool result = _cache.TryGetValue(key, out object value);
			if (result)
				return Ok(value);
			return NotFound("Cached item not found!");
		}
	}
}