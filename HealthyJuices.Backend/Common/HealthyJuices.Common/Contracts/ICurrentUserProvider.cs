namespace HealthyJuices.Common.Contracts
{
    public interface ICurrentUserProvider
    {
        string UserId { get; }
    }
}