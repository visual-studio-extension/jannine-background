using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SoBackground
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [Guid("00000000-0000-0000-0000-000000000000")]
    public class OptionPageGrid : DialogPage
    {
        private string _ImagePath;

        [Category("SoBackground")]
        [DisplayName("Image Path")]
        [Description("Path of the image to be set as background")]
        public string ImagePath
        {
            get
            {
                return _ImagePath;
            }
            set
            {
                _ImagePath = value;
                SoBackgroundPackage.LoadImage(value);
            }
        }

        protected override IWin32Window Window
        {
            get
            {
                SettingControl SettingControl = new SettingControl();
                SettingControl.OptionPageGrid = this;
                SettingControl.Initialize();
                return SettingControl;
            }
        }
    }
}
