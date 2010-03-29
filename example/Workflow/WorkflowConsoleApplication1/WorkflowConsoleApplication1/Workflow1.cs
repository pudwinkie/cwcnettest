using System;
using System.Workflow.Activities;

namespace WorkflowConsoleApplication1
{
	public sealed partial class Workflow1: SequentialWorkflowActivity
	{        
		public Workflow1()
		{
			InitializeComponent();
		}

        private int m_i = 1;
        private int m_sum = 0;
        private void wk_complete(object sender, EventArgs e) {
            Console.WriteLine("total: " +  m_sum);
            Console.ReadKey();
        }

        private void wk_init(object sender, EventArgs e) {
            m_i = 1;
            m_sum = 0;
        }

        private void while_true_condition(object sender, ConditionalEventArgs e) {
            if (m_i <= 9) {                
                e.Result = true;
            } else {
                e.Result = false;
            }            
        }

        private void codeActivity1_ExecuteCode_2(object sender, EventArgs e) {
            m_sum += m_i;
            Console.WriteLine(m_i);            
        }

        private void codeActivity2_ExecuteCode_1(object sender, EventArgs e) {
            ++m_i;
        }
	}

}
