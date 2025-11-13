using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    public float checkRate = 0.05f;
    private float lastCheckTime;
    public float maxCheckDistance;
    public LayerMask layerMask;

    public GameObject curInteractGameObject;
    private IInteractable curInteractable;

    public TextMeshProUGUI promptText;
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (cam == null)
            return;
        if (Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;

            RaycastHit hit = new RaycastHit();
            bool hitSomething = false;

            Ray[] rays = new Ray[3]
           {
                cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2)),        // 중앙
                cam.ScreenPointToRay(new Vector3(Screen.width / 2 + 5, Screen.height / 2)),    // 오른쪽
                cam.ScreenPointToRay(new Vector3(Screen.width / 2 - 5, Screen.height / 2))     // 왼쪽
           };
            foreach (Ray r in rays)
            {
                if (Physics.Raycast(r, out hit, maxCheckDistance, layerMask))
                {
                    hitSomething = true;
                    break;
                }
            }
            if (hitSomething)
            {
                if (hit.collider.gameObject != curInteractGameObject)
                {
                    curInteractGameObject = hit.collider.gameObject;
                    curInteractable = hit.collider.GetComponentInParent<IInteractable>();
                    SetPromptText();
                }
            }
            else
            {
                curInteractGameObject = null;
                curInteractable = null;
                promptText.gameObject.SetActive(false);
            }
        }
    }

    private void SetPromptText()
    {
        if (curInteractable == null || promptText == null)
            return;
        promptText.gameObject.SetActive(true);
        promptText.text = curInteractable.GetInteractPrompt();

    }

    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && curInteractable != null)
        {
            curInteractable.OnInteract();
            curInteractGameObject = null;
            curInteractable = null;
            promptText.gameObject.SetActive(false);
        }
    }
}

