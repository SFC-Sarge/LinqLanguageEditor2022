using LinqLanguageEditor2022;

using Microsoft.VisualStudio.Package;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio.Utilities;

using System.ComponentModel.Composition;
using System.Runtime.InteropServices;

namespace VSIXLinqLanguageEditor2022.LinqEditor
{

    [ComVisible(true)]
    [Guid(PackageGuids.EditorFactoryString)]
    internal sealed class LinqLanguageFactory : LanguageBase
    {
        [Export]
        [Name(Constants.LinqLanguageName)]
        //[BaseDefinition("code")]
        //[BaseDefinition("Intellisense")]
        [BaseDefinition(Constants.LinqBaselanguageName)]
        internal static ContentTypeDefinition LinqContentTypeDefinition { get; set; }

        [Import]
        internal IEditorOptionsFactoryService EditorOptions { get; set; }

        [Export]
        [FileExtension(Constants.LinqExt)]
        [ContentType(Constants.LinqLanguageName)]
        [BaseDefinition(Constants.LinqBaselanguageName)]
        internal static FileExtensionToContentTypeDefinition LinqFileExtensionDefinition { get; set; }

        [Export]
        [Name(Constants.LinqLanguageName)]
        [BaseDefinition(Constants.LinqBaselanguageName)]
        internal static ClassificationTypeDefinition LinqDefinition { get; set; }

        public LinqLanguageFactory(object site) : base(site)
        {

        }

        public override string Name => Constants.LinqLanguageName;

        public override string[] FileExtensions { get; } = new[] { Constants.LinqExt };


        public override void SetDefaultPreferences(LanguagePreferences preferences)
        {
            preferences.EnableCodeSense = true;
            preferences.EnableMatchBraces = true;
            preferences.EnableMatchBracesAtCaret = true;
            preferences.EnableShowMatchingBrace = true;
            preferences.EnableCommenting = true;
            preferences.HighlightMatchingBraceFlags = _HighlightMatchingBraceFlags.HMB_USERECTANGLEBRACES;
            preferences.LineNumbers = true;
            preferences.MaxErrorMessages = 100;
            preferences.AutoOutlining = true;
            preferences.MaxRegionTime = 2000;
            preferences.InsertTabs = true;
            preferences.IndentSize = 2;
            preferences.IndentStyle = IndentingStyle.Smart;
            preferences.ShowNavigationBar = true;
            preferences.EnableFormatSelection = true;

            preferences.WordWrap = true;
            preferences.WordWrapGlyphs = true;

            preferences.AutoListMembers = true;
            preferences.HideAdvancedMembers = false;
            preferences.EnableQuickInfo = true;
            preferences.ParameterInformation = true;
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
