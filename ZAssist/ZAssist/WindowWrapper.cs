/// http://www.ryanfarley.com/blog/archive/2004/03/23/465.aspx

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace ZAssist
{
    public class WindowWrapper : IWin32Window
    {
        public WindowWrapper(IntPtr handle)
        {
            _hwnd = handle;
        }

        public IntPtr Handle
        {
            get { return _hwnd; }
        }

        private IntPtr _hwnd;
    }
}
