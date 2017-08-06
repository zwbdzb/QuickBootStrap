namespace QuickBootstrap.Validations
{
    public interface IValidationRule
    {
        bool Validate(object value);
        string ErrorKey { get; }
    }
}