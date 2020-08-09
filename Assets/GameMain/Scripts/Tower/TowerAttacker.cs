using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityGameFramework.Runtime;
using Flower.Data;

namespace Flower
{
    public class TowerAttacker : MonoBehaviour
    {

        //public LayerMask enemyMask { get; protected set; }
        private Transform[] projectilePoints;
        private Transform epicenter;

        private EntityDataTower entityDataTower;

        private bool IsMultiAttack
        {
            get
            {
                if (entityDataTower == null)
                    return false;
                else
                    return entityDataTower.Tower.IsMultiAttack;
            }
        }

        private float FireRate
        {
            get
            {
                if (entityDataTower == null)
                    return 0;
                else
                    return entityDataTower.Tower.FireRate;
            }
        }

        private RandomSound randomSound;

        private EntityTowerAttacker entityTowerAttacker;
        private TowerTargetter towerTargetter;
        private ILauncher m_Launcher;
        private float m_FireTimer;
        private EntityEnemy m_TrackingEnemy;

        public float searchRate
        {
            get { return towerTargetter.searchRate; }
            set { towerTargetter.searchRate = value; }
        }

        public EntityEnemy trackingEnemy
        {
            get { return m_TrackingEnemy; }
        }

        public TowerTargetter targetter
        {
            get { return towerTargetter; }
        }

        public void OnInit(object userData)
        {
            randomSound = GetComponent<RandomSound>();
        }

        public void OnShow(object userData)
        {
            entityDataTower = userData as EntityDataTower;
            if (entityDataTower == null)
            {
                Log.Error("TowerAttacker show data invaild.");
                return;
            }

            //enemyMask = mask;

            SetUpTimers();
        }

        public void OnHide(bool isShutdown, object userData)
        {
            ResetAttack();
            RemoveTowerTargetter();
            EmptyProjectilePoints();
            EmptyEpicenter();

            entityDataTower = null;
        }

        public void ResetAttack()
        {
            m_TrackingEnemy = null;
            m_FireTimer = 0;
        }

        void OnLostTarget()
        {
            m_TrackingEnemy = null;
        }

        void OnAcquiredTarget(EntityEnemy acquiredTarget)
        {
            m_TrackingEnemy = acquiredTarget;
        }

        protected virtual void SetUpTimers()
        {
            m_FireTimer = 1 / FireRate;
        }

        public void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            m_FireTimer -= elapseSeconds;
            if (trackingEnemy != null && m_FireTimer <= 0.0f)
            {
                OnFireTimer();
                m_FireTimer = 1 / FireRate;
            }
        }

        protected virtual void OnFireTimer()
        {
            FireProjectile();
        }

        protected virtual void FireProjectile()
        {
            if (m_TrackingEnemy == null)
            {
                return;
            }

            if (IsMultiAttack)
            {
                List<EntityEnemy> enemies = towerTargetter.GetAllTargets();
                m_Launcher.Launch(
                    enemies,
                    entityDataTower.Tower,
                    epicenter.position,
                    projectilePoints);
            }
            else
            {
                m_Launcher.Launch(
                    m_TrackingEnemy,
                    entityDataTower.Tower,
                    epicenter.position,
                    projectilePoints);
            }
            if (randomSound != null)
            {
                GameEntry.Sound.PlaySound(randomSound.GetRandomSound(), entityTowerAttacker.Entity);
            }
        }


        protected virtual int ByDistance(EntityEnemy first, EntityEnemy second)
        {
            float firstSqrMagnitude = Vector3.SqrMagnitude(first.transform.position - epicenter.position);
            float secondSqrMagnitude = Vector3.SqrMagnitude(second.transform.position - epicenter.position);
            return firstSqrMagnitude.CompareTo(secondSqrMagnitude);
        }

        public void SetProjectilePoints(Transform[] tfs)
        {
            projectilePoints = tfs;
        }

        public void EmptyProjectilePoints()
        {
            projectilePoints = null;
        }

        public void SetEpicenter(Transform tf)
        {
            epicenter = tf;
        }

        public void EmptyEpicenter()
        {
            epicenter = null;
        }

        public void SetTowerTargetter(TowerTargetter towerTargetter)
        {
            this.towerTargetter = towerTargetter;
            this.towerTargetter.acquiredTarget += OnAcquiredTarget;
            this.towerTargetter.lostTarget += OnLostTarget;
        }

        public void RemoveTowerTargetter()
        {
            towerTargetter.acquiredTarget -= OnAcquiredTarget;
            towerTargetter.lostTarget -= OnLostTarget;

            this.towerTargetter = null;
        }

        public void SetLaunch(ILauncher launcher)
        {
            this.m_Launcher = launcher;
        }

        public void EmptyLaunch()
        {
            this.m_Launcher = null;
        }

        public void SetEntityTowerAttacker(EntityTowerAttacker entityTowerAttacker)
        {
            this.entityTowerAttacker = entityTowerAttacker;
        }

        public void EmptyEntityTowerAttacker()
        {
            this.entityTowerAttacker = null;
        }

    }
}