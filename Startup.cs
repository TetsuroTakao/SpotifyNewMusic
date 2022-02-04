using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

namespace SpotifyNewMusic
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            #region insert settings value
            Environment.SetEnvironmentVariable("EncKey", Configuration["Encryptor:AesCryptoServiceProviderKey"]);
            Environment.SetEnvironmentVariable("EncIV", Configuration["Encryptor:AesCryptoServiceProviderIV"]);
            Environment.SetEnvironmentVariable("Redirect", Configuration["Redirect"]);
            Environment.SetEnvironmentVariable("RedirectLocal", Configuration["RedirectLocal"]);
            Environment.SetEnvironmentVariable("ChallengeSeed", Configuration["ChallengeSeed"]);
            Environment.SetEnvironmentVariable("ChallengeLength", Configuration["ChallengeLength"]);
            Environment.SetEnvironmentVariable("DefaultBrand", Configuration["DefaultBrand"]);
            foreach (var s in Configuration.GetSection("SupportIdentities").GetChildren())
            {
                foreach (var s2 in s.GetChildren())
                {
                    Environment.SetEnvironmentVariable(s.Key + "_" + s2.Key, s2.Value);
                }
            }
            #endregion
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddMvc()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization();
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "contents")),
                RequestPath = "/contents"
            });
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            var supportedCultures = new[] { "ja", "en-US" };
            var localizationOptions = new RequestLocalizationOptions().SetDefaultCulture(supportedCultures[0])
                .AddSupportedCultures(supportedCultures)
                .AddSupportedUICultures(supportedCultures);

            app.UseRequestLocalization(localizationOptions);
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
