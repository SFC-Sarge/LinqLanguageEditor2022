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

namespace LinqLanguageEditor2022.Parse
{
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Tagging;
    using Microsoft.VisualStudio.Utilities;

    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;

    [Export(typeof(ITaggerProvider))]
    [ContentType("Linq!")]
    [TagType(typeof(LinqTokenTag))]
    internal sealed class LinqTokenTagProvider : ITaggerProvider
    {

        public ITagger<T> CreateTagger<T>(ITextBuffer buffer) where T : ITag
        {
            return new LinqTokenTagger(buffer) as ITagger<T>;
        }
    }

    public class LinqTokenTag : ITag
    {
        public LinqTokenTypes type { get; private set; }

        public LinqTokenTag(LinqTokenTypes type)
        {
            this.type = type;
        }
    }

    internal sealed class LinqTokenTagger : ITagger<LinqTokenTag>
    {

        ITextBuffer _buffer;
        IDictionary<string, LinqTokenTypes> _LinqTypes;

        internal LinqTokenTagger(ITextBuffer buffer)
        {
            _buffer = buffer;
            _LinqTypes = new Dictionary<string, LinqTokenTypes>();
            _LinqTypes["Linq!"] = LinqTokenTypes.LinqExclamation;
            _LinqTypes["Linq."] = LinqTokenTypes.LinqPeriod;
            _LinqTypes["Linq?"] = LinqTokenTypes.LinqQuestion;
        }

        public event EventHandler<SnapshotSpanEventArgs> TagsChanged
        {
            add { }
            remove { }
        }

        public IEnumerable<ITagSpan<LinqTokenTag>> GetTags(NormalizedSnapshotSpanCollection spans)
        {

            foreach (SnapshotSpan curSpan in spans)
            {
                ITextSnapshotLine containingLine = curSpan.Start.GetContainingLine();
                int curLoc = containingLine.Start.Position;
                string[] tokens = containingLine.GetText().ToLower().Split(' ');

                foreach (string LinqToken in tokens)
                {
                    if (_LinqTypes.ContainsKey(LinqToken))
                    {
                        var tokenSpan = new SnapshotSpan(curSpan.Snapshot, new Span(curLoc, LinqToken.Length));
                        if (tokenSpan.IntersectsWith(curSpan))
                            yield return new TagSpan<LinqTokenTag>(tokenSpan,
                                                                  new LinqTokenTag(_LinqTypes[LinqToken]));
                    }

                    //add an extra char location because of the space
                    curLoc += LinqToken.Length + 1;
                }
            }

        }
    }
}
