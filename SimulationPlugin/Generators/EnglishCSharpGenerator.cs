using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntMe.Plugin.Simulation.Generators
{
    internal sealed class EnglishCSharpGenerator : VsGenerator, IGenerator
    {
        public string Language
        {
            get { return "English"; }
        }

        public string ProgrammingLanguage
        {
            get { return "C#"; }
        }

        public string Generate(string name, string path)
        {
            return Generate(name, path, 
                GeneratorFiles.solution, 
                GeneratorFiles.project, 
                GeneratorFiles.user, 
                GeneratorFiles.class_en, 
                GeneratorFiles.properties);
        }
    }
}
