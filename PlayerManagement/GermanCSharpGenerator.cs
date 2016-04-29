using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntMe.PlayerManagement
{
    internal sealed class GermanCSharpGenerator : VsGenerator, IGenerator
    {
        public string Language
        {
            get { return "Deutsch"; }
        }

        public string ProgrammingLanguage
        {
            get { return "C#"; }
        }

        public string Generate(string name, string path)
        {
            return GenerateCS(name, "Klasse", path,
                GeneratorFiles.solution,
                GeneratorFiles.project,
                GeneratorFiles.user,
                GeneratorFiles.class_cs_de_docu,
                GeneratorFiles.properties);
        }
    }

    //internal sealed class GermanCSharpDocumentGenerator : VsGenerator, IGenerator
    //{
    //    public string Language
    //    {
    //        get { return "Deutsch"; }
    //    }

    //    public string ProgrammingLanguage
    //    {
    //        get { return "C# (Dokumentiert)"; }
    //    }

    //    public string Generate(string name, string path)
    //    {
    //        return GenerateCS(name, path,
    //            GeneratorFiles.solution,
    //            GeneratorFiles.project,
    //            GeneratorFiles.user,
    //            GeneratorFiles.class_cs_de_docu,
    //            GeneratorFiles.properties);
    //    }
    //}
}
