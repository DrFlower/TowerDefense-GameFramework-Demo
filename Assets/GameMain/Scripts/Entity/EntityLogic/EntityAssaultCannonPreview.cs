using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Flower
{
    public class EntityAssaultCannonPreview : EntityLogicEx
    {
        //public GameObject radiusVisualizer;

        //public float radiusVisualizerHeight = 0.02f;

        //public float dampSpeed = 0.075f;

        //public Material material;

        //public Material invalidPositionMaterial;

        //protected MeshRenderer[] m_MeshRenderers;

        //protected Vector3 m_MoveVel;

        //protected Vector3 m_TargetPosition;

        //protected bool m_ValidPos;

        //public Collider ghostCollider { get; private set; }

        ///// <summary>
        ///// Initialize this ghost
        ///// </summary>
        ///// <param name="tower">The tower controller we're a ghost of</param>
        //public virtual void Initialize(Tower tower)
        //{
        //    m_MeshRenderers = GetComponentsInChildren<MeshRenderer>();
        //    controller = tower;
        //    if (GameUI.instanceExists)
        //    {
        //        GameUI.instance.SetupRadiusVisualizer(controller, transform);
        //    }
        //    ghostCollider = GetComponent<Collider>();
        //    m_MoveVel = Vector3.zero;
        //    m_ValidPos = false;
        //}

        //public virtual void Move(Vector3 worldPosition, Quaternion rotation, bool validLocation)
        //{
        //    m_TargetPosition = worldPosition;

        //    if (!m_ValidPos)
        //    {
        //        // Immediately move to the given position
        //        m_ValidPos = true;
        //        transform.position = m_TargetPosition;
        //    }

        //    transform.rotation = rotation;
        //    foreach (MeshRenderer meshRenderer in m_MeshRenderers)
        //    {
        //        meshRenderer.sharedMaterial = validLocation ? material : invalidPositionMaterial;
        //    }
        //}


        ///// <summary>
        ///// Damp the movement of the ghost
        ///// </summary>
        //protected virtual void Update()
        //{
        //    Vector3 currentPos = transform.position;

        //    if (Vector3.SqrMagnitude(currentPos - m_TargetPosition) > 0.01f)
        //    {
        //        currentPos = Vector3.SmoothDamp(currentPos, m_TargetPosition, ref m_MoveVel, dampSpeed);

        //        transform.position = currentPos;
        //    }
        //    else
        //    {
        //        m_MoveVel = Vector3.zero;
        //    }
        //}

        //protected override void OnShow(object userData)
        //{
        //    base.OnShow(userData);
        //}
    }
}

