
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
    [ClassificationType(ClassificationTypeNames = "operator")]
    [Name("operator")]
    //this should be visible to the end user
    [UserVisible(true)]
    //set the priority to be after the default classifiers
    [Order(After = Priority.Default, Before = Priority.High)]
    internal sealed class LinqOperator : ClassificationFormatDefinition
    {
        /// <summary>
        /// Defines the visual format for the "Operator" classification type
        /// </summary>
        public LinqOperator()
        {
            DisplayName = "operator"; //human readable version of the name
            ForegroundColor = Colors.LimeGreen;
        }
    }
}
