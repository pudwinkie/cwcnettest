using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Reflection;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;

namespace WorkflowConsoleApplication1
{
	partial class Workflow1
	{
		#region Designer generated code
		
		/// <summary> 
		/// 這是設計工具支援所需要的方法 - 請勿使用程式碼編輯器
		/// 修改這個方法的內容。
		/// </summary>
        [System.Diagnostics.DebuggerNonUserCode]
		private void InitializeComponent()
		{
            this.CanModifyActivities = true;
            System.Workflow.Activities.Rules.RuleConditionReference ruleconditionreference1 = new System.Workflow.Activities.Rules.RuleConditionReference();
            System.Workflow.Activities.CodeCondition codecondition1 = new System.Workflow.Activities.CodeCondition();
            this.Print = new System.Workflow.Activities.CodeActivity();
            this.is_even = new System.Workflow.Activities.IfElseBranchActivity();
            this.ifElseBranchActivity1 = new System.Workflow.Activities.IfElseBranchActivity();
            this.Increment = new System.Workflow.Activities.CodeActivity();
            this.ifElseActivity1 = new System.Workflow.Activities.IfElseActivity();
            this.sequenceActivity1 = new System.Workflow.Activities.SequenceActivity();
            this.whileActivity1 = new System.Workflow.Activities.WhileActivity();
            // 
            // Print
            // 
            this.Print.Name = "Print";
            this.Print.ExecuteCode += new System.EventHandler(this.codeActivity1_ExecuteCode_2);
            // 
            // is_even
            // 
            this.is_even.Activities.Add(this.Print);
            ruleconditionreference1.ConditionName = "條件1";
            this.is_even.Condition = ruleconditionreference1;
            this.is_even.Name = "is_even";
            // 
            // ifElseBranchActivity1
            // 
            this.ifElseBranchActivity1.Enabled = false;
            this.ifElseBranchActivity1.Name = "ifElseBranchActivity1";
            // 
            // Increment
            // 
            this.Increment.Description = "變數累加";
            this.Increment.Name = "Increment";
            this.Increment.ExecuteCode += new System.EventHandler(this.codeActivity2_ExecuteCode_1);
            // 
            // ifElseActivity1
            // 
            this.ifElseActivity1.Activities.Add(this.ifElseBranchActivity1);
            this.ifElseActivity1.Activities.Add(this.is_even);
            this.ifElseActivity1.Name = "ifElseActivity1";
            // 
            // sequenceActivity1
            // 
            this.sequenceActivity1.Activities.Add(this.ifElseActivity1);
            this.sequenceActivity1.Activities.Add(this.Increment);
            this.sequenceActivity1.Name = "sequenceActivity1";
            // 
            // whileActivity1
            // 
            this.whileActivity1.Activities.Add(this.sequenceActivity1);
            codecondition1.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.while_true_condition);
            this.whileActivity1.Condition = codecondition1;
            this.whileActivity1.Name = "whileActivity1";
            // 
            // Workflow1
            // 
            this.Activities.Add(this.whileActivity1);
            this.Name = "Workflow1";
            this.Initialized += new System.EventHandler(this.wk_init);
            this.Completed += new System.EventHandler(this.wk_complete);
            this.CanModifyActivities = false;

		}

		#endregion

        private IfElseBranchActivity is_even;
        private IfElseBranchActivity ifElseBranchActivity1;
        private IfElseActivity ifElseActivity1;
        private SequenceActivity sequenceActivity1;
        private CodeActivity Increment;
        private CodeActivity Print;
        private WhileActivity whileActivity1;



















    }
}
