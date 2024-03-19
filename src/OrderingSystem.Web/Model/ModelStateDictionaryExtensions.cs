namespace Microsoft.AspNetCore.Mvc.ModelBinding {
    public static class ModelStateDictionaryExtensions {
        public static string? GetFirstErrorMessage(this ModelStateDictionary modelState) {
            return modelState.First(x => x.Value!.Errors.Count > 0)
                .Value?.Errors.FirstOrDefault()?.ErrorMessage;
        }
    }
}
