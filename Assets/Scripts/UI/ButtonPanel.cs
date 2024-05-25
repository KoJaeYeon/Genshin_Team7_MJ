using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPanel : MonoBehaviour
{
    public PlayerInputHandler _input;
    public void Jump()
    {
        _input.JumpInput(true);
    }

    public void Dash(bool value)
    {
        _input.SprintInput(value);
    }

    public void Attack(bool value)
    {
        _input.AttackInput(value);
    }

    public void Skill(bool value)
    {
        _input.SkillInput(value);
    }

    public void Burst(bool value)
    {
        _input.BurstInput(value);
    }
}
