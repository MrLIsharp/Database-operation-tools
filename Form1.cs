using DataBaseToolByLjj.DataCommon.CreatClassBySql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataBaseToolByLjj
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            txt_connstr.Text = "Data Source=192.168.155.178;Initial Catalog=test2;Persist Security Info=True;User ID=sa;Password=stwcb";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            conn_btn.Enabled = false;
            CreateBySqlServer cb = new CreateBySqlServer(txt_connstr.Text);
            var dt = cb.GetTableName();
            dataGridView1.DataSource = dt;
            AddBtndgv2();

            //try
            //{
            //    CreateBySqlServer.BatchCreateModel();
            //    MessageBox.Show("Success");
            //}
            //catch(Exception ex)
            //{
            //    // MessageBox.Show("ERROR");
            //    throw ex;
            //}

        }
        /// <summary>
        /// dataGridView 添加点击事件
        /// </summary>
        public void AddBtndgv2()
        {
            DataGridViewButtonColumn btn = new DataGridViewButtonColumn();
            btn.Name = "表字段";//添加按钮的名字
            btn.HeaderText = "操作";//添加按钮列的列名称
            btn.DefaultCellStyle.NullValue = "详情";//添加按钮显示的名字
            dataGridView1.Columns.Add(btn);//在dataGridView2的最后一列添加按钮

            //DataGridViewButtonColumn endbtn = new DataGridViewButtonColumn();
            //endbtn.Name = "EndBtn";
            //endbtn.HeaderText = "停止";
            //endbtn.DefaultCellStyle.NullValue = "停止";

            //dataGridView1.Columns.Insert(11, endbtn);//在dataGridView2的指定列添加按钮
        }

        private void btn_creatClass_Click(object sender, EventArgs e)
        {
            btn_creatClass.Enabled = false;
            var filepath = string.Empty;
            if (this.folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                if (this.folderBrowserDialog1.SelectedPath.Trim() != "")
                    filepath = this.folderBrowserDialog1.SelectedPath.Trim();
            }
            CreateBySqlServer cb = new CreateBySqlServer(txt_connstr.Text, filepath);
            cb.BatchCreateModel();
            MessageBox.Show("生成成功");
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dataGridView1.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex > -1)
                {
                    if (this.dataGridView1.CurrentCell.FormattedValue.ToString() == "详情")
                    {
                        //获得当前选中的行   
                        int rowindex = e.RowIndex;
                        //获得选中行中列名为"Column_pe_unit_id"的值
                        string tablename = dataGridView1.Rows[rowindex].Cells["表名"].Value.ToString();
                        CreateBySqlServer cb = new CreateBySqlServer(txt_connstr.Text);
                        //dataGridView2.Columns.Remove("表字段");
                        dataGridView2.DataSource= cb.TableInfoByTableName(tablename);
                    }
                    //else if (this.dataGridView_main.CurrentCell.FormattedValue.ToString() == "删除")
                    //{
                    //    string sMsg = String.Format("确定要删除吗？");
                    //    if (MessageBox.Show(sMsg, "确认删除体检单位", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                    //    {
                    //        //获得当前选中的行   
                    //        int rowindex = e.RowIndex;
                    //        //获得选中行中列名为"Column_pe_unit_id"的值
                    //        int unitid = Convert.ToInt32(dataGridView_main.Rows[rowindex].Cells["Column_pe_unit_id"].Value);
                    //        WebResult<bool> result = PeBookingMgr.DelPeUnit(new pe_unit() { pe_unit_id = unitid });
                    //        if (WebResultCode.CALL_SUCCESS.REquals(result.code) && result.data)
                    //        {
                    //            MessageBox.Show("删除成功！");
                    //            Init();
                    //        }
                    //        else
                    //        {
                    //            MessageBox.Show("删除失败！");
                    //        }
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "分诊工作站", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
