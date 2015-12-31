namespace QuickBootstrap.Validations
{
    public class StringLengthValidation : IValidationRule
    {
        public int Min { get; private set; }

        public int Max { get; private set; }

        public string ErrorKey
        {
            get { return "Length_Error"; }
        }

        public StringLengthValidation(int min, int max)
        {
            Min = min;
            Max = max;
        }

        public bool Validate(object value)
        {
            var str = value as string;
            if (str == null)
            {
                return false;
            }
            return !string.IsNullOrEmpty(str) && str.Length <= Max && str.Length >= Min;
        }
    }
}