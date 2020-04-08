using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Media.Audio;
using Windows.Media.Render;
using Windows.Storage;

namespace Physics.Shared.Services.Sounds
{
    public class SoundPlayer : ISoundPlayer
    {
        private bool _initialized = false;

        private AudioGraph _graph;
        private readonly Dictionary<string, AudioFileInputNode> _fileInputs = new Dictionary<string, AudioFileInputNode>();
        private AudioDeviceOutputNode _deviceOutput;

        public async Task PreloadSoundAsync(Uri sound, string name)
        {
            if (!_initialized)
            {
                await CreateAudioGraphAsync();
                _initialized = true;
            }
            if (!_fileInputs.ContainsKey(name))
            {
                var file = await StorageFile.GetFileFromApplicationUriAsync(sound);
                var fileInputNode = await GetSoundFromFileAsync(file);
                _fileInputs.Add(name, fileInputNode);
            }
        }

        /// <summary>
        /// Plays a sound at a given volume.
        /// </summary>
        /// <param name="name">Sound name.</param>
        /// <param name="volume">Volume.</param>
        public void PlaySound(string name, double volume = 1.0)
        {
            if (_fileInputs.TryGetValue(name, out var node))
            {
                PlayAudioNode(node, volume);
            }
        }

        /// <summary>
        /// Retrieves audio file input node
        /// </summary>
        /// <param name="file">FIle to retrieve from</param>
        /// <returns>Input node</returns>
        private async Task<AudioFileInputNode> GetSoundFromFileAsync(StorageFile file)
        {
            try
            {
                var fileInputResult = await _graph.CreateFileInputNodeAsync(file);
                fileInputResult.FileInputNode.Stop();
                fileInputResult.FileInputNode.AddOutgoingConnection(_deviceOutput);
                return fileInputResult.FileInputNode;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Plays audio node
        /// </summary>
        private void PlayAudioNode(AudioFileInputNode audio, double volume)
        {
            try
            {
                audio.OutgoingGain = volume;
                audio.Seek(TimeSpan.Zero);
                audio.Start();
                Debug.WriteLine("Audio played");
            }
            catch (Exception)
            {
                //ignored
            }
        }

        /// <summary>
        /// Creates audio graph component.
        /// </summary>
        /// <returns>Task.</returns>
        private async Task CreateAudioGraphAsync()
        {
            var settings = new AudioGraphSettings(AudioRenderCategory.Media);
            var result = await AudioGraph.CreateAsync(settings);
            _graph = result.Graph;
            var deviceOutputNodeResult = await _graph.CreateDeviceOutputNodeAsync();
            _deviceOutput = deviceOutputNodeResult.DeviceOutputNode;
            _graph.ResetAllNodes();
            _graph.Start();
        }
    }
}
