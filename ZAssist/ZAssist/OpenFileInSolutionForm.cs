using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using EnvDTE80;
using EnvDTE;

namespace ZAssist
{
    public partial class OpenFileInSolutionForm : Form
    {
        private DTE2 m_app;
        public OpenFileInSolutionForm(DTE2 _app)
        {
            FileCollector.GetInstance().SetDTE(_app);
            FileCollector.GetInstance().StartCollect();

            m_app = _app;
            InitializeComponent();

            m_originalTitle = this.Text;

            UpdateTitle();
        }

        private string m_originalTitle;
        private void UpdateTitle()
        {
            StringBuilder builder = new StringBuilder(m_originalTitle);
            
            if ( FileCollector.GetInstance().Collecting )
            {
                builder.Append(" - collecting - ");
                builder.Append(FileCollector.GetInstance().GetCollectedFileCount());
            }
            else
            {
                builder.Append(" - collected - ");
                builder.Append(FileCollector.GetInstance().GetCollectedFileCount());
            }

            this.Text = builder.ToString();
        }

        private void FindString_TextChanged(object sender, EventArgs e)
        {
            /// 기존의 들어있던 내용은 다 지운다.
            m_lvCandidate.Items.Clear();

            List<ProjectFileData> m_files = FileCollector.GetInstance().GetFiles();

            foreach (ProjectFileData data in m_files)
            {
                string compareStr = data.m_strFileName;

                /// include extension 체크박스가 꺼져 있고, 찾는 문자열에 . 이 포함되어 있지 않으면 파일명에서만 검색한다.
                if (m_cbIncludeExt.Checked == false && m_tbFindString.Text.Contains(".") == false )
                {
                    compareStr = System.IO.Path.GetFileNameWithoutExtension(compareStr);
                }

                if (compareStr.ToLower().Contains(m_tbFindString.Text.ToLower()))
                {
                    ListViewItem item = new ListViewItem(data.m_strFileName);
                    item.SubItems.Add(data.m_strFullPath);
                    m_lvCandidate.Items.Add(item);
                }
            }

            if (m_lvCandidate.Items.Count > 0)
            {
                m_lvCandidate.Items[0].Selected = true;
            }
        }

        private void SelectGo()
        {
            if (m_lvCandidate.SelectedItems.Count > 0)
            {
                string fullpath = m_lvCandidate.SelectedItems[0].SubItems[1].Text;

                if (false == System.IO.File.Exists(fullpath))
                {
                    System.Diagnostics.Debug.Print(fullpath + " does not exist");
                }
                else
                {
                    try
                    {
                        Window w = m_app.OpenFile(EnvDTE.Constants.vsViewKindPrimary, fullpath);

                        if (w != null)
                        {
                            w.SetFocus();
                            w.Activate();

                            Close();
                        }
                    }
                    catch (System.Runtime.InteropServices.COMException ex)
                    {
                        MessageBox.Show(this.Owner, "Cannot open the file");
                    }
                }
            }
            else
            {
                System.Diagnostics.Debug.Print("There is no selected item");
            }
        }


        private void FileCandidateList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            SelectGo();
        }

        private void FileCandidateList_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                Close();
            }
            if (e.KeyChar == (char)Keys.Enter)
            {
                SelectGo();
            }
        }

        private void FindString_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
            }
            if (e.KeyChar == (char)Keys.Enter)
            {
                SelectGo();
            }
        }

        private void FindString_KeyDown(object sender, KeyEventArgs e)
        {
            if (m_lvCandidate.Items.Count <= 0) return;

            if (e.KeyValue == (char)Keys.Down ||
                   e.KeyValue == (char)Keys.Up )
            {
                m_lvCandidate.Focus();
            }
        }

        private void OnIncludeExt_CheckedChanged(object sender, EventArgs e)
        {
            FindString_TextChanged(sender, e);
        }

        private void m_btRefreshStatus_Click(object sender, EventArgs e)
        {
            FileCollector.GetInstance().Recollect();
        }

        private void m_timer_Tick(object sender, EventArgs e)
        {
            UpdateTitle();
        }
    }
}