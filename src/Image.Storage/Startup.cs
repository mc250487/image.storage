using Image.Storage.Configuration;
using Image.Storage.InternalServices;
using Image.Storage.InternalServices.Impl;
using Image.Storage.Services;
using Image.Storage.Services.Impl;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Image.Storage
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddTransient(typeof(IImageStorageConfiguration), typeof(ImageStorageConfiguration));
            
            services.AddTransient(typeof(IFileStorage), typeof(FileStorage));

            services.AddTransient(typeof(IImagePreviewProvider), typeof(ImagePreviewProvider));

            services.AddTransient(typeof(IImageRepository), typeof(ImageRepository));

            services.AddTransient(typeof(IImageUploader), typeof(ImageUploader));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
