﻿using System.Collections.Generic;
using L5Sharp.Core;
using L5Sharp.Enums;

namespace L5Sharp.Abstractions
{
    public interface IProgram : IComponent
    {
        ProgramType Type { get; }
        bool TestEdits { get; }
        bool Disabled { get; set; }
        IEnumerable<ITag> Tags { get; }
        IEnumerable<IRoutine> Routines { get; }
        bool HasRoutine(string name);
        bool HasRoutine(RoutineType type);
        IRoutine GetRoutine(string name);
        T GetRoutine<T>(string name) where T : IRoutine;
        IEnumerable<T> GetRoutines<T>() where T : IRoutine;
        void AddRoutine(string name, RoutineType type);
        void RemoveRoutine(string name);
        ITag GetTag(string name);
        ITag<T> GetTag<T>(string name) where T : IDataType, new();
        IEnumerable<T> GetTags<T>() where T : ITag;
        void AddTag(string name, IDataType dataType);
        void AddTag<T>(string name) where T : IDataType, new();
        void RemoveTag(string name);
    }
}