using UnityEngine;

public class ExampleClass : MonoBehaviour
{
    Rigidbody m_Rigidbody;
    public float power = 5.0f; // 점프대 점프력

    private void OnCollisionEnter(Collision collision)
    {
        // 충돌한 오브젝트에서 Rigidbody 가져오기
        Rigidbody rb = collision.collider.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // ForceMode.Impulse로 순간적인 점프력 적용
            rb.AddForce(Vector3.up * power, ForceMode.Impulse);
        }
    }
}

