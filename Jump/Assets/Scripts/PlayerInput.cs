using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerInput : MonoBehaviour
{

    public LayerMask TouchInputMask;
    public List<GameObject> TouchedObjects = new List<GameObject>();

    private GameObject[] OldTouches;
    private RaycastHit hit;

    void Update()
    {

#if UNITY_EDITOR

        if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0))
        {
            OldTouches = new GameObject[TouchedObjects.Count];
            TouchedObjects.CopyTo(OldTouches);
            TouchedObjects.Clear();

            Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, TouchInputMask))
            {
                GameObject recipient = hit.transform.gameObject;
                TouchedObjects.Add(recipient);

                if (Input.GetMouseButtonDown(0))
                {
                    recipient.SendMessage("OnTouchDown", hit.point, SendMessageOptions.DontRequireReceiver);
                }
                if (Input.GetMouseButtonUp(0))
                {
                    recipient.SendMessage("OnTouchUp", hit.point, SendMessageOptions.DontRequireReceiver);
                }
            }

            foreach (GameObject o in OldTouches)
            {
                if (!TouchedObjects.Contains(o))
                {
                    o.SendMessage("OnTouchExit", hit.point, SendMessageOptions.DontRequireReceiver);
                }
            }
        }

#endif

        if (Input.touchCount > 0)
        {
            OldTouches = new GameObject[TouchedObjects.Count];
            TouchedObjects.CopyTo(OldTouches);
            TouchedObjects.Clear();

            foreach (Touch touch in Input.touches)
            {
                Ray ray = GetComponent<Camera>().ScreenPointToRay(touch.position);

                if (Physics.Raycast(ray, out hit, TouchInputMask))
                {
                    GameObject recipient = hit.transform.gameObject;
                    TouchedObjects.Add(recipient);

                    if (touch.phase == TouchPhase.Began)
                    {
                        recipient.SendMessage("OnTouchDown", hit.point, SendMessageOptions.DontRequireReceiver);
                    }
                    if (touch.phase == TouchPhase.Ended)
                    {
                        recipient.SendMessage("OnTouchUp", hit.point, SendMessageOptions.DontRequireReceiver);
                    }
                    if (touch.phase == TouchPhase.Canceled)
                    {
                        recipient.SendMessage("OnTouchExit", hit.point, SendMessageOptions.DontRequireReceiver);
                    }
                }
            }

            foreach (GameObject o in OldTouches)
            {
                if (!TouchedObjects.Contains(o))
                {
                    o.SendMessage("OnTouchExit");
                }
            }
        }

    }
}
