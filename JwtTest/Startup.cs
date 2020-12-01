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

            // ���������Ϣ
            services.AddAuthentication(option =>
            {
                // ����Ĭ��ʹ��JWT��֤��ʽ
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(option =>
            {
                IConfigurationSection configuration = Configuration.GetSection("Authentication");
                option.TokenValidationParameters = new TokenValidationParameters()
                {
                    // ��֤������
                    ValidateAudience = true,
                    // ��֤������
                    ValidateIssuer = true,
                    // ��֤����ʱ��
                    ValidateLifetime = true,
                    // ��֤��Կ
                    ValidateIssuerSigningKey = true,
                    // ��ȡ����Issure
                    ValidIssuer = configuration["Issure"],
                    // ��ȡ����Audience
                    ValidAudience = configuration["Audience"],
                    // ��������Token����Կ
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

            // ע�������UseAuthorization����
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
