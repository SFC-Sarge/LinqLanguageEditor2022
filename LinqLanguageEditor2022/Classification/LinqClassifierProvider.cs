using LinqLanguageEditor2022.Tokens;

namespace LinqLanguageEditor2022.Classification
{
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Classification;
    using Microsoft.VisualStudio.Text.Tagging;
    using Microsoft.VisualStudio.Utilities;

    using System;
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
}
