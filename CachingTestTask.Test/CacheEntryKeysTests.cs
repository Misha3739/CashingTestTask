using CachingTestTask.Api;
using NUnit.Framework;

namespace CachingTestTask.Test {
	[TestFixture]
	class CacheEntryKeysTests {
		[TestCase(1, 1, 1, 1)]
		[TestCase(1, 2, 1, 2)]
		[TestCase(2, 7, 2, 7)]
		[TestCase(0, 0, 0, 0)]
		public void TwoSameKeysAreEqual(int userId1, int objectId1, int userId2, int objectId2) {
			CacheEntryKey key1 = new CacheEntryKey(userId1, objectId1);
			CacheEntryKey key2 = new CacheEntryKey(userId2, objectId2);
			Assert.AreEqual(key1.GetHashCode(), key2.GetHashCode());
			Assert.AreEqual(key1, key2);
		}

		[TestCase(1, 1, 2, 1)]
		[TestCase(1, 2, 1, 3)]
		[TestCase(2, 7, 5, 9)]
		[TestCase(0, 0, 0, 1)]
		public void TwoDifferentKeysAreDifferent(int userId1, int objectId1, int userId2, int objectId2) {
			CacheEntryKey key1 = new CacheEntryKey(userId1, objectId1);
			CacheEntryKey key2 = new CacheEntryKey(userId2, objectId2);
			Assert.AreNotEqual(key1.GetHashCode(), key2.GetHashCode());
			Assert.AreNotEqual(key1, key2);
		}
	}
}