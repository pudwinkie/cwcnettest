using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Workflow.Runtime;
using System.Workflow.Activities;

namespace SimpleExpenseReport {
    public partial class Form1 : Form, IExpenseReportService {
        private WorkflowRuntime workflowRuntime = null;
        private WorkflowInstance workflowInstance = null;

        private delegate void GetApprovalDelegate(string message);
        private event EventHandler<ExternalDataEventArgs> reportApproved;
        private event EventHandler<ExternalDataEventArgs> reportRejected;

        private ExternalDataExchangeService exchangeService = null;


        public event EventHandler<ExternalDataEventArgs> ExpenseReportApproved
        {
            add
            {
                reportApproved += value;
            }
            remove
            {
                reportApproved -= value;
            }
        }

         
        public event EventHandler<ExternalDataEventArgs> ExpenseReportRejected
        {
            add
            {
                reportRejected += value;
            }
            remove
            {
                reportRejected -= value;
            }
        }





        public Form1() {
            InitializeComponent();

            workflowRuntime = new WorkflowRuntime();
            workflowRuntime.WorkflowCompleted += new EventHandler<WorkflowCompletedEventArgs>(workflowRuntime_WorkflowCompleted);

            this.exchangeService = new ExternalDataExchangeService();
            workflowRuntime.AddService(exchangeService);
            exchangeService.AddService(this);


            this.workflowRuntime.StartRuntime();


        }

        void workflowRuntime_WorkflowCompleted(object sender, WorkflowCompletedEventArgs e) {
            //throw new NotImplementedException();
            if (result.InvokeRequired) {
                result.Invoke(new EventHandler<WorkflowCompletedEventArgs>(workflowRuntime_WorkflowCompleted));
            }else{
                this.result.Text = e.OutputParameters["Result"].ToString();

                // Clear fields
                this.amount.Text = string.Empty;

                // Disable buttons
                //this.approveButton.Enabled = false;
                //this.rejectButton.Enabled = false;

            }
        }


        private void button1_Click(object sender, EventArgs e) {
            Type type = typeof(SimpleExpenseReport);
            Dictionary<string, object> properties = new Dictionary<string, object>();
            properties.Add("Amount", Int32.Parse(this.amount.Text));


            this.workflowInstance = workflowRuntime.CreateWorkflow(type, properties);
            this.workflowInstance.Start();

        }

        public void GetLeadApproval(string message) {
            if (this.approvalState.InvokeRequired)
                this.approvalState.Invoke(new GetApprovalDelegate
                    (this.GetLeadApproval), message);
            else {
                this.approvalState.Text = message;
                this.approveButton.Enabled = true;
                this.rejectButton.Enabled = true;

                // expand the panel
                this.Height = this.MinimumSize.Height + this.panel1.Height;
                this.submitButton.Enabled = false;
            }

        }

        public void GetManagerApproval(string message) {
            if (this.approvalState.InvokeRequired)
                this.approvalState.Invoke(new GetApprovalDelegate
                    (this.GetManagerApproval), message);
            else {
                this.approvalState.Text = message;
                this.approveButton.Enabled = true;
                this.rejectButton.Enabled = true;

                // expand the panel
                this.Height = this.MinimumSize.Height + this.panel1.Height;
                this.submitButton.Enabled = false;
            }

        }


    }
}
