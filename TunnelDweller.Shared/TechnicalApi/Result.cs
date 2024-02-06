namespace TechnicalApi.API.Shared
{
    public enum Result : int
    {
        Empty = -1,
        Success = 0,
        Rejected = 1,
        InvalidParameter = 2,
        InvalidToken = 3,
    }
}
