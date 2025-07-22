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

        public virtual async Task<IEnumerable<T>> GetAllAsync(Guid userId)
        {
            return await _repository.GetAllAsync(userId);
        }

        public virtual async Task<T> GetByIdAsync(TKey id, Guid userId)
        {
            return await _repository.GetByIdAsync(id, userId);
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

        public virtual async Task<T> UpdateAsync(T entity, Guid userId)
        {
            try
            {
                if(entity.UserId != userId)
                {
                    throw new Exception("Id de usuário não compativel para alteração");
                }
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

        public virtual async Task DeleteAsync(TKey id, Guid userId)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                await _repository.DeleteAsync(id, userId);
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