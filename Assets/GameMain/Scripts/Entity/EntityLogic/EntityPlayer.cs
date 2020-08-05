using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Flower.Data;

namespace Flower
{
    public class EntityPlayer : EntityLogicEx
    {
        public GameObject idleEffect;
        public ParticleSystem chargeEffect;
        public ParticleSystem demagedEffect;

        private DataPlayer dataPlayer;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            idleEffect.SetActive(true);
            dataPlayer = GameEntry.Data.GetData<DataPlayer>();
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);


        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);

            idleEffect.SetActive(false);
            dataPlayer = null;
        }

        public void Charge()
        {
            if (chargeEffect != null)
                chargeEffect.Play();

            GameEntry.Sound.PlaySound(EnumSound.zone_enter, Entity);
        }

        public void Damage(int value)
        {
            if (demagedEffect != null)
                demagedEffect.Play();

            GameEntry.Sound.PlaySound(EnumSound.base_attack, Entity);

            dataPlayer.Damage(value);
        }
    }
}

