using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace TestDataGridView
{
    /// <summary>
    /// ManageDataGridview
    /// 管理dataGridView中button
    /// 
    /// 修改纪录
    ///		2008.03.16版本：1.0 WangXiJun 最初开发
    ///		
    /// 版本：1.0
    ///
    /// <author>
    ///		<name>WangXiJun</name>
    ///		<date>2008.03.16</date>
    /// </author> 
    /// </summary>
    class ManageDgv
    {
        DataGridView dataGridView;
        Timer mytimer = new Timer();

        private Dictionary<int, MergeColumnsInfo> MergeColumns = new Dictionary<int, MergeColumnsInfo>();    // 需要合并的列

        public ManageDgv(DataGridView paramDgv)
        {
            dataGridView = paramDgv;
            mytimer.Tick += new EventHandler(mytimer_Tick);
            mytimer.Interval = 100;
            dataGridView.CellPainting += new DataGridViewCellPaintingEventHandler(DataGridView_CellPainting);
            dataGridView.Scroll += new ScrollEventHandler(DataGridView_Scroll);
            dataGridView.ColumnWidthChanged += new DataGridViewColumnEventHandler(dataGridView_ColumnWidthChanged);
        }

        void dataGridView_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ReDrawColumns();
            if (MergeColumns.ContainsKey(e.Column.Index) && MergeColumns[e.Column.Index].Position == 3)  // 被合并的列          
            {
                if (MergeColumns[e.Column.Index].Flag)
                {
                    int width = this.dataGridView.Columns[e.Column.Index].Width;
                    int sub = width - MergeColumns[e.Column.Index].WidthInfo;
                    this.dataGridView.Columns[e.Column.Index - 1].Width += sub;
                    MergeColumns[e.Column.Index].Flag = false;
                    this.dataGridView.Columns[e.Column.Index].Width -= sub;
                }
                else
                {
                    MergeColumns[e.Column.Index].Flag = true;
                }
            }
        }

        private class MergeColumnsInfo  // 合并列的信息
        {
            public MergeColumnsInfo(string paramText, int paramPosition, int paramLeft, int paramRight, int paramRowNum)
            {
                this.Text = paramText;
                this.Position = paramPosition;
                this.Left = paramLeft;
                this.Right = paramRight;
                this.RowNum = paramRowNum;
                this.Flag = true;
                this.WidthInfo = 0;
            }

            public string Text;     // 列信息
            public int Position;    // 位置，1:左，2中，3右
            public int Left;        // 对应左列
            public int Right;       // 对应右列
            public int RowNum;      // 对应行数
            public bool Flag;       // 改变列宽时用的
            public int WidthInfo;   // 记录原来的列宽

        }

        public void AddMergeColumns(int paramColIndex, int paramColCount, string paramText, int paramRowNum)
        {
            if (paramColCount < 2)
                throw new Exception("行宽应大于等于2，合并1列无意义。");

            // 检查范围
            for (int i = 0; i < paramColCount; i++)
            {
                if (MergeColumns.ContainsKey(paramColIndex + i))
                    throw new Exception("单元格范围重叠!");
            }

            // 将这些列加入列表
            int myRight = paramColIndex + paramColCount - 1;    // 最后一列的索引
            MergeColumns[paramColIndex] = new MergeColumnsInfo(paramText, 1, paramColIndex, myRight, paramRowNum);    // 添加最左列
            MergeColumns[myRight] = new MergeColumnsInfo(paramText, 3, paramColIndex, myRight, paramRowNum);   // 添加最右列
            MergeColumns[myRight].WidthInfo = this.dataGridView.Columns[myRight].Width;

            for (int i = paramColIndex + 1; i < myRight; i++)  // 中间的列
            {
                MergeColumns[i] = new MergeColumnsInfo(paramText, 2, paramColIndex, myRight, paramRowNum);
            }
        }

        public void ClearMergeColumnsInfo()
        {
            MergeColumns.Clear();
        }

        private void DataGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            PaintColumnsMerge(sender, e);
        }

        private void PaintColumnsMerge(object sender, DataGridViewCellPaintingEventArgs e)
        {

            if (MergeColumns.ContainsKey(e.ColumnIndex))  // 被合并的列
            {
                if (MergeColumns[e.ColumnIndex].RowNum == e.RowIndex)
                {
                    int left = e.CellBounds.Left, top = e.CellBounds.Top + 1,
                        right = e.CellBounds.Right, bottom = e.CellBounds.Bottom - 4;
                    // 画边框
                    Graphics g = e.Graphics;
                    e.Paint(e.CellBounds, DataGridViewPaintParts.Background | DataGridViewPaintParts.Border);
                    switch (MergeColumns[e.ColumnIndex].Position)
                    {
                        case 1:
                            left += 0;
                            break;
                        case 2:
                            break;
                        case 3:
                            right -= 1;
                            break;
                    }
                    // 画底色
                    g.FillRectangle(new SolidBrush(e.CellStyle.BackColor), left, top, right - left, bottom - top);
                    // 字体格式
                    StringFormat sf = new StringFormat();
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Center;
                    {
                        left = this.dataGridView.GetColumnDisplayRectangle(MergeColumns[e.ColumnIndex].Left, true).Left - 1;
                        if (left < 0) left = this.dataGridView.GetCellDisplayRectangle(-1, -1, true).Width;
                        right = this.dataGridView.GetColumnDisplayRectangle(MergeColumns[e.ColumnIndex].Right, true).Right - 1;
                        if (right < 0) right = this.dataGridView.Width;
                        g.DrawString(MergeColumns[e.ColumnIndex].Text, e.CellStyle.Font, Brushes.Black,
                            new Rectangle(left, top, right - left, bottom - top), sf);
                    }
                    e.Handled = true;
                }
            }
        }

        private void DataGridView_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.ScrollOrientation == ScrollOrientation.HorizontalScroll || e.ScrollOrientation == ScrollOrientation.VerticalScroll)// && e.Type == ScrollEventType.EndScroll)
            {
                mytimer.Enabled = false;
                mytimer.Enabled = true;
            }
        }

        public void ReDrawColumns()
        {
            foreach (int j in MergeColumns.Keys)
            {
                this.dataGridView.Invalidate(this.dataGridView.GetCellDisplayRectangle(j, MergeColumns[j].RowNum, true));
            }
        }

        private void mytimer_Tick(object sender, EventArgs e)
        {
            mytimer.Enabled = false;
            ReDrawColumns();
        }
    }
}
