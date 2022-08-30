using AdPang.FileManager.Shared.Paremeters;
using AdPang.FileManager.Shared;
using HttpRequestClient.Services.IRequestServices;

namespace HttpRequestClient.Services.RequestServices
{
    public class BaseRequestService<TEntity, TKey> : IBaseRequestService<TEntity, TKey> where TEntity : class where TKey : struct
    {
        private readonly HttpRestClient client;
        protected readonly string serviceName;

        public BaseRequestService(HttpRestClient client, string serviceName)
        {
            this.client = client;
            this.serviceName = serviceName;
        }

        public async Task<ApiResponse<TEntity>> AddAsync(TEntity entity)
        {
            BaseRequest request = new();
            request.Method = RestSharp.Method.POST;
            request.Route = $"api/{serviceName}/Add";
            request.Parameter = entity;
            return await client.ExecuteAsync<TEntity>(request);
        }

        public async Task<ApiResponse> DeleteAsync(TKey id)
        {
            BaseRequest request = new();
            request.Method = RestSharp.Method.DELETE;
            request.Route = $"api/{serviceName}/Delete/{id}";
            return await client.ExecuteAsync(request);
        }

        public async Task<ApiResponse<PagedList<TEntity>>> GetAllAsync(QueryParameter parameter)
        {
            BaseRequest request = new();
            request.Method = RestSharp.Method.GET;
            request.Route = $"api/{serviceName}/GetAll" +
                $"?pageIndex={parameter.PageIndex}" +
                $"&pageSize={parameter.PageSize}" +
                $"&search={parameter.Search}";
            var result = await client.ExecuteAsync<PagedList<TEntity>>(request);
            return result;
        }

        public async Task<ApiResponse<TEntity>> GetFirstOfDefaultAsync(TKey id)
        {
            BaseRequest request = new();
            request.Method = RestSharp.Method.GET;
            request.Route = $"api/{serviceName}/Get?id={id}";
            return await client.ExecuteAsync<TEntity>(request);
        }

        public async Task<ApiResponse<TEntity>> UpdateAsync(TEntity entity)
        {
            BaseRequest request = new();
            request.Method = RestSharp.Method.PUT;
            request.Route = $"api/{serviceName}/Edit";
            request.Parameter = entity;
            return await client.ExecuteAsync<TEntity>(request);
        }
        public async Task<ApiResponse<IList<TEntity>>> AddsAsync(IEnumerable<TEntity> entities)
        {
            BaseRequest request = new();
            request.Method = RestSharp.Method.POST;
            request.Route = $"api/{serviceName}/Adds";
            request.Parameter = entities;
            return await client.ExecuteAsync<IList<TEntity>>(request);
        }

    }
}
