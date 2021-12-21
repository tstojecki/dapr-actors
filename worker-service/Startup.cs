using Shared;
using Shared.Actors;

namespace WorkerService
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddDapr();
            services.AddDaprSidekick(Configuration);
            services.AddActors(options =>
            {
                options.Actors.RegisterActor<StateActor>();
            });

            services.AddTransient<IStateManager, ActorStateManager>();

            services.AddMemoryCache();
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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCloudEvents();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapSubscribeHandler();
                endpoints.MapActorsHandlers();

                endpoints.MapControllers();

                // k8 probes (liveness, readiness and startup) 
                // other services, e.g. consul in self-hosted
                endpoints.MapHealthChecks("/health");

                endpoints.MapDaprMetrics();
            });
        }
    }
}
