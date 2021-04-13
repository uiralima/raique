namespace Raique.Microservices.Authenticate.Domain
{
    public abstract class Base
    {
        public static T Invalid<T>() where T : Base, new()
        {
            return new T()
            {
                _isValid = false
            };
        }

        private bool _isValid = true;
        public bool IsValid()
        {
            return _isValid;
        }
    }
}
