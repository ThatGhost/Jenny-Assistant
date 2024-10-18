using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Google.Api;

using Jenny.Core;
using Jenny.front.CommandChoices;
using Jenny.front.Services;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Jenny.front
{
    public class Installer
    {
        public static void Install(IServiceCollection services)
        {
            AddCore(services);
            AddServices(services);
            AddCommandPaths(services);
        }

        private static void AddCore(IServiceCollection Services)
        {
            Services.AddSingleton<DictationBuilder>();
            Services.AddSingleton<SpeechRecognizer>();
            Services.AddSingleton<VoiceSynthesizer>();
            Services.AddTransient<LogService>();
        }

        private static void AddServices(IServiceCollection Services)
        {
            Services.AddTransient<VolumeService>();
        }

        private static void AddCommandPaths(IServiceCollection services)
        {
            services.AddTransient<CC_Entry>();
            services.AddTransient<CC_Config>();
            services.AddTransient<CC_SpanishHelp>();
        }
    }
}
