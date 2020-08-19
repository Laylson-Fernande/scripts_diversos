using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;


public class Joystick : UIBehaviour,IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    //[SerializeField]
    //GameObject[] joy = new GameObject[2];
    [SerializeField]
    RectTransform jBase;
    [SerializeField]
    RectTransform jTop;
    Vector2 axis;
    Vector3 startPosi;
    public Vector2 direction;
    public bool fixedJoystick;

    private void Start() {
        startPosi = jBase.position;
    }
    public void OnDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(jBase, eventData.position, eventData.pressEventCamera, out axis);
        axis.x /= jBase.sizeDelta.x * 0.5f;
        axis.y /= jBase.sizeDelta.y * 0.5f;
        axis = Vector2.ClampMagnitude(axis,1);
        direction = SetDirection(jTop.transform.position - jBase.transform.position);
        jTop.localPosition = axis * Mathf.Max(jBase.sizeDelta.x, jBase.sizeDelta.y) * 0.5f;
    }

    Vector2 SetDirection(Vector2 _axis) {
        Vector2 setDirection = _axis;
        float x = _axis.x;
        float y = _axis.y;
        if (x > 0 && y > 0)
        {
            if (x > y)
                setDirection = Vector2.right;
            else
                setDirection = Vector2.up;
        }
        else if (x > 0 && y < 0)
        {
            y = -(y * 2);
            if (x > y)
                setDirection = Vector2.right;
            else
                setDirection = Vector2.down;
        }
        else if (x < 0 && y < 0)
        {
            x = -(x * 2);
            y = -(y * 2);
            if (x > y)
                setDirection = Vector2.left;
            else
                setDirection = Vector2.down;
        } else if (x < 0 && y > 0)
        {
            x = -(x * 2);
            if (x > y)
                setDirection = Vector2.left;
            else
                setDirection = Vector2.up;
        }
        return setDirection;
       
    }
    public void ToZero() {
        jBase.position = startPosi;
        jTop.localPosition = Vector2.zero;
        direction = Vector2.zero;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(!fixedJoystick)
        jBase.position = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        jBase.position = startPosi;
        jTop.localPosition = Vector2.zero;
        direction = Vector2.zero;
    }
}
