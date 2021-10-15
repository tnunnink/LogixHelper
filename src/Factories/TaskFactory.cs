﻿using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using L5Sharp.Abstractions;
using L5Sharp.Enums;
using L5Sharp.Extensions;

[assembly: InternalsVisibleTo("L5Sharp.Factories.Tests")]

namespace L5Sharp.Factories
{
    internal class TaskFactory : IComponentFactory<ITask>
    {
        public ITask Create(XElement element)
        {
            var type = element.GetValue<ITask>(t => t.Type);
            if (type == null) throw new InvalidOperationException();
            
            var name = element.GetName();
            var description = element.GetDescription();
            var priority = element.GetValue<ITask, byte>(t => t.Priority, Convert.ToByte);
            var watchdog = element.GetValue<ITask, float>(t => t.Watchdog, Convert.ToSingle);
            var inhibitTask = element.GetValue<ITask, bool>(t => t.InhibitTask, Convert.ToBoolean);
            var disableUpdateOutputs = element.GetValue<ITask, bool>(t => t.DisableUpdateOutputs, Convert.ToBoolean);
            
            var task = type.Create(name);

            var programs = element.Descendants("ScheduledProgram").Select(e => e.GetName());
            foreach (var program in programs)
                task.AddProgram(program);

            return task;
        }
    }
}