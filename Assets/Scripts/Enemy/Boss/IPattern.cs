using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPattern
{
    public void InitializePattern(Andrius andrius);
    public void UpdatePattern();
    public void ExitPattern();
}
