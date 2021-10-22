using Assets.Scripts;
using UnityEngine;


public class Lighsaber : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The blade object")]
    private GameObject _blade = null;
     
    [SerializeField]
    [Tooltip("The empty game object located at the tip of the blade")]
    private GameObject _tip = null;

    [SerializeField]
    [Tooltip("The empty game object located at the base of the blade")]
    private GameObject _base = null;

    [SerializeField]
    [Tooltip("The mesh object with the mesh filter and mesh renderer")]
    private GameObject _meshParent = null;

    [SerializeField]
    [Tooltip("The number of frame that the trail should be rendered for")]
    private int _trailFrameLength = 3;

    [SerializeField]
    [ColorUsage(true, true)]
    [Tooltip("The colour of the blade and trail")]
    private Color _colour = Color.red;

    [SerializeField]
    [Tooltip("The amount of force applied to each side of a slice")]
    private float _forceAppliedToCut = 3f;

    private Vector3 _triggerEnterTipPosition;
    private Vector3 _triggerEnterBasePosition;
    private Vector3 _triggerExitTipPosition;
    private Vector3 _otherStartPosition;
    

    private void OnTriggerEnter(Collider other)
    {
        _triggerEnterTipPosition = _tip.transform.position;
        _triggerEnterBasePosition = _base.transform.position;
        _otherStartPosition = other.transform.position;
    }

    private void OnTriggerExit(Collider other)
    {
        _triggerExitTipPosition = _tip.transform.position;
        Vector3 otherOffset = other.transform.position - _otherStartPosition;

        //Create a triangle between the tip and base so that we can get the normal
        Vector3 side1 = _triggerExitTipPosition - _triggerEnterTipPosition + otherOffset;
        Vector3 side2 = _triggerExitTipPosition - _triggerEnterBasePosition + otherOffset;

        //Get the point perpendicular to the triangle above which is the normal
        //https://docs.unity3d.com/Manual/ComputingNormalPerpendicularVector.html
        Vector3 normal = Vector3.Cross(side1, side2).normalized;

        //Transform the normal so that it is aligned with the object we are slicing's transform.
        Vector3 transformedNormal = ((Vector3)(other.gameObject.transform.localToWorldMatrix.transpose * normal)).normalized;

        //Get the enter position relative to the object we're cutting's local transform
        Vector3 transformedStartingPoint = other.gameObject.transform.InverseTransformPoint(_triggerEnterTipPosition);

        Plane plane = new Plane();

        plane.SetNormalAndPosition(
                transformedNormal,
                transformedStartingPoint);

        Debug.Log(plane);

        var direction = Vector3.Dot(Vector3.up, transformedNormal);

        //Flip the plane so that we always know which side the positive mesh is on
        if (direction < 0)
        {
            plane = plane.flipped;
        }

        GameObject[] slices = Slicer.Slice(plane, other.gameObject);
        Destroy(other.gameObject);

        Rigidbody rigidbody = slices[1].GetComponent<Rigidbody>();
        Vector3 positiveNormal = -plane.normal * _forceAppliedToCut;
        rigidbody.AddForce(positiveNormal, ForceMode.Impulse);

        Rigidbody rigidbody2 = slices[0].GetComponent<Rigidbody>();
        Vector3 negativeNormal = plane.normal * _forceAppliedToCut;
        rigidbody2.AddForce(negativeNormal, ForceMode.Impulse);
    }
}
