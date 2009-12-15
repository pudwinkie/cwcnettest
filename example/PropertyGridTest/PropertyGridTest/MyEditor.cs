using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Design;
using System.ComponentModel;
using System.Windows.Forms;

namespace PropertyGridTest {
    public class MyEditor : UITypeEditor {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) {           
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value) {
            
            using (MyEditorDialog dialog = new MyEditorDialog()) {
                if (dialog.ShowDialog() == DialogResult.Yes) {
                    value = dialog.Name;
                }
            }

            return value;

        }
    }

    public class MyEditorDialog : Form {
        public MyEditorDialog()
            : base() {

            
        }

        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);
            FlowLayoutPanel flp = new FlowLayoutPanel();
            flp.Dock = DockStyle.Fill;
            Controls.Add(flp);

            AddRadioButton(flp, "Mavies");
            AddRadioButton(flp, "Chui-Wen Chiu");
            AddRadioButton(flp, "Jasmine");

            Button b = new Button();
            b.Text = "Yes";
            b.Click += new EventHandler(b_Click);
            flp.Controls.Add(b);
            
            b = new Button();
            b.Text = "NO";
            b.Click += new EventHandler(NO_Click    );
            flp.Controls.Add(b);
        }

        private void AddRadioButton(FlowLayoutPanel flp, String text) {
            RadioButton rb = new RadioButton();
            rb.Text = text;
            rb.Click += new EventHandler(rb_Click);
            flp.Controls.Add(rb);
            
        }

        void rb_Click(object sender, EventArgs e) {
            Name = (sender as RadioButton).Text;                        
        }

        void NO_Click(object sender, EventArgs e) {            
            this.DialogResult = DialogResult.No;
        }

        void b_Click(object sender, EventArgs e) {            
            this.DialogResult = DialogResult.Yes; 
        }

        private String m_name;

        public String Name {
            get { return m_name; }
            set { m_name = value; }
        }

    }
}
