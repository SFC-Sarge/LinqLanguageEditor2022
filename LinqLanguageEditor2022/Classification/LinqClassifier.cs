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

using LinqLanguageEditor2022.Parse;

namespace LinqLanguageEditor2022.Classification
{

    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Classification;
    using Microsoft.VisualStudio.Text.Tagging;
    using Microsoft.VisualStudio.Utilities;

    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;

    [Export(typeof(ITaggerProvider))]
    [ContentType("Linq!")]
    [TagType(typeof(ClassificationTag))]
    internal sealed class LinqClassifierProvider : ITaggerProvider
    {

        [Export]
        [Name("Linq!")]
        [BaseDefinition("code")]
        internal static ContentTypeDefinition LinqContentType = null;

        [Export]
        [FileExtension(".Linq")]
        [ContentType("Linq!")]
        internal static FileExtensionToContentTypeDefinition LinqFileType = null;

        [Import]
        internal IClassificationTypeRegistryService ClassificationTypeRegistry = null;

        [Import]
        internal IBufferTagAggregatorFactoryService aggregatorFactory = null;

        public ITagger<T> CreateTagger<T>(ITextBuffer buffer) where T : ITag
        {

            ITagAggregator<LinqTokenTag> LinqTagAggregator =
                                            aggregatorFactory.CreateTagAggregator<LinqTokenTag>(buffer);

            return new LinqClassifier(buffer, LinqTagAggregator, ClassificationTypeRegistry) as ITagger<T>;
        }
    }

    internal sealed class LinqClassifier : ITagger<ClassificationTag>
    {
        ITextBuffer _buffer;
        ITagAggregator<LinqTokenTag> _aggregator;
        IDictionary<LinqTokenTypes, IClassificationType> _LinqTypes;

        /// <summary>
        /// Construct the classifier and define search tokens
        /// </summary>
        internal LinqClassifier(ITextBuffer buffer,
                               ITagAggregator<LinqTokenTag> LinqTagAggregator,
                               IClassificationTypeRegistryService typeService)
        {
            _buffer = buffer;
            _aggregator = LinqTagAggregator;
            _LinqTypes = new Dictionary<LinqTokenTypes, IClassificationType>();
            _LinqTypes[LinqTokenTypes.LinqExclamation] = typeService.GetClassificationType("Linq!");
            _LinqTypes[LinqTokenTypes.LinqPeriod] = typeService.GetClassificationType("Linq.");
            _LinqTypes[LinqTokenTypes.LinqQuestion] = typeService.GetClassificationType("Linq?");
        }

        public event EventHandler<SnapshotSpanEventArgs> TagsChanged
        {
            add { }
            remove { }
        }

        /// <summary>
        /// Search the given span for any instances of classified tags
        /// </summary>
        public IEnumerable<ITagSpan<ClassificationTag>> GetTags(NormalizedSnapshotSpanCollection spans)
        {
            foreach (var tagSpan in _aggregator.GetTags(spans))
            {
                var tagSpans = tagSpan.Span.GetSpans(spans[0].Snapshot);
                yield return
                    new TagSpan<ClassificationTag>(tagSpans[0],
                                                   new ClassificationTag(_LinqTypes[tagSpan.Tag.type]));
            }
        }
    }
}
