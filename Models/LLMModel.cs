namespace KAIROS.Models
{
    public class LLMModel
    {
        public string Name { get; init; } = string.Empty;
        public string DisplayName { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public string SizeText { get; init; } = string.Empty;
        public long SizeBytes { get; init; }
        public string DownloadUrl { get; init; } = string.Empty;
        public string Capabilities { get; init; } = string.Empty;
        public string MinRam { get; init; } = string.Empty;
        public string Category { get; init; } = string.Empty; // "small", "medium", "large"
        public bool IsRecommended { get; init; }
    }
}
