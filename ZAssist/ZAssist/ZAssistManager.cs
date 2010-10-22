using System;
using System.Windows.Forms;
using EnvDTE;
using EnvDTE80;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.VCCodeModel;

namespace ZAssist
{
    class ZAssistManager
    {
        internal static void OpenCorrespondingFile(DTE2 _applicationObject)
        {
            if (_applicationObject.ActiveDocument != null)
            {
                string filename = System.IO.Path.GetFullPath(_applicationObject.ActiveDocument.FullName);

                if (filename.ToLower().EndsWith(".h") || filename.ToLower().EndsWith(".cpp") || filename.ToLower().EndsWith(".c"))
                {
                    string newfilename = System.IO.Path.GetDirectoryName(filename) + "\\" + System.IO.Path.GetFileNameWithoutExtension(filename);

                    switch (System.IO.Path.GetExtension(filename).ToLower())
                    {
                        case ".h":
                            newfilename += ".cpp";
                            break;

                        case ".cpp":
                            newfilename += ".h";
                            break;

                        case ".c":
                            newfilename += ".h";
                            break;
                    }

                    if (File.Exists(newfilename))
                    {
                        Window w = _applicationObject.OpenFile(EnvDTE.Constants.vsViewKindPrimary, newfilename);

                        if (w != null)
                        {
                            w.Activate();
                        }
                    }
                    else
                    {
                        System.Diagnostics.Debug.Print("There is no file : " + newfilename);
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
		}
	}
}
