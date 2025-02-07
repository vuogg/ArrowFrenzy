using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragController : MonoBehaviour
{
    public static Vector3 direct;

    private Vector3 screen;

    private Vector3 MousePosition => Input.mousePosition - screen / 2;

    private Vector3 startPoint;
    private Vector3 updatePoint;

    public RectTransform baseMarker;
    public RectTransform dragMarker;
    public RectTransform directionLine;
    private float magnitude = 200f;

    public GameObject dragPanel;
    public bool canDrag;

    void Awake()
    {
        canDrag = true;
        screen.x = Screen.width;
        screen.y = Screen.height;

        direct = Vector3.zero;

        dragPanel.SetActive(false);
    }

    void Update()
    {
        if (!canDrag)
        {
            return;
        }

        if(Input.GetMouseButtonDown(0))
        {
            startPoint = MousePosition;
            baseMarker.anchoredPosition = startPoint;
            dragPanel.SetActive(true);
        }

        if(Input.GetMouseButton(0))
        {
            updatePoint = MousePosition;
            dragMarker.anchoredPosition = Vector3.ClampMagnitude((updatePoint - startPoint), magnitude) + startPoint;

            direct = (updatePoint - startPoint).normalized;
            direct.z = direct.y;
            direct.y = 0;

            UpdateDirectionLine();
        }

        if(Input.GetMouseButtonUp(0))
        {
            dragPanel.SetActive(false);
        }
    }

    public void OnInit()
    {
        canDrag = true;
        screen.x = Screen.width;
        screen.y = Screen.height;

        direct = Vector3.zero;

        dragPanel.SetActive(false);
    }

    private void UpdateDirectionLine()
    {
        if (baseMarker == null || dragMarker == null || directionLine == null)
            return;

        Vector2 basePos = baseMarker.anchoredPosition;
        Vector2 dragPos = dragMarker.anchoredPosition;

        Vector2 diff = dragPos - basePos;

        Vector2 midPoint = basePos + diff * 0.5f;
        directionLine.anchoredPosition = midPoint;

        float angle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        directionLine.localRotation = Quaternion.Euler(0, 0, angle);

        float distance = diff.magnitude;
        Vector2 size = directionLine.sizeDelta;
        size.x = distance;
        directionLine.sizeDelta = size;
    }

    private void OnDisable()
    {
        direct = Vector3.zero;
    }
}
