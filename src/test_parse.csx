using System;
using DEvahebLib.Parser;
using DEvahebLib.Nodes;

var parser = new IcarusParser();
try {
    var nodes = parser.Parse(System.IO.File.ReadAllText(@"DEvahebLibTests\BasicTests\nested_functions.txt"));
    Console.WriteLine($"Parsed {nodes.Count} nodes");
    foreach (var n in nodes) Console.WriteLine(n.GetType().Name);
} catch (Exception ex) {
    Console.WriteLine($"ERROR: {ex.Message}");
}
