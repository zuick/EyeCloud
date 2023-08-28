using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Game.Audio
{
	[Serializable]
	public class SoundSetting : AudioSetting
	{
		public SoundId id;
		[Tooltip("Loop?")]
		public bool loop;
		[Tooltip("Backward?")]
		public bool backward;

		public override string Name => id.ToString();
		public override int IntId => (int)id;
	}

	public class SoundManager : AbstractAudioManager<SoundSetting, SoundId>
	{
		public override string ParamName => audioMixerSettings.soundVolumeParamName;
		public override string VolumePlayerPref => SystemPlayerPrefsKeys.SoundsVolume.ToString();

		private const float gentilizationMagic = 0.1f;
		private Dictionary<Guid, GameObject> trackingSounds = new Dictionary<Guid, GameObject>();

		protected override SoundSetting GetAudioSetting(int id)
		{
			return new SoundSetting { id = (SoundId)id };
		}

		protected override void SetAudioSourceSettings(AudioSource audioSource, SoundSetting setting, Vector3 position, bool noSpatialBlend = false)
		{
			SetAudioSourceSettings(audioSource, setting.clip, position, setting.loop, setting.backward, noSpatialBlend);
		}

		public virtual Guid Play(SoundId id, Transform holder, bool noSpatialBlend = false)
		{
			var uid = Play(id, holder.position, noSpatialBlend);
			var info = sounds.FirstOrDefault(a => a.uid == uid);
			if (info != null)
			{
				var tracker = info.source.gameObject.AddComponent<TransformTracker>();
				tracker.Target = holder;
				trackingSounds.Add(uid, holder.gameObject);
			}
			return uid;
		}

		public SoundSetting GetAudioSetting(SoundId id)
		{
			return Settings.FirstOrDefault(s => s.id == id);
		}


		protected override void DeactivateSound(SourceInfo sourceInfo)
		{
			if (sourceInfo == null || sourceInfo.source == null)
				return;

			var transformTracker = sourceInfo.source.gameObject.GetComponent<TransformTracker>();
			if (transformTracker != null)
			{
				Destroy(transformTracker);
			}
			base.DeactivateSound(sourceInfo);
		}

		public void StopGently(Guid uid)
		{
			var info = sounds.FirstOrDefault(a => a.uid == uid);
			if (info != null && info.source != null)
			{
				/*
				if (info.source.loop)
				{
					var clipLength = info.source.clip.length;
					var repeatCount = Mathf.Floor(info.source.time / clipLength) + 1;
					// stop a little bit before loop ends
					var delay = clipLength * repeatCount - info.source.time - clipLength * gentilizationMagic;
					StartCoroutine(DeactivateCoroutine(info, delay));
				}
				else
				{
					DeactivateSound(sounds.FirstOrDefault(a => a.uid == uid));
				}
				*/
				DeactivateSound(sounds.FirstOrDefault(a => a.uid == uid));
			}
		}

		private void Update()
		{
			var removeSounds = new List<Guid>();
			foreach (var pair in trackingSounds)
			{
				if (pair.Value == null)
				{
					removeSounds.Add(pair.Key);
					Stop(pair.Key);
				}
			}

			foreach (var uid in removeSounds)
			{
				trackingSounds.Remove(uid);
			}
		}
	}
}