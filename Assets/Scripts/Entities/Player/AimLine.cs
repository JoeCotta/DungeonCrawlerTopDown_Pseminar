using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimLine : MonoBehaviour
{
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] Camera cam;
    [SerializeField] int range;


    private void Update()
    {
        if(GameManager.aimLine)ShootLaser();
    }

    void ShootLaser()
    {
        Vector2 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, mousePosition,range, LayerMask.GetMask("Map","Default"));
        if (hit.Length >= 2) Draw2DRay(new Vector2(0, 0), hit[1].point - (Vector2)transform.position);
        else Draw2DRay(new Vector2(0, 0), hit[0].point - (Vector2)transform.position);
    }


    void Draw2DRay(Vector2 startpoint, Vector2 endpoint)
    {
        lineRenderer.SetPosition(0, startpoint);
        lineRenderer.SetPosition(1, endpoint);
    }
}
