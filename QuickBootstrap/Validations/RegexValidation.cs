using System.Text.RegularExpressions;

namespace QuickBootstrap.Validations
{
    public class RegexValidation : IValidationRule
    {
        public Regex Regex { get; private set; }

        public RegexValidation(string regex)
        {
            Regex = new Regex(regex);
        }
        public string ErrorKey
        { 
            get { return "Regex_Validation_Error"; }
        }

        public RegexValidation(Regex reg)
        {
            Regex = reg;
        }

        public bool Validate(object value)
        {
            var str = value as string ?? "";
            return Regex.IsMatch(str);
        }
    }
}