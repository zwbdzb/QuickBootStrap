namespace QuickBootstrap.Validations
{
    public class RequiredValidation : IValidationRule
    {
        public string ErrorKey
        {
            get { return "Required_Error"; }
        }

        public bool Validate(object value)
        {
            if (value == null)
                return false;
            return !(value is string) || !string.IsNullOrEmpty((string) value);
        }
    }
}