namespace SparseInject.Tests.ComplexTests
{
    public class AuthorizationResult
    {
        public bool IsAuthorized { get; }
        public bool IsOnline { get; }

        public AuthorizationResult(bool isAuthorized, bool isOnline)
        {
            IsAuthorized = isAuthorized;
            IsOnline = isOnline;
        }
    }
}