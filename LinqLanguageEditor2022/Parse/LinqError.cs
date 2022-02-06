using Microsoft.VisualStudio.Shell.Interop;

namespace LinqLanguageEditor2022.Parse
{
    public class LinqError
    {
        public LinqError(string errorCode, string message, string category, __VSERRORCATEGORY severity)
        {
            ErrorCode = errorCode;
            Message = message;
            Category = category;
            Severity = severity;
        }

        public string ErrorCode { get; }
        public string Message { get; }
        public string Category { get; }
        public __VSERRORCATEGORY Severity { get; }

        public LinqError WithFormat(params string[] replacements)
        {
            return new LinqError(ErrorCode, string.Format(Message, replacements), Category, Severity);
        }
    }
}
