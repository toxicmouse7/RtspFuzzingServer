using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RtpPayloadGenerator.Dto;

namespace RtpPayloadGenerator.Controllers;

[Route("[controller]")]
[ApiController]
public class BinaryPayloadController : ControllerBase
{
    [HttpPost("generate")]
    public async Task<IActionResult> GeneratePayload([FromBody] BinaryData data, [FromQuery] int genTimeSec)
    {
        var id = Guid.NewGuid();
        Directory.CreateDirectory($"/tmp/input_dir/{id}");
        Directory.CreateDirectory($"/tmp/output_dir/{id}");
        Directory.CreateDirectory($"/tmp/rtp_bin/{id}");
        
        await System.IO.File.WriteAllBytesAsync($"/tmp/input_dir/{id}/rtp.bin", Convert.FromBase64String(data.Data));
        
        var process = new Process
        {
            StartInfo =
            {
                FileName = "afl-fuzz",
                Arguments = $"-n -i /tmp/input_dir/{id} -o /tmp/output_dir/{id} -- dotnet /app/AflTarget/AflTarget.dll @@ /tmp/rtp_bin/{id}"
            }
        };

        process.Start();
        await Task.Delay(TimeSpan.FromSeconds(genTimeSec));
        process.Kill(true);
        
        await process.WaitForExitAsync();

        var files = Directory.EnumerateFiles($"/tmp/rtp_bin/{id}");

        var result = new List<BinaryData>();
        foreach (var file in files)
        {
            var content = await System.IO.File.ReadAllBytesAsync(file);
            result.Add(new BinaryData { Data = Convert.ToBase64String(content) });
        }
        
        Directory.Delete($"/tmp/input_dir/{id}", true);
        Directory.Delete($"/tmp/output_dir/{id}", true);
        Directory.Delete($"/tmp/rtp_bin/{id}", true);
        
        return Ok(result);
    }
}