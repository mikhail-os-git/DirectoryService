using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Shared.Kernel.Envelopes;
using Shared.Kernel.Failures;

namespace Shared.Core.API;

public abstract class BaseHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger _logger;

    protected BaseHttpClient(HttpClient httpClient, ILogger logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<Result<TResponse, FailList>> GetAsync<TResponse>(Uri url, CancellationToken cancellationToken) => 
        await SendAsync<TResponse>(HttpMethod.Get, url, null, cancellationToken);

    public async Task<Result<TResponse, FailList>> PostAsync<TResponse, TBody>(Uri url, TBody body,
        CancellationToken cancellationToken) => 
        await SendAsync<TResponse>(HttpMethod.Post, url, JsonContent.Create(body), cancellationToken);
    
    public async Task<Result<TResponse, FailList>> DeleteAsync<TResponse>(Uri url, CancellationToken cancellationToken) => 
        await SendAsync<TResponse>(HttpMethod.Delete, url, null, cancellationToken);
    
    public async Task<Result<TResponse, FailList>> PutAsync<TResponse, TBody>(Uri url, TBody body,
        CancellationToken cancellationToken) => 
        await SendAsync<TResponse>(HttpMethod.Put, url, JsonContent.Create(body), cancellationToken);

    public async Task<Result<TResponse, FailList>> PatchAsync<TResponse, TBody>(Uri url, TBody body,
        CancellationToken cancellationToken) => 
        await SendAsync<TResponse>(HttpMethod.Patch, url, JsonContent.Create(body), cancellationToken);
    
    protected async Task<UnitResult<FailList>> SendAsync(
        HttpMethod method,
        Uri url,
        HttpContent? content,
        CancellationToken cancellationToken)
    {
        var response = await ExecuteQueryAsync<Envelope>(method, url, content, cancellationToken);
        return response.IsFailure ? response.Error : response.Value.ToUnitResult();
    }
    
    protected async Task<Result<TResponse, FailList>> SendAsync<TResponse>(
        HttpMethod method,
        Uri url,
        HttpContent? content,
        CancellationToken cancellationToken)
    {
        var response = await ExecuteQueryAsync<Envelope<TResponse>>(method, url, content, cancellationToken);
        return response.IsFailure ? response.Error : response.Value.ToResult();
    }
    
    protected async Task<Result<TEnvelope, FailList>> ExecuteQueryAsync<TEnvelope>(
        HttpMethod method,
        Uri url,
        HttpContent? content,
        CancellationToken cancellationToken)
        where TEnvelope : class
    {
        try
        {
            using var request = new HttpRequestMessage(method, url) { Content = content };
            using var response = await _httpClient.SendAsync(request, cancellationToken);

            TEnvelope? envelope;
            try
            {
                envelope = await response.Content.ReadFromJsonAsync<TEnvelope>(cancellationToken);
            }
            catch (JsonException exception)
            {
                _logger.LogError(
                    exception,
                    "HTTP {Method} {Url} returned non-envelope response, status {StatusCode}",
                    method, url, (int)response.StatusCode);

                return CommonErrors.InternalError.ToFailList();
            }

            if (envelope is null)
            {
                _logger.LogError(
                    "HTTP {Method} {Url} returned null envelope, status {StatusCode}",
                    method, url, (int)response.StatusCode);

                return CommonErrors.InternalError.ToFailList();
            }

            return envelope;
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "HTTP {Method} {Url} failed", method, url);
            return CommonErrors.InternalError.ToFailList();
        }
    }
}