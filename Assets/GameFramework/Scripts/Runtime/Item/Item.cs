using GameFramework;
using GameFramework.Item;
using System;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 物品。
    /// </summary>
    public sealed class Item : MonoBehaviour, IItem
    {
        private int m_Id;
        private string m_ItemAssetName;
        private IItemGroup m_ItemGroup;
        private ItemLogic m_ItemLogic;

        private Transform initRoot;
        private Vector3 initPosition;
        private Vector3 initRotation;
        private Vector3 initScale;

        /// <summary>
        /// 获取物品编号。
        /// </summary>
        public int Id
        {
            get
            {
                return m_Id;
            }
        }

        /// <summary>
        /// 获取物品资源名称。
        /// </summary>
        public string ItemAssetName
        {
            get
            {
                return m_ItemAssetName;
            }
        }

        /// <summary>
        /// 获取物品实例。
        /// </summary>
        public object Handle
        {
            get
            {
                return gameObject;
            }
        }

        /// <summary>
        /// 获取物品所属的物品组。
        /// </summary>
        public IItemGroup ItemGroup
        {
            get
            {
                return m_ItemGroup;
            }
        }

        /// <summary>
        /// 获取物品逻辑。
        /// </summary>
        public ItemLogic Logic
        {
            get
            {
                return m_ItemLogic;
            }
        }

        /// <summary>
        /// 物品初始化。
        /// </summary>
        /// <param name="itemId">物品编号。</param>
        /// <param name="itemAssetName">物品资源名称。</param>
        /// <param name="itemGroup">物品所属的物品组。</param>
        /// <param name="isNewInstance">是否是新实例。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void OnInit(int itemId, string itemAssetName, IItemGroup itemGroup, bool isNewInstance, object userData)
        {
            m_Id = itemId;
            m_ItemAssetName = itemAssetName;
            if (isNewInstance)
            {
                m_ItemGroup = itemGroup;
            }
            else if (m_ItemGroup != itemGroup)
            {
                Log.Error("Item group is inconsistent for non-new-instance item.");
                return;
            }

            initRoot = transform.parent;
            initPosition = transform.localPosition;
            initRotation = transform.eulerAngles;
            initScale = transform.localScale;

            ShowItemInfo showItemInfo = (ShowItemInfo)userData;
            Type itemLogicType = showItemInfo.ItemLogicType;

            if (m_ItemLogic != null)
            {
                if (m_ItemLogic.GetType() == itemLogicType)
                {
                    m_ItemLogic.enabled = true;
                    return;
                }

                Destroy(m_ItemLogic);
                m_ItemLogic = null;
            }

            if (itemLogicType == null)
                return;

            m_ItemLogic = gameObject.GetComponent(itemLogicType) as ItemLogic;

            if (m_ItemLogic == null)
                m_ItemLogic = gameObject.AddComponent(itemLogicType) as ItemLogic;

            if (m_ItemLogic == null)
            {
                Log.Error("Item '{0}' can not add item logic.", itemAssetName);
                return;
            }

            try
            {
                m_ItemLogic.OnInit(showItemInfo.UserData);
            }
            catch (Exception exception)
            {
                Log.Error("Item '[{0}]{1}' OnInit with exception '{2}'.", m_Id.ToString(), m_ItemAssetName, exception.ToString());
            }
        }

        /// <summary>
        /// 物品回收。
        /// </summary>
        public void OnRecycle()
        {
            transform.SetParent(initRoot, false);
            transform.localPosition = transform.localPosition;
            transform.eulerAngles = initRotation;
            transform.localScale = initScale;

            if (m_ItemLogic == null)
                return;

            try
            {
                m_ItemLogic.OnRecycle();
                m_ItemLogic.enabled = false;
            }
            catch (Exception exception)
            {
                Log.Error("Item '[{0}]{1}' OnRecycle with exception '{2}'.", m_Id.ToString(), m_ItemAssetName, exception.ToString());
            }

            m_Id = 0;
        }

        /// <summary>
        /// 物品显示。
        /// </summary>
        /// <param name="userData">用户自定义数据。</param>
        public void OnShow(object userData)
        {
            ShowItemInfo showItemInfo = (ShowItemInfo)userData;

            if (m_ItemLogic == null)
                return;

            try
            {
                m_ItemLogic.OnShow(showItemInfo.UserData);
            }
            catch (Exception exception)
            {
                Log.Error("Item '[{0}]{1}' OnShow with exception '{2}'.", m_Id.ToString(), m_ItemAssetName, exception.ToString());
            }
        }

        /// <summary>
        /// 物品隐藏。
        /// </summary>
        /// <param name="isShutdown">是否是关闭物品管理器时触发。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void OnHide(bool isShutdown, object userData)
        {
            if (m_ItemLogic == null)
                return;

            try
            {
                m_ItemLogic.OnHide(isShutdown, userData);
            }
            catch (Exception exception)
            {
                Log.Error("Item '[{0}]{1}' OnHide with exception '{2}'.", m_Id.ToString(), m_ItemAssetName, exception.ToString());
            }
        }

        /// <summary>
        /// 物品轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        public void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            if (m_ItemLogic == null)
                return;

            try
            {
                m_ItemLogic.OnUpdate(elapseSeconds, realElapseSeconds);
            }
            catch (Exception exception)
            {
                Log.Error("Item '[{0}]{1}' OnUpdate with exception '{2}'.", m_Id.ToString(), m_ItemAssetName, exception.ToString());
            }
        }
    }
}
