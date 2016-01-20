using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;

namespace SoBackground
{
    [Export(typeof(IWpfTextViewCreationListener))]
    [ContentType("Text")]
    [ContentType("BuildOutput")]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    class Listener : IWpfTextViewCreationListener
    {
        [Import]
        IEditorFormatMapService EditorFormatMapService = null;

        public void TextViewCreated(IWpfTextView rpTextView)
        {
            new EditorBackground(rpTextView);

            //去掉断点边栏的背景
            var rProperties = EditorFormatMapService.GetEditorFormatMap(rpTextView).GetProperties("Indicator Margin");
            rProperties["BackgroundColor"] = Colors.Transparent;
            rProperties["Background"] = Brushes.Transparent;
        }
    }

    class EditorBackground
    {
        IWpfTextView WpfTextView;
        ContentControl ContentControl;
        Grid Grid;
        Canvas Canvas;

        public EditorBackground(IWpfTextView _WpfTextView)
        {
            WpfTextView = _WpfTextView;
            ContentControl = (ContentControl)WpfTextView;
            WpfTextView.Background = Brushes.Transparent;
            WpfTextView.BackgroundBrushChanged += TextView_BackgroundBrushChanged;
            WpfTextView.Closed += TextView_Closed;
            ContentControl.Loaded += TextView_Loaded;
        }
        void MakeBackgroundTransparent()
        {
            WpfTextView.Background = Brushes.Transparent;
            Canvas.Background = Brushes.Transparent;
            Grid.ClearValue(Grid.BackgroundProperty);
        }
        void TextView_Loaded(object sender, RoutedEventArgs e)
        {
            if (Grid == null)
                Grid = (Grid)ContentControl.Parent;
            if (Canvas == null)
                Canvas = (Canvas)ContentControl.Content;
            MakeBackgroundTransparent();
        }
        void TextView_BackgroundBrushChanged(object sender, BackgroundBrushChangedEventArgs e)
        {
            ContentControl.Dispatcher.BeginInvoke(new Action(() =>
            {
                while (Grid.Background != null)
                    MakeBackgroundTransparent();
            }), DispatcherPriority.Render);
        }
        void TextView_Closed(object sender, EventArgs e)
        {
            //清除委托，以防内存泄露
            WpfTextView.Closed -= TextView_Closed;
            WpfTextView.BackgroundBrushChanged -= TextView_BackgroundBrushChanged;
        }
    }
}
