using DomainModels;
using EntityFrameworkCore.UnitOfWork.Interfaces;
using Infrastructure.Data;
using System;
using System.Linq.Expressions;

namespace DomainServices.Services;

public abstract class BaseService
{
    protected IUnitOfWork UnitOfWork;
    protected IRepositoryFactory RepositoryFactory;

    public BaseService(IUnitOfWork<ApplicationDbContext> unitOfWork, IRepositoryFactory<ApplicationDbContext> repositoryFactory)
    {
        UnitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        RepositoryFactory = repositoryFactory ?? (IRepositoryFactory)UnitOfWork;
    }

    protected TResult GetFieldById<T, TResult>(long id, Expression<Func<T, TResult>> selector)
        where T : class, IEntity
    {
        var repository = RepositoryFactory.Repository<T>();
        var query = repository.SingleResultQuery()
            .AndFilter(x => x.Id.Equals(id))
            .Select(selector);

        return repository.SingleOrDefault(query);
    }
   
}