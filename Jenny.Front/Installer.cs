using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Jenny.Core;
using Jenny.front.CommandChoices;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Jenny.front
{
    public class Installer
    {
        public static void Install(IServiceCollection Services)
        {
            Services.AddSingleton<DictationChoicesBuilder>();
            Services.AddSingleton<SpeechRecognitionWrapper>();
            Services.AddSingleton<VoiceSynthWrapper>();

            Services.AddTransient<CC_Entry>();
            Services.AddTransient<CC_Config>();
        }
    }
}
