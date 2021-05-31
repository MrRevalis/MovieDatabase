using MovieDatabase.Validation;

namespace MovieDatabase.ValidationRules
{
    public class IsLenghtValidRule<T> : IValidationRule<T>
    {
        public string ValidationMessage { get; set; }
        public int MinimunLength { get; set; }
        public int MaximunLength { get; set; }

        public bool Check(T value)
        {
            if (value == null)
            {
                return false;
            }

            var str = value as string;
            return (str.Length >= MinimunLength && str.Length <= MaximunLength);
        }
    }
}
