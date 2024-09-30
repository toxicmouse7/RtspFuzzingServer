namespace RtspServer.SessionDescriptionProtocol;

public class Origin
{
    public const string NetworkType = "IN";
    public const string AddressType = "IP4";
    public string Username { get; }
    public int SessionId { get; }
    public int SessionVersion { get; }
    public string UnicastAddress { get; }

    public Origin(string username, string unicastAddress)
    {
        Username = username;
        SessionId = Random.Shared.Next(1000000000, 1999999999);
        SessionVersion = SessionId;
        UnicastAddress = unicastAddress;
    }

    public Origin(string unicastAddress) : this("-", unicastAddress)
    {
    }

    public override string ToString()
    {
        return $"{Username} {SessionId} {SessionVersion} {NetworkType} {AddressType} {UnicastAddress}";
    }
}