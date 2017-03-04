using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using iTunesAssistantLib;

namespace iTunesAssistant
{
    public partial class iTunesAssistant : Form
    {
        private WorkflowRunner _workflowRunner;

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

            switch (e.NewValue)
            {
                case CheckState.Checked:
                    clearButton.Enabled = true;
                    startButton.Enabled = true;
                    break;
                case CheckState.Unchecked:
                    var isLastCheckedItem = workflowList.CheckedItems.Count == 1;
                    clearButton.Enabled = isLastCheckedItem == false;
                    startButton.Enabled = isLastCheckedItem == false;
                    break;
            }
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
            foreach (Workflow workflow in Enum.GetValues(typeof (Workflow)))
            {
                workflowList.Items.Add(workflow);
            }
        }

        private List<Workflow> GetCheckedWorkflows()
        {
            var checkedWorkflows = new List<Workflow>();
            foreach (var workflow in Enum.GetValues(typeof (Workflow)))
            {
                if (workflowList.CheckedItems.Contains(workflow))
                {
                    checkedWorkflows.Add((Workflow) Enum.Parse(typeof (Workflow), workflow.ToString()));
                }
            }
            return checkedWorkflows;
        }

        private void RunCheckedWorkflows()
        {
            _workflowRunner = new WorkflowRunner();

            timer.Interval = 100;
            timer.Start();

            Task.Run(() => 
            {
                _workflowRunner.Run(GetCheckedWorkflows());
            })
            .ContinueWith(task => timer.Stop(), TaskScheduler.FromCurrentSynchronizationContext())
            .ContinueWith(task => SetIdleState(), TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void SetBusyState()
        {
            workflowList.Enabled = false;
            clearButton.Enabled = false;
            startButton.Enabled = false;
        }

        private void SetCountLabel(int processedTrackCount, int totalTrackCount)
        {
            countLabel.Text = $@"{processedTrackCount}/{totalTrackCount}";
        }

        private void SetIdleState()
        {
            BuildWorkflowList();
            clearButton.Enabled = false;
            startButton.Enabled = false;
            progressBar.Value = 0;
            stateLabel.Text = @"Idle";
            SetCountLabel(0, 0);
        }
    }
}