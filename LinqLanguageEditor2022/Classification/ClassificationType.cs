//***************************************************************************
// 
//    Copyright (c) Microsoft Corporation. All rights reserved.
//    This code is licensed under the Visual Studio SDK license terms.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//***************************************************************************

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
        [Name("LinqComment")]
        internal static ClassificationTypeDefinition LinqComment = null;

        /// <summary>
        /// Defines the "LinqKeyword" classification type.
        /// </summary>
        [Export(typeof(ClassificationTypeDefinition))]
        [Name("LinqKeyword")]
        internal static ClassificationTypeDefinition LinqKeyword = null;

        /// <summary>
        /// Defines the "LinqNumber" classification type.
        /// </summary>
        [Export(typeof(ClassificationTypeDefinition))]
        [Name("LinqNumber")]
        internal static ClassificationTypeDefinition LinqNumber = null;

        /// <summary>
        /// Defines the "LinqOperator" classification type.
        /// </summary>
        [Export(typeof(ClassificationTypeDefinition))]
        [Name("LinqOperator")]
        internal static ClassificationTypeDefinition LinqOperator = null;


        /// <summary>
        /// Defines the "LinqString" classification type.
        /// </summary>
        [Export(typeof(ClassificationTypeDefinition))]
        [Name("LinqString")]
        internal static ClassificationTypeDefinition LinqString = null;

        /// <summary>
        /// Defines the "LinqWhiteSpace" classification type.
        /// </summary>
        [Export(typeof(ClassificationTypeDefinition))]
        [Name("LinqWhiteSpace")]
        internal static ClassificationTypeDefinition LinqWhiteSpace = null;

        /// <summary>
        /// Defines the "LinqPunctuation" classification type.
        /// </summary>
        [Export(typeof(ClassificationTypeDefinition))]
        [Name("LinqPunctuation")]
        internal static ClassificationTypeDefinition LinqPunctuation = null;

        /// <summary>
        /// Defines the "LinqIdentifier" classification type.
        /// </summary>
        [Export(typeof(ClassificationTypeDefinition))]
        [Name("LinqIdentifier")]
        internal static ClassificationTypeDefinition LinqIdentifier = null;

        /// <summary>
        /// Defines the "LinqSeparator" classification type.
        /// </summary>
        [Export(typeof(ClassificationTypeDefinition))]
        [Name("LinqSeparator")]
        internal static ClassificationTypeDefinition LinqSeparator = null;

        /// <summary>
        /// Defines the "LinqLiteral" classification type.
        /// </summary>
        [Export(typeof(ClassificationTypeDefinition))]
        [Name("LinqLiteral")]
        internal static ClassificationTypeDefinition LinqLiteral = null;

        /// <summary>
        /// Defines the "LinqUnknown" classification type.
        /// </summary>
        [Export(typeof(ClassificationTypeDefinition))]
        [Name("Unknown")]
        internal static ClassificationTypeDefinition LinqUnknown = null;


        #endregion
    }
}
