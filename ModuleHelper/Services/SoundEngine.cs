using ModuleHelper.Utils;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleHelper.Services
{
    public class SoundEngine : ISoundEngine
    {
        public ISampleProvider ConnectSamples(IList<ISampleProvider> samples)
        {
            if (!samples.Any()) return null;

            var concatenatedWaveforms = samples[0];

            foreach (var input in samples.Skip(1))
            {
                concatenatedWaveforms = concatenatedWaveforms.FollowedBy(input);
            }

            return concatenatedWaveforms;
        }

        public IEnumerable<ISampleProvider> CreateArpeggioSamples(IEnumerable<int> chordKeyNumbers, int length, double speed)
        {
            if (!chordKeyNumbers.Any()) return null;

            List<ISampleProvider> arpeggioInputs = new List<ISampleProvider>();

            for (int i = 0; i <= length; i++)
            {
                foreach (var num in chordKeyNumbers)
                {
                    var squareWave = new SignalGenerator()
                    {
                        Gain = 0.12,
                        Frequency = MusicalMathHelper.CalculateFrequency(num, 4),
                        Type = SignalGeneratorType.Square
                    };

                    var trimmed = new OffsetSampleProvider(squareWave);
                    var trimmedWithTimeSpan = trimmed.Take(TimeSpan.FromSeconds(speed));
                    arpeggioInputs.Add(trimmedWithTimeSpan);
                }
            }

            return arpeggioInputs;
        }

        public void PlayKey(int keyNumber)
        {
            var squareWave = new SignalGenerator()
            {
                Gain = 0.12,
                Frequency = MusicalMathHelper.CalculateFrequency(keyNumber, 4),
                Type = SignalGeneratorType.Square
            };

            var trimmed = new OffsetSampleProvider(squareWave);
            var trimmedWithTimeSpan = trimmed.Take(TimeSpan.FromSeconds(0.4));

            WaveformPlayer.Instance.PlayWaveform(trimmedWithTimeSpan);
        }

        public void PlayArpeggio(IEnumerable<int> keyNumbers, int length, double speed)
        {
            if (!keyNumbers.Any() || keyNumbers == null) return;
            var arpSamples = CreateArpeggioSamples(keyNumbers, length, speed);
            var arpeggio = ConnectSamples(arpSamples.ToList());
            StopAndPlay(arpeggio);
        }

        public void StopAndPlay(ISampleProvider provider)
        {
            WaveformPlayer.Instance.StopPlayback();
            WaveformPlayer.Instance.PlayWaveform(provider);
        }
    }
}