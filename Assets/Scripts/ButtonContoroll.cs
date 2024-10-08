using UnityEngine;

public class ButtonContoroll : MonoBehaviour
{
    public enum State { Idle, Move, Other }
    [SerializeField] State curState = State.Idle;
    [SerializeField] Animator animator;

    private void Start()
    {
        animator = transform.parent.GetComponent<Animator>();
    }

    //Move ��ư
    public void OnMove()
    {
        if (curState == State.Move)
            return;

        curState = State.Move;
        animator.SetBool("Walk", true);
    }

    //Idle ��ư
    public void OnIdle()
    {
        if (curState == State.Idle)
            return;
    }

    //OtherIdle ��ư
    public void OnOtherIdle()
    {
        if (curState == State.Other)
            return;
    }
}
