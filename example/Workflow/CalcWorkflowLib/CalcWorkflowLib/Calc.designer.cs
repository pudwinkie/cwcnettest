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

namespace CalcWorkflowLib
{
	partial class Calc
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
            System.Workflow.Activities.Rules.RuleConditionReference ruleconditionreference2 = new System.Workflow.Activities.Rules.RuleConditionReference();
            System.Workflow.Activities.Rules.RuleConditionReference ruleconditionreference3 = new System.Workflow.Activities.Rules.RuleConditionReference();
            System.Workflow.Activities.Rules.RuleConditionReference ruleconditionreference4 = new System.Workflow.Activities.Rules.RuleConditionReference();
            this.codeActivity5 = new System.Workflow.Activities.CodeActivity();
            this.codeActivity3 = new System.Workflow.Activities.CodeActivity();
            this.codeActivity4 = new System.Workflow.Activities.CodeActivity();
            this.codeActivity1 = new System.Workflow.Activities.CodeActivity();
            this.codeActivity2 = new System.Workflow.Activities.CodeActivity();
            this.ifElseBranchActivity1 = new System.Workflow.Activities.IfElseBranchActivity();
            this.divAction = new System.Workflow.Activities.IfElseBranchActivity();
            this.mulAction = new System.Workflow.Activities.IfElseBranchActivity();
            this.subAction = new System.Workflow.Activities.IfElseBranchActivity();
            this.addAction = new System.Workflow.Activities.IfElseBranchActivity();
            this.ifElseActivity1 = new System.Workflow.Activities.IfElseActivity();
            // 
            // codeActivity5
            // 
            this.codeActivity5.Name = "codeActivity5";
            this.codeActivity5.ExecuteCode += new System.EventHandler(this.codeActivity5_ExecuteCode);
            // 
            // codeActivity3
            // 
            this.codeActivity3.Name = "codeActivity3";
            this.codeActivity3.ExecuteCode += new System.EventHandler(this.codeActivity3_ExecuteCode);
            // 
            // codeActivity4
            // 
            this.codeActivity4.Name = "codeActivity4";
            this.codeActivity4.ExecuteCode += new System.EventHandler(this.codeActivity4_ExecuteCode);
            // 
            // codeActivity1
            // 
            this.codeActivity1.Name = "codeActivity1";
            this.codeActivity1.ExecuteCode += new System.EventHandler(this.codeActivity1_ExecuteCode);
            // 
            // codeActivity2
            // 
            this.codeActivity2.Name = "codeActivity2";
            this.codeActivity2.ExecuteCode += new System.EventHandler(this.codeActivity2_ExecuteCode);
            // 
            // ifElseBranchActivity1
            // 
            this.ifElseBranchActivity1.Activities.Add(this.codeActivity5);
            this.ifElseBranchActivity1.Name = "ifElseBranchActivity1";
            // 
            // divAction
            // 
            this.divAction.Activities.Add(this.codeActivity3);
            ruleconditionreference1.ConditionName = "除";
            this.divAction.Condition = ruleconditionreference1;
            this.divAction.Name = "divAction";
            // 
            // mulAction
            // 
            this.mulAction.Activities.Add(this.codeActivity4);
            ruleconditionreference2.ConditionName = "乘";
            this.mulAction.Condition = ruleconditionreference2;
            this.mulAction.Name = "mulAction";
            // 
            // subAction
            // 
            this.subAction.Activities.Add(this.codeActivity1);
            ruleconditionreference3.ConditionName = "相加";
            this.subAction.Condition = ruleconditionreference3;
            this.subAction.Name = "subAction";
            // 
            // addAction
            // 
            this.addAction.Activities.Add(this.codeActivity2);
            ruleconditionreference4.ConditionName = "相減";
            this.addAction.Condition = ruleconditionreference4;
            this.addAction.Name = "addAction";
            // 
            // ifElseActivity1
            // 
            this.ifElseActivity1.Activities.Add(this.addAction);
            this.ifElseActivity1.Activities.Add(this.subAction);
            this.ifElseActivity1.Activities.Add(this.mulAction);
            this.ifElseActivity1.Activities.Add(this.divAction);
            this.ifElseActivity1.Activities.Add(this.ifElseBranchActivity1);
            this.ifElseActivity1.Name = "ifElseActivity1";
            // 
            // Calc
            // 
            this.Activities.Add(this.ifElseActivity1);
            this.Name = "Calc";
            this.CanModifyActivities = false;

		}

		#endregion

        private IfElseBranchActivity subAction;
        private IfElseBranchActivity addAction;
        private CodeActivity codeActivity1;
        private CodeActivity codeActivity2;
        private IfElseBranchActivity divAction;
        private IfElseBranchActivity mulAction;
        private CodeActivity codeActivity3;
        private CodeActivity codeActivity4;
        private CodeActivity codeActivity5;
        private IfElseBranchActivity ifElseBranchActivity1;
        private IfElseActivity ifElseActivity1;












    }
}
