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

namespace Flower
{
    public static class SoundExtension
    {
        private const float FadeVolumeDuration = 1f;
        private static int? s_MusicSerialId = null;

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

            soundComponent.StopSound(s_MusicSerialId.Value, FadeVolumeDuration);
            s_MusicSerialId = null;
        }

        public static int? PlaySound(this SoundComponent soundComponent, int soundId, Entity bindingEntity = null, object userData = null)
        {

            IDataTable<DRSound> dtSound = GameEntry.DataTable.GetDataTable<DRSound>();
            DRSound drSound = dtSound.GetDataRow(soundId);
            if (drSound == null)
            {
                Log.Warning("Can not load music '{0}' from data table.", soundId.ToString());
                return null;
            }

            IDataTable<DRAssetsPath> dtAssetPath = GameEntry.DataTable.GetDataTable<DRAssetsPath>();
            DRAssetsPath drAssetPath = dtAssetPath.GetDataRow(drSound.AssetId);
            string assetPath = drAssetPath.AssetPath;

            IDataTable<DRSoundGroup> dtSoundGroup = GameEntry.DataTable.GetDataTable<DRSoundGroup>();
            DRSoundGroup dRSoundGroup = dtSoundGroup.GetDataRow(drSound.SoundGroupId);

            IDataTable<DRSoundPlayParam> dtSoundPlayParam = GameEntry.DataTable.GetDataTable<DRSoundPlayParam>();
            DRSoundPlayParam dRSoundPlayParam = dtSoundPlayParam.GetDataRow(drSound.SoundPlayParamId);

            PlaySoundParams playSoundParams = PlaySoundParams.Create();
            playSoundParams.Time = dRSoundPlayParam.Time;
            playSoundParams.MuteInSoundGroup = dRSoundPlayParam.Mute;
            playSoundParams.Loop = dRSoundPlayParam.Loop;
            playSoundParams.Priority = dRSoundPlayParam.Priority;
            playSoundParams.VolumeInSoundGroup = dRSoundPlayParam.Volume;
            playSoundParams.FadeInSeconds = dRSoundPlayParam.FadeInSeconds;
            playSoundParams.Pitch = dRSoundPlayParam.Pitch;
            playSoundParams.PanStereo = dRSoundPlayParam.PanStereo;
            playSoundParams.SpatialBlend = dRSoundPlayParam.SpatialBlend;
            playSoundParams.MaxDistance = dRSoundPlayParam.MaxDistance;
            playSoundParams.DopplerLevel = dRSoundPlayParam.DopplerLevel;

            return soundComponent.PlaySound(assetPath, dRSoundGroup.Name, Constant.AssetPriority.MusicAsset, playSoundParams, bindingEntity, userData);
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
