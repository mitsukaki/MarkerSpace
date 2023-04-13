using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pen : MonoBehaviour
{
    public float brushSize = 10f;

    [SerializeField]  private Transform _raycastOrigin;

    // whiteboard layer
    [SerializeField] private LayerMask _layerMask;

    [SerializeField] private Color _color;

    [SerializeField] private ThreeDPen _threeDPen;

    public void SetColor(Color color)
    {
        _color = color;
        _threeDPen.SetColor(color);

        // get the second material on this object and update it's color
        GetComponent<Renderer>().materials[1].color = color;
    }

    private void Update()
    {
        // debug draw the raycast
        Debug.DrawRay(
            _raycastOrigin.position,
            _raycastOrigin.forward * 0.02f,
            Color.red
        );

        // raycast from the pen raycast origin
        if (Physics.Raycast(
            _raycastOrigin.position,
            _raycastOrigin.forward,
            out RaycastHit hit,
            0.02f,
            _layerMask
        ))
        {
            // get the whiteboard surface
            Whiteboard surface = hit.collider.GetComponent<Whiteboard>();

            // get the 3D hit point in whiteboard space
            Vector3 boardHit = ConvertSpace(hit.point, surface.transform);

            // get ratio of distance from origin to board and use as scale
            float scale = 1f - Vector3.Distance(
                _raycastOrigin.position, hit.point) / 0.02f;
            
            // draw on the whiteboard
            surface.Draw(
                surface.GetPixelPos(new Vector2(-boardHit.x, boardHit.y)),
                _color,
                brushSize // * scale
            );
        }
    }

    // function that converts a point from world space to local space
    private Vector3 ConvertSpace(Vector3 point, Transform targetSpace)
    {
        point -= targetSpace.position;
        point = Quaternion.Inverse(targetSpace.rotation) * point;
        return point;
    }
}
