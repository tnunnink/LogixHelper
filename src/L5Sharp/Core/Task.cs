﻿using System;
using System.Collections.Generic;
using System.Linq;
using L5Sharp.Abstractions;
using L5Sharp.Enumerations;
using L5Sharp.Utilities;

namespace L5Sharp.Core
{
    public class Task : IComponent
    {
        private string _name;
        private TaskType _type;
        private float _rate;
        private short _priority;
        private float _watchdog;
        private readonly Dictionary<string, Program> _programs = new Dictionary<string, Program>();

        public Task(string name, TaskType type = null, string description = null,
            float rate = 10, short priority = 10, float watchdog = 500,
            bool inhibitTask = false, bool disableUpdateOutputs = false)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));

            Validate.Name(name);

            Name = name;
            Type = type ?? TaskType.Periodic;
            Description = description ?? string.Empty;
            Rate = rate;
            Priority = priority;
            Watchdog = watchdog;
            InhibitTask = inhibitTask;
            DisableUpdateOutputs = disableUpdateOutputs;
        }

        public string Name
        {
            get => _name;
            set
            {
                Validate.Name(value);
                
                _name = value;
            }
        }

        public string Description { get; set; }

        public TaskType Type
        {
            get => _type;
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));
                
                _type = value;
            }
        }

        public float Rate
        {
            get => _rate;
            set
            {
                if (value < 0.1f || value > 2000000.0f)
                    throw new ArgumentOutOfRangeException(nameof(Priority),
                        "Rate must be value between 0.1 and 2,000,000.0 ms");
                
                _rate = value;
            }
        }

        public short Priority
        {
            get => _priority;
            set
            {
                if (value < 1 || value > 15)
                    throw new ArgumentOutOfRangeException(nameof(Priority), "Priority must be value between 1 and 15");

                _priority = value;
            }
        }

        public float Watchdog
        {
            get => _watchdog;
            set
            {
                if (value < 0.1f || value > 2000000.0f)
                    throw new ArgumentOutOfRangeException(nameof(Watchdog),
                        "Watchdog must be value between 0.1 and 2,000,000.0 ms");
                
                _watchdog = value;
            }
        }

        public bool InhibitTask { get; set; }
        public bool DisableUpdateOutputs { get; set; }
        public IEnumerable<string> ScheduledPrograms => _programs.Keys.AsEnumerable();
        public IEnumerable<Program> Programs => _programs.Values.AsEnumerable();

        public void AddProgram(Program program)
        {
            if (program == null) throw new ArgumentNullException(nameof(program));

            if (_programs.ContainsKey(program.Name))
                Throw.ComponentNameCollisionException(program.Name, typeof(Program));

            _programs.Add(program.Name, program);
        }

        public void NewProgram(string name)
        {
            var program = new Program(name);
            AddProgram(program);
        }

        public void RemoveProgram(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));

            if (!_programs.ContainsKey(name)) return;

            _programs.Remove(name);
        }
    }
}