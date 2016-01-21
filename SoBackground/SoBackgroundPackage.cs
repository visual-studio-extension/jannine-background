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
                    OptionPageGrid OptionPageGrid = GetDialogPage(typeof(OptionPageGrid)) as OptionPageGrid;
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
                //Window Window = sender as Window;
                Image Image = new Image();
                Image.Name = "SoBackground_Image";
                Image.Stretch = Stretch.UniformToFill;
                Image.HorizontalAlignment = HorizontalAlignment.Center;
                Image.VerticalAlignment = VerticalAlignment.Center;

                Grid.SetRowSpan(Image, 4);
                Grid RootGrid = Application.Current.MainWindow.Template.FindName("RootGrid", Application.Current.MainWindow) as Grid;
                RootGrid.Children.Insert(0, Image);

                LoadImage(ImagePath);
            }
            #endregion
            
        public static void LoadImage(string imagePath)
        {
            BitmapFrame BitmapFrame;
            try
            {
                BitmapFrame = BitmapFrame.Create(new Uri(imagePath), BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
            }
            catch (Exception ex)
            {
                //FileStream tempStream = new FileStream(@"D:\log.txt", FileMode.Append);
                //StreamWriter writer = new StreamWriter(tempStream);
                //writer.WriteLine(GetDefaultImagePath() + "\n");
                //writer.Close();
                //tempStream.Close();

                BitmapFrame = BitmapFrame.Create(new Uri(GetDefaultImagePath(), UriKind.RelativeOrAbsolute), BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
            }

            BitmapFrame.Freeze();

            Grid RootGrid = (Grid)Application.Current.MainWindow.Template.FindName("RootGrid", Application.Current.MainWindow);

            Image image = RootGrid.Children[0] as Image;
            if (image != null)
            {
                if (image.Name == "SoBackground_Image")
                {
                    image.Source = BitmapFrame;
                }
            }
        }

        private static string GetDefaultImagePath()
        {
            string path = typeof(SoBackgroundPackage).Assembly.Location.Replace("SoBackground.dll", "DefaultBackground.png");
            return path;
        }

        }
}
