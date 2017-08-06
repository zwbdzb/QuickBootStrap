using System.Text.RegularExpressions;

namespace QuickBootstrap.Validations
{
    public static class Validation
    {
        private static RegexValidation _email;
        private static RequiredValidation _requiredRule;
        private static StringLengthValidation _passwordLength;
        private static StringLengthValidation _nameLength;

        public static IValidationRule Email
        {
            get
            {
                return _email ?? (_email = new RegexValidation(new Regex(@"[\w!#$%&'*+/=?^_`{|}~-]+(?:\.[\w!#$%&'*+/=?^_`{|}~-]+)*@[a-zA-Z0-9]{1,}(\-)?[a-zA-Z0-9]{0,}(\.)[a-zA-Z]{2,}")));
            }
        }


        public static IValidationRule Required
        {
            get { return _requiredRule ?? (_requiredRule = new RequiredValidation()); }
        }

        public static IValidationRule PasswordLength
        {
            get { return _passwordLength ?? (_passwordLength = new StringLengthValidation(6, 15)); }
        }

        public static IValidationRule NameLength
        {
            get { return _nameLength ?? (_nameLength = new StringLengthValidation(0, 50)); }
        }

    }
}