using UnityEngine;

public class RotateControll : MonoBehaviour
{
    [SerializeField] float speed;

    public void LeftRotate()
    {
        Debug.Log("왼쪽으로 턴");
    }

    public void RightRotate()
    {
        Debug.Log("오른쪽으로 턴");
    }

    public void ReSetRotate()
    {
        Debug.Log("정위치");
    }
}
