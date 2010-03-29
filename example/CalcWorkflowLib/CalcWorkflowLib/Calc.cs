using System;
using System.Workflow.Activities;

namespace CalcWorkflowLib
{
	public sealed partial class Calc: SequentialWorkflowActivity
	{
		public Calc()
		{
			InitializeComponent();
		}

        public String Operation { get; set; }
        public int Number1 { get; set; }
        public int Number2 { get; set; }
        public double Result { get; set; }

        private void codeActivity2_ExecuteCode(object sender, EventArgs e) {
            Result = Number1 + Number2;
        }

        private void codeActivity1_ExecuteCode(object sender, EventArgs e) {
            Result = Number1 - Number2;
        }

        private void codeActivity4_ExecuteCode(object sender, EventArgs e) {
            Result = Number1 * Number2;
        }

        private void codeActivity3_ExecuteCode(object sender, EventArgs e) {
            Result = Number1 / Number2;
        }

        private void codeActivity5_ExecuteCode(object sender, EventArgs e) {
            throw new ArgumentException("Invalid Operation");
        }
	}

}
