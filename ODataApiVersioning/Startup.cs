namespace ODataApiVersioning
{
    using Microsoft.AspNet.OData;
    using Microsoft.AspNet.OData.Builder;
    using Microsoft.AspNet.OData.Extensions;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Mvc.Versioning;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using static Microsoft.OData.ODataUrlKeyDelimiter;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options => options.EnableEndpointRouting = false);
            services.AddApiVersioning(
                options =>
                {
                    options.ApiVersionReader = ApiVersionReader.Combine(
                        new QueryStringApiVersionReader(), 
                        new HeaderApiVersionReader("api-version"));
                    options.AssumeDefaultVersionWhenUnspecified = true;
                    options.DefaultApiVersion = new ApiVersion(2, 0);
                    options.ReportApiVersions = true;
                });
            services.AddOData().EnableApiVersioning();
        }

        public void Configure(IApplicationBuilder app, VersionedODataModelBuilder modelBuilder)
        {
            app.UseMvc(
                routeBuilder =>
                {
                    var models = modelBuilder.GetEdmModels();

                    routeBuilder.ServiceProvider.GetRequiredService<ODataOptions>().UrlKeyDelimiter = Parentheses;
                    routeBuilder.MapVersionedODataRoutes("odata", "api", models);
                });
        }
    }
}