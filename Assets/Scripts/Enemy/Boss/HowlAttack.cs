using UnityEngine;

public class HowlAttack : IPattern
{
    private enum SelectHowl
    {
        howl,
        iceRain
    }

    private Andrius _andrius;
    private Animator _animator;
    
    public void InitializePattern(Andrius andrius)
    {
        if(_andrius == null)
        {
            _andrius = andrius;
            _animator = _andrius.GetComponent<Animator>();
        }

        RandomHowl();
    }

    public void UpdatePattern() { }

    public void RandomHowl()
    {
        int random = Random.Range(0, 2);

        switch (random)
        {
            case (int)SelectHowl.howl:
                _animator.SetTrigger("Howl");
                break;
            case (int)SelectHowl.iceRain:
                _animator.SetTrigger("IceRain");
                break;
        }
    }

    public void ExitPattern() { }
}
