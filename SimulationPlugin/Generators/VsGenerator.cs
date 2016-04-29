using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace AntMe.Plugin.Simulation.Generators
{
    internal abstract class VsGenerator
    {
        private DirectoryInfo _outputPath;
        private string _classname;
        private string _solutionname;
        private string _projectname;
        private Guid _solutionguid;
        private Guid _projectguid;
        private string _playername;
        private string _antmeroot;

        /// <summary>
        /// Generates the project files
        /// {0}: Name der Ameisenklasse
        /// {1}: Solution Name
        /// {2}: Projekt Name
        /// {3}: Solution Guid
        /// {4}: Projekt Guid
        /// {5}: Player Display Name
        /// {6}: AntMe Install Path
        /// {7}: Project Root
        /// </summary>
        /// <param name="name"></param>
        /// <param name="path"></param>
        /// <param name="solutionFile"></param>
        /// <param name="projectFile"></param>
        /// <param name="projectUserFile"></param>
        /// <param name="classFile"></param>
        /// <param name="propertiesFile"></param>
        /// <returns>Path to Solution File</returns>
        protected string Generate(string name, string path, string solutionFile, string projectFile, string projectUserFile, string classFile, string propertiesFile)
        {
            // Check KI Name
            if (!Regex.IsMatch(name, @"^[a-zA-Z][a-zA-Z0-9]{1,19}$"))
                throw new ArgumentException("Der Name darf nur Buchstaben und Zahlen enthalten, nicht mit einer Zahl beginnen und zwischen 2 und 20 Zeichen lang sein.");

            // Prüfen, ob das Ausgabeverzeichnis existiert
            DirectoryInfo root = new DirectoryInfo(path);
            if (!root.Exists)
                throw new ArgumentException("Ausgabeverzeichnis existiert nicht");

            // Verzeichnis erzeugen
            _outputPath = root.CreateSubdirectory(name);
            _outputPath.CreateSubdirectory("Properties");

            // Identify AntMe! Root
            _antmeroot = new FileInfo(Assembly.GetEntryAssembly().Location).DirectoryName;

            _classname = name + "Class";
            _solutionname = name;
            _projectname = name;
            _solutionguid = Guid.NewGuid();
            _projectguid = Guid.NewGuid();
            _playername = name;

            GenerateFile(_outputPath.FullName + "\\" + _solutionname + ".sln", solutionFile);
            GenerateFile(_outputPath.FullName + "\\" + _projectname + ".csproj", projectFile);
            GenerateFile(_outputPath.FullName + "\\" + _projectname + ".csproj.user", projectUserFile);
            GenerateFile(_outputPath.FullName + "\\" + _classname + ".cs", classFile);
            GenerateFile(_outputPath.FullName + "\\Properties\\AssemblyInfo.cs", propertiesFile);

            return _outputPath.FullName + "\\" + _solutionname + ".sln";
        }

        private void GenerateFile(string filename, string content)
        {
            content = content.Replace("{0}", _classname);
            content = content.Replace("{1}", _solutionname);
            content = content.Replace("{2}", _projectname);
            content = content.Replace("{3}", _solutionguid.ToString());
            content = content.Replace("{4}", _projectguid.ToString());
            content = content.Replace("{5}", _playername);
            content = content.Replace("{6}", _antmeroot);
            content = content.Replace("{7}", _outputPath.FullName);

            File.WriteAllText(filename, content);
        }
    }
}
