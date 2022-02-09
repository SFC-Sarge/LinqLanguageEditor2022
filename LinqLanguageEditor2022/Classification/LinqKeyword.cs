
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

using System.ComponentModel.Composition;
using System.Windows.Media;

namespace LinqLanguageEditor2022.Classification
{
    /// <summary>
    /// Defines the editor format for the LinqKeyword classification type. Text is colored DarkBlue
    /// </summary>
    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "Keyword")]
    [Name("Keyword")]
    //this should be visible to the end user
    [UserVisible(true)]
    //set the priority to be after the default classifiers
    [Order(Before = Priority.Default)]
    internal sealed class LinqKeyword : ClassificationFormatDefinition
    {
        /// <summary>
        /// Defines the visual format for the "Keyword" classification type
        /// </summary>
        public LinqKeyword()
        {
            DisplayName = "Keyword"; //human readable version of the name
            ForegroundColor = Colors.DarkBlue;
        }
    }
}
