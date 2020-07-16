namespace DataBaseToolByLjj
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.conn_btn = new System.Windows.Forms.Button();
            this.txt_connstr = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btn_creatClass = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.SuspendLayout();
            // 
            // conn_btn
            // 
            this.conn_btn.Location = new System.Drawing.Point(721, 40);
            this.conn_btn.Name = "conn_btn";
            this.conn_btn.Size = new System.Drawing.Size(75, 23);
            this.conn_btn.TabIndex = 0;
            this.conn_btn.Text = "链接数据库";
            this.conn_btn.UseVisualStyleBackColor = true;
            this.conn_btn.Click += new System.EventHandler(this.button1_Click);
            // 
            // txt_connstr
            // 
            this.txt_connstr.Location = new System.Drawing.Point(108, 42);
            this.txt_connstr.Name = "txt_connstr";
            this.txt_connstr.Size = new System.Drawing.Size(607, 21);
            this.txt_connstr.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "链接字符串： ";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(21, 71);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(322, 446);
            this.dataGridView1.TabIndex = 3;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // btn_creatClass
            // 
            this.btn_creatClass.Location = new System.Drawing.Point(808, 42);
            this.btn_creatClass.Name = "btn_creatClass";
            this.btn_creatClass.Size = new System.Drawing.Size(75, 23);
            this.btn_creatClass.TabIndex = 4;
            this.btn_creatClass.Text = "生成类";
            this.btn_creatClass.UseVisualStyleBackColor = true;
            this.btn_creatClass.Click += new System.EventHandler(this.btn_creatClass_Click);
            // 
            // dataGridView2
            // 
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Location = new System.Drawing.Point(349, 71);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.RowTemplate.Height = 23;
            this.dataGridView2.Size = new System.Drawing.Size(534, 446);
            this.dataGridView2.TabIndex = 5;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(920, 547);
            this.Controls.Add(this.dataGridView2);
            this.Controls.Add(this.btn_creatClass);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txt_connstr);
            this.Controls.Add(this.conn_btn);
            this.Name = "Form1";
            this.Text = "ljj数据库操作工具";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button conn_btn;
        private System.Windows.Forms.TextBox txt_connstr;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btn_creatClass;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.DataGridView dataGridView2;
    }
}

