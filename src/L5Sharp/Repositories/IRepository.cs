﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using L5Sharp.Abstractions;

namespace L5Sharp.Repositories
{
    public interface IRepository
    {
    }

    public interface IRepository<T> : IRepository where T : IComponent
    {
        IEnumerable<T> GetAll();
        /*IEnumerable<T> GetAllInclude<TComponent>(params Expression<Func<T, TComponent>>[] includeProperties)
            where TComponent : IEnumerable<IComponent>;*/
        T Get(string name);
        /*IEnumerable<T> Find(Expression<Func<T, bool>> expression);
        IEnumerable<T> FindInclude(Expression<Func<T, bool>> expression,
            params Expression<Func<T, object>>[] includeProperties);*/
        void Add(T component);
        void Remove(T component);
        void Update(T component);
    }
}