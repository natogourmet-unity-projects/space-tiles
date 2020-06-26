using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Vector3 touchStart;
    public float minZoom, maxZoom;

    public Transform target;
    public float smoothSpeed = 0.25f;
    public Vector3 offset;

    public static int size = 0;
    Vector3 middlePoint;
    Vector3 viewPoint;

    bool canMove = false;
    


    private void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, viewPoint + offset, smoothSpeed * Time.deltaTime);
    }


    void Start () {
        viewPoint = Vector3.zero;

        middlePoint.x = (float)(size)/2f;
        middlePoint.z = (float)(size)/2f;

        maxZoom = size + 2;
        Camera.main.orthographicSize = size;
	}

	
	// Update is called once per frame
	void Update () {

        viewPoint.x = (target.position.x + middlePoint.x) / 2f;
        viewPoint.z = (target.position.z + middlePoint.z) / 2f;

        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Tile hitdTile = hit.collider.GetComponent<Tile>();
                touchStart = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                canMove = (hitdTile == null);
            }
        }
        if (Input.touchCount == 1 && (canMove && Input.GetTouch(0).phase == TouchPhase.Moved))
        {
            Vector3 direction = touchStart - Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            Camera.main.transform.position += direction;
        }

        if (canMove && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            canMove = false;
        }

        if (Input.touchCount == 2)
        {
            Touch firstTouch = Input.GetTouch(0);
            Touch secondTouch = Input.GetTouch(1);

            Vector2 firstTouchIniPos = firstTouch.position - firstTouch.deltaPosition;
            Vector2 secondTouchIniPos = secondTouch.position - secondTouch.deltaPosition;

            float firstMagnitude = (firstTouchIniPos - secondTouchIniPos).magnitude;
            float currentMagnitude = (firstTouch.position - secondTouch.position).magnitude;

            float difference = currentMagnitude - firstMagnitude;

            Zoom(difference * 0.01f);
        }

        if (Camera.main.transform.position.x > size - 2)
        {
            Vector3 newPos = Camera.main.transform.position;
            newPos.x = size - 2;
            Camera.main.transform.position = newPos;
        }
        if (Camera.main.transform.position.x < 0)
        {
            Vector3 newPos = Camera.main.transform.position;
            newPos.x = 0;
            Camera.main.transform.position = newPos;
        }
        if (Camera.main.transform.position.y > size - 2)
        {
            Vector3 newPos = Camera.main.transform.position;
            newPos.y = size - 2;
            Camera.main.transform.position = newPos;
        }
        if (Camera.main.transform.position.y < 0)
        {
            Vector3 newPos = Camera.main.transform.position;
            newPos.y = 0;
            Camera.main.transform.position = newPos;
        }

    }

    void Zoom(float increment)
    {
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - increment, minZoom, maxZoom);
    }

    Ray GenerateTouchRay(Vector3 touchPos)
    {
        Vector3 touchPosFar = new Vector3(touchPos.x, touchPos.y, Camera.main.farClipPlane);
        Vector3 touchPosNear = new Vector3(touchPos.x, touchPos.y, Camera.main.nearClipPlane);
        Vector3 touchPosF = Camera.main.ScreenToWorldPoint(touchPosFar);
        Vector3 touchPosN = Camera.main.ScreenToWorldPoint(touchPosNear);

        Ray mr = new Ray(touchPosN, touchPosF - touchPosN);

        return mr;
    }
}
