using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement3D : MonoBehaviour
{
    [Header("移動設定")]
    [SerializeField] private float speed = 5f;

    private Vector2 moveInput;

    // Input Systemのメッセージ「OnMove」を受け取る
    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void Update()
    {
        // 2D入力を3D空間の平面移動（X軸とZ軸）に変換
        Vector3 move = new Vector3(moveInput.x, 0f, moveInput.y);

        // 世界の座標軸（Space.World）を基準に移動
        transform.Translate(move * speed * Time.deltaTime, Space.World);
    }
}
