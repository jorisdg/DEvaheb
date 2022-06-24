using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEvahebLib
{
    public class Variables
    {
        Dictionary<string, string> types;

        List<string> readonlyVariables;

        public Variables(Dictionary<string, string> types, List<string> readonlyVariables)
        {
            this.types = types ?? new Dictionary<string, string>();
            this.readonlyVariables = readonlyVariables ?? new List<string>();
        }

        public bool Exists(string variableName)
        {
            return types.ContainsKey(variableName);
        }

        public string GetVariableType(string variableName)
        {
            return types.ContainsKey(variableName) ? types[variableName] : "STRING";
        }

        public bool IsVariablesReadOnly(string variableName)
        {
            return readonlyVariables.Contains(variableName);
        }

        public static Variables FromCsv(string filename)
        {
            if (File.Exists(filename))
            {
                Dictionary<string, string> types = new Dictionary<string, string>();
                List<string> readOnly = new List<string>();

                var lines = File.ReadLines(filename).GetEnumerator();
                while (lines.MoveNext())
                {
                    if (!string.IsNullOrWhiteSpace(lines.Current))
                    {
                        var columns = lines.Current.Split(",");

                        if (!string.IsNullOrWhiteSpace(columns[0]) && !columns[0].StartsWith("//"))
                        {
                            types.Add(columns[0], (string.IsNullOrWhiteSpace(columns[1]) || columns.Length < 2) ? "STRING" : columns[1]);

                            if (columns.Length > 2 && !string.IsNullOrWhiteSpace(columns[2]) && columns[2] != "0")
                            {
                                readOnly.Add(columns[0]);
                            }
                        }
                    }
                }

                return new Variables(types, readOnly);
            }

            return new Variables(types: null, readonlyVariables: null);
        }
    }
}
