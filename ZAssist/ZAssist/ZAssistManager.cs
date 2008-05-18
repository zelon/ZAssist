using System;
using EnvDTE;
using EnvDTE80;
using System.IO;
using System.Collections.Generic;
using System.Text;

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
    }
}
