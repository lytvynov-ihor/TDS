using UnityEngine;

public class TowerInteraction : MonoBehaviour
{
    private Tower tower;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Tower"))
                {
                    Debug.Log(hit.collider.name);
                    tower = hit.collider.GetComponent<Tower>();
                         
                    if (tower != null)
                    {
                        SpriteRenderer[] renderers = tower.GetComponentsInChildren<SpriteRenderer>();
                        Canvas canvas = tower.GetComponentInChildren<Canvas>();
                        if (renderers != null)
                        {
                            foreach(Renderer r in renderers)
                            {
                                r.enabled = !r.enabled;
                            }
                        }
                        if (canvas != null)
                        {
                            canvas.enabled = !canvas.enabled;
                        }
                    }
                    else
                    {
                        return;
                    }
                }
            }
        }
    }
}
