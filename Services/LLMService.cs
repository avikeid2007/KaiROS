using LLama;
using LLama.Common;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KAIROS.Services
{
    public interface ILLMService
    {
        Task<bool> InitializeAsync(string modelPath);
        IAsyncEnumerable<string> GenerateResponseAsync(List<Models.ChatMessage> chatHistory, CancellationToken cancellationToken = default);
        bool IsInitialized { get; }
        void Dispose();
    }

    public class LLMService : ILLMService, IDisposable
    {
        private LLamaWeights? _model;
        private LLamaContext? _context;
        private bool _isInitialized;

        public bool IsInitialized => _isInitialized;

        public async Task<bool> InitializeAsync(string modelPath)
        {
            try
            {
                if (!File.Exists(modelPath))
                {
                    throw new FileNotFoundException($"Model file not found: {modelPath}");
                }

                // Backend initialization removed; native libraries are copied via project configuration.

                var parameters = new ModelParams(modelPath)
                {
                    ContextSize = 4096,
                    GpuLayerCount = 0 // CPU only, increase if you have GPU
                };

                _model = await Task.Run(() => LLamaWeights.LoadFromFile(parameters));
                _context = _model.CreateContext(parameters);

                _isInitialized = true;
                return true;
            }
            catch (Exception ex)
            {
                _isInitialized = false;
                throw new Exception($"Failed to initialize LLM: {ex.Message}", ex);
            }
        }

        public async IAsyncEnumerable<string> GenerateResponseAsync(
            List<Models.ChatMessage> chatHistory,
            [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            if (!_isInitialized || _context == null || _model == null)
            {
                throw new InvalidOperationException("LLM is not initialized. Call InitializeAsync first.");
            }

            // Use StatelessExecutor for better control and continuous generation
            var executor = new StatelessExecutor(_model, _context.Params);

            // Build prompt from chat history
            var prompt = BuildPromptFromHistory(chatHistory);

            var inferenceParams = new InferenceParams
            {
                MaxTokens = 2048, // Increased for longer responses
                AntiPrompts = new List<string>
                {
                    "User:",
                    "\nUser:",
                    "\n\nUser:",
                    "<|user|>",
                    "<|endoftext|>",
                    "</s>"
                }
            };

            await foreach (var text in executor.InferAsync(prompt, inferenceParams, cancellationToken))
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    yield break;
                }

                // Filter out repetitive assistant tags
                if (text.Contains("<|assistant|>") || text.Contains("[assistant]"))
                {
                    continue;
                }

                yield return text;
            }
        }

        private string BuildPromptFromHistory(List<Models.ChatMessage> chatHistory)
        {
            // Use chat template format for better quality
            var promptBuilder = new System.Text.StringBuilder();

            // System prompt
            promptBuilder.AppendLine("### System:");
            promptBuilder.AppendLine("You are KAIROS, a helpful and knowledgeable AI assistant. Provide clear, accurate, and concise responses.");
            promptBuilder.AppendLine();

            // Add conversation history
            foreach (var msg in chatHistory)
            {
                if (msg.Role == "user")
                {
                    promptBuilder.AppendLine("### User:");
                    promptBuilder.AppendLine(msg.Content);
                    promptBuilder.AppendLine();
                }
                else
                {
                    promptBuilder.AppendLine("### Assistant:");
                    promptBuilder.AppendLine(msg.Content);
                    promptBuilder.AppendLine();
                }
            }

            // Prompt for assistant response
            var lastMessage = chatHistory.LastOrDefault();
            if (lastMessage?.Role == "user")
            {
                promptBuilder.Append("### Assistant:");
            }

            return promptBuilder.ToString();
        }

        public void Dispose()
        {
            _context?.Dispose();
            _model?.Dispose();
            _isInitialized = false;
        }
    }
}
