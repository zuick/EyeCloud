using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;

namespace Game.Audio
{
	public abstract class AudioSetting
	{
		public AudioClip clip;
		public virtual string Name { get; }
		public virtual int IntId { get; }
		public int maxItems = 0;
	}

	public abstract class AbstractAudioManager<S, E> : MonoBehaviour
		where S : AudioSetting where E : struct, IConvertible, IComparable, IFormattable
	{
		protected class SourceInfo
		{
			public Guid uid;
			public AudioSource source;
			public int intId;
			public float timestamp;

			public SourceInfo(int intId, AudioSource source)
			{
				Init(intId);
				this.source = source;
			}

			public void Init(int intId)
			{
				this.intId = intId;
				uid = Guid.NewGuid();
				timestamp = Time.realtimeSinceStartup;
			}
		}
		public int maxSoundsSimultaneously = 2;
		[Tooltip("Delay between two identical sounds")]
		public float simultaneouslyInterval = 0.3f;
		public float simultaneouslyMaxDistance = 5f;
		public AudioMixerGroup output;
		public AudioMixerSettings audioMixerSettings;
		public float spread = 0;
		public AudioRolloffMode mode = AudioRolloffMode.Logarithmic;
		public float minDistance = 1f;
		public float maxDistance = 500f;
		public float spatialBlend = 0f;

		public virtual string ParamName => "";
		public virtual string VolumePlayerPref => "";
		public float Volume => GetCurrentVolume();

		public List<S> Settings = new List<S>();

		protected List<SourceInfo> sounds = new List<SourceInfo>();

		protected virtual void Start()
		{
			output.audioMixer.SetFloat(ParamName, GetSavedVolume());
		}

		protected float GetSavedVolume()
		{
			var saved = PlayerPrefs.GetFloat(VolumePlayerPref, float.NegativeInfinity);
			return float.IsNegativeInfinity(saved) ? GetCurrentVolume() : saved;
		}

		protected float GetCurrentVolume()
		{
			output.audioMixer.GetFloat(ParamName, out var volume);
			return volume;
		}

		public virtual void SetVolume(float v)
		{
			output.audioMixer.SetFloat(ParamName, v);
		}

		public void SaveVolume()
		{
			PlayerPrefs.SetFloat(VolumePlayerPref, GetCurrentVolume());
		}

		public virtual Guid Play(E id)
		{
			return Play(id, Vector3.zero, noSpatialBlend: true);
		}

		public virtual Guid Play(E id, Vector3 position, bool noSpatialBlend = false)
		{
			var setting = Settings.FirstOrDefault(s => s.Name == id.ToString());
			if (setting == null)
				return Guid.Empty;

			var maxItems = setting.maxItems <= 0 ? maxSoundsSimultaneously : setting.maxItems;

			if (sounds.Count(s =>
				 s.intId == setting.IntId &&
				 s.source.gameObject.activeSelf &&
				 (s.source.transform.position - position).magnitude < simultaneouslyMaxDistance &&
				 (Time.realtimeSinceStartup - s.timestamp) < simultaneouslyInterval
			) > maxItems - 1)
				return Guid.Empty;

			AudioSource audioSource = null;
			var info = sounds.FirstOrDefault(i => !i.source.gameObject.activeSelf);
			if (info == null)
			{
				var instance = new GameObject();
				audioSource = instance.AddComponent<AudioSource>();
				audioSource.playOnAwake = false;
				info = new SourceInfo(setting.IntId, audioSource);
				sounds.Add(info);
			}
			else
			{
				audioSource = info.source;
				info.Init(setting.IntId);
			}

			SetAudioSourceSettings(audioSource, setting, position, noSpatialBlend);

			if (!audioSource.loop)
				StartCoroutine(DeactivateCoroutine(info, audioSource.clip.length));

			audioSource.gameObject.SetActive(true);
			audioSource.gameObject.name = setting.clip.name;
			audioSource.Play();

			return info.uid;
		}

		public virtual void Stop(Guid uid)
		{
			DeactivateSound(sounds.FirstOrDefault(a => a.uid == uid));
		}

		public virtual void Stop(E id)
		{
			var setting = Settings.FirstOrDefault(s => s.Name == id.ToString());
			if (setting == null)
				return;

			DeactivateSound(sounds.FirstOrDefault(a => a.intId == setting.IntId));
		}

		public virtual void StopAll()
		{
			foreach (var sound in sounds)
			{
				sound.source.Pause();
				DeactivateSound(sound);
			}
		}

		public AudioSetting GetSetting(E id)
		{
			return Settings.FirstOrDefault(s => s.Name == id.ToString());
		}

		protected virtual void DeactivateSound(SourceInfo sourceInfo)
		{
			if (sourceInfo == null)
				return;

			sourceInfo.source.Stop();
			sourceInfo.source.gameObject.SetActive(false);
			sourceInfo.source.gameObject.name = "Inactive";
		}

		protected virtual IEnumerator DeactivateCoroutine(SourceInfo sourceInfo, float delay)
		{
			yield return new WaitForSeconds(delay);

			sourceInfo.source.volume = 0f;

			yield return new WaitForEndOfFrame();

			DeactivateSound(sourceInfo);
		}

		protected virtual void SetAudioSourceSettings(AudioSource audioSource, S setting, Vector3 position, bool noSpatialBlend = false)
		{
			SetAudioSourceSettings(audioSource, setting.clip, position);
		}

		protected virtual void SetAudioSourceSettings(AudioSource audioSource, AudioClip clip, Vector3 position, bool loop = false, bool backward = false, bool noSpatialBlend = false)
		{
			audioSource.transform.position = position;
			audioSource.transform.SetParent(transform);
			audioSource.outputAudioMixerGroup = output;
			audioSource.clip = clip;
			audioSource.loop = loop;
			audioSource.spread = spread;
			audioSource.rolloffMode = mode;
			audioSource.minDistance = minDistance;
			audioSource.maxDistance = maxDistance;
			audioSource.spatialBlend = noSpatialBlend ? 0f : spatialBlend;
			audioSource.volume = 1f;
			if (backward)
			{
				audioSource.timeSamples = audioSource.clip.samples - 1;
				audioSource.pitch = -1f;
			}
		}

		protected virtual S GetAudioSetting(int id)
		{
			return null;
		}

		[ContextMenu("Update Settings")]
		protected void UpdateSettings()
		{
			foreach (var sound in Enum.GetValues(typeof(E)))
			{
				if ((int)sound == (int)SoundId.None)
					continue;

				if (!Settings.Any(s => s.IntId == (int)sound))
					Settings.Add(GetAudioSetting((int)sound));
			}
		}

		protected void OnValidate()
		{
			foreach (var setting in Settings)
			{
				if (Settings.Count(s => s.IntId == setting.IntId) > 1)
				{
					Debug.LogWarning($"Sound {setting.Name} duplicated!");
					break;
				}
			}
		}
	}
}