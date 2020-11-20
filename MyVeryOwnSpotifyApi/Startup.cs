using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using MyVeryOwnSpotifyApi.Controllers;
using MyVeryOwnSpotifyApi.Extensions;
using MyVeryOwnSpotifyApi.Models.Config;
using MyVeryOwnSpotifyApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;


namespace MyVeryOwnSpotifyApi
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
           .SetBasePath(env.ContentRootPath)
           .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
           .AddJsonFile("AuthConfig.json", optional: false, reloadOnChange: true)
           .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            //Memory cache for storing tokens with SpotifyAuthService
            services.AddMemoryCache();

     

            Action<HttpClient> httpClientCfg = client =>
            {
                var authConfig = Configuration.Get<AuthConfig>();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(
                        System.Text.Encoding.ASCII.GetBytes($"{authConfig.ClientID}:{authConfig.ClientSecret}")));
              
            };

            //Inject a httpclient for initally getting and refreshing the bearer token from http://https://accounts.spotify.com/api/token
            services.AddHttpClient<SpotifyAuthService>()
                    .ConfigureHttpClient(httpClientCfg).ConfigurePrimaryHttpMessageHandler(_ => new HttpClientHandler { UseDefaultCredentials = true, }); 

            services.AddTransient<SpotifyAuthHttpClientHandler>();
            
            services.AddHttpClient<SpotifyApi>()
                    .ConfigureHttpClient((provider, c) => c.BaseAddress = new Uri("https://api.spotify.com/v1/search"))
                    .AddHttpMessageHandler<SpotifyAuthHttpClientHandler>();


            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MyVeryOwnSpotifyApi", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyVeryOwnSpotifyApi v1"));
            }


            app.ConfigureCustomExceptionMiddleware();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
