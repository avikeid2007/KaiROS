using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace KAIROS.Services
{
    public class DownloadProgress
    {
        public double PercentComplete { get; set; }
        public long BytesDownloaded { get; set; }
        public long TotalBytes { get; set; }
        public double SpeedMBps { get; set; }
        public string FormattedSpeed { get; set; } = string.Empty;
        public TimeSpan? EstimatedTimeRemaining { get; set; }
    }

    public interface IModelDownloaderService
    {
        Task<string> DownloadModelAsync(string modelUrl, string modelName, IProgress<double>? progress = null, CancellationToken cancellationToken = default);
        Task<string> DownloadModelWithDetailsAsync(string modelUrl, string modelName, IProgress<DownloadProgress>? progress = null, CancellationToken cancellationToken = default);
        string GetModelPath(string modelName);
        bool IsModelDownloaded(string modelName);
    }

    public class ModelDownloaderService : IModelDownloaderService
    {
        private readonly HttpClient _httpClient;
        private readonly string _modelsDirectory;

        public ModelDownloaderService()
        {
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromHours(2); // Long timeout for large model downloads

            _modelsDirectory = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "KAIROS",
                "Models");

            if (!Directory.Exists(_modelsDirectory))
            {
                Directory.CreateDirectory(_modelsDirectory);
            }
        }

        public bool IsModelDownloaded(string modelName)
        {
            var modelPath = GetModelPath(modelName);
            return File.Exists(modelPath);
        }

        public string GetModelPath(string modelName)
        {
            return Path.Combine(_modelsDirectory, modelName);
        }

        public async Task<string> DownloadModelAsync(
            string modelUrl,
            string modelName,
            IProgress<double>? progress = null,
            CancellationToken cancellationToken = default)
        {
            var modelPath = GetModelPath(modelName);

            if (File.Exists(modelPath))
            {
                return modelPath;
            }

            try
            {
                // Download on background thread to avoid blocking UI
                return await Task.Run(async () =>
                {
                    using var response = await _httpClient.GetAsync(modelUrl, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
                    response.EnsureSuccessStatusCode();

                    var totalBytes = response.Content.Headers.ContentLength ?? 0;
                    var downloadedBytes = 0L;
                    var lastReportedPercentage = 0.0;

                    using var contentStream = await response.Content.ReadAsStreamAsync(cancellationToken);
                    
                    // Use larger buffer for faster downloads (1 MB)
                    using var fileStream = new FileStream(modelPath, FileMode.Create, FileAccess.Write, FileShare.None, 1024 * 1024, true);

                    var buffer = new byte[1024 * 1024]; // 1 MB buffer
                    int bytesRead;

                    while ((bytesRead = await contentStream.ReadAsync(buffer, cancellationToken)) > 0)
                    {
                        await fileStream.WriteAsync(buffer.AsMemory(0, bytesRead), cancellationToken);
                        downloadedBytes += bytesRead;

                        if (totalBytes > 0)
                        {
                            var percentage = (double)downloadedBytes / totalBytes * 100;
                            
                            // Only report progress if it changed by at least 0.1% to reduce UI updates
                            if (Math.Abs(percentage - lastReportedPercentage) >= 0.1)
                            {
                                lastReportedPercentage = percentage;
                                progress?.Report(percentage);
                            }
                        }
                    }

                    // Ensure file is flushed to disk
                    await fileStream.FlushAsync(cancellationToken);

                    return modelPath;
                }, cancellationToken);
            }
            catch (Exception ex)
            {
                if (File.Exists(modelPath))
                {
                    try
                    {
                        File.Delete(modelPath);
                    }
                    catch
                    {
                        // Ignore deletion errors
                    }
                }
                throw new Exception($"Failed to download model: {ex.Message}", ex);
            }
        }

        public async Task<string> DownloadModelWithDetailsAsync(
            string modelUrl,
            string modelName,
            IProgress<DownloadProgress>? progress = null,
            CancellationToken cancellationToken = default)
        {
            var modelPath = GetModelPath(modelName);

            if (File.Exists(modelPath))
            {
                return modelPath;
            }

            try
            {
                // Download on background thread to avoid blocking UI
                return await Task.Run(async () =>
                {
                    using var response = await _httpClient.GetAsync(modelUrl, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
                    response.EnsureSuccessStatusCode();

                    var totalBytes = response.Content.Headers.ContentLength ?? 0;
                    var downloadedBytes = 0L;
                    var startTime = DateTime.Now;
                    var lastUpdateTime = startTime;
                    var lastDownloadedBytes = 0L;

                    using var contentStream = await response.Content.ReadAsStreamAsync(cancellationToken);
                    
                    // Use larger buffer for faster downloads (1 MB)
                    using var fileStream = new FileStream(modelPath, FileMode.Create, FileAccess.Write, FileShare.None, 1024 * 1024, true);

                    var buffer = new byte[1024 * 1024]; // 1 MB buffer
                    int bytesRead;

                    while ((bytesRead = await contentStream.ReadAsync(buffer, cancellationToken)) > 0)
                    {
                        await fileStream.WriteAsync(buffer.AsMemory(0, bytesRead), cancellationToken);
                        downloadedBytes += bytesRead;

                        var now = DateTime.Now;
                        var timeSinceLastUpdate = (now - lastUpdateTime).TotalSeconds;

                        // Update progress every 0.5 seconds to reduce UI updates
                        if (timeSinceLastUpdate >= 0.5 && totalBytes > 0)
                        {
                            var percentage = (double)downloadedBytes / totalBytes * 100;
                            var bytesSinceLastUpdate = downloadedBytes - lastDownloadedBytes;
                            var speedBytesPerSecond = bytesSinceLastUpdate / timeSinceLastUpdate;
                            var speedMBps = speedBytesPerSecond / (1024 * 1024);

                            var remainingBytes = totalBytes - downloadedBytes;
                            TimeSpan? eta = null;
                            if (speedBytesPerSecond > 0)
                            {
                                var remainingSeconds = remainingBytes / speedBytesPerSecond;
                                eta = TimeSpan.FromSeconds(remainingSeconds);
                            }

                            string formattedSpeed;
                            if (speedMBps >= 1)
                            {
                                formattedSpeed = $"{speedMBps:F2} MB/s";
                            }
                            else
                            {
                                formattedSpeed = $"{speedMBps * 1024:F0} KB/s";
                            }

                            progress?.Report(new DownloadProgress
                            {
                                PercentComplete = percentage,
                                BytesDownloaded = downloadedBytes,
                                TotalBytes = totalBytes,
                                SpeedMBps = speedMBps,
                                FormattedSpeed = formattedSpeed,
                                EstimatedTimeRemaining = eta
                            });

                            lastUpdateTime = now;
                            lastDownloadedBytes = downloadedBytes;
                        }
                    }

                    // Ensure file is flushed to disk
                    await fileStream.FlushAsync(cancellationToken);

                    // Report 100% completion
                    if (totalBytes > 0)
                    {
                        progress?.Report(new DownloadProgress
                        {
                            PercentComplete = 100,
                            BytesDownloaded = totalBytes,
                            TotalBytes = totalBytes,
                            SpeedMBps = 0,
                            FormattedSpeed = "Complete",
                            EstimatedTimeRemaining = TimeSpan.Zero
                        });
                    }

                    return modelPath;
                }, cancellationToken);
            }
            catch (Exception ex)
            {
                if (File.Exists(modelPath))
                {
                    try
                    {
                        File.Delete(modelPath);
                    }
                    catch
                    {
                        // Ignore deletion errors
                    }
                }
                throw new Exception($"Failed to download model: {ex.Message}", ex);
            }
        }
    }
}
