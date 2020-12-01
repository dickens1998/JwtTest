using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace JwtTest
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
            services.AddControllers();

            // 添加配置信息
            services.AddAuthentication(option =>
            {
                // 设置默认使用JWT验证方式
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(option =>
            {
                IConfigurationSection configuration = Configuration.GetSection("Authentication");
                option.TokenValidationParameters = new TokenValidationParameters()
                {
                    // 验证接收者
                    ValidateAudience = true,
                    // 验证发布者
                    ValidateIssuer = true,
                    // 验证过期时间
                    ValidateLifetime = true,
                    // 验证密钥
                    ValidateIssuerSigningKey = true,
                    // 读取配置Issure
                    ValidIssuer = configuration["Issure"],
                    // 读取配置Audience
                    ValidAudience = configuration["Audience"],
                    // 设置生成Token的密钥
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["SecurityKey"]))

                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            // 注意必须在UseAuthorization上面
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
