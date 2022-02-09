
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

using System.ComponentModel.Composition;
using System.Windows.Media;

namespace LinqLanguageEditor2022.Classification
{
    #region Format definition
    /// <summary>
    /// Defines the editor format for the LinqComment classification type. Text is colored Green
    /// </summary>
    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "Comment")]
    [Name("Comment")]
    //this should be visible to the end user
    [UserVisible(true)]
    //set the priority to be after the default classifiers
    [Order(Before = Priority.Default)]
    internal sealed class LinqComment : ClassificationFormatDefinition
    {
        /// <summary>
        /// Defines the visual format for the "Comment" classification type
        /// </summary>
        public LinqComment()
        {
            DisplayName = "Comment"; //human readable version of the name
            ForegroundColor = Colors.Green;
        }
    }
    #endregion //Format definition
}
