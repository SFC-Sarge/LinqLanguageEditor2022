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

using LinqLanguageEditor2022.Tokens;

namespace LinqLanguageEditor2022.Classification
{
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Classification;
    using Microsoft.VisualStudio.Text.Editor;
    using Microsoft.VisualStudio.Text.Tagging;
    using Microsoft.VisualStudio.Utilities;

    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;

    [Export(typeof(ITaggerProvider))]
    [ContentType(Constants.LinqLanguageName)]
    [TagType(typeof(ClassificationTag))]
    internal sealed class LinqClassifierProvider : ITaggerProvider
    {

        [Export]
        [Name(Constants.LinqLanguageName)]
        [BaseDefinition("code")]
        internal static ContentTypeDefinition LinqContentType = null;

        [Export]
        [FileExtension(Constants.LinqExt)]
        [Name(Constants.LinqLanguageName)]
        internal static FileExtensionToContentTypeDefinition LinqFileType = null;

        [Import]
        internal IClassificationTypeRegistryService ClassificationTypeRegistry = null;

        [Import]
        internal IBufferTagAggregatorFactoryService aggregatorFactory = null;

        public ITagger<T> CreateTagger<T>(ITextBuffer buffer) where T : ITag
        {

            ITagAggregator<LinqTokenTag> linqTagAggregator =
                                            aggregatorFactory.CreateTagAggregator<LinqTokenTag>(buffer);

            return new LinqClassifier(buffer, linqTagAggregator, ClassificationTypeRegistry) as ITagger<T>;
        }
    }

    internal sealed class LinqClassifier : ITagger<ClassificationTag>
    {
        ITextBuffer _buffer;
        ITagAggregator<LinqTokenTag> _aggregator;
        IDictionary<LinqTokenTypes, IClassificationType> _linqTypes;

        /// <summary>
        /// Construct the classifier and define search tokens
        /// </summary>
        internal LinqClassifier(ITextBuffer buffer,
                               ITagAggregator<LinqTokenTag> linqTagAggregator,
                               IClassificationTypeRegistryService typeService)
        {
            _buffer = buffer;
            _aggregator = linqTagAggregator;
            _linqTypes = new Dictionary<LinqTokenTypes, IClassificationType>();
            _linqTypes[LinqTokenTypes.comment] = typeService.GetClassificationType("comment");
            _linqTypes[LinqTokenTypes.keyword] = typeService.GetClassificationType("keyword");
            _linqTypes[LinqTokenTypes.number] = typeService.GetClassificationType("number");
            _linqTypes[LinqTokenTypes.@operator] = typeService.GetClassificationType("operator");
            _linqTypes[LinqTokenTypes.@string] = typeService.GetClassificationType("string");
            _linqTypes[LinqTokenTypes.whitespace] = typeService.GetClassificationType("whiteSpace");
            _linqTypes[LinqTokenTypes.punctuation] = typeService.GetClassificationType("punctuation");
            _linqTypes[LinqTokenTypes.identifier] = typeService.GetClassificationType("identifier");
            _linqTypes[LinqTokenTypes.separator] = typeService.GetClassificationType("separator");
            _linqTypes[LinqTokenTypes.identifier] = typeService.GetClassificationType("literal");
            _linqTypes[LinqTokenTypes.unknown] = typeService.GetClassificationType("unknown");
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
                                                   new ClassificationTag(_linqTypes[tagSpan.Tag.type]));
            }
        }
    }
}
