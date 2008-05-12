namespace ZAssist
{
    partial class OpenFileInSolutionForm
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다.
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.m_tbFindString = new System.Windows.Forms.TextBox();
            this.m_lvCandidate = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.SuspendLayout();
            // 
            // m_tbFindString
            // 
            this.m_tbFindString.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_tbFindString.Location = new System.Drawing.Point(0, 0);
            this.m_tbFindString.Name = "m_tbFindString";
            this.m_tbFindString.Size = new System.Drawing.Size(624, 21);
            this.m_tbFindString.TabIndex = 0;
            this.m_tbFindString.TextChanged += new System.EventHandler(this.FindString_TextChanged);
            this.m_tbFindString.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FindString_KeyDown);
            this.m_tbFindString.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FindString_KeyPress);
            // 
            // m_lvCandidate
            // 
            this.m_lvCandidate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_lvCandidate.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.m_lvCandidate.FullRowSelect = true;
            this.m_lvCandidate.GridLines = true;
            this.m_lvCandidate.HideSelection = false;
            this.m_lvCandidate.Location = new System.Drawing.Point(0, 27);
            this.m_lvCandidate.Name = "m_lvCandidate";
            this.m_lvCandidate.Size = new System.Drawing.Size(624, 218);
            this.m_lvCandidate.TabIndex = 1;
            this.m_lvCandidate.UseCompatibleStateImageBehavior = false;
            this.m_lvCandidate.View = System.Windows.Forms.View.Details;
            this.m_lvCandidate.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.FileCandidateList_MouseDoubleClick);
            this.m_lvCandidate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FileCandidateList_KeyPress);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "FileName";
            this.columnHeader1.Width = 160;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Full Path";
            this.columnHeader2.Width = 460;
            // 
            // OpenFileInSolutionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 266);
            this.Controls.Add(this.m_lvCandidate);
            this.Controls.Add(this.m_tbFindString);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "OpenFileInSolutionForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Open File In Solution Dialog";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox m_tbFindString;
        private System.Windows.Forms.ListView m_lvCandidate;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
    }
}