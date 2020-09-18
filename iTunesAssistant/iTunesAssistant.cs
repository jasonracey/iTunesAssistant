using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using iTunesAssistantLib;

namespace iTunesAssistant
{
    public partial class iTunesAssistant : Form
    {
        private HashSet<Workflow> _selectedWorkflows = new HashSet<Workflow>();
        private WorkflowRunner _workflowRunner = new WorkflowRunner(new AppClassWrapper());

        public iTunesAssistant()
        {
            InitializeComponent();
        }

        private void iTunesAssistant_Load(object sender, EventArgs e)
        {
            SetIdleState();
        }

        private void workflowList_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (!workflowList.Enabled)
            {
                e.NewValue = e.CurrentValue;
                return;
            }

            if (e.NewValue == CheckState.Checked && workflowList.Text == WorkflowName.ImportTrackNames)
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    _selectedWorkflows.Add(new Workflow(name: workflowList.Text, fileName: openFileDialog.FileName));
                }
                else
                {
                    e.NewValue = CheckState.Unchecked;
                }
            }
            else if (e.NewValue == CheckState.Checked)
            {
                _selectedWorkflows.Add(new Workflow(name: workflowList.Text));
            }
            else
            {
                RemoveWorkflow(workflowList.Text);
            }

            SetButtonState(e.NewValue);
        }

        private void findText_TextChanged(object sender, EventArgs e)
        {
            var haveFindText = HaveFindText();

            if (haveFindText)
            {
                UpdateFindAndReplaceWorkflow(findText.Text, replaceText.Text);
            }
            else
            {
                RemoveWorkflow(WorkflowName.FindAndReplace);
            }

            clearButton.Enabled = haveFindText;
            startButton.Enabled = haveFindText;
        }

        private void replaceText_TextChanged(object sender, EventArgs e)
        {
            UpdateFindAndReplaceWorkflow(findText.Text, replaceText.Text);
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            SetIdleState();
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            SetBusyState();
            RunCheckedWorkflows();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (_workflowRunner.State != null)
            {
                stateLabel.Text = _workflowRunner.State;
            }

            SetCountLabel(_workflowRunner.ItemsProcessed, _workflowRunner.ItemsTotal);

            progressBar.Maximum = _workflowRunner.ItemsTotal;
            progressBar.Value = _workflowRunner.ItemsProcessed;
        }

        private void BuildWorkflowList()
        {
            workflowList.Enabled = true;
            workflowList.Items.Clear();
            workflowList.Items.Add(WorkflowName.FixCountOfTracksOnAlbum);
            workflowList.Items.Add(WorkflowName.FixGratefulDeadTracks);
            workflowList.Items.Add(WorkflowName.FixTrackNames);
            workflowList.Items.Add(WorkflowName.FixTrackNumbers);
            workflowList.Items.Add(WorkflowName.ImportTrackNames);
            workflowList.Items.Add(WorkflowName.MergeAlbums);
            workflowList.Items.Add(WorkflowName.SetAlbumNames);
        }

        private bool HaveFindText()
        {
            return findText.Text != string.Empty;
        }

        private void RemoveWorkflow(string workflowName)
        {
            _selectedWorkflows.RemoveWhere(workflow => workflow.Name == workflowName);
        }

        private void RunCheckedWorkflows()
        {
            _workflowRunner = new WorkflowRunner(new AppClassWrapper());

            timer.Interval = 100;
            timer.Start();

            Task.Run(() => 
            {
                try
                {
                    _workflowRunner.Run(_selectedWorkflows);
                }
                catch (Exception e)
                {
                    MessageBox.Show("Error: " + e.Message, "Error", MessageBoxButtons.OK);
                }
                
            })
            .ContinueWith(task => timer.Stop(), TaskScheduler.FromCurrentSynchronizationContext())
            .ContinueWith(task => SetIdleState(), TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void SetBusyState()
        {
            workflowList.Enabled = false;
            findAndReplaceGroup.Enabled = false;
            clearButton.Enabled = false;
            startButton.Enabled = false;
        }

        private void SetButtonState(CheckState checkState)
        {
            if (checkState == CheckState.Checked)
            {
                clearButton.Enabled = true;
                startButton.Enabled = true;
            }
            else
            {
                var isLastCheckedItem = workflowList.CheckedItems.Count <= 1;
                var enableButton = isLastCheckedItem == false || HaveFindText();
                clearButton.Enabled = enableButton;
                startButton.Enabled = enableButton;
            }
        }

        private void SetCountLabel(int processedTrackCount, int totalTrackCount)
        {
            countLabel.Text = $@"{processedTrackCount}/{totalTrackCount}";
        }

        private void SetIdleState()
        {
            BuildWorkflowList();
            findAndReplaceGroup.Enabled = true;
            findText.Clear();
            replaceText.Clear();
            clearButton.Enabled = false;
            startButton.Enabled = false;
            progressBar.Value = 0;
            stateLabel.Text = @"Idle";
            SetCountLabel(0, 0);
            _selectedWorkflows = new HashSet<Workflow>();
        }

        private void UpdateFindAndReplaceWorkflow(string findText, string replaceText)
        {
            RemoveWorkflow(WorkflowName.FindAndReplace);
            _selectedWorkflows.Add(new Workflow(name: WorkflowName.FindAndReplace, oldValue: findText, newValue: replaceText));
        }
    }
}