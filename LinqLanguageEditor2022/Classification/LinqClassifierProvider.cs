using LinqLanguageEditor2022.Tokens;

namespace LinqLanguageEditor2022.Classification
{
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Classification;
    using Microsoft.VisualStudio.Text.Tagging;
    using Microsoft.VisualStudio.Utilities;

    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    [Export(typeof(ITaggerProvider))]
    [ContentType(Constants.LinqLanguageName)]
    [TagType(typeof(ClassificationTag))]
    internal sealed class LinqClassifierProvider : ITaggerProvider, ITagger<ClassificationTag>
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

        ITextBuffer _buffer;
        ITagAggregator<LinqTokenTag> _aggregator;
        IDictionary<LinqTokenTypes, IClassificationType> _LinqTypes;

        public ITagger<T> CreateTagger<T>(ITextBuffer buffer) where T : ITag
        {
            ITagAggregator<LinqTokenTag> LinqTagAggregator = aggregatorFactory.CreateTagAggregator<LinqTokenTag>(buffer);
            return new LinqClassifierProvider(buffer, LinqTagAggregator, ClassificationTypeRegistry) as ITagger<T>;
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
        /// <summary>
        /// Construct the classifier and define search tokens
        /// </summary>
        internal LinqClassifierProvider(ITextBuffer buffer, ITagAggregator<LinqTokenTag> LinqTagAggregator, IClassificationTypeRegistryService typeService)
        {
            _buffer = buffer;
            _aggregator = LinqTagAggregator;

            _LinqTypes = new Dictionary<LinqTokenTypes, IClassificationType>();
            _LinqTypes[LinqTokenTypes.comment] = typeService.GetClassificationType("comment"); ;
            _LinqTypes[LinqTokenTypes.keyword] = typeService.GetClassificationType("keyword"); ;
            _LinqTypes[LinqTokenTypes.number] = typeService.GetClassificationType("number"); ;
            _LinqTypes[LinqTokenTypes.@operator] = typeService.GetClassificationType("operator"); ;
            _LinqTypes[LinqTokenTypes.@string] = typeService.GetClassificationType("string"); ;
            _LinqTypes[LinqTokenTypes.whitespace] = typeService.GetClassificationType("whiteSpace"); ;
            _LinqTypes[LinqTokenTypes.punctuation] = typeService.GetClassificationType("punctuation"); ;
            _LinqTypes[LinqTokenTypes.identifier] = typeService.GetClassificationType("identifier"); ;
            _LinqTypes[LinqTokenTypes.separator] = typeService.GetClassificationType("separator"); ;
            _LinqTypes[LinqTokenTypes.identifier] = typeService.GetClassificationType("literal"); ;
            _LinqTypes[LinqTokenTypes.unknown] = typeService.GetClassificationType("unknown"); ;

        }

        public event EventHandler<SnapshotSpanEventArgs> TagsChanged
        {
            add { }
            remove { }
        }


    }
}
