using AntMe.SharedComponents.States;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AntMe.Plugin.GdiPlusPlugin
{

    /// <summary>
    /// Window for 2D-display.
    /// </summary>
    /// <author>Wolfgang Gallo (wolfgang@antme.net)</author>
    internal partial class Window : Form
    {

        // The playground control element.
        private Playground playground;

        /// <summary>
        /// Provides the state to the playground control element.
        /// </summary>
        public SimulationState State = null;

        /// <summary>
        /// The window constructor.
        /// </summary>
        public Window()
        {
            // Initializes the window as set in the Designer in Visual Studio.
            InitializeComponent();

            // Tell Windows that we will do the buffering ourselves when drawing.
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            UpdateStyles();

            // Create the playfield control and add it to the window.
            playground = new Playground(this);
            playground.Dock = DockStyle.Fill;
            splitContainer.Panel1.Controls.Add(playground);
        }

        /// <summary>
        /// Specifies and sets whether to draw with anti-aliasing turned on.
        /// </summary>
        public bool UseAntiAliasing
        {
            get { return cbUseAntiAliasing.Checked; }
            set { cbUseAntiAliasing.Checked = value; }
        }

        /// <summary>
        /// Specifies and defines whether the score chart should be displayed.
        /// </summary>
        public bool ShowScore
        {
            get { return cbShowScore.Checked; }
            set { cbShowScore.Checked = value; }
        }

        /// <summary>
        /// Starts drawing of the game.
        /// </summary>
        public void Start()
        {
            playground.ResetSelection();
            Show();
        }

        /// <summary>
        /// Stops drawing of the game.
        /// </summary>
        public void Stop()
        {
            Hide();
            insectsPanel.Controls.Clear();
        }

        /// <summary>
        /// Represents state off the field and in the info boxes.
        /// </summary>
        /// <param name="state">state of simulation</param>
        public void Update(SimulationState state)
        {
            // Is this the first time drawing?
            bool doResize = State == null;

            // Save state.
            State = state;

            // Force a new background graphic to be created when drawing for the first time.
            if (doResize)
                playground.DoResize();

            // Update the information and draw the playground.
            updateInformation();
            playground.Draw();

            // Let Windows handle events.
            Application.DoEvents();
        }

        /// <summary>
        /// Refreshes the displayed InfoBox objects.
        /// </summary>
        private void updateInformation()
        {
            // The current values of all insects are stored as classes in the state.
            // Here in the 2D-display these values are drawn on the playground and
            // displayed in information boxes. These boxes are control elements for
            // Windows and their generation is expensive. The generation consumes
            // relatively much time. Therefore, they are not generated every turn,
            // but are used as long as the corresponding insect is alive.
            
            // To update the boxes, the structures in the state must be matched with the
            // objects from here. The matching is done using the unique ID of all insects.
            // The following loop is responsible for performing this update as quickly as
            // possible. It runs simultaneously over the existing info boxes and the insects
            // in the state. Each time the loop runs, it moves to the next element in one
            // or both lists.

            // The prerequisite for this algorithm are:
            // 1) the IDs in both lists are sorted in ascending order
            // 2) a new insect has an ID that has not yet been used
            // 3) the ID is higher than all previously used Ids
            // The simulation fulfills these requirements.

            // This algorithm is also included in the WPF display in a very similar way and
            // performs the same task.

            AntInfoBox antbox;
            BugInfoBox bugbox;

            int i = 0; // Index of InfoBox
            int a = 0; // Index of ant in state
            int b = 0; // Index of bug in state

            // Update the bug InfoBox instances.
            while (i < insectsPanel.Controls.Count && b < State.BugStates.Count)
            {
                // Convert the current control into a BugInfoBox.
                bugbox = insectsPanel.Controls[i] as BugInfoBox;

                // If it is not a BugInfoBox, it must be an AntInfoBox.
                // So let's move on to the next loop.
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

                // Update the ant InfoBox instances.
                while (i < insectsPanel.Controls.Count && a < State.ColonyStates[c].AntStates.Count)
                {
                    // Convert the current control into a ant InfoBox.
                    // It must be an AntInfoBox.
                    antbox = (AntInfoBox)insectsPanel.Controls[i];

                    // If the box belongs to a new colony move on to the next loop.
                    if (antbox.ColonyId != State.ColonyStates[c].Id)
                        break;

                    // If the ID of the ant box is smaller than the ID of the current ant
                    // in the state, then this ant is no longer included in the state.
                    // That means the ant has died. So the box can be removed.
                    // This moves the following boxes forward.
                    // There is no need to change the indices.
                    if (antbox.Id < State.ColonyStates[c].AntStates[a].Id)
                        insectsPanel.Controls.RemoveAt(i);

                    // If the ids match, then update the information and go forward
                    // to the next item in both lists.
                    else if (antbox.Id == State.ColonyStates[c].AntStates[a].Id)
                    {
                        antbox.Ant = State.ColonyStates[c].AntStates[a];
                        i++;
                        a++;
                    }

                    // Otherwise, continue with the next ant in the state.
                    else
                        a++;
                }
            }
        }

        /// <summary>
        /// Creates InfoBox objects for displaying information and displays them.
        /// </summary>
        public void ShowInformation(List<int> selectedBugs, List<int> selectedAnts)
        {
            // Cancel if no state has been passed yet.
            if (State == null)
                return;

            // Tell Windows we are now making major changes to the controls in the panel.
            insectsPanel.SuspendLayout();
            insectsPanel.Controls.Clear();

            // Create info boxes for all insects that lie within the selection rectangle.
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

            // Tell Windows that the changes are complete.
            insectsPanel.ResumeLayout();
        }

        // Called when the close button of the window is pressed.
        private void Window_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Prevent the window from being closed, i.e. the window object
            // from being is destroyed. Hide the window instead.
            e.Cancel = true;
            Hide();
        }

        // Called when the anti-aliasing checkbox has changed its state.
        private void antialiasingCheckbox_CheckedChanged(object s, EventArgs e)
        {
            // Force a new background graphic to be created so that the
            // anti-aliasing setting is applied.
            playground.DoResize();
        }

        // Called when the checkbox for showing the charts table has changed its status.
        private void showPointsCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            playground.ShowScore = cbShowScore.Checked;
            playground.DoResize();
        }

        // Called when the button for resetting the view is pressed.
        private void resetButton_Click(object sender, EventArgs e)
        {
            playground.ResetView();
            playground.DoResize();
        }

    }

}