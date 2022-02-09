
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

using System.ComponentModel.Composition;
using System.Windows.Media;

namespace LinqLanguageEditor2022.Classification
{
    /// <summary>
    /// Defines the editor format for the LinqNumber classification type. Text is colored LimeGreen
    /// </summary>
    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "Number")]
    [Name("Number")]
    //this should be visible to the end user
    [UserVisible(true)]
    //set the priority to be after the default classifiers
    [Order(Before = Priority.Default)]
    internal sealed class LinqNumber : ClassificationFormatDefinition
    {
        /// <summary>
        /// Defines the visual format for the "Number" classification type
        /// </summary>
        public LinqNumber()
        {
            DisplayName = "Number"; //human readable version of the name
            ForegroundColor = Colors.LimeGreen;
        }
    }
}
