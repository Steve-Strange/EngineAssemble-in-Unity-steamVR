using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using Valve.VR;

public class Test : MonoBehaviour
{
    /// <summary>
    /// �������Rigidbody���
    /// </summary>
    Rigidbody thisRigidbody;
    /// <summary>
    /// �������Ƿ�������
    /// </summary>
    public bool isOnHand = false;
    /// <summary>
    /// �ɵ���ײ������
    /// </summary>
    private Collider[] overlappingColliders;
    /// <summary>
    /// ��ײ����˲�
    /// </summary>
    public LayerMask colliderLayerMask;
    /// <summary>
    /// ��ײ�����������
    /// </summary>
    public int ColliderArraySize = 32;
    /// <summary>
    /// ��ͣ��ⷶΧ���ĵ�
    /// </summary>
    public Transform colliderPoint;
    /// <summary>
    /// ��ͣ���뾶
    /// </summary>
    public float colliderRadius = 0.1f;
    /// <summary>
    /// ����״̬
    /// false ��ж��true �����ڼ��Ϳ���
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
    [Tooltip(" ����״̬,false ��ж��true ����Ŀ��λ��")]
    private bool useMode = false;
    /// <summary>
    /// ��⵽������ѡ��Interactable����
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
    ///  ���ָ��λ���ܱ�ָ���뾶��Χ�ڵ���ײ���������Interactable����
    /// </summary>
    /// <param name="hoverPosition">ָ������ͣ��</param>
    /// <param name="hoverRadius">��ͣ��뾶��Χ</param>
    /// <param name="closestDistance">�������</param>
    /// <param name="closestInteractable">����ѡ��Interactable����</param>
    /// <returns></returns>
    private bool MyCheckHoveringForTransform(Vector3 hoverPosition, float hoverRadius, ref float closestDistance, ref Interactable closestInteractable)
    {
        bool foundCloser = false;

        // ��վɵ���ײ������
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

            // Ŀǰ��Ѻ�ѡ�ˡ�����
            float distance = Vector3.Distance(contacting.transform.position, hoverPosition);

            bool lowerPriority = false;
            if (closestInteractable != null)
            {
                // ������Ŀɽ������бȽ��Լ�����ȼ�
                lowerPriority = contacting.hoverPriority < closestInteractable.hoverPriority;
            }

            // �жϾ����Ƿ����
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
    /// ��������ʱ����
    /// </summary>
    public void ColliderUpdate()
    {
        InteractableOBJ = LookForTheNearestObject();
    }
    /// <summary>
    /// �ҵ�����Ŀɽ�������
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
    /// ��������ֱ۷���
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
    /// ���������Ӷ�����ײ����isTrigger����
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
    /// �����屻ץ������ʱ
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