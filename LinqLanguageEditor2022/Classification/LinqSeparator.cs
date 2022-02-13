
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

using System.ComponentModel.Composition;
using System.Windows.Media;

namespace LinqLanguageEditor2022.Classification
{
    /// <summary>
    /// Defines the editor format for the LinqSeparator classification type. Text is colored Red
    /// </summary>
    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "separator")]
    [Name("LinqSeparator")]
    //this should be visible to the end user
    [UserVisible(true)]
    //set the priority to be after the default classifiers
    [Order(After = Priority.Default, Before = Priority.High)]
    internal sealed class LinqSeparator : ClassificationFormatDefinition
    {
        /// <summary>
        /// Defines the visual format for the "Punctuation" classification type
        /// </summary>
        public LinqSeparator()
        {
            DisplayName = "separator"; //human readable version of the name
            ForegroundColor = Colors.LightCyan;

        }
    }
}
