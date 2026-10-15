using System.Text.Json;
using System.Text.Json.Serialization;

namespace WhatYourPain.Content;

/// <summary>
/// Opções de System.Text.Json compartilhadas por conteúdo e snapshot: chaves camelCase
/// (data-model), enums como texto SNAKE_CASE maiúsculo, campo desconhecido rejeitado
/// (evita erro de digitação silencioso), números não aceitos como texto, sem comentários.
/// </summary>
public static class ContentJson
{
    public static JsonSerializerOptions Options { get; } = Create();

    private static JsonSerializerOptions Create()
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow,
            NumberHandling = JsonNumberHandling.Strict,
            ReadCommentHandling = JsonCommentHandling.Disallow,
            AllowTrailingCommas = false,
            WriteIndented = true,
        };
        options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseUpper, allowIntegerValues: false));
        options.MakeReadOnly(populateMissingResolver: true);
        return options;
    }
}
