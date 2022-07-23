using DomainServices.GenericRepositories.Interface;
using EntityFrameworkCore.UnitOfWork.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DomainServices.GenericRepositories;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
{
    private readonly ApplicationDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly DbSet<TEntity> _dbSet;

    public GenericRepository(ApplicationDbContext context, IUnitOfWork unitOfWork)
    {
        _context = context ?? throw new System.ArgumentNullException(nameof(context));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _dbSet = context.Set<TEntity>();
    }
    public virtual bool Insert(TEntity entity)
    {
        var repository = _unitOfWork.Repository<TEntity>();
        repository.Add(entity);
        if (_unitOfWork.SaveChanges() > 0) return true;
        
        return false;
    }

    public virtual IEnumerable<TEntity> GetAll()
    {
        return _dbSet.AsQueryable().ToList();
    }

    public virtual IQueryable GetByName(object fullName)
    {
        var query = from name in _context.Customers
                    where name.FullName.Contains((string)fullName)
                    select name;

        return query;
    }

    public virtual TEntity GetById(object id)
    {
        return _dbSet.Find(id);
    }

    public virtual bool Update(TEntity entity)
    {
        var repository = _unitOfWork.Repository<TEntity>();
        repository.Update(entity);
        if (_unitOfWork.SaveChanges() > 0) return true;

        return false;
    }

    public virtual bool Delete(object id)
    {
        var repository = _unitOfWork.Repository<TEntity>();
        TEntity entityToDelete = GetById(id);
        repository.Remove(entityToDelete);

        if (_unitOfWork.SaveChanges() > 0) return true;

        return false;
    }
}
