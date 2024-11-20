using UnityEngine;

public class TowerInteraction : MonoBehaviour
{
    private Tower tower; // Reference to the Tower component

    void Update()
    {
        // Check for mouse click
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Check if the clicked object is a tower
                if (hit.collider.CompareTag("Tower"))
                {
                    // Get the Tower component from the clicked object
                    tower = hit.collider.GetComponent<Tower>();
                         
                    // Enable/disable the SpriteRenderer of the child object
                    if (tower != null)
                    {
                        SpriteRenderer[] renderers = tower.GetComponentsInChildren<SpriteRenderer>();
                        Canvas canvas = tower.GetComponentInChildren<Canvas>();
                        if (renderers != null)
                        {
                            foreach(Renderer r in renderers)
                            {
                                r.enabled = !r.enabled; // Toggle the visibility
                            }
                        }
                        if (canvas != null)
                        {
                            canvas.enabled = !canvas.enabled;
                        }
                    }
                    else
                    {
                        Debug.Log(hit.collider.name);
                        return;
                    }
                }
            }
        }
    }
}
