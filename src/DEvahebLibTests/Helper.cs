﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DEvahebLib;
using DEvahebLib.Nodes;
using DEvahebLib.Parser;
using DEvahebLib.Visitors;

namespace DEvahebLibTests
{
    internal class Helper
    {
        public static string GenerateSource(Variables variables, List<Node> nodes, SourceCodeParity parity = SourceCodeParity.BehavED)
        {
            var icarusText = new GenerateIcarus(variables) { Parity = parity };
            icarusText.Visit(nodes);

            StringBuilder sb = new StringBuilder();

            if (parity == SourceCodeParity.BehavED)
            {
                sb.AppendLine("//Generated by BehavEd");
                sb.AppendLine();
            }

            sb.Append(icarusText.SourceCode.ToString());

            return sb.ToString();
        }

        public static List<Node> ReadIBI(string filename)
        {
            List<Node> nodes = new List<Node>();

            try
            {
                using (var file = new FileStream(filename, FileMode.Open))
                {
                    using (var reader = new BinaryReader(file))
                    {
                        var header = reader.ReadChars(4);

                        if (new string(header) != "IBI\0") // IBI string terminating
                            throw new Exception($"File {filename} is not a valid IBI file");

                        Console.WriteLine($"IBI File Version: {reader.ReadSingle()}");

                        var parser = new IBIParser();
                        while (reader != null && reader.BaseStream.Position < reader.BaseStream.Length)
                        {
                            nodes.Add(parser.ReadIBIBlock(reader));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine(ex.ToString());
            }

            return nodes;
        }

        public static void GenerateSourceFromIBI(string ibiFile, string newSourceFile, string variablesCsvFile)
        {
            GenerateSourceFromIBI(ibiFile, newSourceFile, Variables.FromCsv(variablesCsvFile));
        }

        public static void GenerateSourceFromIBI(string ibiFile, string newSourceFile, Variables variables)
        {
            var nodes = Helper.ReadIBI(ibiFile);
            File.WriteAllText(newSourceFile, Helper.GenerateSource(variables, nodes, SourceCodeParity.BehavED));
        }

        public static void GetSourceFilesDifferences(string originalFile, string newFile)
        {
            StringBuilder differences = new StringBuilder();

            var originalSource = File.ReadLines(originalFile).GetEnumerator();
            var newSource = File.ReadLines(newFile).GetEnumerator();

            while(originalSource.MoveNext())
            {
                // account for empty lines or comment lines
                if (string.IsNullOrWhiteSpace(originalSource.Current)
                    || originalSource.Current.TrimStart().StartsWith("rem ")
                    || originalSource.Current.TrimStart().StartsWith("rem(")
                    || originalSource.Current.TrimStart().StartsWith("//"))
                    continue;

                do
                {
                    if (!newSource.MoveNext())
                    {
                        differences.AppendLine("New source file is shorter");
                        throw new Exception(differences.ToString());
                    }
                }
                while (string.IsNullOrWhiteSpace(newSource.Current)
                    || newSource.Current.TrimStart().StartsWith("rem ")
                    || newSource.Current.TrimStart().StartsWith("rem(")
                    || newSource.Current.TrimStart().StartsWith("//"));

                Assert.AreEqual<string>(expected: originalSource.Current, actual: newSource.Current);
            }

            while (newSource.MoveNext())
            {
                differences.AppendLine("New source file is longer");
                if (!string.IsNullOrWhiteSpace(newSource.Current))
                    throw new Exception(differences.ToString());
            }
        }

        public static void GenerateSourceFromIBIAndCompareOriginal(string filenameBase, string variablesCsvFile)
        {
            var ibiFile = filenameBase + ".IBI";
            var originalSourceFile = filenameBase + ".txt";
            var outputFile = filenameBase + ".test";

            Helper.GenerateSourceFromIBI(ibiFile, outputFile, variablesCsvFile);

            Helper.GetSourceFilesDifferences(originalSourceFile, outputFile);
        }

        public static void GenerateSourceFromIBIAndCompareOriginal(string filenameBase, Variables variables)
        {
            GenerateSourceFromIBIAndCompareOriginal(filenameBase, variables, ".txt");
        }

        public static void GenerateSourceFromIBIAndCompareOriginal(string filenameBase, Variables variables, string originalExtension)
        {
            var ibiFile = filenameBase + ".IBI";
            var originalSourceFile = filenameBase + originalExtension;
            var outputFile = filenameBase + ".test";

            Helper.GenerateSourceFromIBI(ibiFile, outputFile, variables);

            Helper.GetSourceFilesDifferences(originalSourceFile, outputFile);
        }
    }
}
