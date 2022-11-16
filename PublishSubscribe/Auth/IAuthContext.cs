namespace PublishSubscribe.Auth
{
    public interface IAuthContext
    {
        bool Authenticate(string token);
    }
}
