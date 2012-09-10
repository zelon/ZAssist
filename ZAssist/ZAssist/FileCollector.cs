using System;
using System.Threading;
using System.Collections.Generic;
using System.Text;

using EnvDTE80;

namespace ZAssist
{
    class FileCollector
    {
        private FileCollector()
        {
            needReCollect = true;
            m_thread = null;
            m_files = new List<ProjectFileData>();
        }

        public void SetDTE(DTE2 app)
        {
            m_app = app;
        }

        public static FileCollector GetInstance()
        {
            if ( instance == null )
            {
                instance = new FileCollector();
            }
            return instance;
        }

        private DTE2 m_app;
        private static FileCollector instance = null;
        
        private Thread m_thread;

        protected List<ProjectFileData> m_files;
        public List<ProjectFileData> GetFiles() { return m_files; }

        public int GetCollectedFileCount()
        {
            if (m_files != null)
            {
                return m_files.Count;
            }
            return 0;
        }

        private bool collecting;
        public bool Collecting
        {
            get { return collecting; }
            set { collecting = value; }
        }

        private bool needReCollect;
        public void Recollect()
        {
            needReCollect = true;

            if (m_thread != null)
            {
                m_thread.Join();
                m_thread = null;
            }

            StartCollect();
        }
        
        public void StartCollect()
        {
            System.Diagnostics.Debug.Assert(m_app != null);

            if (needReCollect)
            {
                needReCollect = false;

                m_files.Clear();

                m_thread = new Thread(new ThreadStart(startCollect));
                m_thread.Start();
            }
        }

        private void startCollect()
        {
            Collecting = true;

            BuildFilesListInSolution();

            Collecting = false;
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

        private void EnumProjectItems(EnvDTE.ProjectItem item)
        {
            try
            {
                if (item == null) return;

                if (item.SubProject != null)
                {
                    foreach (EnvDTE.ProjectItem subitem in item.SubProject.ProjectItems)
                    {
                        EnumProjectItems(subitem);

                        if (needReCollect) break;
                    }
                }

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
                            if (needReCollect) break;
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
                        if (needReCollect) break;
                        EnumProjectItems(subItem);
                    }
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.Print("ZAssist : " + e.Message);
            }
        }

    }
}
