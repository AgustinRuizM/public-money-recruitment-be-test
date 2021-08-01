using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using VacationRental.Api.Core.Services.Contracts;
using VacationRental.Api.Core.Services.Implementations;
using VacationRental.Api.Model;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using AutoMapper;
using Microsoft.OpenApi.Models;
using VacationRental.Api.Core.Repository;

namespace VacationRental.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options => options.EnableEndpointRouting = false).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddAutoMapper(typeof(Startup));
            services.AddSwaggerGen(opts => opts.SwaggerDoc("v1", new OpenApiInfo { Title = "Vacation rental information", Version = "v1" }));
            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture("en");
                var supportedCultures = new List<CultureInfo>
                {
                    new CultureInfo("en"),
                    new CultureInfo("es"),
                };
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
                options.FallBackToParentUICultures = true;
            });

            services.AddSingleton<IMapper, Mapper>();
            services.AddSingleton(typeof(IRepository<>), typeof(BaseRepository<>));
            services.AddSingleton<IBookingService<Booking>, BookingService<Booking>>();
            services.AddSingleton<IRentalService<Rental>, RentalService<Rental>>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(opts => {
                opts.SwaggerEndpoint("/swagger/v1/swagger.json", "VacationRental v1");
                opts.RoutePrefix = "swagger";
            });

            //This determines the user culture with information provided by the browser, used for localization
            var options = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(options.Value);

        }
    }
}
