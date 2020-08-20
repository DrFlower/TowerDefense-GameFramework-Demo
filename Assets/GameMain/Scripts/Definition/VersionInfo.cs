//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace Flower
{
    public class VersionInfo
    {
        // 是否需要强制更新游戏应用
        public bool ForceUpdateGame
        {
            get;
            set;
        }

        // 最新的游戏版本号
        public string LatestGameVersion
        {
            get;
            set;
        }

        // 最新的游戏内部版本号
        public int InternalGameVersion
        {
            get;
            set;
        }

        // 最新的资源内部版本号
        public int InternalResourceVersion
        {
            get;
            set;
        }

        // 资源更新下载地址
        public string UpdatePrefixUri
        {
            get;
            set;
        }

        // 资源版本列表长度
        public int VersionListLength
        {
            get;
            set;
        }

        // 资源版本列表哈希值
        public int VersionListHashCode
        {
            get;
            set;
        }

        // 资源版本列表压缩后长度
        public int VersionListZipLength
        {
            get;
            set;
        }

        // 资源版本列表压缩后哈希值
        public int VersionListZipHashCode
        {
            get;
            set;
        }
    }
}
