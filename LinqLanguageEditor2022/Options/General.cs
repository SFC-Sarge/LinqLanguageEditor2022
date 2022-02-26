using System.ComponentModel;

namespace LinqLanguageEditor2022.Options
{
    public class General : BaseOptionModel<General>
    {
        [Category("My category")]
        [DisplayName("My Option")]
        [Description("An informative description.")]
        [DefaultValue(true)]
        public bool MyOption { get; set; } = true;
    }
}
