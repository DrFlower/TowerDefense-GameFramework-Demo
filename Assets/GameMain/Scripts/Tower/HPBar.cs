using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Flower
{
    public class HPBar : MonoBehaviour
    {

        public Transform healthBar;
        public Transform backgroundBar;
        private bool showWhenFull = false;
        private Transform cameraToFace;

        public void OnInit(object userData)
        {

        }

        public void OnShow(object userData)
        {
            cameraToFace = Camera.main.transform;
        }

        public void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            if (cameraToFace != null)
            {
                Vector3 direction = cameraToFace.transform.forward;
                transform.forward = -direction;
            }
        }

        public void OnHide(bool isShutdown, object userData)
        {
            cameraToFace = null;
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

            SetVisible(showWhenFull || normalizedHealth < 1.0f);
        }

        public void SetVisible(bool visible)
        {
            gameObject.SetActive(visible);
        }
    }
}


