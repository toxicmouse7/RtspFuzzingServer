using ManagementServer.Infrastructure.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ManagementServer.Configuration.EntityConfiguration;

public class RawFuzzingDataConfiguration : IEntityTypeConfiguration<RawFuzzingData>
{
    public void Configure(EntityTypeBuilder<RawFuzzingData> builder)
    {
        builder.HasKey(e => e.Id);
        
        var valueComparer = new ValueComparer<byte[]>(
            (c1, c2) => c1 != null && c1.SequenceEqual(c2 ?? ArraySegment<byte>.Empty),
            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
            c => c.ToArray());
        
        builder.Property(e => e.RawData).HasConversion(
            raw => Convert.ToBase64String(raw),
            raw => Convert.FromBase64String(raw),
            valueComparer);
    }
}