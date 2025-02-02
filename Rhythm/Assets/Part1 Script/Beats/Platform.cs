using System.Collections;
using UnityEngine;

public class Platform : MonoBehaviour
{
    private UI UIObject;
    private float movementSpeed = 2f;
    private Vector3 originalPosition;
    private float timer = 0f;
    private Renderer objRenderer;
    private Color originalColor;
    public GameObject beatsBar;
    private bool done = false;
    private bool moving;
    private void Start()
    {
        UIObject = FindObjectOfType<UI>();
        originalPosition = transform.position;
        objRenderer = GetComponent<Renderer>();
        originalColor = objRenderer.material.color;
        HideBeatsBar();
    }

    void Update()
    {
        // Check if the player is on the elevator
        if (moving)
        {
            // Move the elevator upward
            if (transform.position.y >= originalPosition.y + 10)
            {
                UIObject.beat = false;
                UIObject.count = 0;
                UIObject.pkey = 1;
                moving = false;
                done = true;
                HideBeatsBar();
            }
            else
            {
                if (timer > 2.0f)
                {
                    moving = false;
                    timer = 0;
                }
                transform.Translate(Vector3.up * movementSpeed * Time.deltaTime);
                timer += Time.deltaTime;


            }
        }
        else if(transform.position.y > originalPosition.y && !done)
        {
            transform.Translate(Vector3.down * movementSpeed * Time.deltaTime);
        }

        //if (UIObject.color)
        //{
        //    ChangeObjectColor(Color.red);
        //}
        //else
        //{
        //    ChangeObjectColor(originalColor);
        //}
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (timer == 0f)
            {
                UIObject.beat = true;
                ShowBeatsBar();
            }
        }
    }

    void ChangeObjectColor(Color newColor)
    {
        // objRenderer.material.color = newColor;
        objRenderer.material = new Material(objRenderer.material);
        objRenderer.material.color = newColor;
    }

    void ChangeColorBack()
    {
        // Use the originalColor variable to revert the color
        ChangeObjectColor(originalColor);
    }

    //private void OnTriggerExit2D(Collider2D collider2D)
    //{
    //    if (collider2D.CompareTag("Player"))
    //    {

    //        //UIObject.gameObject.SetActive(true);
    //        UIObject.beat = false;
    //        UIObject.count = 0;
    //        UIObject.pkey = true;
    //        UIObject.moving = false;
    //        Debug.Log("not");
    //    }
    //}
    public void ShowBeatsBar()
    {
        if (beatsBar != null)
        {
            // Try to find the renderer in the children
            Renderer[] renderers = beatsBar.GetComponentsInChildren<Renderer>(true);

            if (renderers.Length > 0)
            {
                foreach (Renderer renderer in renderers)
                {
                    renderer.enabled = true;
                }
            }
            else
            {
                Debug.LogError("Renderer component not found in the children of beatsBar GameObject.");
            }
        }
        else
        {
            Debug.LogError("beatsBar GameObject not assigned.");
        }
    }

    // Method to make the GameObject invisible
    public void HideBeatsBar()
    {
        if (beatsBar != null)
        {
            // Try to find the renderer in the children
            Renderer[] renderers = beatsBar.GetComponentsInChildren<Renderer>(true);

            if (renderers.Length > 0)
            {
                foreach (Renderer renderer in renderers)
                {
                    renderer.enabled = false;
                }
            }
            else
            {
                Debug.LogError("Renderer component not found in the children of beatsBar GameObject.");
            }
        }
        else
        {
            Debug.LogError("beatsBar GameObject not assigned.");
        }
    }

    public void allowMove()
    {
        moving = true;
    }


}