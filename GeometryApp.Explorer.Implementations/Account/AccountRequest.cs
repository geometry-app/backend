namespace GeometryApp.Explorer.Implementations.Account;

public class AccountRequest : IExploreRequest
{
    public const string RequestType = "account_download";
    public string Type => RequestType;

    public int Id { get; set; }
    public string Resource { get; set; } = null!;
}
