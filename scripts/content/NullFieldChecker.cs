using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;

namespace WhatYourPain.Content;

/// <summary>
/// No .NET 8, System.Text.Json aceita <c>null</c> explícito em propriedades de referência não
/// anuláveis. Este verificador percorre o DTO desserializado e usa as anotações de
/// nulidade do código para apontar cada campo obrigatório nulo (com caminho JSON).
/// </summary>
internal static class NullFieldChecker
{
    private const string ModelNamespace = "WhatYourPain.Domain.Models";

    public static List<string> FindNullRequiredFields(object root)
    {
        var problems = new List<string>();
        CheckObject(root, "$", new NullabilityInfoContext(), problems);
        return problems;
    }

    private static void CheckObject(object obj, string path, NullabilityInfoContext context, List<string> problems)
    {
        foreach (var property in obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            // Só propriedades de dados (com init/set); propriedades calculadas ficam de fora.
            if (property.SetMethod is null || property.GetIndexParameters().Length > 0)
                continue;

            var info = context.Create(property);
            var propertyPath = $"{path}.{JsonNamingPolicy.CamelCase.ConvertName(property.Name)}";
            var value = property.GetValue(obj);
            if (value is null)
            {
                if (info.ReadState == NullabilityState.NotNull)
                    problems.Add(propertyPath);
                continue;
            }

            CheckValue(value, info, propertyPath, context, problems);
        }
    }

    private static void CheckValue(object value, NullabilityInfo info, string path, NullabilityInfoContext context, List<string> problems)
    {
        if (value is string || value.GetType().IsValueType)
            return;

        if (value is IDictionary dictionary)
        {
            var valueInfo = info.GenericTypeArguments.Length == 2 ? info.GenericTypeArguments[1] : null;
            foreach (DictionaryEntry entry in dictionary)
            {
                var entryPath = $"{path}.{entry.Key}";
                if (entry.Value is null)
                {
                    if (valueInfo?.ReadState == NullabilityState.NotNull)
                        problems.Add(entryPath);
                }
                else if (valueInfo is not null)
                    CheckValue(entry.Value, valueInfo, entryPath, context, problems);
            }
            return;
        }

        if (value is IEnumerable sequence)
        {
            var elementInfo = info.GenericTypeArguments.Length == 1 ? info.GenericTypeArguments[0] : info.ElementType;
            var index = 0;
            foreach (var element in sequence)
            {
                var elementPath = $"{path}[{index++}]";
                if (element is null)
                {
                    if (elementInfo?.ReadState == NullabilityState.NotNull)
                        problems.Add(elementPath);
                }
                else if (elementInfo is not null)
                    CheckValue(element, elementInfo, elementPath, context, problems);
            }
            return;
        }

        if (value.GetType().Namespace == ModelNamespace)
            CheckObject(value, path, context, problems);
    }
}
