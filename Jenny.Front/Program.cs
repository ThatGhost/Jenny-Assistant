using Microsoft.Extensions.DependencyInjection;
using Jenny.Core;
using Jenny.front.CommandChoices;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Jenny.front;

namespace Jenny_front
{
    internal class Program
    {
        private static CancellationToken CancellationToken { get; set; }

        static void Main(string[] args)
        {
            HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
            builder.Configuration.AddUserSecrets<Program>();

            Installer.Install(builder.Services);

            InitializeCore(builder);
        }

        static void InitializeCore(HostApplicationBuilder builder)
        {
            using IHost host = builder.Build();

            // initialize dictation builder
            DictationBuilder dictationBuilder = host.Services.GetService<DictationBuilder>()!;
            CC_Entry entry = host.Services.GetService<CC_Entry>()!;
            dictationBuilder.EntryCommand = entry; // for stop command
            dictationBuilder.AddCommandPath(entry); // for now

            // initialize speech recognizer
            SpeechRecognizer speechRecognizer = host.Services.GetService<SpeechRecognizer>()!;
            dictationBuilder.updateGrammerFunction = speechRecognizer.UpdateGrammar; // update the grammar when stop is done
            speechRecognizer.UpdateGrammar();

            // initialize voice synthesizer
            VoiceSynthesizer voiceSynthesizer = host.Services.GetService<VoiceSynthesizer>()!;
            var googleTTSApiKey = builder.Configuration["Google:ApiKey"]; // secret api key
            voiceSynthesizer.SetTTSApiKey(googleTTSApiKey!);

            Console.WriteLine("Setup complete, you can speak now");
            Console.ReadLine();
        }
    }
}
