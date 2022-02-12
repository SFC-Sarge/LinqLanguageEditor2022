
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

using System.ComponentModel.Composition;
using System.Windows.Media;

namespace LinqLanguageEditor2022.Classification
{
    /// <summary>
    /// Defines the editor format for the LinqPunctuation classification type. Text is colored Red
    /// </summary>
    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "linqPunctuation")]
    [Name("linqPunctuation")]
    //this should be visible to the end user
    [UserVisible(true)]
    //set the priority to be after the default classifiers
    [Order(Before = Priority.Default)]
    internal sealed class LinqPunctuation : ClassificationFormatDefinition
    {
        /// <summary>
        /// Defines the visual format for the "Punctuation" classification type
        /// </summary>
        public LinqPunctuation()
        {
            DisplayName = "linqPunctuation"; //human readable version of the name
            ForegroundColor = Colors.DarkViolet;

        }
    }
}
