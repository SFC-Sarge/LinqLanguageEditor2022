
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

using System.ComponentModel.Composition;

namespace LinqLanguageEditor2022.Classification
{
    internal static class OrdinaryClassificationDefinition
    {
        #region Type definition

        /// <summary>
        /// Defines the "LinqComment" classification type.
        /// </summary>
        [Export(typeof(ClassificationTypeDefinition))]
        [Name("Comment")]
        internal static ClassificationTypeDefinition LinqComment = null;

        /// <summary>
        /// Defines the "LinqString" classification type.
        /// </summary>
        [Export(typeof(ClassificationTypeDefinition))]
        [Name("String")]
        internal static ClassificationTypeDefinition LinqString = null;

        /// <summary>
        /// Defines the "LinqKeyword" classification type.
        /// </summary>
        [Export(typeof(ClassificationTypeDefinition))]
        [Name("Keyword")]
        internal static ClassificationTypeDefinition LinqKeyword = null;

        /// <summary>
        /// Defines the "LinqOperator" classification type.
        /// </summary>
        [Export(typeof(ClassificationTypeDefinition))]
        [Name("Operator")]
        internal static ClassificationTypeDefinition LinqOperator = null;

        /// <summary>
        /// Defines the "LinqNumber" classification type.
        /// </summary>
        [Export(typeof(ClassificationTypeDefinition))]
        [Name("Number")]
        internal static ClassificationTypeDefinition LinqNumber = null;

        /// <summary>
        /// Defines the "LinqWhiteSpace" classification type.
        /// </summary>
        [Export(typeof(ClassificationTypeDefinition))]
        [Name("WhiteSpace")]
        internal static ClassificationTypeDefinition LinqWhiteSpace = null;

        /// <summary>
        /// Defines the "LinqUnknown" classification type.
        /// </summary>
        [Export(typeof(ClassificationTypeDefinition))]
        [Name("Unknown")]
        internal static ClassificationTypeDefinition LinqUnknown = null;

        #endregion
    }
}
