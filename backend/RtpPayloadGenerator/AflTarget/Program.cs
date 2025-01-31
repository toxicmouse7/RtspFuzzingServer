var file = args[0];
var savePath = args[1];

var content = File.ReadAllBytes(file);

File.WriteAllBytes($"{savePath}/rtp{(ulong)Random.Shared.NextInt64()}.bin", content);