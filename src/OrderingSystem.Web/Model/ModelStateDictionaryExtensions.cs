namespace Microsoft.AspNetCore.Mvc.ModelBinding;

public static class ModelStateDictionaryExtensions {
    public static string? GetFirstErrorMessage(this ModelStateDictionary modelState) {
        ModelStateEntry? modelStateEntry = modelState.Select(x => x.Value)
            .FirstOrDefault(x => x?.Errors.Count > 0);

        return modelStateEntry?.Errors.FirstOrDefault()?.ErrorMessage;
    }
}