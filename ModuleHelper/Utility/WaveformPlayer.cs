using System;
using System.Collections.Generic;
using System.Text;
using NAudio.Wave.SampleProviders;
using NAudio.Wave;

namespace ModuleHelper.Utility
{
    public class WaveformPlayer : IDisposable
    {
        private readonly IWavePlayer _outputDevice;
        private readonly MixingSampleProvider _mixer;

        public static WaveformPlayer Instance { get; } = new WaveformPlayer();

        static WaveformPlayer() {}
        private WaveformPlayer()
        {
            _outputDevice = new WaveOutEvent();

            _mixer = new MixingSampleProvider(WaveFormat.CreateIeeeFloatWaveFormat(44100, 2))
            {
                ReadFully = true
            };

            _outputDevice.Init(_mixer);
            _outputDevice.Play();
        }

        public void PlayWaveform(ISampleProvider input)
        {
            _mixer.AddMixerInput(input);
        }

        public void Dispose()
        {
            _outputDevice.Dispose();
        }
    }
}