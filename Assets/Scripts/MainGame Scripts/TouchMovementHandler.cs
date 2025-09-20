// TouchMovementHandler.cs using UnityEngine.InputSystem
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class TouchMovementHandler : MonoBehaviour
{
    public static TouchMovementHandler Instance;
    [HideInInspector] public GameObject PointerGO;
    public GameObject PointerPrefab;
    private Vector3 PointerPosition;
    private Plane newPlane;
    private float CalcRayDistance;
    public bool isAligned = false;
    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        newPlane = new Plane(Camera.main.transform.forward * 0.1f, this.transform.position);
    }
    private void Update()
    {
        PointerHandle();
    }
    void PointerHandle()
    {
        var touchscreen = Touchscreen.current;
        var mouse = Mouse.current;
        bool isTouchBegan = touchscreen != null && touchscreen.primaryTouch.press.isPressed && touchscreen.primaryTouch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Began;
        bool isTouchMoved = touchscreen != null && touchscreen.primaryTouch.press.isPressed && touchscreen.primaryTouch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Moved;
        bool isMousePressed = mouse != null && mouse.leftButton.isPressed;
        bool isMouseDown = mouse != null && mouse.leftButton.wasPressedThisFrame;
        if (isTouchBegan || isMouseDown)
        {
            Ray newRay = Camera.main.ScreenPointToRay(mouse != null ? mouse.position.ReadValue() : touchscreen.primaryTouch.position.ReadValue());
            if (newPlane.Raycast(newRay, out CalcRayDistance))
            {
                PointerPosition = newRay.GetPoint(CalcRayDistance);
                PointerGO = Instantiate(PointerPrefab, PointerPosition, Quaternion.identity);
            }
        }
        else if (isTouchMoved || isMousePressed)
        {
            Ray newRay = Camera.main.ScreenPointToRay(mouse != null ? mouse.position.ReadValue() : touchscreen.primaryTouch.position.ReadValue());
            if (newPlane.Raycast(newRay, out CalcRayDistance))
            {
                if (PointerGO != null)
                {
                    PointerGO.transform.position = newRay.GetPoint(CalcRayDistance);
                }
            }
        }
    }
}
