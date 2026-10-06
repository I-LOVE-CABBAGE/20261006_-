using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float jumpForce = 5f;
    public Collider groundCollider;

    private Rigidbody rb;
    private float jumpTimer;
    private Transform playerTarget;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // 1. �հ� �������� ���� ����: ���� �浹 ������ '������(Continuous)'���� ����
        if (rb != null)
        {
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        }

        GameObject player = GameObject.Find("Capsule");
        if (player != null)
        {
            playerTarget = player.transform;
        }

        jumpTimer = Random.Range(1f, 3f);
    }

    // 2. ���� ����(�̵� ��)�� Update�� �ƴ� FixedUpdate���� ó���ϴ� ���� �������Դϴ�.
    void FixedUpdate()
    {
        MoveTowardsPlayer();
        ClampToGround();
    }

    void Update()
    {
        HandleJump();
    }

    void MoveTowardsPlayer()
    {
        if (playerTarget != null && rb != null)
        {
            // �÷��̾ ���ϴ� ���� ���
            Vector3 direction = (playerTarget.position - transform.position).normalized;

            // 3. transform.Translate ��� Rigidbody�� �ӵ�(velocity)�� �����Ͽ� �о�� �̵�
            // �̷��� �ϸ� ���鳢�� �ε����� �� �İ����� �ʰ� �ڿ������� �̲������� ���Ѱ��ϴ�.
            rb.linearVelocity = new Vector3(direction.x * moveSpeed, rb.linearVelocity.y, direction.z * moveSpeed);
        }
    }

    void HandleJump()
    {
        jumpTimer -= Time.deltaTime;
        if (jumpTimer <= 0)
        {
            if (Random.value < 0.3f && rb != null)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
            jumpTimer = Random.Range(1f, 3f);
        }
    }

    void ClampToGround()
    {
        if (groundCollider != null)
        {
            Bounds bounds = groundCollider.bounds;
            float clampedX = Mathf.Clamp(transform.position.x, bounds.min.x + 0.5f, bounds.max.x - 0.5f);
            float clampedZ = Mathf.Clamp(transform.position.z, bounds.min.z + 0.5f, bounds.max.z - 0.5f);

            // 4. �߶� ���� ������ġ: ���� ���� ������ �ٴ�(Plane) �Ʒ��� �������ٸ� ���� �ٽ� ����ø�
            float clampedY = transform.position.y;
            if (clampedY < bounds.center.y - 1f)
            {
                clampedY = bounds.max.y + 1f; // �ٴ� ���� ��ġ ������
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z); // �������� ���ӵ� �ʱ�ȭ
            }

            // ��ġ�� ���� ������ ����� ���� ���� ����
            if (transform.position.x != clampedX || transform.position.z != clampedZ || transform.position.y != clampedY)
            {
                transform.position = new Vector3(clampedX, clampedY, clampedZ);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Capsule")
        {
            Debug.Log("�÷��̾ �����߽��ϴ�!");
        }
    }
}