using System;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using System.Workflow.ComponentModel;


namespace SimpleExpenseReport {
    public class SimpleExpenseReport : SequentialWorkflowActivity {
        private int reportAmount = 0;
        private IfElseActivity evaluateExpenseReportAmount;
        private IfElseBranchActivity ifNeedsLeadApproval;
        private IfElseBranchActivity elseNeedsManagerApproval;
        private CallExternalMethodActivity invokeGetLeadApproval;
        private CallExternalMethodActivity invokeGetManagerApproval;



        public int Amount {
            set {
                this.reportAmount = value;
            }
        }

        private string reportResult = "";

        public string Result {
            get {
                return this.reportResult;
            }
        }


        public SimpleExpenseReport() {
            this.CanModifyActivities = true;
            this.Name = "ExpenseReportWorkflow";
            this.CanModifyActivities = false;

            this.evaluateExpenseReportAmount = new IfElseActivity();
            this.ifNeedsLeadApproval = new IfElseBranchActivity();
            this.elseNeedsManagerApproval = new IfElseBranchActivity();
            this.invokeGetLeadApproval = new CallExternalMethodActivity();
            this.invokeGetManagerApproval = new CallExternalMethodActivity();

            this.invokeGetLeadApproval.InterfaceType = typeof(IExpenseReportService);
            this.invokeGetLeadApproval.MethodName = "GetLeadApproval";
            this.invokeGetLeadApproval.Name = "invokeGetLeadApproval";
            //workflowparameterbinding1.ParameterName = "message";
            //workflowparameterbinding1.Value = "Lead approval needed";
            this.invokeGetLeadApproval.ParameterBindings.Add(workflowparameterbinding1);

            this.invokeGetManagerApproval.InterfaceType = typeof(IExpenseReportService);
            this.invokeGetManagerApproval.MethodName = "GetManagerApproval";
            this.invokeGetManagerApproval.Name = "invokeGetManagerApproval";
            //workflowparameterbinding2.ParameterName = "message";
            //workflowparameterbinding2.Value = "Manager approval needed";
            //this.invokeGetManagerApproval.ParameterBindings.Add
            //    (workflowparameterbinding2);

            this.Activities.Add(this.evaluateExpenseReportAmount);


        }
    }

    [ExternalDataExchange]
    public interface IExpenseReportService {
        void GetLeadApproval(string message);
        void GetManagerApproval(string message);
        event EventHandler<ExternalDataEventArgs> ExpenseReportApproved;
        event EventHandler<ExternalDataEventArgs> ExpenseReportRejected;
    }


}

