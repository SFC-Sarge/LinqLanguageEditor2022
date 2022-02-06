
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
    [ContentType(Constants.LinqLanguageName)]
    [TagType(typeof(ClassificationTag))]
    internal sealed class LinqClassifierProvider : ITaggerProvider
    {

        [Export]
        [Name(Constants.LinqLanguageName)]
        [BaseDefinition(Constants.LinqBaselanguageName)]
        internal static ContentTypeDefinition LinqContentType = null;

        [Export]
        [FileExtension(Constants.LinqExt)]
        [ContentType(Constants.LinqLanguageName)]
        internal static FileExtensionToContentTypeDefinition LinqFileType = null;

        [Import]
        internal IClassificationTypeRegistryService ClassificationTypeRegistry = null;

        [Import]
        internal IBufferTagAggregatorFactoryService aggregatorFactory = null;

        public ITagger<T> CreateTagger<T>(ITextBuffer buffer) where T : ITag
        {

            ITagAggregator<LinqTokenTag> LinqTagAggregator = aggregatorFactory.CreateTagAggregator<LinqTokenTag>(buffer);

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
        internal LinqClassifier(ITextBuffer buffer, ITagAggregator<LinqTokenTag> LinqTagAggregator, IClassificationTypeRegistryService typeService)
        {
            _buffer = buffer;
            _aggregator = LinqTagAggregator;

            _LinqTypes = new Dictionary<LinqTokenTypes, IClassificationType>();
            _LinqTypes[LinqTokenTypes.Comment] = typeService.GetClassificationType("Comment"); ;
            _LinqTypes[LinqTokenTypes.String] = typeService.GetClassificationType("String"); ;
            _LinqTypes[LinqTokenTypes.Keyword] = typeService.GetClassificationType("Keyword"); ;
            _LinqTypes[LinqTokenTypes.Number] = typeService.GetClassificationType("Number"); ;
            _LinqTypes[LinqTokenTypes.Operator] = typeService.GetClassificationType("Operator"); ;
            _LinqTypes[LinqTokenTypes.WhiteSpace] = typeService.GetClassificationType("WhiteSpace"); ;
            _LinqTypes[LinqTokenTypes.Punctuation] = typeService.GetClassificationType("Punctuation"); ;
            _LinqTypes[LinqTokenTypes.Identifier] = typeService.GetClassificationType("Identifier"); ;
            _LinqTypes[LinqTokenTypes.Literal] = typeService.GetClassificationType("Literal"); ;
            _LinqTypes[LinqTokenTypes.Unknown] = typeService.GetClassificationType("Unknown"); ;

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
                yield return new TagSpan<ClassificationTag>(tagSpans[0], new ClassificationTag(_LinqTypes[tagSpan.Tag.type]));
            }
        }
    }
}
