using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodePropogation : MonoBehaviour
{
    [SerializeField]
    private LineRenderer lineRenderer;
    [SerializeField]
    private List<Vector3> points;


    private Vector3 mouseWorldPosition;

    // Start is called before the first frame update
    void Start()
    {
        InputManager.Instance.OnMouseClickEvent.AddListener(OnMouseClickEventHandler);
    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnMouseClickEventHandler(Vector2 mousePos)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray, out hit, 100))
        {
            Debug.Log(hit.transform.name);
            mouseWorldPosition = hit.point;
        }

        points.Add(hit.point);
        CreateTrail(points);
        //Debug.Log("Mouse Click : " + mousePos);
    }

    private void CreateTrail(List<Vector3> points)
    {
        lineRenderer.positionCount = points.Count;
        for (int i = 0; i < points.Count; i++)
        {
            lineRenderer.SetPosition(i, points[i]);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(mouseWorldPosition, 0.1f);
    }

}
