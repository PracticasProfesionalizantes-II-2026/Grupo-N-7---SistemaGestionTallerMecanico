using System.Globalization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TallerMecanicoFront.Infrastructure;

public sealed class FlexibleDecimalModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
        if (value == ValueProviderResult.None)
        {
            return Task.CompletedTask;
        }

        var text = value.FirstValue?.Trim();
        if (string.IsNullOrWhiteSpace(text))
        {
            return Task.CompletedTask;
        }

        var normalized = Normalizar(text);
        if (decimal.TryParse(normalized, NumberStyles.Number, CultureInfo.InvariantCulture, out var result))
        {
            bindingContext.Result = ModelBindingResult.Success(result);
        }
        else
        {
            bindingContext.ModelState.AddModelError(bindingContext.ModelName, "El valor debe ser un número decimal válido.");
        }

        return Task.CompletedTask;
    }

    private static string Normalizar(string value)
    {
        var lastComma = value.LastIndexOf(',');
        var lastDot = value.LastIndexOf('.');

        if (lastComma >= 0 && lastDot >= 0)
        {
            return lastComma > lastDot
                ? value.Replace(".", string.Empty).Replace(',', '.')
                : value.Replace(",", string.Empty);
        }

        return value.Replace(',', '.');
    }
}

public sealed class FlexibleDecimalModelBinderProvider : IModelBinderProvider
{
    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        var modelType = Nullable.GetUnderlyingType(context.Metadata.ModelType) ?? context.Metadata.ModelType;
        return modelType == typeof(decimal)
            ? new FlexibleDecimalModelBinder()
            : null;
    }
}
