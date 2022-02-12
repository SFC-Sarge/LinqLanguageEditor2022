
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
    [ClassificationType(ClassificationTypeNames = "linqString")]
    [Name("linqString")]
    //this should be visible to the end user
    [UserVisible(true)]
    //set the priority to be after the default classifiers
    [Order(Before = Priority.Default)]
    internal sealed class LinqString : ClassificationFormatDefinition
    {
        /// <summary>
        /// Defines the visual format for the "String" classification type
        /// </summary>
        public LinqString()
        {
            DisplayName = "linqString"; //human readable version of the name
            ForegroundColor = Colors.Brown;
        }
    }
}
