using AntMe.SharedComponents.States;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AntMe.Plugin.GdiPlusPlugin
{

    /// <summary>
    /// Fenster für die 2D-Anzeige.
    /// </summary>
    /// <author>Wolfgang Gallo (wolfgang@antme.net)</author>
    internal partial class Window : Form
    {

        // Das Spielfeld-Kontrollelement.
        private Playground playground;

        /// <summary>
        /// Stellt dem Spielfeld-Kontrollelement den Zustand zur Verfügung.
        /// </summary>
        public SimulationState State = null;

        /// <summary>
        /// Der Fenster-Konstruktor.
        /// </summary>
        public Window()
        {
            // Initialisiert das Fenster wie im Designer in Visual Studio festgelegt.
            InitializeComponent();

            // Sage Windows, daß wir das Puffern beim Zeichnen selbst übernehmen.
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            UpdateStyles();

            // Erzeuge das Spielfeld-Kontrollelement und füge es dem Fenster hinzu.
            playground = new Playground(this);
            playground.Dock = DockStyle.Fill;
            splitContainer.Panel1.Controls.Add(playground);
        }

        /// <summary>
        /// Gibt an und legt fest, ob mit eingeschaltetem Anti-Aliasing gezeichnet
        /// werden soll.
        /// </summary>
        public bool UseAntiAliasing
        {
            get { return cbUseAntiAliasing.Checked; }
            set { cbUseAntiAliasing.Checked = value; }
        }

        /// <summary>
        /// Gibt an und legt fest, ob die Punkte der Spieler angezeigt werden sollen.
        /// </summary>
        public bool ShowScore
        {
            get { return cbShowScore.Checked; }
            set { cbShowScore.Checked = value; }
        }

        /// <summary>
        /// Startet das Zeichnen des Spiels.
        /// </summary>
        public void Start()
        {
            playground.ResetSelection();
            Show();
        }

        /// <summary>
        /// Stoppt das Zeichnen des Spiels.
        /// </summary>
        public void Stop()
        {
            Hide();
            insectsPanel.Controls.Clear();
        }

        /// <summary>
        /// Stellt einen Zustand auf dem Spielfeld und in den Infokästen dar.
        /// </summary>
        /// <param name="state"></param>
        public void Update(SimulationState state)
        {
            // Wird zum ersten Mal gezeichnet?
            bool doResize = State == null;

            // Speichere den Zustand.
            State = state;

            // Erzwinge das Erzeugen einer neuen Hintergrund-Grafik, wenn zum ersten
            // Mal gezeichnet wird.
            if (doResize)
                playground.DoResize();

            // Aktualisiere die Informationen und zeichne das Spielfeld.
            updateInformation();
            playground.Draw();

            // Lass Windows Ereignisse bearbeiten.
            Application.DoEvents();
        }

        /// <summary>
        /// Aktualisiert die dargestellten InfoKasten Objekte.
        /// </summary>
        private void updateInformation()
        {
            // Die aktuellen Werte aller Insekten sind als Klassen im Zustand
            // gespeichert. Hier in der 2D-Anzeige werden diese Werte auf das Spiel-
            // feld gezeichnet und in Informationskästen angezeigt. Diese Kästen sind
            // für Windows Kontrollemente und ihre Erzeugung ist teuer, d.h. sie
            // kostet relativ viel Zeit. Daher werden sie nicht in jeder Runde neu
            // erzeugt, sondern so lange weiterverwendet, wie das zugehörende Insekt
            // am Leben ist.

            // Um die Kästen zu aktualisieren, müssen die Strukturen im Zustand mit
            // den Objekten von hier abgeglichen werden. Der Abgleich erfolgt über
            // die eindeutige Id aller Insekten. Die folgende Schleife ist dafür
            // zuständig, diese Aktualisierung möglichst schnell durchzuführen. Sie
            // läuft dazu gleichzeitig über die vorhandenen InfoKästen und die
            // Insekten im Zustand. Pro Durchlauf der Schleife wird in einer oder in
            // beiden Listen zum Nächsten Element übergegangen.

            // Vorraussetzung für diesen Algorithmus ist es, daß die Ids in beiden
            // Listen aufsteigend sortiert sind, und das ein neues Insekt eine Id
            // hat, die noch nicht verwendet wurde und höher ist als alle bisher
            // verwendeten Ids. Die Simulation erfüllt diese Vorraussetzungen.

            // In der WPF-Anzeige ist dieser Algorithmus in sehr änhlicher Form
            // ebenfalls enthalten und erfüllt die selbe Aufgabe.

            AntInfoBox antbox;
            BugInfoBox bugbox;

            int i = 0; // Index der InfoBox
            int a = 0; // Index der Ameise im Zustand
            int b = 0; // Index der Wanze im Zustand

            // Aktualisiere die BugInfoBox-Instanzen.
            while (i < insectsPanel.Controls.Count && b < State.BugStates.Count)
            {
                // Wandle das aktuelle Kontrollelement in einen BugInfoBox um.
                bugbox = insectsPanel.Controls[i] as BugInfoBox;

                // Wenn es keine BugInfoBox ist, muß es eine AntInfoBox sein. Also
                // weiter zur nächsten Schleife.
                if (bugbox == null)
                    break;

                if (bugbox.Id < State.BugStates[b].Id)
                    insectsPanel.Controls.RemoveAt(i);
                else if (bugbox.Id == State.BugStates[b].Id)
                {
                    bugbox.Bug = State.BugStates[b];
                    i++;
                    b++;
                }
                else
                    b++;
            }

            for (int c = 0; c < State.ColonyStates.Count; c++)
            {
                a = 0;

                // Aktualisiere die AmeisenKästen.
                while (i < insectsPanel.Controls.Count && a < State.ColonyStates[c].AntStates.Count)
                {
                    // Wandle das aktuelle Kontrollelement in einen AmeisenKasten um.
                    // Hier kann es nur noch ein AmeisenKasten sein.
                    antbox = (AntInfoBox)insectsPanel.Controls[i];

                    // Wenn der Kasten zu einem neuen Volk gehört, dann weiter zur nächsten
                    // Schleife.
                    if (antbox.ColonyId != State.ColonyStates[c].Id)
                        break;

                    // Wenn die Id des AmeisenKasten kleiner ist als die Id der aktuellen
                    // Ameise im Zustand, dann ist die Ameise für die der Kasten
                    // Informationen liefert, nicht mehr um Zustand enthalten, d.h. die
                    // Ameise ist gestorben. Der Kasten kann also entfernt werden. Dadurch
                    // rücken die folgenden Kästen nach vorne, der Index muß also nicht
                    // erhöht werden.
                    if (antbox.Id < State.ColonyStates[c].AntStates[a].Id)
                        insectsPanel.Controls.RemoveAt(i);

                    // Wenn die Ids übereinstimmen, dann aktualisiere die Informationen
                    // und rücke in beiden Listen zum nächsten Element vor.
                    else if (antbox.Id == State.ColonyStates[c].AntStates[a].Id)
                    {
                        antbox.Ant = State.ColonyStates[c].AntStates[a];
                        i++;
                        a++;
                    }

                    // Ansonten fahre mit der nächsten Ameise im Zustand fort.
                    else
                        a++;
                }
            }
        }

        /// <summary>
        /// Erzeugt InfoKasten Objekte zur Anzeige von Informationen und stellt
        /// diese dar.
        /// </summary>
        public void ShowInformation(List<int> selectedBugs, List<int> selectedAnts)
        {
            // Brich ab, falls noch kein Zustand übergeben wurde.
            if (State == null)
                return;

            // Sage Windows, daß wir jetzt größere Änderungen an den Kontroll-
            // elementen in dem Panel vornehmen.
            insectsPanel.SuspendLayout();
            insectsPanel.Controls.Clear();

            // Erzeuge InfoKästen für alle Insekten, die innerhalb des Auswahl-
            // rechtecks liegen.

            if (selectedBugs.Count > 0)
                for (int b = 0; b < State.BugStates.Count; b++)
                    if (selectedBugs.Contains(State.BugStates[b].Id))
                        insectsPanel.Controls.Add(new BugInfoBox(State.BugStates[b]));

            int v = 0;
            if (selectedAnts.Count > 0)
                for (int c = 0; c < State.ColonyStates.Count; c++)
                {
                    for (int a = 0; a < State.ColonyStates[c].AntStates.Count; a++)
                        if (selectedAnts.Contains(State.ColonyStates[c].AntStates[a].Id))
                        {
                            string casteName = string.Empty;
                            for (int d = 0; d < State.ColonyStates[c].CasteStates.Count; d++)
                                if (State.ColonyStates[c].CasteStates[d].Id == State.ColonyStates[c].AntStates[a].CasteId)
                                {
                                    casteName = State.ColonyStates[c].CasteStates[d].Name;
                                    break;
                                }

                            insectsPanel.Controls.Add(new AntInfoBox
                                (State.ColonyStates[c].AntStates[a], State.ColonyStates[c].Id,
                                 State.ColonyStates[c].ColonyName, casteName, Playground.playerBrushes[v]));
                        }
                    v++;
                }

            // Sage Windows, daß die Änderungen abgeschlossen sind.
            insectsPanel.ResumeLayout();
        }

        // Wird aufgerufen, wenn der Schließen Knopf des Fensters gedrückt wurde.
        private void Window_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Verhindere, daß das Fenster geschlossen, d.h. das Fenster-Objekt
            // zerstört wird. Verstecke das Fenster stattdessen.
            e.Cancel = true;
            Hide();
        }

        // Wird aufgerufen, wenn die Checkbox für das Anti-Aliasing ihren Status
        // geändert hat.
        private void antialiasingCheckbox_CheckedChanged(object s, EventArgs e)
        {
            // Erzwinge das Erzeugen einer neuen Hintergrund-Grafik, damit die
            // Anti-Aliasing Einstellung übernommen wird.
            playground.DoResize();
        }

        // Wird aufgerufen, wenn die Checkbox für das Zeigen der Punktetabelle
        // ihren Status geändert hat.
        private void showPointsCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            playground.ShowScore = cbShowScore.Checked;
            playground.DoResize();
        }

        // Wird aufgerufen, wenn der Knopf für das Zurücksetzen der Ansicht 
        // gedrückt wurde.
        private void resetButton_Click(object sender, EventArgs e)
        {
            playground.ResetView();
            playground.DoResize();
        }

    }

}