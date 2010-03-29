using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Workflow.Runtime;
using System.Threading;

namespace CalcWin {
    public partial class Form1 : Form {
        private WorkflowRuntime m_wk_runtime;
        private AutoResetEvent m_event = new AutoResetEvent(false) ;
        private int n1;
        private int n2;
        private string op;
        private double result;

        public Form1() {
            InitializeComponent();

            InitWorkflowRuntime();
        }

        private void Form1_Load(object sender, EventArgs e) {

        }

        /// <summary>
        /// 工作流程環境初始化
        /// </summary>
        private void InitWorkflowRuntime() {
            m_wk_runtime = new WorkflowRuntime();
            m_wk_runtime.WorkflowCompleted += new EventHandler<WorkflowCompletedEventArgs>(m_wk_runtime_WorkflowCompleted);
            m_wk_runtime.WorkflowTerminated += new EventHandler<WorkflowTerminatedEventArgs>(m_wk_runtime_WorkflowTerminated);
        }

        void m_wk_runtime_WorkflowTerminated(object sender, WorkflowTerminatedEventArgs e) {
            MessageBox.Show("Terminate");
            m_event.Set();
        }

        void m_wk_runtime_WorkflowCompleted(object sender, WorkflowCompletedEventArgs e) {
            result = (double)e.OutputParameters["Result"];
            MessageBox.Show("Result: " + result);
            m_event.Set();
        }

        /// <summary>
        /// 數字鍵
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNum_click(object sender, EventArgs e) {
            txtNumber.AppendText(((Button)sender).Text.Trim());

        }

        /// <summary>
        /// 清除鍵
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button11_Click(object sender, EventArgs e) {
            Clear();
        }


        /// <summary>
        /// 運算鍵
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOp_click(object sender, EventArgs e) {
            try {
                n1 = Int32.Parse(txtNumber.Text);
                op = ((Button)sender).Text.Trim();
                txtNumber.Clear();
            }catch(Exception){
                MessageBox.Show("Invalid Opeartion");
            }
        }

        private void Clear() {
            n1 = 0;
            n2 = 0;
            op = String.Empty;
            txtNumber.Clear();
        }

        private void btnResult_Click(object sender, EventArgs e) {
            try {
                n2 = Int32.Parse(txtNumber.Text);
                var args = new Dictionary<string, object>()
                {
                    {"Number1", n1},
                    {"Number2", n2},
                    {"Operation", op}
                };
                WorkflowInstance wfi = m_wk_runtime.CreateWorkflow(typeof(CalcWorkflowLib.Calc), args);
                wfi.Start();
                m_event.WaitOne();
                Clear();                
                txtNumber.Text = result.ToString();
            } catch (Exception) {
                MessageBox.Show("Invalid Opeartion");
            }
        }


    }
}
