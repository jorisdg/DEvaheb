using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DEvahebLib.Enums;

namespace DEvahebLib
{
    public class SymbolTable
    {
        Dictionary<string, DECLARE_TYPE> variables;

        List<string> tasks;

        public SymbolTable()
            : this(null, null)
        {
        }

        public SymbolTable(Dictionary<string, DECLARE_TYPE> knownVariables, List<string> knownTasks)
        {
            variables = knownVariables != null ? new(knownVariables, StringComparer.OrdinalIgnoreCase) : new(StringComparer.OrdinalIgnoreCase);
            tasks = knownTasks ?? new();
        }

        public void AddVariable(string name, DECLARE_TYPE type)
        {
            variables[name] = type;
        }

        public void RemoveVariable(string name)
        {
            variables.Remove(name);
        }

        public bool VariableExists(string name)
        {
            return variables.ContainsKey(name);
        }

        public DECLARE_TYPE? GetVariableType(string name)
        {
            DECLARE_TYPE? foundType = null;

            if (variables.TryGetValue(name, out DECLARE_TYPE type))
            {
                foundType = type;
            }

            return type;
        }

        public void AddTask(string taskName)
        {
            if (!TaskExists(taskName))
                tasks.Add(taskName);
        }

        public void RemoveTask(string name)
        {
            if (TaskExists(name))
            {
                tasks.Remove(name);
            }
        }

        public bool TaskExists(string name)
        {
            return tasks.Contains(name, StringComparer.OrdinalIgnoreCase);
        }
    }
}
