
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

using System.ComponentModel.Composition;
using System.Windows.Media;

namespace LinqLanguageEditor2022.Classification
{
    /// <summary>
    /// Defines the editor format for the LinqLiteral classification type. Text is colored Red
    /// </summary>
    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "Literal")]
    [Name("Literal")]
    //this should be visible to the end user
    [UserVisible(true)]
    //set the priority to be after the default classifiers
    [Order(Before = Priority.Default)]
    internal sealed class LinqLiteral : ClassificationFormatDefinition
    {
        /// <summary>
        /// Defines the visual format for the "Literal" classification type
        /// </summary>
        public LinqLiteral()
        {
            DisplayName = "Literal"; //human readable version of the name
            ForegroundColor = Colors.Brown;

        }
    }
}
