using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Flower
{
    public class EntityHPBar : EntityLogicEx
    {
        public Transform healthBar;
        public Transform backgroundBar;
        private Transform cameraToFace;

        private EntityDataFollower entityDataFollower;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            cameraToFace = Camera.main.transform;

            entityDataFollower = userData as EntityDataFollower;
            if (entityDataFollower == null)
            {
                Log.Error("EntityHPBar param invaild");
                return;
            }
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            if (cameraToFace != null)
            {
                Vector3 direction = cameraToFace.transform.forward;
                transform.forward = -direction;
            }

            if (entityDataFollower != null && entityDataFollower.Follow != null)
            {
                transform.position = entityDataFollower.Follow.position + entityDataFollower.Offset;
            }
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);

            cameraToFace = null;
            entityDataFollower = null;
            UpdateHealth(1);
            SetVisible(false);
        }

        public void UpdateHealth(float normalizedHealth)
        {
            Vector3 scale = Vector3.one;

            if (healthBar != null)
            {
                scale.x = normalizedHealth;
                healthBar.transform.localScale = scale;
            }

            if (backgroundBar != null)
            {
                scale.x = 1 - normalizedHealth;
                backgroundBar.transform.localScale = scale;
            }

            SetVisible(normalizedHealth < 1.0f);
        }

        public void SetVisible(bool visible)
        {
            gameObject.SetActive(visible);
        }
    }
}

