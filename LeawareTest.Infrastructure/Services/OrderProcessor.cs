using LeawareTest.Application.Interfaces;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using OpenAI;
using OpenAI.Chat;

namespace LeawareTest.Infrastructure.Services;
internal record OpenAISettings
{
    public string ApiKey { get; init; }
    public string Model { get; init; }
    public int MaxTokens { get; init; }
}

internal class OrderProcessor : IOrderProcessor
{
    private const string Prompt = """
                                  The body of the attached email contains an order with order items.
                                  Process the email and extract the order items into a table with the following fields:
                                  - product
                                  - count
                                  - price

                                  Return the result as a JSON array, where each element has the fields:
                                  - "product" (string)
                                  - "count" (integer)
                                  - "price" (number, in the currency used in the email)

                                  Example output:
                                  ```json
                                  [
                                    { "product": "Product Name", "count": 1, "price": 49.00 }
                                  ]
                                  ```
                                  """;

    private readonly OpenAIClient _client;
    private readonly OpenAISettings _settings;
    private readonly ILogger<OrderProcessor> _logger;

    public OrderProcessor(IOptions<OpenAISettings> options, ILogger<OrderProcessor> logger)
    {
        _logger = logger;
        _settings = options.Value;
        _client = new OpenAIClient(_settings.ApiKey);
    }

    public async Task<IReadOnlyCollection<OrderDto>> ExtractOrder(string emailBody, CancellationToken cancellationToken)
    {
        try
        {
            var fullPrompt = $"{Prompt}\n\n---\nAttached email body:\n{emailBody}\n---";

            var chatRequest = new ChatRequest(
                messages: new[]
                {
                    new Message(Role.System, "You are a helpful assistant that extracts order items from emails."),
                    new Message(Role.User, fullPrompt)
                },
                model: _settings.Model,
                maxTokens:    _settings.MaxTokens
            );

            var chatResponse = await _client.ChatEndpoint.GetCompletionAsync(chatRequest, cancellationToken);

            var text = chatResponse.FirstChoice.Message.Content.ToString();

            var jsonStart = text.IndexOf('[');
            var jsonEnd = text.LastIndexOf(']');
            if (jsonStart == -1 || jsonEnd == -1 || jsonEnd < jsonStart)
                throw new InvalidOperationException("No JSON array found in OpenAI response.");

            var json = text.Substring(jsonStart, jsonEnd - jsonStart + 1);

            var orders = JsonSerializer.Deserialize<List<OrderDto>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return orders ?? new List<OrderDto>();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error processing order from email body.");
            return new List<OrderDto>();
        }
    }
}
