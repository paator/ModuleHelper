using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModuleHelper.Utility
{
    public interface ISoundEngine
    {
        public IEnumerable<ISampleProvider> CreateArpeggioSamples(IEnumerable<int> chordKeyNumbers, int length, double speed);
        public ISampleProvider ConnectSamples(IList<ISampleProvider> samples);
        public void PlayKey(int keyNumber);
        public void PlayArpeggio(IEnumerable<int> keys, int times, double speed);
        public void StopAndPlay(ISampleProvider provider);
    }
}