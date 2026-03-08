using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHouseInteract : MonoBehaviour
{
    [SerializeField] private InputReaderSO input;

    private PlayerController playerController;

    [SerializeField] private LayerMask interactableHouseLayerMask;

    private Transform selectedHouse;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if ( !playerController.IsDocked || !playerController.CanInteractWithHouses || GameManager.Instance.IsPaused)
        {
            if (selectedHouse != null)
            {
                selectedHouse.GetComponent<Outline>().enabled = false;
                selectedHouse = null;
            }

            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, 1000f, interactableHouseLayerMask) && UIManager.Instance.activeUIWindow == UIManager.UIWindow.None)
        {
            Transform newHouse = hitInfo.transform.parent;

            if (newHouse != selectedHouse)
            {
                if (selectedHouse != null)
                    selectedHouse.GetComponent<Outline>().enabled = false;

                newHouse.GetComponent<Outline>().enabled = true;
                selectedHouse = newHouse;
            }
        }
        else if(selectedHouse != null)
        {
            selectedHouse.GetComponent<Outline>().enabled = false;
            selectedHouse = null;
        }

        if (selectedHouse != null && Input.GetMouseButtonDown(0))
        {
            UIManager.Instance.OpenUIWindow(selectedHouse.GetComponent<InteractableHouse>().selfUIWindow);
        }
    }
}
