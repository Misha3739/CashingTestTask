namespace CachingTestTask.Api {
	public struct CacheEntryKey {
		private readonly int _userId;
		private readonly int _objectId;

		public CacheEntryKey(int userId, int objectId) {
			_userId = userId;
			_objectId = objectId;
		}

		public int UserId => _userId;
		public int ObjectId => _objectId;

		public override bool Equals(object obj) {
			if (!(obj is CacheEntryKey)) return false;
			CacheEntryKey val = (CacheEntryKey) obj;
			return _userId == val.UserId && ObjectId == val.ObjectId;
		}

		public override int GetHashCode() {
			unchecked {
				int result = 0;
				result = (result * 397) ^ _userId;
				result = (result * 397) ^ _objectId;
				return result;
			}
		}
	}
}