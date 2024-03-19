using Autofac;
using Autofac.Extensions.DependencyInjection;
using CloudyWing.OrderingSystem.DataAccess.Entities;
using CloudyWing.OrderingSystem.Domain.Services;
using CloudyWing.OrderingSystem.Infrastructure.DependencyInjection;
using CloudyWing.OrderingSystem.Web.Infrastructure.Localizations;
using CloudyWing.OrderingSystem.Web.Infrastructure.Localizations.Resources;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Host.ConfigureContainer<ContainerBuilder>(
    builder => {
        builder.RegisterModule(new DependencyInjectionModule(typeof(Program).Assembly, typeof(IDomainService).Assembly));
    }
);

builder.Services.AddHttpContextAccessor();

builder.Services.AddRazorPages()
    .AddRazorRuntimeCompilation()
    .AddMvcOptions(options => {
        Microsoft.AspNetCore.Mvc.ModelBinding.Metadata.DefaultModelBindingMessageProvider provider = options.ModelBindingMessageProvider;
        provider.SetAttemptedValueIsInvalidAccessor((x, y) => string.Format(ModelBindingMessage.AttemptedValueIsInvalid, x, y));
        provider.SetMissingBindRequiredValueAccessor(x => string.Format(ModelBindingMessage.MissingBindRequiredValue, x));
        provider.SetMissingKeyOrValueAccessor(() => ModelBindingMessage.MissingKeyOrValue);
        provider.SetMissingRequestBodyRequiredValueAccessor(() => ModelBindingMessage.MissingRequestBodyRequiredValue);
        provider.SetNonPropertyAttemptedValueIsInvalidAccessor(x => string.Format(ModelBindingMessage.NonPropertyAttemptedValueIsInvalid, x));
        provider.SetNonPropertyUnknownValueIsInvalidAccessor(() => ModelBindingMessage.NonPropertyUnknownValueIsInvalid);
        provider.SetNonPropertyValueMustBeANumberAccessor(() => ModelBindingMessage.NonPropertyValueMustBeANumber);
        provider.SetUnknownValueIsInvalidAccessor(x => string.Format(ModelBindingMessage.UnknownValueIsInvalid, x));
        provider.SetValueIsInvalidAccessor(x => string.Format(ModelBindingMessage.ValueIsInvalid, x));
        provider.SetValueMustBeANumberAccessor(x => string.Format(ModelBindingMessage.ValueMustBeANumber, x));
        provider.SetValueMustNotBeNullAccessor(x => string.Format(ModelBindingMessage.ValueMustNotBeNull, x));

        options.ModelMetadataDetailsProviders.Add(new LocalizationValidationMetadataProvider(typeof(ValidationMetadataMessage)));
    });

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();

WebApplication app = builder.Build();

string[] supportedCultures = ["zh-TW"];
RequestLocalizationOptions localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

app.UseRequestLocalization(localizationOptions);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    ApplicationDbContext dbContext = app.Services.GetService<ApplicationDbContext>()!;
    dbContext.Database.EnsureCreated();
} else {
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCookiePolicy(new CookiePolicyOptions { MinimumSameSitePolicy = SameSiteMode.Strict });

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
