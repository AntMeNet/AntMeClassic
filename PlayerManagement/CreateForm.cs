using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace AntMe.PlayerManagement
{
    /// <summary>
    /// Selection Form for all available Code Generators.
    /// </summary>
    public partial class CreateForm : Form
    {
        private List<IGenerator> generators;

        /// <summary>
        /// Path to the created Solution File.
        /// </summary>
        public string GeneratedSolutionFile { get; private set; }

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public CreateForm()
        {
            InitializeComponent();

            generators = new List<IGenerator>();
            generators.Add(new GermanCSharpGenerator());
            generators.Add(new EnglishCSharpGenerator());
            generators.Add(new GermanVBGenerator());
            generators.Add(new EnglishVBGenerator());

            foreach (var language in generators.Select(g => g.Language).Distinct())
                languageComboBox.Items.Add(language);
            if (languageComboBox.Items.Count > 0)
                languageComboBox.SelectedIndex = 0;

            // TODO: Später abhängig von der ausgewählten Sprache auflisten (vielleicht...)
            foreach (var language in generators.Select(g => g.ProgrammingLanguage).Distinct())
                codeComboBox.Items.Add(language);
            if (codeComboBox.Items.Count > 0)
                codeComboBox.SelectedIndex = 0;

            // Setup default folder for new projects
            var personalFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            folderTextBox.Text = Path.Combine(personalFolder, "source", "repos");
        }

        private void browseButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(folderTextBox.Text))
                folderBrowserDialog.SelectedPath = folderTextBox.Text;

            if (folderBrowserDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                folderTextBox.Text = folderBrowserDialog.SelectedPath;
        }

        private void CreateForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult != System.Windows.Forms.DialogResult.OK)
                return;

            // Check den Namen (muss den Namenskonventionen von Klassennamen entsprechen)
            if (!Regex.IsMatch(nameTextBox.Text, @"^[a-zA-Z][a-zA-Z0-9]{1,19}$"))
            {
                MessageBox.Show(Resource.AntColonyNameNotValid);
                e.Cancel = true;
                return;
            }

            // Finde Generator
            var generator = generators.Where(g => g.Language.Equals(languageComboBox.Text) && g.ProgrammingLanguage.Equals(codeComboBox.Text)).FirstOrDefault();
            if (generator == null)
            {
                MessageBox.Show(Resource.AntColonyLanguageNotValid);
                e.Cancel = true;
                return;
            }

            // Prüfen, ob das Ausgabeverzeichnis existiert
            DirectoryInfo root = new DirectoryInfo(folderTextBox.Text);
            if (!root.Exists)
            {
                MessageBox.Show(Resource.AntColonyOutputFolderDoesNotExist);
                e.Cancel = true;
                return;
            }

            try
            {
                string solution = generator.Generate(nameTextBox.Text, folderTextBox.Text);
                GeneratedSolutionFile = solution;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                e.Cancel = true;
                return;
            }
        }
    }
}
