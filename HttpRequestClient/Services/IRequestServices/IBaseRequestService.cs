using AdPang.FileManager.Shared.Paremeters;
using AdPang.FileManager.Shared;

namespace HttpRequestClient.Services.IRequestServices
{
    public interface IBaseRequestService<TEntity, TKey> where TEntity : class
    {
        /// <summary>
        /// 添加 Route:/api/{serviceName}/Add
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<ApiResponse<TEntity>> AddAsync(TEntity entity);
        /// <summary>
        /// 更新 Route:/api/{serviceName}/Edit
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<ApiResponse<TEntity>> UpdateAsync(TEntity entity);
        /// <summary>
        /// 删除 Route:/api/{serviceName}/Delete/{id}
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ApiResponse> DeleteAsync(TKey id);
        /// <summary>
        /// 获取单个 Route:/api/{serviceName}/Get/{id}
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ApiResponse<TEntity>> GetFirstOfDefaultAsync(TKey id);
        /// <summary>
        /// 查询列表 Route:/api/{serviceName}/GetAll
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        Task<ApiResponse<PagedList<TEntity>>> GetAllAsync(QueryParameter parameter);
        /// <summary>
        /// 批量添加 Route:/api/{serviceName}/Adds
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<ApiResponse<IList<TEntity>>> AddsAsync(IEnumerable<TEntity> entities);
        //Task<ApiResponse> DeletesAsync(ICollection<int> ids);
        //Task<ApiResponse<TEntity>> UpdatesAsync(IEnumerable<TEntity> entities);

    }
}
