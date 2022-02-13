
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

using System.ComponentModel.Composition;
using System.Windows.Media;

namespace LinqLanguageEditor2022.Classification
{
    /// <summary>
    /// Defines the editor format for the LinqString classification type. Text is colored brown
    /// </summary>
    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "string")]
    [Name("string")]
    //this should be visible to the end user
    [UserVisible(true)]
    //set the priority to be after the default classifiers
    [Order(After = Priority.Default, Before = Priority.High)]
    internal sealed class LinqString : ClassificationFormatDefinition
    {
        /// <summary>
        /// Defines the visual format for the "String" classification type
        /// </summary>
        public LinqString()
        {
            DisplayName = "string"; //human readable version of the name
            ForegroundColor = Colors.Brown;
        }
    }
}
