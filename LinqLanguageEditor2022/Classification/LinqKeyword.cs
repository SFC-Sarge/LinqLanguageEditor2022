
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
    [ClassificationType(ClassificationTypeNames = "keyword")]
    [Name("keyword")]
    //this should be visible to the end user
    [UserVisible(true)]
    //set the priority to be after the default classifiers
    [Order(After = Priority.Default, Before = Priority.High)]
    internal sealed class LinqKeyword : ClassificationFormatDefinition
    {
        /// <summary>
        /// Defines the visual format for the "Keyword" classification type
        /// </summary>
        public LinqKeyword()
        {
            DisplayName = "keyword"; //human readable version of the name
            ForegroundColor = Colors.DarkBlue;
        }
    }
}
