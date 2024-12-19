using ManagementServer.Infrastructure.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ManagementServer.Configuration.EntityConfiguration;

public class RtpFuzzingPresetConfiguration : IEntityTypeConfiguration<RtpFuzzingPreset>
{
    public void Configure(EntityTypeBuilder<RtpFuzzingPreset> builder)
    {
        builder.HasKey(e => e.Id);
        builder.OwnsOne(e => e.Header);
        builder.OwnsOne(e => e.AppendSettings);

        var valueComparer = new ValueComparer<byte[]>(
            (c1, c2) => c1 != null && c1.SequenceEqual(c2 ?? ArraySegment<byte>.Empty),
            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
            c => c.ToArray());
        
        builder.Property(e => e.Payload).HasConversion(
            payload => payload == null ? null : Convert.ToBase64String(payload),
            payload => payload == null ? null : Convert.FromBase64String(payload),
            valueComparer);
    }
}