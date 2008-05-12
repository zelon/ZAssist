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
            m_lvCandidate.Items.Clear();

            foreach (ProjectFileData data in m_files)
            {
                if (data.m_strFileName.ToLower().Contains(m_tbFindString.Text.ToLower()))
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
            if (item == null) return;

            if (item.Properties == null || item.ProjectItems == null) return;

            if ( item.ProjectItems.Count <= 0)
            {
                ProjectFileData data = new ProjectFileData();
                data.m_strFileName = item.Name;
                data.m_strFullPath = item.Properties.Item("FullPath").Value.ToString();

                m_files.Add(data);
            }
            else
            {
                foreach (EnvDTE.ProjectItem subItem in item.ProjectItems)
                {
                    EnumProjectItems(subItem);
                }
            }
        }

        private void BuildFilesListInSolution()
        {
            List<string> ret = new List<string>();

            EnvDTE.Projects proj = m_app.Solution.Projects;
            foreach (EnvDTE.Project obj in proj)
            {
                foreach (EnvDTE.ProjectItem item in obj.ProjectItems)
                {
                    EnumProjectItems(item);
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
    }
}