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
            return GenerateVB(name, "Class", path,
                GeneratorFiles.solution_vb,
                GeneratorFiles.project_vb,
                GeneratorFiles.user_vb,
                GeneratorFiles.class_vb_en_docu,
                GeneratorFiles.properties_vb);
        }
    }
}
