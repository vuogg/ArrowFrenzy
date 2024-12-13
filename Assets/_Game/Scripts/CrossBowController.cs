using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossBowController : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Collider crossbowCollider;
    Vector2 startTouch;
    public Transform crossbowTransform;
    public GameObject arrowPrefab;
    public Transform shootPoint;
    bool isDragging;

    public int maxBounces = 3;
    //private bool hasShot = false;
    //public float arrowSpeed = 10f;

    void Update()
    {
        //if (hasShot)
        //    return;
        if (Input.GetMouseButtonDown(0))
        {
            startTouch = Input.mousePosition;
            isDragging = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            ShootArrow();
        }

        if (isDragging)
        {
            Vector2 currentTouch = Input.mousePosition;
            float deltaTouch = currentTouch.x - startTouch.x;
            RotateBow(deltaTouch);
            DrawAimLine();
        }
    }

    void DrawAimLine()
    {
        Vector3 aimDirection = shootPoint.forward;
        Vector3 currentPosition = shootPoint.position;

        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, currentPosition);

        for (int i = 0; i < maxBounces; i++)
        {
            RaycastHit hit;
            if(Physics.Raycast(currentPosition, aimDirection, out hit, 10f))
            {
                lineRenderer.positionCount += 1;
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.point);

                if(hit.collider.CompareTag("Target") || hit.collider.CompareTag("Buff"))
                {
                    break;
                }

                aimDirection = Vector3.Reflect(aimDirection, hit.normal);
                currentPosition = hit.point;
            }
            else
            {
                lineRenderer.positionCount += 1;
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, currentPosition + aimDirection * 10f);
                break;
            }
        }
    }

    void RotateBow(float deltaTouch)
    {
        //gioi han goc xoay
        float rotationAngle = Mathf.Clamp(deltaTouch, -360, 360);

        //interpolating bang lerpangle
        float targetRotationAngle = rotationAngle;
        float smoothSpeed = 5f;

        //lam muot goc quay
        float currentRotation = crossbowTransform.eulerAngles.y;
        float newRotation = Mathf.LerpAngle(currentRotation, targetRotationAngle, smoothSpeed * Time.deltaTime);

        //Cap nhat goc quay cua no
        crossbowTransform.rotation = Quaternion.Euler(0, newRotation, 0);
    }

    void ShootArrow()
    {
        //Tao mui ten
        GameObject arrow = Instantiate(arrowPrefab, shootPoint.position, shootPoint.rotation);

        //Truyen van toc cho mui ten
        Arrow arrowScript = arrow.GetComponent<Arrow>();
        if (arrowScript != null)
        {
            arrowScript.Launch(shootPoint.forward * arrow.GetComponent<Arrow>().arrowSpeed);
        }
        
        //hasShot = true;
        //lineRenderer.enabled = false;
        //crossbowCollider.enabled = false;
    }
}
