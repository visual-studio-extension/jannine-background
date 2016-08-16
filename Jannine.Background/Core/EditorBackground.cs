using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;

namespace Jannine.Background.Core
{
    [Export(typeof(IWpfTextViewCreationListener))]
    [ContentType("Text")]
    [ContentType("BuildOutput")]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    class Listener : IWpfTextViewCreationListener
    {
        [Export(typeof(AdornmentLayerDefinition))]
        [Name("JannineBackground")]
        [Order(Before = PredefinedAdornmentLayers.DifferenceChanges)]
        public AdornmentLayerDefinition EditorAdornmentLayer { get; set; }

        public void TextViewCreated(IWpfTextView rpTextView)
        {
            new JannineBackground(rpTextView);
        }
    }
}
