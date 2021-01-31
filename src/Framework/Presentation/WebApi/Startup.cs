using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Core;
using Core.Configuration;
using PluginCore.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Repositories.Core;
using WebApi.Infrastructure;
using Framework.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace WebApi
{
    public class Startup
    {
        #region Fields

        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private RemConfig _remConfig;

        #endregion

        #region Ctor

        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
        }

        #endregion

        #region ConfigureServices
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // ����ע��
            _remConfig = _configuration.GetSection(RemConfig.Rem).Get<RemConfig>();
            services.Configure<RemConfig>(_configuration.GetSection(
                RemConfig.Rem));

            #region ѡ�����ݿ�����
            string dbType = _configuration["Rem:DbType"];
            string connStr = _configuration.GetConnectionString("DefaultConnection");
            switch (dbType.ToLower())
            {
                case "sqlite":

                    if (connStr.StartsWith("~"))
                    {
                        // ���·��ת����·��
                        string dir = Directory.GetCurrentDirectory();
                        string dbFilePath = Path.Combine(dir, connStr);

                        connStr = dbFilePath;
                    }

                    services.AddDbContext<RemDbContext>(options =>
                        options.UseSqlite(connStr));
                    break;
                case "mysql":
                    services.AddDbContext<RemDbContext>(options =>
                        options.UseMySQL(connStr));
                    break;
                case "sqlserver":
                    services.AddDbContext<RemDbContext>(options =>
                        options.UseSqlServer(connStr));
                    break;
                default:

                    if (connStr.StartsWith("~"))
                    {
                        // ���·��ת����·��
                        string dir = Directory.GetCurrentDirectory();
                        string dbFilePath = Path.Combine(dir, connStr);

                        connStr = dbFilePath;
                    }

                    services.AddDbContext<RemDbContext>(options =>
                        options.UseSqlite(connStr));
                    break;
            }
            #endregion

            //services.AddControllers();

            #region for UHub IdentityServer4
            // accepts any access token issued by identity server
            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = _configuration["Rem:Authority"];

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false,
                        // �೤ʱ������֤���� Token
                        ClockSkew = TimeSpan.FromSeconds(5),
                        // ����Ҫ�� Token ��Ҫ�г�ʱʱ���������
                        RequireExpirationTime = true,
                    };

                    options.RequireHttpsMetadata = false;
                });
            #endregion

            #region �����Ȩ����-���б�� [WebApiAuthorize] ����ҪȨ�޼��
            services.AddSingleton<IAuthorizationHandler, WebApiAuthorizationHandler>();

            // adds an authorization policy to make sure the token is for scope 'webapi'
            services.AddAuthorization(options =>
            {
                options.AddPolicy("WebApi", policy =>
                {
                    // �޷����� �·��κ�һ�HTTP 403 ����
                    // 1. ���¼������֤�û�  (ע�⣺�޷��������� JWT Token �޷�ͨ��Ч��, HTTP 401 ����)
                    policy.RequireAuthenticatedUser();
                    // 2.��Ҫ JWT scope �а��� Remember.Core.WebApi
                    policy.RequireClaim("scope", "Remember.Core.WebApi");
                    // 3.��Ҫ ����Ƿ�ӵ�е�ǰ������Դ��Ȩ��
                    policy.Requirements.Add(new WebApiRequirement());
                });
            });
            #endregion

            // MVC: Install ҳ��ʹ�� Views
            services.AddControllersWithViews();

            // ��Ӳ�����
            services.AddPluginFramework();

            // ����������������
            if (_webHostEnvironment.IsDevelopment())
            {
                services.AddCors(m => m.AddPolicy("Development", a => a.AllowAnyOrigin().AllowAnyHeader()));
            }
        }
        #endregion

        #region Configure
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                // ����������������
                app.UseCors("Development");
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            // ��Ҫ��Ȩ: Ϊ�˱��� api ��Դ
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // TODO: Debug Assemblies ��
            //var ass = AppDomain.CurrentDomain.GetAssemblies();
        }
        #endregion

        #region ConfigureContainer
        // ConfigureContainer is where you can register things directly
        // with Autofac. This runs after ConfigureServices so the things
        // here will override registrations made in ConfigureServices.
        // Don't build the container; that gets done for you by the factory.
        public void ConfigureContainer(ContainerBuilder builder)
        {
            // Register your own things directly with Autofac, like:
            builder.RegisterModule(new AutofacApplicationModule());
        }
        #endregion
    }
}
