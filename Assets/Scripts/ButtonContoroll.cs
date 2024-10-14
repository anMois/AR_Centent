using System.Text;
using TMPro;
using UnityEngine;

public class ButtonContoroll : MonoBehaviour
{
    public enum State { Idle, Idle2, Walk }
    [SerializeField] State curState = State.Idle;
    [SerializeField] Animator animator;
    [SerializeField] TextMeshProUGUI curStatusText;

    private StringBuilder sb = new StringBuilder();

    private void Start()
    {
        ChangeStatusText();
    }

    private void ChangeStatusText()
    {
        sb.Clear();
        sb.Append(curState.ToString());
        curStatusText.SetText(sb);
    }

    //Move 버튼
    public void OnMove()
    {
        if (curState == State.Walk)
            return;

        curState = State.Walk;
        ChangeStatusText();
        animator.SetTrigger("Walk");
    }

    //Idle 버튼
    public void OnIdle()
    {
        if (curState == State.Idle)
            return;

        curState= State.Idle;
        ChangeStatusText();
        animator.SetTrigger("Idle");
    }

    //OtherIdle 버튼
    public void OnOtherIdle()
    {
        if (curState == State.Idle2)
            return;

        curState= State.Idle2;
        ChangeStatusText();
        animator.SetTrigger("Other");
    }
}
