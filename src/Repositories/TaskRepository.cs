﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using L5Sharp.Extensions;

namespace L5Sharp.Repositories
{
    internal class TaskRepository : IReadOnlyRepository<ITask>
    {
        private readonly LogixContext _context;

        public TaskRepository(LogixContext context)
        {
            _context = context;
        }

        public bool Contains(string name) => _context.L5X.GetComponents<ITask>().Any(t => t.GetComponentName() == name);

        public ITask? Get(string name) =>
            _context.L5X.GetComponents<ITask>()
                .FirstOrDefault(t => t.GetComponentName() == name)
                ?.Deserialize<ITask>();

        public IEnumerable<ITask> GetAll() => _context.L5X.GetComponents<ITask>().Select(t => t.Deserialize<ITask>());

        public ITask? Find(Expression<Func<ITask, bool>> predicate) => 
            _context.L5X.GetComponents<ITask>().FirstOrDefault(predicate.ToXExpression())?.Deserialize<ITask>();

        public IEnumerable<ITask> FindAll(Expression<Func<ITask, bool>> predicate) => 
            _context.L5X.GetComponents<ITask>().Where(predicate.ToXExpression()).Select(e => e.Deserialize<ITask>());
    }
}