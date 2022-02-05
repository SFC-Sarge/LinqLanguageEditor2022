using Microsoft.VisualStudio.Shell.Interop;

using System.Linq;

using type = Microsoft.VisualStudio.Text.Adornments.PredefinedErrorTypeNames;

namespace LinqLanguageEditor2022.Parse
{
    public partial class LinqDocument
    {
        public bool IsValid { get; private set; }

        private class LinqErrors
        {
            public static LinqError PL001 { get; } = new("PL001", "Unknown token at this location", type.SyntaxError, __VSERRORCATEGORY.EC_ERROR);
            public static LinqError PL002 { get; } = new("PL002", "Unclosed registry key entry. Add the missing ] character", type.SyntaxError, __VSERRORCATEGORY.EC_ERROR);
            public static LinqError PL003 { get; } = new("PL003", "Use the backslash character as delimiter instead of forward slash.", type.SyntaxError, __VSERRORCATEGORY.EC_ERROR);
            public static LinqError PL004 { get; } = new("PL004", "To set a registry key's default value, use '@' without quotation marks", type.Warning, __VSERRORCATEGORY.EC_WARNING);
            public static LinqError PL005 { get; } = new("PL005", "Value names must be enclosed in quotation marks.", type.SyntaxError, __VSERRORCATEGORY.EC_ERROR);
            public static LinqError PL006 { get; } = new("PL006", "The variable \"{0}\" doesn't exist.", type.Warning, __VSERRORCATEGORY.EC_WARNING);
            public static LinqError PL007 { get; } = new("PL007", "Variables must begin and end with $ character.", type.SyntaxError, __VSERRORCATEGORY.EC_ERROR);
            public static LinqError PL008 { get; } = new("PL008", "This registry key \"{0}\" was already defined earlier in the document", type.Suggestion, __VSERRORCATEGORY.EC_MESSAGE);
        }

        private void AddError(LinqParseItem item, LinqError error)
        {
            item.Errors.Add(error);
            IsValid = false;
        }

        private void LinqValidateDocument()
        {
            IsValid = true;

            foreach (LinqParseItem item in Items)
            {
                // Unknown symbols
                if (item.Type == LinqTokenTypes.Unknown)
                {
                    AddError(item, LinqErrors.PL001);
                }

                // Registry key

                // Properties
                else if (item.Type == LinqTokenTypes.Operator)
                {
                    LinqParseItem name = item.Previous;
                    LinqParseItem value = item.Next;

                    if (name?.Type == LinqTokenTypes.String)
                    {
                        if (name.Text == "\"@\"")
                        {
                            AddError(name, LinqErrors.PL004);
                        }
                    }
                    else if (name?.Type == LinqTokenTypes.Literal && name?.Text != "@")
                    {
                        AddError(name, LinqErrors.PL005);
                    }
                }

                // Make sure strings are correctly closed with quotation mark
                if (item.Type == LinqTokenTypes.String)
                {
                    if (!item.Text.EndsWith("\""))
                    {
                        AddError(item, LinqErrors.PL005);
                    }
                }

                // References
                foreach (LinqParseItem reference in item.References)
                {
                    string refTrim = reference.Text.Trim();

                    //if (!refTrim.EndsWith("$"))
                    //{
                    //    AddError(reference, Errors.PL007);
                    //}
                    //else
                    //{
                    if (refTrim.EndsWith("$"))
                    {
                        if (!LinqPredefinedVariables.Variables.Any(v => v.Key.Equals(refTrim.Trim('$'), StringComparison.OrdinalIgnoreCase)))
                        {
                            AddError(reference, LinqErrors.PL006.WithFormat(refTrim));
                        }
                    }
                    //}
                }
            }
        }
    }
}
