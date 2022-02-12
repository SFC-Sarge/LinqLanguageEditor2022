
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

using System.ComponentModel.Composition;
using System.Windows.Media;

namespace LinqLanguageEditor2022.Classification
{
    /// <summary>
    /// Defines the editor format for the LinqIdentifier classification type. Text is colored Aquamarine
    /// </summary>
    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "linqIdentifier")]
    [Name("linqIdentifier")]
    //this should be visible to the end user
    [UserVisible(true)]
    //set the priority to be after the default classifiers
    [Order(Before = Priority.Default)]
    internal sealed class LinqIdentifier : ClassificationFormatDefinition
    {
        /// <summary>
        /// Defines the visual format for the "Identifier" classification type
        /// </summary>
        public LinqIdentifier()
        {
            DisplayName = "linqIdentifier"; //human readable version of the name
            ForegroundColor = Colors.Silver;

        }
    }
}
