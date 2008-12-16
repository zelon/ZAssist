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
            m_app = _app;
            InitializeComponent();
            BuildFilesListInSolution();
        }

        protected struct ProjectFileData
        {
            public string m_strFileName;
            public string m_strFullPath;
        }

        protected List<ProjectFileData> m_files = new List<ProjectFileData>();

        private void FindString_TextChanged(object sender, EventArgs e)
        {
            /// 기존의 들어있던 내용은 다 지운다.
            m_lvCandidate.Items.Clear();

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
                    Window w = m_app.OpenFile(EnvDTE.Constants.vsViewKindPrimary, fullpath);

                    if (w != null)
                    {
                        w.SetFocus();
                        w.Activate();

                        Close();
                    }
                }
            }
            else
            {
                System.Diagnostics.Debug.Print("There is no selected item");
            }
        }

        void EnumProjectItems(EnvDTE.ProjectItem item)
        {
            try
            {
                if (item == null) return;

                if (item.Properties == null || item.ProjectItems == null) return;

                if (item.ProjectItems.Count <= 0)
                {
                    ProjectFileData data = new ProjectFileData();
                    data.m_strFileName = item.Name;

                    try
                    {
                        data.m_strFullPath = item.Properties.Item("FullPath").Value.ToString();

                        bool bFound = false;
                        for (int i = 0; i < m_files.Count; ++i)
                        {
                            if (m_files[i].m_strFullPath == data.m_strFullPath)
                            {
                                bFound = true;
                                break;
                            }
                        }

                        /// 같은 파일은 다시 넣지 않는다.
                        if (bFound == false)
                        {
                            m_files.Add(data);
                        }
                    }
                    catch (ArgumentException ex)///< 이 exception 에 걸리는건 item.Name 이 파일이 없는 "리소스 파일" 같은 폴더(혹은 필터)들이다.
                    {
                        System.Diagnostics.Debug.Print("ZAssist : " + ex.Message);
                    }


                }
                else
                {
                    foreach (EnvDTE.ProjectItem subItem in item.ProjectItems)
                    {
                        EnumProjectItems(subItem);
                    }
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.Print("ZAssist : " + e.Message);
            }
        }

        private void BuildFilesListInSolution()
        {
            List<string> ret = new List<string>();

            EnvDTE.Projects proj = m_app.Solution.Projects;
            foreach (EnvDTE.Project obj in proj)
            {
                try
                {
                    if (obj.ProjectItems != null)
                    {
                        foreach (EnvDTE.ProjectItem item in obj.ProjectItems)
                        {
                            try
                            {
                                EnumProjectItems(item);
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.Print(ex.Message);
                            }
                        }
                    }
                }
                catch (Exception eee)
                {
                    System.Diagnostics.Debug.Print("ZAssist : " + eee.Message);
                }
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
    }
}