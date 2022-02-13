
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

using System.ComponentModel.Composition;
using System.Windows.Media;

namespace LinqLanguageEditor2022.Classification
{
    /// <summary>
    /// Defines the editor format for the LinqUnknown classification type. Text is colored Red
    /// </summary>
    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "unknown")]
    [Name("LinqUnknown")]
    //this should be visible to the end user
    [UserVisible(true)]
    //set the priority to be after the default classifiers
    [Order(After = Priority.Default, Before = Priority.High)]
    internal sealed class LinqUnknown : ClassificationFormatDefinition
    {
        /// <summary>
        /// Defines the visual format for the "Unknown" classification type
        /// </summary>
        public LinqUnknown()
        {
            DisplayName = "unknown"; //human readable version of the name
            ForegroundColor = Colors.Red;
        }
    }
}
