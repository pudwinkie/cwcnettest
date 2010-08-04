namespcae chuiwenchiu.thread{
///<example>
/*
public partial class Form1 : Form
    {
        private Form2 mProgressFrom = new Form2();

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AsyncTemplate.DoWorkAsync(
                () =>
                {
                    return doWork(int.Parse(textBox1.Text), int.Parse(textBox2.Text));
                },
                (result) =>
                {
                    MessageBox.Show("Success, Result is " + result.ToString());
                },
                (exception) =>
                {
                    MessageBox.Show(exception.Message);
                    //error handling
                });
        }

        private int doWork(int a, int b)
        {

            Thread.Sleep(10000);

            return a + b;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            AsyncTemplate.OnInvokeStarting =
                () =>
                {
                    mProgressFrom.ShowDialog();
                };

            AsyncTemplate.OnInvokeEnding =
                () =>
                {
                    if (mProgressFrom.InvokeRequired)
                    {
                        mProgressFrom.Invoke(new MethodInvoker(
                            ()
                            =>
                            {
                                mProgressFrom.Close();
                            })
                        );
                    }
                    else
                    {
                        mProgressFrom.Close();
                    }
                };
        }
    }
 */
///</example>
public class AsyncTemplate
    {
        public static Action OnInvokeStarting { get; set; }

        public static Action OnInvokeEnding { get; set; }       

        public static void DoWorkAsync(Action beginAction, Action endAction, Action<Exception> errorAction)
        {            
            ThreadPool.QueueUserWorkItem(new WaitCallback(
                (o) =>
                {
                    try
                    {
                        beginAction();

                        endAction();
                    }
                    catch (Exception ex)
                    {
                        errorAction(ex);
                        return;
                    }
                    finally
                    {
                        if (OnInvokeEnding != null)
                        {
                            OnInvokeEnding();
                        }
                    }
                })
            , null);

            if (OnInvokeStarting != null)
            {
                OnInvokeStarting();
            }
        }

        public static void DoWorkAsync<TResult>(Func<TResult> beginAction, Action<TResult> endAction, Action<Exception> errorAction)
        {            
            ThreadPool.QueueUserWorkItem(new WaitCallback(
                (o) =>
                {
                    TResult result = default(TResult);

                    try
                    {
                        result = beginAction();

                        endAction(result);
                    }
                    catch (Exception ex)
                    {
                        errorAction(ex);
                        return;
                    }
                    finally
                    {
                        if (OnInvokeEnding != null)
                        {
                            OnInvokeEnding();
                        }
                    }
                })
            , null);

            if (OnInvokeStarting != null)
            {
                OnInvokeStarting();
            }
        }
    }
}