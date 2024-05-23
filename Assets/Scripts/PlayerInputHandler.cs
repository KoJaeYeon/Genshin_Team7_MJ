using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;
#endif

public class PlayerInputHandler : MonoBehaviour
{
    [Space(10)]
    [Header("Player Input Values")]
    public Vector2 move;
    public Vector2 look;
    public float zoom;
    public bool attack;
    public bool jump;
    public bool sprint;
    public bool windfield;
    public bool aim;
    public bool skill;
    public bool burst;

    [Header("Movement Settings")]
    public bool analogMovement;

    [Header("Mouse Cursor Settings")]
    public bool cursorLocked = true;
    public bool cursorInputForLook = true;

#if ENABLE_INPUT_SYSTEM
    public void OnMove(InputValue value)
    {
        MoveInput(value.Get<Vector2>());
    }

    public void OnLook(InputValue value)
    {
        if (cursorInputForLook)
        {
            LookInput(value.Get<Vector2>());
        }
    }

    public void OnJump(InputValue value)
    {
        JumpInput(value.isPressed);
    }

    public void OnSprint(InputValue value)
    {
        SprintInput(value.isPressed);
        
    }

    public void OnAttack(InputValue value)
    {
        AttackInput(value.isPressed);
    }

    public void OnZoom(InputValue value)
    {
        if (cursorInputForLook)
        {
            ZoomInput(value.Get<float>());
        }
    }

    public void OnCursor(InputValue value)
    {
        CursorInput(value.isPressed);
    }

    public void OnAim(InputValue value)
    {
        AimInput(value.isPressed);
    }

    public void OnSkill(InputValue value)
    {
        SkillInput(value.isPressed);
    }

    public void OnBurst(InputValue value)
    {
        BurstInput(value.isPressed);
    }

    
#endif

    public void MoveInput(Vector2 newMoveDirection)
    {
        move = newMoveDirection;
    }

    public void LookInput(Vector2 newLookDirection)
    {
        look = newLookDirection;
    }

    public void JumpInput(bool newJumpState)
    {
        jump = newJumpState;
    }

    public void SprintInput(bool newSprintState)
    {
        sprint = newSprintState;
    }

    public void AttackInput(bool newAttackState)
    {
        attack = newAttackState;
    }

    public void ZoomInput(float newZoomValue)
    {
        zoom += newZoomValue;
    }

    public void CursorInput(bool cursorValue)
    {
        cursorLocked = cursorValue;
        cursorInputForLook = !cursorValue;
        if (cursorValue)
        {
            Cursor.lockState = CursorLockMode.None;
            look = Vector2.zero;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void AimInput(bool newAimState)
    {
        aim = newAimState;
    }

    public void SkillInput(bool newSkillState)
    {
        skill = newSkillState;
    }

    public void BurstInput(bool newBurstState)
    {
        burst = newBurstState;
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        SetCursorState(cursorLocked);
    }

    private void SetCursorState(bool newState)
    {
        Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
    }
}
