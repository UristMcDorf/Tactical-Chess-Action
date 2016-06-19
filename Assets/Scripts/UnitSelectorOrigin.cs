using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class UnitSelectorOrigin : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
    Vector3 startPosition = Vector3.zero;

    public string origin;
    public UnitSelectorType connectedType;

    UnitSelectorType oldConnected = null; //to reduce workload on updates

    Image line;
    float lineWidth = 2.0f;

    void Start()
    {
        Image[] lines = GetComponentsInChildren<Image>();

        foreach (Image potentialLine in lines)
        {
            if (potentialLine != GetComponent<Image>())
            {
                line = potentialLine;
                break;
            }
        }

        if (!line)
            Debug.LogError ("No line in children!");
    }

    void Update()
    {
        if (connectedType == oldConnected)
            return;

        if (connectedType == null)
        {
            line.enabled = false;
            return;
        }

        line.enabled = true;

        oldConnected = connectedType;

        Vector3 differenceVector = connectedType.transform.position - this.transform.position;

        line.rectTransform.sizeDelta = new Vector2(differenceVector.magnitude, lineWidth);
        line.rectTransform.pivot = new Vector2(0, 0.5f); //middle?
        line.rectTransform.position = this.transform.position;
        float angle =  Mathf.Atan2(differenceVector.y, differenceVector.x) * Mathf.Rad2Deg;
        line.rectTransform.rotation = Quaternion.Euler(0, 0, angle);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        line.enabled = false;
        startPosition = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        line.enabled = true;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (RaycastResult result in results)
        {
            UnitSelectorType newConnected = result.gameObject.GetComponent<UnitSelectorType>();
            if (newConnected)
            {
                SetConnection(newConnected);
                break;
            }
        }

        transform.position = startPosition;
    }

    public void SetConnection(UnitSelectorType newConnected) //So shitty. TODO
    {
        if (connectedType) //clearing all current connections
        {
            connectedType.connectedOrigin = null;
        }

        connectedType = newConnected;

        if (connectedType.connectedOrigin) //clearing the new type's old connections
            connectedType.connectedOrigin.connectedType = null;

        connectedType.connectedOrigin = this;
    }
}
