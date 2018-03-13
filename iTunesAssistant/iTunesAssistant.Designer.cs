﻿namespace iTunesAssistant
{
    partial class iTunesAssistant
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.workflowList = new System.Windows.Forms.CheckedListBox();
            this.startButton = new System.Windows.Forms.Button();
            this.clearButton = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.countLabel = new System.Windows.Forms.Label();
            this.stateLabel = new System.Windows.Forms.Label();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.findText = new System.Windows.Forms.TextBox();
            this.replaceText = new System.Windows.Forms.TextBox();
            this.findAndReplaceGroup = new System.Windows.Forms.GroupBox();
            this.replaceLabel = new System.Windows.Forms.Label();
            this.findLabel = new System.Windows.Forms.Label();
            this.findAndReplaceGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // workflowList
            // 
            this.workflowList.CheckOnClick = true;
            this.workflowList.FormattingEnabled = true;
            this.workflowList.Location = new System.Drawing.Point(12, 42);
            this.workflowList.Name = "workflowList";
            this.workflowList.Size = new System.Drawing.Size(360, 169);
            this.workflowList.TabIndex = 0;
            this.workflowList.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.workflowList_ItemCheck);
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(297, 351);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 23);
            this.startButton.TabIndex = 2;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // clearButton
            // 
            this.clearButton.Location = new System.Drawing.Point(216, 351);
            this.clearButton.Name = "clearButton";
            this.clearButton.Size = new System.Drawing.Size(75, 23);
            this.clearButton.TabIndex = 1;
            this.clearButton.Text = "Clear";
            this.clearButton.UseVisualStyleBackColor = true;
            this.clearButton.Click += new System.EventHandler(this.clearButton_Click);
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(13, 322);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(359, 23);
            this.progressBar.TabIndex = 4;
            // 
            // countLabel
            // 
            this.countLabel.AutoSize = true;
            this.countLabel.Location = new System.Drawing.Point(10, 348);
            this.countLabel.Name = "countLabel";
            this.countLabel.Size = new System.Drawing.Size(24, 13);
            this.countLabel.TabIndex = 5;
            this.countLabel.Text = "0/0";
            // 
            // stateLabel
            // 
            this.stateLabel.AutoSize = true;
            this.stateLabel.Location = new System.Drawing.Point(12, 13);
            this.stateLabel.Name = "stateLabel";
            this.stateLabel.Size = new System.Drawing.Size(24, 13);
            this.stateLabel.TabIndex = 6;
            this.stateLabel.Text = "Idle";
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // openFileDialog
            // 
            this.openFileDialog.CheckPathExists = false;
            this.openFileDialog.InitialDirectory = "Y:\\Files\\Music\\add";
            this.openFileDialog.Title = "Select Track Name File";
            // 
            // findText
            // 
            this.findText.Location = new System.Drawing.Point(62, 30);
            this.findText.Name = "findText";
            this.findText.Size = new System.Drawing.Size(290, 20);
            this.findText.TabIndex = 8;
            this.findText.TextChanged += new System.EventHandler(this.findText_TextChanged);
            // 
            // replaceText
            // 
            this.replaceText.Location = new System.Drawing.Point(62, 60);
            this.replaceText.Name = "replaceText";
            this.replaceText.Size = new System.Drawing.Size(290, 20);
            this.replaceText.TabIndex = 9;
            this.replaceText.TextChanged += new System.EventHandler(this.replaceText_TextChanged);
            // 
            // findAndReplaceGroup
            // 
            this.findAndReplaceGroup.Controls.Add(this.replaceLabel);
            this.findAndReplaceGroup.Controls.Add(this.findLabel);
            this.findAndReplaceGroup.Controls.Add(this.replaceText);
            this.findAndReplaceGroup.Controls.Add(this.findText);
            this.findAndReplaceGroup.Location = new System.Drawing.Point(12, 222);
            this.findAndReplaceGroup.Name = "findAndReplaceGroup";
            this.findAndReplaceGroup.Size = new System.Drawing.Size(360, 94);
            this.findAndReplaceGroup.TabIndex = 10;
            this.findAndReplaceGroup.TabStop = false;
            this.findAndReplaceGroup.Text = "Find and replace:";
            // 
            // replaceLabel
            // 
            this.replaceLabel.AutoSize = true;
            this.replaceLabel.Location = new System.Drawing.Point(6, 63);
            this.replaceLabel.Name = "replaceLabel";
            this.replaceLabel.Size = new System.Drawing.Size(50, 13);
            this.replaceLabel.TabIndex = 11;
            this.replaceLabel.Text = "Replace:";
            // 
            // findLabel
            // 
            this.findLabel.Location = new System.Drawing.Point(6, 33);
            this.findLabel.Name = "findLabel";
            this.findLabel.Size = new System.Drawing.Size(50, 13);
            this.findLabel.TabIndex = 10;
            this.findLabel.Text = "Find:";
            this.findLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // iTunesAssistant
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 386);
            this.Controls.Add(this.findAndReplaceGroup);
            this.Controls.Add(this.stateLabel);
            this.Controls.Add(this.countLabel);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.clearButton);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.workflowList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "iTunesAssistant";
            this.Text = "iTunesAssistant";
            this.Load += new System.EventHandler(this.iTunesAssistant_Load);
            this.findAndReplaceGroup.ResumeLayout(false);
            this.findAndReplaceGroup.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.CheckedListBox workflowList;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button clearButton;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label countLabel;
        private System.Windows.Forms.Label stateLabel;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.TextBox findText;
        private System.Windows.Forms.TextBox replaceText;
        private System.Windows.Forms.GroupBox findAndReplaceGroup;
        private System.Windows.Forms.Label replaceLabel;
        private System.Windows.Forms.Label findLabel;
    }
}

