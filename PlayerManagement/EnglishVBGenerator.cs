using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntMe.PlayerManagement
{
    internal sealed class EnglishVBGenerator : VsGenerator, IGenerator
    {
        public string Language
        {
            get { return "English"; }
        }

        public string ProgrammingLanguage
        {
            get { return "VB.NET"; }
        }

        public string Generate(string name, string path)
        {
            return GenerateVB(name, path, 
                GeneratorFiles.solution_vb, 
                GeneratorFiles.project_vb, 
                GeneratorFiles.user_vb, 
                GeneratorFiles.class_vb_en, 
                GeneratorFiles.properties_vb);
        }
    }
}
