
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

    /// <summary>
    /// Defines the editor format for the LinqKeyword classification type. Text is colored DarkBlue
    /// </summary>
    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "Keyword")]
    [Name("Keyword")]
    //this should be visible to the end user
    [UserVisible(true)]
    //set the priority to be after the default classifiers
    [Order(Before = Priority.Default)]
    internal sealed class LinqKeyword : ClassificationFormatDefinition
    {
        /// <summary>
        /// Defines the visual format for the "Keyword" classification type
        /// </summary>
        public LinqKeyword()
        {
            DisplayName = "Keyword"; //human readable version of the name
            ForegroundColor = Colors.DarkBlue;
        }
    }

    /// <summary>
    /// Defines the editor format for the LinqString classification type. Text is colored Orange
    /// </summary>
    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "String")]
    [Name("String")]
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
            DisplayName = "String"; //human readable version of the name
            ForegroundColor = Colors.Orange;
        }
    }

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
            ForegroundColor = Colors.Gray;
        }
    }

    /// <summary>
    /// Defines the editor format for the LinqUnknown classification type. Text is colored Red
    /// </summary>
    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "Unknown")]
    [Name("Unknown")]
    //this should be visible to the end user
    [UserVisible(true)]
    //set the priority to be after the default classifiers
    [Order(Before = Priority.Default)]
    internal sealed class LinqUnknown : ClassificationFormatDefinition
    {
        /// <summary>
        /// Defines the visual format for the "Unknown" classification type
        /// </summary>
        public LinqUnknown()
        {
            DisplayName = "Unknown"; //human readable version of the name
            ForegroundColor = Colors.Red;

        }
    }

    #endregion //Format definition
}
