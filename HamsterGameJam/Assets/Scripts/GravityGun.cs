using UnityEngine;
using UnityEngine.UI;

public class GravityGun : MonoBehaviour
{
    public Camera playerCamera;
    public float grabRange = 5f;
    public float throwForce = 500f;
    public Transform holdPoint;
    public LayerMask grabbableLayer;
    public GameObject grabPrompt; // UI reference

    private Rigidbody grabbedObject;
    private bool isHolding = false;
    private float holdDistance = 3f;
    private float minDistance = 1f;
    private float maxDistance = 10f;
    private float scrollSpeed = 2f;
    private bool lookingAtGrabbable = false;

    void Start()
    {
        if (grabPrompt != null)
        {
            grabPrompt.SetActive(false);
        }
    }

    void Update()
    {
        // Check if the grabbed object was destroyed
        if (isHolding && grabbedObject == null)
        {
            isHolding = false;
        }

        CheckForGrabbable();

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (isHolding) DropObject();
            else TryGrabObject();
        }

        if (isHolding && grabbedObject != null)
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            holdDistance += scroll * scrollSpeed;
            holdDistance = Mathf.Clamp(holdDistance, minDistance, maxDistance);

            MoveObject();
        }

        if (Input.GetMouseButtonDown(1) && isHolding)
        {
            ThrowObject();
        }
    }
    
    void CheckForGrabbable()
    {
        RaycastHit hit;
        if (!isHolding && Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, grabRange, grabbableLayer))
        {
            // Show prompt
            if (!lookingAtGrabbable)
            {
                grabPrompt.gameObject.SetActive(true);
                lookingAtGrabbable = true;
            }
        }
        else
        {
            // Hide prompt
            if (lookingAtGrabbable)
            {
                grabPrompt.gameObject.SetActive(false);
                lookingAtGrabbable = false;
            }
        }
    }

    void TryGrabObject()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, grabRange, grabbableLayer))
        {
            Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                grabbedObject = rb;
                grabbedObject.useGravity = false;
                grabbedObject.drag = 10f;
                isHolding = true;
                holdDistance = Vector3.Distance(playerCamera.transform.position, grabbedObject.position);
                grabPrompt.gameObject.SetActive(false); // Hide when grabbing
            }
        }
    }

    void MoveObject()
    {
        if (grabbedObject == null) return;

        Vector3 targetPosition = playerCamera.transform.position + playerCamera.transform.forward * holdDistance;
        Vector3 direction = targetPosition - grabbedObject.position;
        grabbedObject.velocity = direction * 10f;
    }

    void DropObject()
    {
        if (grabbedObject != null)
        {
            grabbedObject.useGravity = true;
            grabbedObject.drag = 0;
            grabbedObject = null;
            isHolding = false;
        }
    }

    void ThrowObject()
    {
        if (grabbedObject != null)
        {
            grabbedObject.useGravity = true;
            grabbedObject.drag = 0;
            grabbedObject.AddForce(playerCamera.transform.forward * throwForce);
            grabbedObject = null;
            isHolding = false;
        }
    }
}
