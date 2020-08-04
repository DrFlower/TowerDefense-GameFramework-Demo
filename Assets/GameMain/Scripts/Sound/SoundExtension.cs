//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using GameFramework.DataTable;
using GameFramework.Sound;
using UnityGameFramework.Runtime;
using Flower.Data;

namespace Flower
{
    public static class SoundExtension
    {
        private const float FadeVolumeDuration = 1f;
        private static int? s_MusicSerialId = null;

        public static int? PlayMusic(this SoundComponent soundComponent, EnumSound enumSound, object userData = null)
        {
            if (enumSound == EnumSound.None)
                return null;

            soundComponent.StopMusic();
            s_MusicSerialId = soundComponent.PlaySound((int)enumSound, null, userData);

            return s_MusicSerialId;
        }

        public static int? PlayMusic(this SoundComponent soundComponent, int musicId, object userData = null)
        {
            soundComponent.StopMusic();
            s_MusicSerialId = soundComponent.PlaySound(musicId, null, userData);

            return s_MusicSerialId;
        }

        public static void StopMusic(this SoundComponent soundComponent)
        {
            if (!s_MusicSerialId.HasValue)
            {
                return;
            }

            soundComponent.StopSound(s_MusicSerialId.Value, 0);
            s_MusicSerialId = null;
        }

        public static int? PlaySound(this SoundComponent soundComponent, EnumSound enumSound, Entity bindingEntity = null, object userData = null)
        {
            if (enumSound == EnumSound.None)
                return null;

            return soundComponent.PlaySound((int)enumSound, bindingEntity, userData);
        }

        public static int? PlaySound(this SoundComponent soundComponent, int soundId, Entity bindingEntity = null, object userData = null)
        {
            SoundData soundData = GameEntry.Data.GetData<DataSound>().GetSoundDataBySoundId(soundId);

            PlaySoundParams playSoundParams = PlaySoundParams.Create();
            playSoundParams.Time = soundData.SoundPlayParam.Time;
            playSoundParams.MuteInSoundGroup = soundData.SoundPlayParam.Mute;
            playSoundParams.Loop = soundData.SoundPlayParam.Loop;
            playSoundParams.Priority = soundData.SoundPlayParam.Priority;
            playSoundParams.VolumeInSoundGroup = soundData.SoundPlayParam.Volume;
            playSoundParams.FadeInSeconds = soundData.SoundPlayParam.FadeInSeconds;
            playSoundParams.Pitch = soundData.SoundPlayParam.Pitch;
            playSoundParams.PanStereo = soundData.SoundPlayParam.PanStereo;
            playSoundParams.SpatialBlend = soundData.SoundPlayParam.SpatialBlend;
            playSoundParams.MaxDistance = soundData.SoundPlayParam.MaxDistance;
            playSoundParams.DopplerLevel = soundData.SoundPlayParam.DopplerLevel;

            return soundComponent.PlaySound(soundData.AssetPath, soundData.SoundGroupData.Name, Constant.AssetPriority.MusicAsset, playSoundParams, bindingEntity, userData);
        }

        public static bool IsMuted(this SoundComponent soundComponent, string soundGroupName)
        {
            if (string.IsNullOrEmpty(soundGroupName))
            {
                Log.Warning("Sound group is invalid.");
                return true;
            }

            ISoundGroup soundGroup = soundComponent.GetSoundGroup(soundGroupName);
            if (soundGroup == null)
            {
                Log.Warning("Sound group '{0}' is invalid.", soundGroupName);
                return true;
            }

            return soundGroup.Mute;
        }

        public static void Mute(this SoundComponent soundComponent, string soundGroupName, bool mute)
        {
            if (string.IsNullOrEmpty(soundGroupName))
            {
                Log.Warning("Sound group is invalid.");
                return;
            }

            ISoundGroup soundGroup = soundComponent.GetSoundGroup(soundGroupName);
            if (soundGroup == null)
            {
                Log.Warning("Sound group '{0}' is invalid.", soundGroupName);
                return;
            }

            soundGroup.Mute = mute;

            GameEntry.Setting.SetBool(Utility.Text.Format(Constant.Setting.SoundGroupMuted, soundGroupName), mute);
            GameEntry.Setting.Save();
        }

        public static float GetVolume(this SoundComponent soundComponent, string soundGroupName)
        {
            if (string.IsNullOrEmpty(soundGroupName))
            {
                Log.Warning("Sound group is invalid.");
                return 0f;
            }

            ISoundGroup soundGroup = soundComponent.GetSoundGroup(soundGroupName);
            if (soundGroup == null)
            {
                Log.Warning("Sound group '{0}' is invalid.", soundGroupName);
                return 0f;
            }

            return soundGroup.Volume;
        }

        public static void SetVolume(this SoundComponent soundComponent, string soundGroupName, float volume)
        {
            if (string.IsNullOrEmpty(soundGroupName))
            {
                Log.Warning("Sound group is invalid.");
                return;
            }

            ISoundGroup soundGroup = soundComponent.GetSoundGroup(soundGroupName);
            if (soundGroup == null)
            {
                Log.Warning("Sound group '{0}' is invalid.", soundGroupName);
                return;
            }

            soundGroup.Volume = volume;

            GameEntry.Setting.SetFloat(Utility.Text.Format(Constant.Setting.SoundGroupVolume, soundGroupName), volume);
            GameEntry.Setting.Save();
        }
    }
}
