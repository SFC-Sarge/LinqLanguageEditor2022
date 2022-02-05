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
        /// Defines the "LinqExclamation" classification type.
        /// </summary>
        [Export(typeof(ClassificationTypeDefinition))]
        [Name("Linq!")]
        internal static ClassificationTypeDefinition LinqExclamation = null;

        /// <summary>
        /// Defines the "LinqQuestion" classification type.
        /// </summary>
        [Export(typeof(ClassificationTypeDefinition))]
        [Name("Linq?")]
        internal static ClassificationTypeDefinition LinqQuestion = null;

        /// <summary>
        /// Defines the "LinqPeriod" classification type.
        /// </summary>
        [Export(typeof(ClassificationTypeDefinition))]
        [Name("Linq.")]
        internal static ClassificationTypeDefinition LinqPeriod = null;

        #endregion
    }
}
