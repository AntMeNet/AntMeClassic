using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntMe.PlayerManagement
{
    internal sealed class GermanVBGenerator : VsGenerator, IGenerator
    {
        public string Language
        {
            get { return "Deutsch"; }
        }

        public string ProgrammingLanguage
        {
            get { return "VB.NET"; }
        }

        public string Generate(string name, string path)
        {
            return GenerateVB(name, "Klasse", path,
                GeneratorFiles.solution_vb,
                GeneratorFiles.project_vb,
                GeneratorFiles.user_vb,
                GeneratorFiles.class_vb_de_docu,
                GeneratorFiles.properties_vb);
        }
    }
}
