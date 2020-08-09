using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Flower.Data;
using System;
using UnityGameFramework.Runtime;

namespace Flower
{
    public abstract class EntityTargetable : EntityLogicEx
    {
        protected Transform hpBarRoot;

        private Vector3 m_CurrentPosition, m_PreviousPosition;

        protected float hp;

        public float HP
        {
            get
            {
                return hp;
            }

            protected set
            {
                hp = value;
            }
        }

        protected abstract float MaxHP { get; }

        public bool IsDead
        {
            get
            {
                return HP <= 0;
            }
        }

        public Vector3 Velocity
        {
            get;
            private set;
        }

        private bool loadedHPBar = false;
        private EntityHPBar entityHPBar;

        public event Action<EntityTargetable> OnDead;
        public event Action<EntityTargetable> OnHidden;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            hpBarRoot = transform.Find("HealthBar");
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);

            if (OnHidden != null)
                OnHidden(this);

            OnHidden = null;
            OnDead = null;

            HideHpBar();
        }

        protected virtual void FixedUpdate()
        {
            m_CurrentPosition = transform.position;
            Velocity = (m_CurrentPosition - m_PreviousPosition) / Time.fixedDeltaTime;
            m_PreviousPosition = m_CurrentPosition;
        }

        public virtual void Damage(float value)
        {
            if (IsDead)
                return;

            if (!loadedHPBar)
            {
                GameEntry.Event.Fire(this, ShowEntityInLevelEventArgs.Create(
                    (int)EnumEntity.HPBar,
                    typeof(EntityHPBar),
                    OnLoadHpBarSuccess,
                    EntityDataFollower.Create(hpBarRoot)));

                loadedHPBar = true;
            }

            if (entityHPBar)
            {
                entityHPBar.UpdateHealth(hp / MaxHP);
            }

            hp -= value;

            if (hp <= 0)
            {
                hp = 0;
                Dead();
            }
        }

        protected virtual void Dead()
        {
            if (OnDead != null)
                OnDead(this);
        }

        private void OnLoadHpBarSuccess(Entity entity)
        {
            entityHPBar = entity.Logic as EntityHPBar;
            if (IsDead || !Available)
            {
                HideHpBar();
            }
            else
            {
                entityHPBar.UpdateHealth(hp / MaxHP);
            }
        }

        private void HideHpBar()
        {
            if (entityHPBar)
            {
                GameEntry.Event.Fire(this, HideEntityInLevelEventArgs.Create(entityHPBar.Id));
                loadedHPBar = false;
                entityHPBar = null;
            }
        }

    }
}

