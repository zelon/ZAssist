using System;
using System.Windows.Forms;
using EnvDTE;
using EnvDTE80;
using System.IO;
using System.Collections.Generic;
using System.Text;
//using Microsoft.VisualStudio;

namespace ZAssist
{
    class ZAssistManager
    {
        internal static void OpenCorrespondingFile(DTE2 _applicationObject)
        {
            if (_applicationObject.ActiveDocument != null)
            {
                string filename = System.IO.Path.GetFullPath(_applicationObject.ActiveDocument.FullName);

                string[] extChain = new string[] { ".h", ".hpp", ".cpp", ".c" };

                for ( int i=0; i<extChain.Length; ++i )
                {
                    if (filename.ToLower().EndsWith(extChain[i]))
                    {
                        int nextIndex = i;

                        for (int j = 0; j < extChain.Length; ++j)
                        {
                            nextIndex = (nextIndex + 1) % extChain.Length;

                            string nextExt = extChain[nextIndex];

                            string newfilename = System.IO.Path.GetDirectoryName(filename) + "\\" + System.IO.Path.GetFileNameWithoutExtension(filename);
                            newfilename += nextExt;

                            if (File.Exists(newfilename))
                            {
                                Window w = _applicationObject.OpenFile(EnvDTE.Constants.vsViewKindPrimary, newfilename);

                                if (w != null)
                                {
                                    w.Activate();
                                    break;
                                }
                            }
                            else
                            {
                                System.Diagnostics.Debug.Print("There is no file : " + newfilename);
                            }
                        }
                        break;
                    }
                }
            }
            else
            {
                System.Diagnostics.Debug.Print("ActiveDocument is null");
            }
        }

        internal static void OpenFileInSolution(DTE2 _applicationObject)
        {
            string strSolutionFileName = _applicationObject.Solution.FileName;
            if (strSolutionFileName.Length <= 0)
            {
                System.Windows.Forms.MessageBox.Show(new WindowWrapper((IntPtr)_applicationObject.MainWindow.HWnd), "Load solution first");
            }
            else
            {
                OpenFileInSolutionForm form = new OpenFileInSolutionForm(_applicationObject);
                form.Show(new WindowWrapper((IntPtr)_applicationObject.MainWindow.HWnd));
            }
        }

        internal static void OpenSolutionFolderCmd(DTE2 _applicationObject)
        {
            string strSolutionFileName = _applicationObject.Solution.FileName;
            if (strSolutionFileName.Length <= 0)
            {
                System.Windows.Forms.MessageBox.Show("Load solution first");
            }
            else
            {
                string openDir = System.IO.Path.GetDirectoryName(strSolutionFileName);

                System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo("cmd.exe");
                info.WorkingDirectory = openDir;
                System.Diagnostics.Process.Start(info);
            }
        }

        internal static void OpenSolutionFolderExplorer(DTE2 _applicationObject)
        {
            string strSolutionFileName = _applicationObject.Solution.FileName;
            if (strSolutionFileName.Length <= 0)
            {
                System.Windows.Forms.MessageBox.Show("Load solution first");
            }
            else
            {
                string openDir = System.IO.Path.GetDirectoryName(strSolutionFileName);
                System.Diagnostics.Process.Start(openDir);
            }
        }

		internal static void QuickFindFunction(DTE2 _applicationObject)
		{
            /*
			VCCodeModel vcCM = null;
			VCCodeElement vcCodeElement = null;

            vcCM = ((Microsoft.VisualStudio.VCCodeModel.VCCodeModel)(_applicationObject.Solution.Item(1).CodeModel));

            foreach (Microsoft.VisualStudio.VCCodeModel.VCCodeElement temp in vcCM.CodeElements)
			{
				vcCodeElement = temp;
				System.Diagnostics.Debug.Print(vcCodeElement.Name + " is declared in "
				  + vcCodeElement.get_Location(vsCMWhere.vsCMWhereDefault));

				if (vcCodeElement.Name.StartsWith("Get"))
				{
					int k = 0;
					++k;
				}
			}
             * */
		}
	}
}
