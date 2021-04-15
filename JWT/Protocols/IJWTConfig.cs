namespace Raique.JWT.Protocols
{
    public interface IJWTConfig
    {
        int Expires { get; }

        string Secret { get; }

    }
}
