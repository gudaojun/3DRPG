using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseManager : Singleton<MouseManager>
{
    public event Action<Vector3> OnMouseClick;
    public event Action<GameObject> OnEnemyClick;
    RaycastHit hitInfo;

    public Texture2D point, DoorWay, Attack, Target, Arrow;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    private void Update()
    {

        SetCursorTexture();
        if (InteractWithUI())
        {
            return;
        }
        MouseControl();
    }
    /// <summary>
    /// …Ë÷√ Û±ÍÃ˘Õº
    /// </summary>
    void SetCursorTexture()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (InteractWithUI())
        {
            Cursor.SetCursor(point, Vector2.zero, CursorMode.Auto);
            return;
        }
        if (Physics.Raycast(ray, out hitInfo))
        {
            switch (hitInfo.collider.gameObject.tag)
            {
                case "Ground":
                    Cursor.SetCursor(Target, new Vector2(16, 16), CursorMode.Auto);
                    break;
                case "Enemy":
                    Cursor.SetCursor(Attack, new Vector2(16, 16), CursorMode.Auto);
                    break;
                case "Door":
                    Cursor.SetCursor(DoorWay, new Vector2(16, 16), CursorMode.Auto);
                    break;
                case "Item":
                    Cursor.SetCursor(point, new Vector2(16, 16), CursorMode.Auto);
                    break;

                default:
                    Cursor.SetCursor(Arrow, new Vector2(16, 16), CursorMode.Auto);
                    break;
            }
        }
    }

    void MouseControl()
    {
        if (Input.GetMouseButtonDown(0) && hitInfo.collider != null)
        {
            if (hitInfo.collider.gameObject.CompareTag("Ground"))
            {
                OnMouseClick?.Invoke(hitInfo.point);
            }
            if (hitInfo.collider.gameObject.CompareTag("Enemy"))
            {
                OnEnemyClick?.Invoke(hitInfo.collider.gameObject);
            }
            if (hitInfo.collider.gameObject.CompareTag("Attackable"))
            {
                OnEnemyClick?.Invoke(hitInfo.collider.gameObject);
            }
            if (hitInfo.collider.gameObject.CompareTag("Door"))
            {
                OnMouseClick?.Invoke(hitInfo.point);
            }
            if (hitInfo.collider.gameObject.CompareTag("Item"))
            {
                OnMouseClick?.Invoke(hitInfo.point);
            }
        }
    }
    public bool InteractWithUI()
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            return true;
        }
        else return false;
    }

}
