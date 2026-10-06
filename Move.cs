using UnityEngine;

public class Move : MonoBehaviour
{
    // CollisionTest 스크립트(bulletMove.Speed)에서 값을 넣을 수 있도록 public 선언
    public float Speed = 0f;

    void Update()
    {
        // 총알이 바라보는 정면(Z축 방향)으로 설정된 Speed만큼 매 프레임 이동
        transform.position += transform.forward * Speed * Time.deltaTime;
    }
}