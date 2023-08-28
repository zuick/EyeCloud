using UnityEngine;

namespace Game.Audio
{
	[CreateAssetMenu(fileName = "AudioMixerSettings", menuName = "Infos/AudioMixerSettings", order = 0)]
	public class AudioMixerSettings : ScriptableObject
	{
		public float minVolume = -80f;
		public float volumeSettingsScale = 0.5f;
		public float lowPassCutoffHiding = 1200f;
		public float lowPassCutoffScent = 1600f;
		public float lowPassCutoffMax = 22000f;
		public float pitchingMinValue = 0.75f;

		public string musicVolumeParamName = "MusicVolume";
		public string musicWrapperVolumeParamName = "MusicWrapperVolume";
		public string soundVolumeParamName = "SoundVolume";
		public string lowPassCutoffParamName = "LowPassCutoff";
		public string masterPitchParamName = "MasterPitch";
	}
}