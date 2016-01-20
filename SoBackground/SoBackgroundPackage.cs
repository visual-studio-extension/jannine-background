//------------------------------------------------------------------------------
// <copyright file="SoBackgroundPackage.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.Win32;

namespace SoBackground
{
    public class OptionPageGrid : DialogPage
    {
        private string _ImagePath;

        [Category("SoBackground")]
        [DisplayName("Path")]
        [Description("Set the path of background, takes effect after restarting Visual Studio")]
        public string ImagePath
        {
            get
            {
                return _ImagePath;
            }
            set
            {
                _ImagePath = value;
            }
        }

        /// <summary>
        /// This is the class that implements the package exposed by this assembly.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The minimum requirement for a class to be considered a valid package for Visual Studio
        /// is to implement the IVsPackage interface and register itself with the shell.
        /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
        /// to do it: it derives from the Package class that provides the implementation of the
        /// IVsPackage interface and uses the registration attributes defined in the framework to
        /// register itself and its components with the shell. These attributes tell the pkgdef creation
        /// utility what data to put into .pkgdef file.
        /// </para>
        /// <para>
        /// To get loaded into VS, the package must be referred by &lt;Asset Type="Microsoft.VisualStudio.VsPackage" ...&gt; in .vsixmanifest file.
        /// </para>
        /// </remarks>
        [PackageRegistration(UseManagedResourcesOnly = true)]
        [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Info on this package for Help/About
        [Guid(SoBackgroundPackage.PackageGuidString)]
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
        [ProvideAutoLoad(UIContextGuids80.SolutionExists)]
        [ProvideAutoLoad(UIContextGuids80.NoSolution)]
        [ProvideMenuResource("Menus.ctmenu", 1)]
        [ProvideOptionPage(typeof(OptionPageGrid),
        "SoBackground", "Settings", 0, 0, true)]
        public sealed class SoBackgroundPackage : Package
        {
            public string ImagePath
            {
                get
                {
                    OptionPageGrid OptionPageGrid = (OptionPageGrid)GetDialogPage(typeof(OptionPageGrid));
                    return OptionPageGrid.ImagePath;
                }
            }

            /// <summary>
            /// SoBackgroundPackage GUID string.
            /// </summary>
            public const string PackageGuidString = "54542d53-8eae-4322-8424-a1e0f4260324";

            /// <summary>
            /// Initializes a new instance of the <see cref="SoBackgroundPackage"/> class.
            /// </summary>
            public SoBackgroundPackage()
            {
                // Inside this method you can place any initialization code that does not require
                // any Visual Studio service because at this point the package object is created but
                // not sited yet inside Visual Studio environment. The place to do all the other
                // initialization is the Initialize method.
            }

            #region Package Members

            /// <summary>
            /// Initialization of the package; this method is called right after the package is sited, so this is the place
            /// where you can put all the initialization code that rely on services provided by VisualStudio.
            /// </summary>
            protected override void Initialize()
            {
                base.Initialize();
                Application.Current.MainWindow.Loaded += MainWindow_Loaded;
            }

            private void MainWindow_Loaded(object sender, RoutedEventArgs e)
            {
                Window Window = (Window)sender;

                BitmapFrame BitmapFrame;
                try
                {
                    BitmapFrame = BitmapFrame.Create(new Uri(ImagePath), BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                }
                catch (Exception ex)
                {
                    BitmapFrame = BitmapFrame.Create(new Uri(Path.Combine(Environment.CurrentDirectory, "DefaultBackground.jpg")), BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                }
                BitmapFrame.Freeze();

                Image rImageControl = new Image()
                {
                    Source = BitmapFrame,
                    Stretch = Stretch.UniformToFill,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                };

                Grid.SetRowSpan(rImageControl, 4);
                Grid RootGrid = (Grid)Window.Template.FindName("RootGrid", Window);
                RootGrid.Children.Insert(0, rImageControl);
            }

            #endregion
        }
    }
}
