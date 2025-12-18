using KAIROS.Models;
using System.Collections.Generic;

namespace KAIROS.Services
{
    public static class LLMModelCatalog
    {
        public static List<LLMModel> GetAvailableModels()
        {
            return new List<LLMModel>
            {
                // Small Models (1-3GB)
                new LLMModel
                {
                    Name = "phi-3-mini-4k-instruct-q4.gguf",
                    DisplayName = "Phi-3 Mini 3.8B",
                    Description = "Microsoft's excellent small model with impressive capabilities for its size. Great for general conversations, reasoning, and basic coding.",
                    SizeText = "2.2 GB",
                    SizeBytes = 2362232012,
                    DownloadUrl = "https://huggingface.co/microsoft/Phi-3-mini-4k-instruct-gguf/resolve/main/Phi-3-mini-4k-instruct-q4.gguf",
                    Capabilities = "Conversation • Reasoning • Coding • Fast",
                    MinRam = "4 GB",
                    Category = "small",
                    IsRecommended = true
                },
                new LLMModel
                {
                    Name = "phi-2.Q4_K_M.gguf",
                    DisplayName = "Phi-2 2.7B",
                    Description = "Compact yet powerful model with strong reasoning abilities. Excellent for coding tasks and technical conversations.",
                    SizeText = "1.6 GB",
                    SizeBytes = 1719664935,
                    DownloadUrl = "https://huggingface.co/TheBloke/phi-2-GGUF/resolve/main/phi-2.Q4_K_M.gguf",
                    Capabilities = "Coding • Reasoning • Technical • Compact",
                    MinRam = "4 GB",
                    Category = "small",
                    IsRecommended = false
                },
                new LLMModel
                {
                    Name = "Llama-3.2-3B-Instruct-Q4_K_M.gguf",
                    DisplayName = "LLaMA 3.2 3B Instruct",
                    Description = "Meta's latest small model with excellent quality. Great balance of size and capability for everyday use.",
                    SizeText = "1.9 GB",
                    SizeBytes = 2040109465,
                    DownloadUrl = "https://huggingface.co/bartowski/Llama-3.2-3B-Instruct-GGUF/resolve/main/Llama-3.2-3B-Instruct-Q4_K_M.gguf",
                    Capabilities = "Conversation • Reasoning • Multilingual",
                    MinRam = "4 GB",
                    Category = "small",
                    IsRecommended = false
                },

                // Medium Models (3-5GB)
                new LLMModel
                {
                    Name = "mistral-7b-instruct-v0.2.Q4_K_M.gguf",
                    DisplayName = "Mistral 7B Instruct v0.2",
                    Description = "Popular and highly capable 7B model with excellent instruction following. Great for complex conversations and tasks.",
                    SizeText = "4.4 GB",
                    SizeBytes = 4721669018,
                    DownloadUrl = "https://huggingface.co/TheBloke/Mistral-7B-Instruct-v0.2-GGUF/resolve/main/mistral-7b-instruct-v0.2.Q4_K_M.gguf",
                    Capabilities = "Advanced Conversation • Coding • Analysis • Creative",
                    MinRam = "8 GB",
                    Category = "medium",
                    IsRecommended = true
                },

                // Large Models (5-8GB)
                new LLMModel
                {
                    Name = "Meta-Llama-3.1-8B-Instruct-Q4_K_M.gguf",
                    DisplayName = "LLaMA 3.1 8B Instruct",
                    Description = "Meta's powerful 8B model with strong reasoning and coding abilities. Excellent for complex tasks and detailed responses.",
                    SizeText = "4.9 GB",
                    SizeBytes = 5268916985,
                    DownloadUrl = "https://huggingface.co/bartowski/Meta-Llama-3.1-8B-Instruct-GGUF/resolve/main/Meta-Llama-3.1-8B-Instruct-Q4_K_M.gguf",
                    Capabilities = "Advanced Reasoning • Coding • Long Context • Detailed",
                    MinRam = "12 GB",
                    Category = "large",
                    IsRecommended = false
                },
                new LLMModel
                {
                    Name = "gemma-2-9b-it-Q4_K_M.gguf",
                    DisplayName = "Gemma 2 9B Instruct",
                    Description = "Google's high-quality 9B model with excellent performance. Great for demanding tasks requiring deep understanding.",
                    SizeText = "5.4 GB",
                    SizeBytes = 5796827013,
                    DownloadUrl = "https://huggingface.co/bartowski/gemma-2-9b-it-GGUF/resolve/main/gemma-2-9b-it-Q4_K_M.gguf",
                    Capabilities = "Premium Quality • Advanced Reasoning • Coding • Creative",
                    MinRam = "12 GB",
                    Category = "large",
                    IsRecommended = false
                }
            };
        }
    }
}
