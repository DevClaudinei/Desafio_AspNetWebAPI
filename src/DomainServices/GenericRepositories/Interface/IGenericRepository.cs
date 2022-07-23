using System;
using System.Collections.Generic;
using System.Linq;

namespace DomainServices.GenericRepositories.Interface;

public interface IGenericRepository<TEntity> where TEntity : class
{
    bool Insert(TEntity obj);
    IEnumerable<TEntity> GetAll();
    TEntity GetById(object id);
    IQueryable GetByName(object fullName);
    bool Update(TEntity obj);
    bool Delete(object obj);
}