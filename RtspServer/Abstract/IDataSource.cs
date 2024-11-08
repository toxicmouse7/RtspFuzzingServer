namespace RtspServer.Abstract;

public interface IDataSource
{
    public Task<byte[]> GetStreamableDataAsync();
}