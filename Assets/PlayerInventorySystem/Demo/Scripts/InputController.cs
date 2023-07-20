using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{



    // holders for ration value values used for orbit cam calculation
    [HideInInspector]
    public float XRotation = 0;
    [HideInInspector]
    public float YRotation = 0;

    private bool aiming = false;

    public void OnEscape(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {

            GameController.instance.OnPause();
        }
    }


    public void OnForward(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            PlayerController.instance.OnForward(1);
        }
        else
        {
            PlayerController.instance.OnForward(0);
        }
    }

    public void OnBack(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            PlayerController.instance.OnBack(-1);
        }
        else
        {
            PlayerController.instance.OnBack(0);
        }
    }

    public void OnLeft(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            PlayerController.instance.OnLeft(-1);
        }
        else
        {

            PlayerController.instance.OnLeft(0);
        }
    }

    public void OnRight(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            PlayerController.instance.OnRight(1);
        }
        else
        {
            PlayerController.instance.OnRight(0);
        }
    }

    public void OnLeftBumper(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            PlayerInventorySystem.InventoryController.Instance.ItemBar.SelectPreviousSlot();
        }
    }

    public void OnRightBumper(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            PlayerInventorySystem.InventoryController.Instance.ItemBar.SelectNextSlot();
        }
    }

    public void OnInteract(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            // if aiming
            if (aiming)
            {

                // Place the selected item from the item bar into the world
                Debug.Log("Place or consume Item");



                // call to method in PlayerController
                PlayerController.instance.OnInteract();

            }
            else
            {
                // use the equiped item
                Debug.Log("Use Item");
            }

        }
    }

    public void OnAim(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            PlayerController.instance.OnAim(true);
            aiming = true;
        }
        else if (ctx.canceled)
        {
            PlayerController.instance.OnAim(false);
            aiming = false;
        }
    }

    public void OnAimSwitch(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            PlayerController.instance.OnAimSwitch();
        }
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            //  PlayerController.instance.OnJump();
        }
    }

    public void OnInventoryPanel(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            PlayerInventorySystem.InventoryController.Instance.ToggleInventoryPanel();
        }
    }

    public void OnPressDrop(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            PlayerInventorySystem.InventoryController.Instance.ItemBar.DropSelectedItem();
        }
    }

    public void OnRotationX(InputAction.CallbackContext ctx)
    {
        XRotation = ctx.ReadValue<float>();
    }

    public void OnRotationY(InputAction.CallbackContext ctx)
    {
        YRotation = ctx.ReadValue<float>();
    }

    public void OnNumberSelect(InputAction.CallbackContext ctx)
    {
        Debug.Log("t => " + ctx.action.activeControl.ToString());
        if (ctx.performed)
        {
            string s = ctx.action.activeControl.ToString();
            // get the number from the string
            int slotNum = int.Parse(s.Substring(s.Length - 1));
            if (slotNum == 0)
            {
                slotNum = 9;
            }
            else { slotNum--; }

            PlayerInventorySystem.InventoryController.Instance.ItemBar.SelectedSlotID = slotNum;
        }
    }
}
