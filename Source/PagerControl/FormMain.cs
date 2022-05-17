using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace PagerControl
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }
        bool bexit = false;
        int rowcount = 4; //默认数据行数
        int sleeptime = 3000; //间隔时间 mm;
        private void FormMain_Load(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(o =>
            {
                while (!bexit)
                {
                    DataTable dt = SQLHelper.Instance.GetDataTable("select * from t_JQ");
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        //dgvList.DataSource = dt;
                        int start = 0;
                        while (true && !bexit)
                        {
                            if (start >= dt.Rows.Count)
                            {
                                break;
                            }
                            DataTable dtNew = dt.Clone();
                            List<DataRow> list = new List<DataRow>();
                            for (int i = start; i < start + rowcount && i < dt.Rows.Count; i++)
                            {
                                DataRow dr = dtNew.NewRow();
                                CopyDataRow(dt.Rows[i], dr);
                                dtNew.Rows.Add(dr);
                                list.Add(dr);
                            }
                            ShowLed(list);
                            dgvList.Invoke((EventHandler)(delegate
                            {
                                dgvList.DataSource = dtNew;

                            }));

                            start += rowcount;
                            Thread.Sleep(sleeptime);
                        }
                    }
                    Thread.Sleep(200);
                }

            }, 0);
            
        }
        private void CopyDataRow(DataRow sourceRow, DataRow destRow)
        {
            if (sourceRow == null || destRow == null)
                return;
            foreach (DataColumn aDataColumn in sourceRow.Table.Columns)
            {
                try
                {
                    destRow[aDataColumn.ColumnName] = sourceRow[aDataColumn.ColumnName];
                }
                catch
                { }
            }
        }

        public void ShowLed(List<DataRow> list)
        {
            //读取List 里边的行数显示在Led
        }
    }
}
