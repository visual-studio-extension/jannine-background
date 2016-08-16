//------------------------------------------------------------------------------
// <copyright file="SoBackgroundPackage.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Windows;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace Jannine.Background.Core
{
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Info on this package for Help/About
    [Guid(SoBackgroundPackage.PackageGuidString)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    [ProvideAutoLoad(UIContextGuids80.SolutionExists)]
    [ProvideAutoLoad(UIContextGuids80.NoSolution)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    public sealed class SoBackgroundPackage : Package
    {
        public const string PackageGuidString = "54542d53-8eae-4322-8424-a1e0f4260324";
        public SoBackgroundPackage()
        {
            // Inside this method you can place any initialization code that does not require
            // any Visual Studio service because at this point the package object is created but
            // not sited yet inside Visual Studio environment. The place to do all the other
            // initialization is the Initialize method.
        }

        protected override void Initialize()
        {
            base.Initialize();
            Application.Current.MainWindow.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e) { }
    }
}