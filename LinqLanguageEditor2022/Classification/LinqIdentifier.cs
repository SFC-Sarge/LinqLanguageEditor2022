
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
    [ClassificationType(ClassificationTypeNames = "identifier")]
    [Name("identifier")]
    //this should be visible to the end user
    [UserVisible(true)]
    //set the priority to be after the default classifiers
    [Order(After = Priority.Default, Before = Priority.High)]
    internal sealed class LinqIdentifier : ClassificationFormatDefinition
    {
        /// <summary>
        /// Defines the visual format for the "Identifier" classification type
        /// </summary>
        public LinqIdentifier()
        {
            DisplayName = "identifier"; //human readable version of the name
            ForegroundColor = Colors.Silver;

        }
    }
}
