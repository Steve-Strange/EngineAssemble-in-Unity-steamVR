using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using Valve.VR;

public class Test : MonoBehaviour
{
    /// <summary>
    /// 该物体的Rigidbody组件
    /// </summary>
    Rigidbody thisRigidbody;
    /// <summary>
    /// 该物体是否在手上
    /// </summary>
    public bool isOnHand = false;
    /// <summary>
    /// 旧的碰撞体数组
    /// </summary>
    private Collider[] overlappingColliders;
    /// <summary>
    /// 碰撞体过滤层
    /// </summary>
    public LayerMask colliderLayerMask;
    /// <summary>
    /// 碰撞体最大检测数量
    /// </summary>
    public int ColliderArraySize = 32;
    /// <summary>
    /// 悬停检测范围中心点
    /// </summary>
    public Transform colliderPoint;
    /// <summary>
    /// 悬停检测半径
    /// </summary>
    public float colliderRadius = 0.1f;
    /// <summary>
    /// 吸附状态
    /// false 拆卸，true 吸附在加油口上
    /// </summary>
    public bool UseMode
    {
        get
        {
            return useMode;
        }
        set
        {
            if (useMode != value)
            {
                useMode = value;

            }
        }
    }
    [SerializeField]
    [Tooltip(" 吸附状态,false 拆卸，true 吸附目标位置")]
    private bool useMode = false;
    /// <summary>
    /// 检测到的最优选的Interactable物体
    /// </summary>
    public Interactable InteractableOBJ
    {
        get
        {
            return _interactableOBJ;
        }
        set
        {
        }
    }
    private Interactable _interactableOBJ;

    private void Start()
    {
        thisRigidbody = GetComponent<Rigidbody>();
        overlappingColliders = new Collider[ColliderArraySize];
    }
    /// <summary>
    ///  检查指定位置周边指定半径范围内的碰撞到的最近的Interactable物体
    /// </summary>
    /// <param name="hoverPosition">指定的悬停点</param>
    /// <param name="hoverRadius">悬停点半径范围</param>
    /// <param name="closestDistance">最近距离</param>
    /// <param name="closestInteractable">最优选的Interactable物体</param>
    /// <returns></returns>
    private bool MyCheckHoveringForTransform(Vector3 hoverPosition, float hoverRadius, ref float closestDistance, ref Interactable closestInteractable)
    {
        bool foundCloser = false;

        // 清空旧的碰撞体数组
        for (int i = 0; i < overlappingColliders.Length; ++i)
        {
            overlappingColliders[i] = null;
        }

        int numColliding = Physics.OverlapSphereNonAlloc(hoverPosition, hoverRadius, overlappingColliders, colliderLayerMask.value);

        if (numColliding >= ColliderArraySize)
            Debug.LogWarning("<b>[SteamVR Interaction]</b> This hand is overlapping the max number of colliders: " + ColliderArraySize + ". Some collisions may be missed. Increase ColliderArraySize on Hand.cs");


        // Pick the closest hovering
        for (int colliderIndex = 0; colliderIndex < overlappingColliders.Length; colliderIndex++)
        {
            Collider collider = overlappingColliders[colliderIndex];

            if (collider == null)
                continue;

            if (collider.name != "CubeEndPosition")
                continue;

            Interactable contacting = collider.GetComponentInParent<Interactable>();

            // Yeah, it's null, skip
            if (contacting == null)
                continue;

            if (contacting.gameObject == this.gameObject)
                continue;

            // 目前最佳候选人。。。
            float distance = Vector3.Distance(contacting.transform.position, hoverPosition);

            bool lowerPriority = false;
            if (closestInteractable != null)
            {
                // 与最近的可交互进行比较以检查优先级
                lowerPriority = contacting.hoverPriority < closestInteractable.hoverPriority;
            }

            // 判断距离是否最近
            bool isCloser = (distance < closestDistance);
            if (isCloser && !lowerPriority)
            {
                closestDistance = distance;
                closestInteractable = contacting;
                foundCloser = true;
            }
        }
        return foundCloser;
    }

    /// <summary>
    /// 拿在手上时更新
    /// </summary>
    public void ColliderUpdate()
    {
        InteractableOBJ = LookForTheNearestObject();
    }
    /// <summary>
    /// 找到最近的可交互物体
    /// </summary>
    /// <returns></returns>
    private Interactable LookForTheNearestObject()
    {
        float closestDistance = float.MaxValue;
        Interactable _interactable = null;
        MyCheckHoveringForTransform(colliderPoint.position, colliderRadius, ref closestDistance, ref _interactable);
        return _interactable;
    }

    /// <summary>
    /// 该物体从手臂分离
    /// </summary>
    public void DetachFromHand()
    {
        if (!UseMode)
        {
            thisRigidbody.isKinematic = false;
            SetChildColliderToTrigger(transform, false);
            if (InteractableOBJ != null)
            {
                if (!UseMode)
                {
                    thisRigidbody.isKinematic = true;
                    transform.position = InteractableOBJ.transform.position;
                    transform.rotation = InteractableOBJ.transform.rotation;
                    transform.SetParent(InteractableOBJ.transform);
                    SetChildColliderToTrigger(transform, true);
                    UseMode = true;

                }
            }

        }
        InteractableOBJ = null;
        isOnHand = false;
    }

    /// <summary>
    /// 设置物体子对象碰撞器的isTrigger属性
    /// </summary>
    /// <param name="objTransform"></param>
    /// <param name="_isTrigger"></param>
    private void SetChildColliderToTrigger(Transform objTransform, bool _isTrigger)
    {
        foreach (Transform item in objTransform)
        {
            MeshCollider _meshCollider = item.GetComponent<MeshCollider>();
            if (_meshCollider != null)
            {
                _meshCollider.isTrigger = _isTrigger;
            }
        }
    }

    /// <summary>
    /// 当物体被抓到手中时
    /// </summary>
    public void AttachedToHand()
    {
        if (UseMode)
        {

            UseMode = false;

        }
        isOnHand = true;
    }
}