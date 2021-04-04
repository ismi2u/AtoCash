using AtoCash.Authentication;
using AtoCash.Data;
using EmailService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;




namespace AtoCash
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
            //"SQLConnectionString": "server=DESKTOP-SMJTEA9\\SQLEXPRESS; database=AtoCashDB; User=sa; Password=Pa55word2019!123;trusted_connection=false; MultipleActiveResultSets=true",
            //    "WithinContainerSQLConnectionString": "server=sqldata; database=AtoCashDB; User=sa; Password=Pa55word2019!123;trusted_connection=false; MultipleActiveResultSets=true",
            //    "LocalSQLConnectionString": "server=host.docker.internal,1433; database=AtoCashDB; User=sa; Password=Pa55word2019!123;trusted_connection=false; MultipleActiveResultSets=true"
            // "AtoCashDBServer":
            //StagingServer : "server=143.110.188.154,1433; 


            services.AddDbContextPool<AtoCashDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("WithinContainerSQLConnectionString")));
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AtoCashDbContext>();
            //services.AddHttpsRedirection(options => options.HttpsPort = 443);
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(opts =>
            {
                opts.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false, // setting it to false, as we dont know the users connecting to this server
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,


                    ValidIssuer ="https://localhost:5001",
                    ValidAudience = "https://localhost:5001",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MySecretKey12323232"))
                };
            });

            //.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost")
            //                .WithOrigins("http://localhost")
            //                .WithOrigins("https://localhost")
            services.AddControllers();
            services.AddCors(options =>
               options.AddPolicy("myCorsPolicy", builder => {
                   builder.AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                   }
               ));
            //email service
            var emailConfig = Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
            services.AddSingleton(emailConfig);

            services.AddScoped<IEmailSender, EmailSender>();
            ///

            //for file upload from Angular Form
            services.Configure<FormOptions>(o =>
            {
                o.ValueLengthLimit = int.MaxValue;
               o.MultipartBodyLengthLimit = int.MaxValue;
                o.MemoryBufferThreshold = int.MaxValue;
                });
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AtoCash", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AtoCash v1"));
            }

            app.UseHttpsRedirection();
            app.UseCors("myCorsPolicy");
            app.UseRouting();
            
            app.UseAuthentication(); //add before MVC
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
