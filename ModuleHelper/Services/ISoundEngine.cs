using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModuleHelper.Services
{
    public interface ISoundEngine
    {
        IEnumerable<ISampleProvider> CreateArpeggioSamples(IEnumerable<int> chordKeyNumbers, int length, double speed);
        ISampleProvider ConnectSamples(IList<ISampleProvider> samples);
        void PlayKey(int keyNumber);
        void PlayArpeggio(IEnumerable<int> keys, int times, double speed);
        void StopAndPlay(ISampleProvider provider);
    }
}