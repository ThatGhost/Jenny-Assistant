using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AudioSwitcher.AudioApi.CoreAudio;

namespace Jenny.front.Services
{
    public class VolumeService
    {
        private readonly CoreAudioDevice defaultPlaybackDevice;

        public VolumeService()
        {
            defaultPlaybackDevice = new CoreAudioController().DefaultPlaybackDevice;
        }

        public void VolumeUp()
        {
            if (defaultPlaybackDevice.Volume <= 95)
                defaultPlaybackDevice.Volume += 5;
        }

        public void VolumeDown()
        {
            if (defaultPlaybackDevice.Volume >= 5)
                defaultPlaybackDevice.Volume -= 5;
        }

        public double Volume { get { return defaultPlaybackDevice.Volume; } }
    }
}
