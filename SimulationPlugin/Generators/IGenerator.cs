using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntMe.Plugin.Simulation.Generators
{
    internal interface IGenerator
    {
        string Language { get; }

        string ProgrammingLanguage { get; }

        string Generate(string name, string path);
    }
}
