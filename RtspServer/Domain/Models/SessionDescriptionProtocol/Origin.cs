namespace RtspServer.Domain.Models.SessionDescriptionProtocol;

public class Origin
{
    public const string NetworkType = "IN";
    public const string AddressType = "IP4";
    public string Username { get; }
    public int SessionId { get; }
    public int SessionVersion { get; }
    public string Address { get; }

    public Origin(string username, string address)
    {
        Username = username;
        SessionId = Random.Shared.Next(1000000000, 1999999999);
        SessionVersion = SessionId;
        Address = address;
    }

    public Origin(string address) : this("-", address)
    {
    }

    public override string ToString()
    {
        return $"{Username} {SessionId} {SessionVersion} {NetworkType} {AddressType} {Address}";
    }
}