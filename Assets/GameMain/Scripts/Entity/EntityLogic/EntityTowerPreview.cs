using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Flower
{
    public class EntityTowerPreview : EntityLogicEx
    {
        [SerializeField]
        private float sphereCastRadius = 1;
        [SerializeField]
        private float dampSpeed = 5;
        [SerializeField]
        private LayerMask ghostWorldPlacementMask;
        [SerializeField]
        private LayerMask placementAreaMask;
        [SerializeField]
        private Material material;
        [SerializeField]
        private Material invalidPositionMaterial;

        private IPlacementArea currentArea;
        private IntVector2 m_GridPosition;

        private EntityDataTowerPreview entityDataTowerPreview;

        private Vector3 targetPos;
        private Vector3 moveVel;

        private bool validPos = false;
        private bool visible = true;
        private bool canPlace = false;

        private MeshRenderer[] renderers;

        public bool CanPlace
        {
            get
            {
                return canPlace;
            }
        }


        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            renderers = transform.GetComponentsInChildren<MeshRenderer>(true);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            entityDataTowerPreview = userData as EntityDataTowerPreview;
            if (entityDataTowerPreview == null)
            {
                Log.Error("EntityDataTowerPreview param is invaild");
                return;
            }

            canPlace = false;
            validPos = false;
            moveVel = Vector3.zero;
            SetVisiable(true);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            if (entityDataTowerPreview == null)
                return;

            MoveGhost(false);

            Vector3 currentPos = transform.position;

            if (Vector3.SqrMagnitude(currentPos - targetPos) > 0.01f)
            {
                currentPos = Vector3.SmoothDamp(currentPos, targetPos, ref moveVel, dampSpeed);
                transform.position = currentPos;
            }
            else
            {
                moveVel = Vector3.zero;
            }
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);

            currentArea = null;
            m_GridPosition = IntVector2.zero;
            entityDataTowerPreview = null;
        }

        private void SetVisiable(bool value)
        {
            if (visible == value)
                return;

            if (visible == false)
            {
                moveVel = Vector3.zero;
                validPos = false;
            }

            foreach (var item in renderers)
            {
                item.enabled = value;
            }

            visible = value;
        }

        private void MoveGhost(bool hideWhenInvalid = true)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, float.MaxValue, placementAreaMask))
            {
                MoveGhostWithRaycastHit(hit);
            }
            else
            {
                MoveGhostOntoWorld(ray, hideWhenInvalid);
            }

        }

        private void MoveGhostWithRaycastHit(RaycastHit raycast)
        {
            currentArea = raycast.collider.GetComponent<IPlacementArea>();

            if (currentArea == null)
            {
                Log.Error("There is not an IPlacementArea attached to the collider found on the m_PlacementAreaMask");
                return;
            }
            m_GridPosition = currentArea.WorldToGrid(raycast.point, entityDataTowerPreview.TowerData.Dimensions);
            TowerFitStatus fits = currentArea.Fits(m_GridPosition, entityDataTowerPreview.TowerData.Dimensions);

            SetVisiable(true);
            canPlace = fits == TowerFitStatus.Fits;
            Move(currentArea.GridToWorld(m_GridPosition, entityDataTowerPreview.TowerData.Dimensions),
                                currentArea.transform.rotation,
                                canPlace);
        }

        private void MoveGhostOntoWorld(Ray ray, bool hideWhenInvalid)
        {
            currentArea = null;

            if (!hideWhenInvalid)
            {
                RaycastHit hit;
                // check against all layers that the ghost can be on
                Physics.SphereCast(ray, sphereCastRadius, out hit, float.MaxValue, ghostWorldPlacementMask);
                if (hit.collider == null)
                {
                    return;
                }
                SetVisiable(true);
                Move(hit.point, hit.collider.transform.rotation, false);
            }
            else
            {
                SetVisiable(false);
            }
        }

        private void Move(Vector3 worldPosition, Quaternion rotation, bool validLocation)
        {
            targetPos = worldPosition;

            if (!validPos)
            {
                // Immediately move to the given position
                validPos = true;
                transform.position = targetPos;
            }

            transform.rotation = rotation;
            foreach (MeshRenderer meshRenderer in renderers)
            {
                meshRenderer.sharedMaterial = validLocation ? material : invalidPositionMaterial;
            }
        }

        public bool TryBuildTower()
        {
            if (currentArea == null)
            {
                Log.Error("Current area is null");
                return false;
            }

            Vector3 position = Vector3.zero;
            Quaternion rotation = currentArea.transform.rotation;

            TowerFitStatus fits = currentArea.Fits(m_GridPosition, entityDataTowerPreview.TowerData.Dimensions);

            if (fits == TowerFitStatus.Fits)
            {
                position = currentArea.GridToWorld(m_GridPosition, entityDataTowerPreview.TowerData.Dimensions);
                currentArea.Occupy(m_GridPosition, entityDataTowerPreview.TowerData.Dimensions);
                GameEntry.Event.Fire(this, BuildTowerEventArgs.Create(entityDataTowerPreview.TowerData, currentArea, m_GridPosition, position, rotation));
                return true;
            }

            return false;
        }
    }
}

