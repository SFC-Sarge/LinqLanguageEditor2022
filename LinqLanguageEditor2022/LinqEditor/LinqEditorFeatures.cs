﻿using LinqLanguageEditor2022.Parse;

using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Language.Intellisense.AsyncCompletion;
using Microsoft.VisualStudio.Language.StandardClassification;
using Microsoft.VisualStudio.Text.BraceCompletion;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;

using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace LinqLanguageEditor2022.LinqEditor
{
    [Export(typeof(ITaggerProvider))]
    [TagType(typeof(IClassificationTag))]
    [ContentType(Constants.LinqLanguageName)]
    public class LinqSyntaxHighligting : TokenClassificationTaggerBase
    {
        public override Dictionary<object, string> ClassificationMap { get; } = new()
        {
            { LinqTokenTypes.String, PredefinedClassificationTypeNames.String },
            { LinqTokenTypes.Literal, PredefinedClassificationTypeNames.Literal },
            { LinqTokenTypes.Comment, PredefinedClassificationTypeNames.Comment },
            { LinqTokenTypes.Operator, PredefinedClassificationTypeNames.Operator },
            { LinqTokenTypes.Identifier, PredefinedClassificationTypeNames.Identifier },
            { LinqTokenTypes.Keyword, PredefinedClassificationTypeNames.Keyword },
            { LinqTokenTypes.Number, PredefinedClassificationTypeNames.Number },
            { LinqTokenTypes.WhiteSpace, PredefinedClassificationTypeNames.WhiteSpace },
            { LinqTokenTypes.Punctuation, PredefinedClassificationTypeNames.Punctuation },
        };
    }

    [Export(typeof(ITaggerProvider))]
    [TagType(typeof(IStructureTag))]
    [ContentType(Constants.LinqLanguageName)]
    public class LinqOutlining : TokenOutliningTaggerBase
    { }

    [Export(typeof(ITaggerProvider))]
    [TagType(typeof(IErrorTag))]
    [ContentType(Constants.LinqLanguageName)]
    public class LinqErrorSquigglies : TokenErrorTaggerBase
    { }

    [Export(typeof(IAsyncQuickInfoSourceProvider))]
    [ContentType(Constants.LinqLanguageName)]
    internal sealed class LinqTooltips : TokenQuickInfoBase
    { }



    [Export(typeof(IBraceCompletionContextProvider))]
    [BracePair('{', '}')]
    [BracePair('[', ']')]
    [BracePair('(', ')')]
    //[BracePair('"', '"')]
    //[BracePair('$', '$')]
    [ContentType(Constants.LinqLanguageName)]
    [TagType(typeof(TextMarkerTag))]
    [ProvideBraceCompletion(Constants.LinqLanguageName)]
    internal sealed class LinqBraceCompletion : BraceCompletionBase
    {
    }

    [Export(typeof(IAsyncCompletionCommitManagerProvider))]
    [ContentType(Constants.LinqLanguageName)]
    internal sealed class LinqCompletionCommitManager : CompletionCommitManagerBase
    {
        public override IEnumerable<char> CommitChars => new char[] { ' ', '\'', '"', ',', '.', ';', ':', '\\', '$' };
    }
    [Export(typeof(IViewTaggerProvider))]
    [TagType(typeof(TextMarkerTag))]
    [ContentType(Constants.LinqLanguageName)]
    internal sealed class LinqBraceMatchingTaggerProvider : BraceMatchingBase
    {
        // This will match parenthesis, curly brackets, and square brackets by default.
        // Override the BraceList property to modify the list of braces to match.
    }

    [Export(typeof(IViewTaggerProvider))]
    [TagType(typeof(TextMarkerTag))]
    [ContentType(Constants.LinqLanguageName)]
    public class LinqSameWordHighlighter : SameWordHighlighterBase
    { }
}