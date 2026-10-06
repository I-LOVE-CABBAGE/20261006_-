using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private float etime;

    // 유니티 인스펙터 창에서 직접 만든 Plane을 여기에 드래그해서 넣으세요!
    public Collider groundCollider;

    void Update()
    {
        etime += Time.deltaTime;

        if (etime >= 3)
        {
            int randomIndex = Random.Range(0, this.transform.childCount);
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cylinder);

            // 1. 원기둥이 바닥에 파묻히지 않도록 스폰 위치의 Y축을 1만큼 위로 올려줍니다.
            Vector3 spawnPos = this.transform.GetChild(randomIndex).position;
            go.transform.position = new Vector3(spawnPos.x, spawnPos.y + 1f, spawnPos.z);

            Rigidbody rb = go.AddComponent<Rigidbody>();

            // 2. 원기둥이 쓰러져서 바닥으로 굴러떨어지지 않도록 X, Z축 회전을 고정합니다.
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

            EnemyMove moveScript = go.AddComponent<EnemyMove>();
            // 3. 직접 설정한 Plane의 범위를 적(EnemyMove)에게 전달해 줍니다.
            moveScript.groundCollider = this.groundCollider;

            etime = 0;
        }
    }
}