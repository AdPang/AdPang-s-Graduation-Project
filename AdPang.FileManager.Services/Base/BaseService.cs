﻿using AdPang.FileManager.IRepositories.Base;
using AdPang.FileManager.IServices.Base;
using System.Linq.Expressions;

namespace AdPang.FileManager.Services.Base
{
    public class BaseService<TEntity> : IBaseService<TEntity> where TEntity : class, new()
    {
        public IBaseRepository<TEntity> _baseRepository;

        public BaseService(IBaseRepository<TEntity> baseRepository)
        {
            _baseRepository = baseRepository;
        }

        public async Task<TEntity> InsertAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            return await _baseRepository.InsertAsync(entity, autoSave, cancellationToken);
        }

        public async Task InsertManyAsync(IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            await _baseRepository.InsertManyAsync(entities, autoSave, cancellationToken);
        }

        public async Task<TEntity> UpdateAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            return await _baseRepository.UpdateAsync(entity, autoSave, cancellationToken);
        }

        public async Task UpdateManyAsync(IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            await _baseRepository.UpdateManyAsync(entities, autoSave, cancellationToken);
        }

        public async Task DeleteAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            await _baseRepository.DeleteAsync(entity, autoSave, cancellationToken);
        }

        public async Task DeleteAsync(Expression<Func<TEntity, bool>> predicate, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            await _baseRepository.DeleteAsync(predicate, autoSave, cancellationToken);
        }

        public async Task DeleteManyAsync(IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            await _baseRepository.DeleteManyAsync(entities, autoSave, cancellationToken);
        }

        public async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _baseRepository.FindAsync(predicate, cancellationToken);
        }

        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _baseRepository.GetAsync(predicate, cancellationToken);
        }

        public async Task<List<TEntity>> GetListAsync(CancellationToken cancellationToken = default, bool IsTracking = false)
        {
            return await _baseRepository.GetListAsync(cancellationToken, IsTracking);
        }

        public async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default, bool IsTracking = false)
        {
            return await _baseRepository.GetListAsync(predicate, cancellationToken, IsTracking);
        }

        public async Task<List<TEntity>> GetPagedListAsync(int skipCount, int maxResultCount, string sorting,
            CancellationToken cancellationToken = default, bool IsTracking = false)
        {
            return await _baseRepository.GetPagedListAsync(skipCount, maxResultCount, sorting, cancellationToken,IsTracking);
        }
        public async Task<List<TEntity>> GetPagedListAsync(Expression<Func<TEntity, bool>> predicate, int skipCount, int maxResultCount, string sorting, CancellationToken cancellationToken = default, bool IsTracking = false)
        {
            return await _baseRepository.GetPagedListAsync(predicate, skipCount, maxResultCount, sorting, cancellationToken, IsTracking);
        }
        public async Task<long> GetCountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _baseRepository.GetCountAsync(predicate, cancellationToken);
        }
        public async Task<long> GetCountAsync(CancellationToken cancellationToken = default)
        {
            return await _baseRepository.GetCountAsync(cancellationToken);
        }


    }
}
