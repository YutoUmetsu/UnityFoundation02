using UnityEngine;

public class FallRespawnManager : MonoBehaviour
{
    [Header("落下判定設定")]
    [SerializeField] private float thresholdY = -3f; // 復活を行うY座標の閾値
    float RespawnHigh = 1.5f;//リスポーン時の高さ

    private Rigidbody rb;
    private Vector3 startPosition;

    void Start()
    {
        // 物理演算コンポーネントを取得
        rb = GetComponent<Rigidbody>();

        // 1. ゲーム開始時の初期位置を記憶
        startPosition = transform.position;
    }

    void FixedUpdate()
    {
        // 2. 指定したY座標を下回ったら復活処理を呼ぶ
        if (transform.position.y < thresholdY)
        {
            Respawn();
        }
    }

    // 復活（再配置）処理
    private void Respawn()
    {
        // 3. 落下時の移動速度や回転の勢い（慣性）を完全にゼロにする
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        // 4. 初期位置のY座標に+0.5した位置を計算
        Vector3 spawnPosition = startPosition;
        spawnPosition.y +=RespawnHigh;

        // 5. プレイヤーを移動させる
        transform.position = spawnPosition;
    }
}
