
using System.Windows.Controls;
using System.Windows.Threading;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows.Media;
using System;
using System.Windows.Media.Imaging;

namespace Jannine.Background.Core
{
    public class JannineBackground
    {
        private readonly IWpfTextView _view;
        private readonly IAdornmentLayer _adornmentLayer;
        private readonly Dispatcher _dispacher;
        private Canvas _editorCanvas = new Canvas() { IsHitTestVisible = false };
        private Brush _themeViewBackground = null;
        private Brush _themeViewStackBackground = null;
        private bool _isMainWindow;

        public JannineBackground(IWpfTextView view)
        {
            try
            {
                RenderOptions.SetBitmapScalingMode(_editorCanvas, BitmapScalingMode.Fant);

                _dispacher = Dispatcher.CurrentDispatcher;
                _view = view;
                _themeViewBackground = _view.Background;
                _adornmentLayer = view.GetAdornmentLayer("JannineBackground");
                _view.LayoutChanged += (s, e) =>
                {
                    RepositionImage();
                    _isMainWindow = IsRootWindow();
                    SetCanvasBackground(false);
                };
                _view.Closed += (s, e) =>
                {

                };
                _view.BackgroundBrushChanged += (s, e) =>
                {
                    _isMainWindow = IsRootWindow();
                    SetCanvasBackground(false);
                };


                ChangeImage();
                RefreshAdornment();
            }
            catch
            {
            }
        }

        private void InvokeChangeImage(object sender, System.EventArgs e)
        {
            try
            {
                _dispacher.Invoke(ChangeImage);
                GC.Collect();
            }
            catch
            {
            }
        }

        private void ReloadSettings(object sender, System.EventArgs e)
        {
            _isMainWindow = IsRootWindow();
            _dispacher.Invoke(ChangeImage);
        }

        private void ChangeImage()
        {
            try
            {
                SetCanvasBackground(true);

                var newImage = BitmapFrame.Create(new Uri("pack://application:,,,/Jannine.Background;component/Resources/DefaultBackground.png", UriKind.RelativeOrAbsolute));
                var opacity = 0.1;

                _editorCanvas.Background = new ImageBrush(newImage)
                {
                    Opacity = opacity,
                    //Stretch = _setting.ImageStretch.ConvertTo(),
                    //AlignmentX = _setting.PositionHorizon.ConvertTo(),
                    //AlignmentY = _setting.PositionVertical.ConvertTo()
                };
            }
            catch (Exception ex)
            {
            }
        }

        private void RepositionImage()
        {
            try
            {
                Canvas.SetLeft(_editorCanvas, _view.ViewportLeft);
                Canvas.SetTop(_editorCanvas, _view.ViewportTop);

                _editorCanvas.Width = _view.ViewportWidth;
                _editorCanvas.Height = _view.ViewportHeight;
            }
            catch
            {
            }
        }

        private void RefreshAdornment()
        {
            _adornmentLayer.RemoveAdornmentsByTag("JannineBackground");
            _adornmentLayer.AddAdornment(AdornmentPositioningBehavior.ViewportRelative,
                null,
                "JannineBackground",
                _editorCanvas,
                null);
        }

        private void SetCanvasBackground(bool isTransparent)
        {
            var control = (ContentControl)_view;
            var parent = (Grid)control.Parent;
            var viewstack = (Canvas)control.Content;
            var opacity = isTransparent && _isMainWindow ? 0.0 : 0.5;
            if (_themeViewBackground == null)
            {
                _themeViewBackground = _view.Background;
            }
            if (_themeViewStackBackground == null)
            {
                _themeViewStackBackground = viewstack.Background;
            }

            if (isTransparent && _isMainWindow)
            {
                _dispacher.Invoke(() =>
                {
                    try
                    {
                        viewstack.Background = Brushes.Transparent;
                        _view.Background = Brushes.Transparent;
                        var b = _editorCanvas.Background;
                        if (b != null)
                        {
                            b.Opacity = opacity;
                            _editorCanvas.Background = b;
                        }
                        parent?.ClearValue(Grid.BackgroundProperty);
                    }
                    catch
                    {
                    }
                });
            }
            else
            {
                _dispacher.Invoke(() =>
                {
                    try
                    {
                        viewstack.Background = _themeViewStackBackground;
                        _view.Background = _themeViewBackground;
                        var b = _editorCanvas.Background;
                        if (b != null)
                        {
                            b.Opacity = opacity;
                            _editorCanvas.Background = b;
                        }
                    }
                    catch
                    {
                    }
                });
            }
        }

        private bool IsRootWindow()
        {
            var root = FindUI(_view as System.Windows.DependencyObject);
            if (root.GetType().FullName.Equals("Microsoft.VisualStudio.PlatformUI.MainWindow", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private System.Windows.DependencyObject FindUI(System.Windows.DependencyObject d)
        {
            var p = VisualTreeHelper.GetParent(d);
            if (p == null)
            {
                // is root
                return d;
            }
            else
            {
                return FindUI(p);
            }
        }
    }
}
