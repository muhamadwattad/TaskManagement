namespace TaskManagement.Framework.Exceptions
{
    public class ValidationException : BaseException
    {
        public ValidationException(string key, params string[] args) : base(key, "Validation Exception", args)
        {
        }
    }
}
