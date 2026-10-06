using UnityEngine;

public class CollisionTest : MonoBehaviour
{
    public float moveSpeed = 5f; // 이동 속도를 조절하는 변수 추가 (인스펙터에서 수정 가능)
    public float jumpForce = 5f;
    private Rigidbody rb;
    private bool isGrounded = true;

    void Start()
    {
        // 플레이어의 Rigidbody 컴포넌트를 가져옵니다.
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // 1. 이동 로직 개선 (상하좌우 모든 방향으로 이동)
        float v = Input.GetAxis("Vertical");   // W, S 또는 위, 아래 방향키
        float h = Input.GetAxis("Horizontal"); // A, D 또는 좌, 우 방향키

        // 입력받은 값으로 이동할 방향을 정하고 대각선 이동 시 속도 빨라짐 방지(normalized)
        Vector3 moveDirection = new Vector3(h, 0, v).normalized;

        // 정해진 방향으로 moveSpeed 만큼 곱해서 이동시킵니다.
        this.transform.position += moveDirection * moveSpeed * Time.deltaTime;

        // 2. 이동하는 방향으로 캐릭터가 고개를 돌리도록 설정 (총알을 쏘기 위함)
        if (moveDirection != Vector3.zero)
        {
            this.transform.forward = moveDirection;
        }

        // 3. 점프 로직
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && rb != null)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false; // 공중에 뜸
        }

        // 4. 총알 발사 로직
        if (Input.GetMouseButtonDown(0))
        {
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            go.transform.position = this.transform.position +
                                   this.transform.forward +
                                   this.transform.up * 0.3f;
            go.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            go.transform.rotation = this.transform.rotation;

            Rigidbody bulletRb = go.AddComponent<Rigidbody>();
            bulletRb.AddForce(go.transform.forward * 5, ForceMode.VelocityChange);

            Move bulletMove = go.AddComponent<Move>();
            bulletMove.Speed = 20f;

            go.AddComponent<Bullet>();
            Destroy(go, 3.0f);
        }
    }

    // 충돌 감지 처리 (점프 초기화 및 사망)
    void OnCollisionEnter(Collision collision)
    {
        // 바닥에 닿으면 다시 점프할 수 있도록 상태 초기화
        if (collision.gameObject.name == "Plane" || collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }

        // 적 오브젝트(EnemyMove 스크립트를 가진 대상)와 충돌 시 플레이어 파괴(사망)
        if (collision.gameObject.GetComponent<EnemyMove>() != null)
        {
            Debug.Log("적에게 공격당해 플레이어가 사망했습니다!");
            Destroy(gameObject); // 플레이어 삭제
        }
    }
}