using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossBowController : AnimationsController
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Collider crossbowCollider;

    public Transform crossbowTransform;
    public Arrow arrowPrefab;
    public Transform shootPoint;
    public int maxBounces = 3;

    public bool isControl;
    public bool hasShot;
    public bool isDragging;

    void Update()
    {
        if (hasShot || UIManager.Instance.IsOpened<Settings>())
        {
            lineRenderer.enabled = false;
            return;
        }

        if (GameManager.Instance.IsState(GameState.GamePlay))
        {
            if (Input.GetMouseButtonDown(0))
            {
                ChangeAnim(Constants.ANIM_HOLD);
                //startTouch = Input.mousePosition;
                lineRenderer.enabled = true;
                isDragging = true;
            }
            else if (Input.GetMouseButtonUp(0) && isDragging)
            {
                ChangeAnim(Constants.ANIM_SHOOT);
                isDragging = false;
                ShootArrow();
                hasShot = true;
            }

            if (isDragging)
            {
                RotateBow();
                DrawAimLine();
            }
        }
    }

    public void OnInit()
    {
        isDragging = false;
        hasShot = false;
        lineRenderer.enabled = false;
        crossbowCollider.enabled = true;
        crossbowTransform.localPosition = Vector3.zero;
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
            if (Physics.Raycast(currentPosition, aimDirection, out hit, 12f))
            {
                lineRenderer.positionCount++;
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.point);

                if (hit.collider.CompareTag(Constants.TAG_WALL))
                {
                    aimDirection = Vector3.Reflect(aimDirection, hit.normal);
                    currentPosition = hit.point;
                }
                else
                {
                    break;
                }
            }
            else
            {
                lineRenderer.positionCount++;
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, currentPosition + aimDirection * 12f);
                break;
            }
        }
    }

    void RotateBow()
    {
        //lay vi tri input
        Vector3 mouseScreenPosition = Input.mousePosition;

        //chuyen vi tri input sang camera world
        Ray ray = Camera.main.ScreenPointToRay(mouseScreenPosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity))
        {
            //tinh huong tu input den vi tri cua no
            Vector3 targetDirection = (crossbowTransform.position - hitInfo.point).normalized;

            //tinh goc quay nguoc
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(targetDirection.x, 0, targetDirection.z));

            //no quay cham va muot
            //float smoothSpeed = 3f;
            //crossbowTransform.rotation = Quaternion.Slerp(crossbowTransform.rotation, targetRotation, smoothSpeed * Time.deltaTime);

            float angle = Quaternion.Angle(crossbowTransform.rotation, targetRotation);
            if (angle > 5f)
            {
                float smoothSpeed = 3f;
                crossbowTransform.rotation = Quaternion.Slerp(crossbowTransform.rotation, targetRotation, smoothSpeed * Time.deltaTime);
            }
        }
    }

    void ShootArrow()
    {
        //Tao mui ten
        //Arrow arrow = Instantiate(arrowPrefab, shootPoint.position, shootPoint.rotation);
        //Arrow arrow = SimplePool.Spawn<Arrow>(arrowPrefab, shootPoint.position, shootPoint.rotation);
        Arrow arrow = SimplePool.Spawn<Arrow>(PoolType.Arrow, shootPoint.position, shootPoint.rotation);

        //Truyen van toc cho mui ten
        //Arrow arrowScript = arrow.GetComponent<Arrow>();
        //Arrow arrowScript = Cache.GetArrow(arrow.gameObject);
        //if (arrowScript != null)
        //{
        arrow.Launch(shootPoint.forward * arrowPrefab.arrowSpeed);
            //arrowScript.Launch(shootPoint.forward * arrow.GetComponent<Arrow>().arrowSpeed);
        //}

        hasShot = true;
        lineRenderer.enabled = false;
        crossbowCollider.enabled = false;
    }
    
    public void ResetState()
    {
        isDragging = false;
    }
}
