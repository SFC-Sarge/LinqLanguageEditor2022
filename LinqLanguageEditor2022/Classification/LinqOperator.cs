
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

using System.ComponentModel.Composition;
using System.Windows.Media;

namespace LinqLanguageEditor2022.Classification
{
    /// <summary>
    /// Defines the editor format for the LinqOperator classification type. Text is colored Gray
    /// </summary>
    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "Operator")]
    [Name("Operator")]
    //this should be visible to the end user
    [UserVisible(true)]
    //set the priority to be after the default classifiers
    [Order(Before = Priority.Default)]
    internal sealed class LinqOperator : ClassificationFormatDefinition
    {
        /// <summary>
        /// Defines the visual format for the "Operator" classification type
        /// </summary>
        public LinqOperator()
        {
            DisplayName = "Operator"; //human readable version of the name
            ForegroundColor = Colors.LimeGreen;
        }
    }
}
