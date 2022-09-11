namespace AdPang.FileManager.Common.MemoryCache
{
    /// <summary>
    /// 简单的缓存接口，只有查询和添加
    /// </summary>
    public interface ICachingProvider
    {
        object Get(string cacheKey);

        void Set(string cacheKey, object cacheValue);
    }
}
