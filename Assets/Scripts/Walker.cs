using UnityEngine;

public class Walker : MonoBehaviour
{
    public Transform LeftFootTarget;
    public Transform RightFootTarget;

    public AnimationCurve horizontalCurve;
    public AnimationCurve verticalCurve;

    private Vector3 LeftTargetOffset;
    private Vector3 RightTargetOffset;

    private float LeftLegLast = 0;
    private float RightLegLast = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LeftTargetOffset = LeftFootTarget.localPosition;
        RightTargetOffset = RightFootTarget.localPosition;

    }

    // Update is called once per frame
    void Update()
    {
        float leftLegForwardMovement = horizontalCurve.Evaluate(Time.time);
        float rightLegForwardMovement = horizontalCurve.Evaluate(Time.time - 1);


        LeftFootTarget.localPosition = LeftTargetOffset 
            + this.transform.InverseTransformVector(LeftFootTarget.forward) * leftLegForwardMovement
            + this.transform.InverseTransformVector(LeftFootTarget.up) * verticalCurve.Evaluate(Time.time + 0.5f);

        RightFootTarget.localPosition = RightTargetOffset 
            + this.transform.InverseTransformVector(RightFootTarget.forward) * rightLegForwardMovement
            + this.transform.InverseTransformVector(RightFootTarget.up) * verticalCurve.Evaluate(Time.time - 0.5f);

        float leftLegDirection = leftLegForwardMovement - LeftLegLast;
        float rightLegDirection = rightLegForwardMovement - RightLegLast;

        RaycastHit hit;
        if (leftLegDirection < 0 && 
            Physics.Raycast(LeftFootTarget.position + LeftFootTarget.up, -LeftFootTarget.up, out hit, Mathf.Infinity))
        {
            LeftFootTarget.position = hit.point;
            this.transform.position += this.transform.forward * Mathf.Abs(leftLegDirection);
        }

        if (rightLegDirection < 0 &&
            Physics.Raycast(RightFootTarget.position + RightFootTarget.up, -RightFootTarget.up, out hit, Mathf.Infinity))
        {
            RightFootTarget.position = hit.point;
            this.transform.position += this.transform.forward * Mathf.Abs(rightLegDirection);

        }

        LeftLegLast = leftLegForwardMovement;
        RightLegLast = rightLegForwardMovement;
    }
}
