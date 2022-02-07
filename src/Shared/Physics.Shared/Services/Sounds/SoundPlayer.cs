using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AppCenter.Analytics;
using Nito.Disposables;
using Windows.Media.Audio;
using Windows.Media.Render;
using Windows.Storage;

namespace Physics.Shared.Services.Sounds
{
	public class SoundPlayer : ISoundPlayer
	{
		private bool _initialized = false;
		private object _syncLock = new object();


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
				lock (_fileInputs)
				{
					if (!_fileInputs.ContainsKey(name))
					{
						_fileInputs.Add(name, fileInputNode);
					}
				}
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

		public IDisposable PlayIndefinitely(string name, double volume = 1.0)
		{
			if (_fileInputs.TryGetValue(name, out var node))
			{
				PlayAudioNodeIndefinitely(node, volume);
				return Disposable.Create(() => node.Stop());
			}
			return NoopDisposable.Instance;
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
			catch (Exception e)
			{
				Analytics.TrackEvent("Sound could not be retrieved", new Dictionary<string, string>() { { "Message", e.Message } });
			}

			return null;
		}

		/// <summary>
		/// Plays audio node
		/// </summary>
		private void PlayAudioNode(AudioFileInputNode audio, double volume)
		{
			try
			{
				if (audio != null)
				{
					audio.OutgoingGain = volume;
					audio.Seek(TimeSpan.Zero);
					audio.Start();
				}
			}
			catch (Exception e)
			{
				Analytics.TrackEvent("Audio failed playback", new Dictionary<string, string>() { { "Message", e.Message } });
			}
		}


		/// <summary>
		/// Plays audio node
		/// </summary>
		private void PlayAudioNodeIndefinitely(AudioFileInputNode audio, double volume)
		{
			try
			{
				audio.OutgoingGain = volume;
				audio.LoopCount = null;
				audio.Seek(TimeSpan.Zero);
				audio.Start();
			}
			catch (Exception e)
			{
				Analytics.TrackEvent("Audio failed playback", new Dictionary<string, string>() { { "Message", e.Message } });
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
