using Microsoft.Extensions.Caching.Memory;

namespace CachingTestTask.Api {
	public sealed class CustomMemoryCache : IMemoryCache {
		private readonly MemoryCache _cache;

		private readonly object _locker = new object();

		public CustomMemoryCache() {
			_cache = new MemoryCache(new MemoryCacheOptions());
		}


		public bool TryGetValue(object key, out object value) {
			lock (_locker) {
				return _cache.TryGetValue(key, out value);
			}
		}

		public ICacheEntry CreateEntry(object key) {
			lock (_locker) {
				return _cache.CreateEntry(key);
			}
		}

		public void Remove(object key) {
			lock (_locker) {
				_cache.Remove(key);
			}
		}

		public void Dispose() {
			_cache.Dispose();
		}
	}
}