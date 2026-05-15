using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class BallMovement : MonoBehaviour
{
    [Header("移動設定")]
    [SerializeField] private float rollPower = 10f; // 転がす・押す強さ
    [SerializeField] private float maxSpeed = 8f;   // 最高速度（これ以上加速しない）

    [Header("ジャンプ設定")]
    [SerializeField] private float jumpForce = 5f;  // ジャンプする力
    [SerializeField] private LayerMask groundLayer; // 地面判定用のレイヤー

    private Rigidbody rb;
    private Vector2 moveInput;
    private bool jumpRequested = false;
    private SphereCollider sphereCollider;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        sphereCollider = GetComponent<SphereCollider>();

        // 初速を遅く、最高速度を安定させるために摩擦（空気抵抗）を少し与える
        rb.linearDamping = 0.5f;
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    // Input Systemの「OnJump」メッセージ（Spaceキーなど）を受け取る
    void OnJump(InputValue value)
    {
        if (value.isPressed && IsGrounded())
        {
            jumpRequested = true;
        }
    }

    void FixedUpdate()
    {
        // 入力方向のベクトル
        Vector3 inputDirection = new Vector3(moveInput.x, 0f, moveInput.y);

        // 1. 速度とブレーキの調整
        if (inputDirection.magnitude > 0)
        {
            // 現在の進行方向と、入力された方向がどれくらい一致しているか（内積）
            // 1 = 完全な同方向、 0 = 直角、 -1 = 完全な真後ろ（反対入力）
            Vector3 currentVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            float dotProduct = Vector3.Dot(currentVelocity.normalized, inputDirection.normalized);

            // 反対入力（dotProduct < 0）のときは、最高速度に関係なく力を加える（ブレーキを緩くする）
            // 同方向への加速のときだけ maxSpeed で制限する
            if (dotProduct < 0f || currentVelocity.magnitude < maxSpeed)
            {
                // トルクによる回転
                Vector3 torque = new Vector3(moveInput.y, 0f, -moveInput.x);
                rb.AddTorque(torque * rollPower);

                // フォースによる加速（ForceMode.Force に変更して質量による慣性を残す）
                rb.AddForce(inputDirection * rollPower, ForceMode.Force);
            }
        }

        // 2. ジャンプ処理
        if (jumpRequested)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpRequested = false;
        }
    }


    // 接地判定（球体の底面が地面に触れているか）
    private bool IsGrounded()
    {
        // 球体の中心から少し下に向けて短めの光線（レイ）を飛ばす
        float radius = sphereCollider.radius * transform.localScale.y;
        float rayLength = radius + 0.1f;

        return Physics.Raycast(transform.position, Vector3.down, rayLength, groundLayer);
    }
}
