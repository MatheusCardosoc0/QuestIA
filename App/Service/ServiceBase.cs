using QuestIA.Core.Models;
using QuestIA.Core.Repository;
using QuestIA.Core.Service;

namespace QuestIA.App.Service
{
    public class ServiceBase<T, TKey> : IServiceBase<T, TKey> where T : class, IEntity<TKey>
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IRepositoryBase<T, TKey> _repository;

        public ServiceBase(IUnitOfWork unitOfWork, IRepositoryBase<T, TKey> repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public virtual async Task<T> GetByIdAsync(TKey id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public virtual async Task<T> CreateAsync(T entity)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                var createdEntity = await _repository.CreateAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();
                return createdEntity;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public virtual async Task<T> UpdateAsync(T entity)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                var updatedEntity = await _repository.UpdateAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();
                return updatedEntity;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public virtual async Task DeleteAsync(TKey id)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                await _repository.DeleteAsync(id);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
    }
} 