using UnityEngine;

public class Behavior : StateMachineBehaviour
{

    [SerializeField] private float _timeUntillBored;
    [SerializeField] private int _numberOfBoredAnimations;

    
    private bool _isBored;
    private float _idleTime;
    private int _boredAnimation;
    
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
     ResetIdle();
    }
    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_isBored == false)
        {
            _idleTime += Time.deltaTime;
            if (_idleTime > _timeUntillBored && stateInfo.normalizedTime % 1 < 0.02f)
            {
                _isBored = true;
                _boredAnimation = Random.Range(1, _numberOfBoredAnimations + 1);
                
                
            }
        }
        else if (stateInfo.normalizedTime % 1 > 0.98)
        {
            ResetIdle();
        }
        
        animator.SetFloat("BoredAnimation", _boredAnimation, 0.2f, Time.deltaTime);
    }

    private void ResetIdle()
    {
        _idleTime = 0;
        _isBored = false;
        _boredAnimation = 0;
    }
}
