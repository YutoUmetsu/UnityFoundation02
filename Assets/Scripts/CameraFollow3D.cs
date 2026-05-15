using UnityEngine;

public class CameraFollow3D : MonoBehaviour
{
    [Header("追従対象")]
    [SerializeField] private Transform target;

    [Header("位置設定")]
    [SerializeField] private Vector3 offset = new Vector3(0f, 5f, -7f); // 球体との理想の距離・角度
    [SerializeField] private float smoothSpeed = 5f; // 通常時の追従のスムーズさ

    [Header("距離の制限（範囲）")]
    [SerializeField] private float minDistance = 5f; // これ以上近づけない
    [SerializeField] private float maxDistance = 9f; // これ以上離れない

    void LateUpdate()
    {
        if (target == null) return;

        // 1. 目標となる球体の基準座標（Yが0未満なら0で固定）
        float targetY = target.position.y < 0f ? 0f : target.position.y;
        Vector3 targetBasePos = new Vector3(target.position.x, targetY, target.position.z);

        // 2. 理想のカメラ位置
        Vector3 desiredPosition = targetBasePos + offset;

        // 3. 【通常時】理想の位置に向けて滑らかに移動
        Vector3 nextPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // 4. 【距離制限の判定】次の移動位置と、球体との直接の距離・方向を計算
        Vector3 directionFromTarget = nextPosition - targetBasePos;
        float distanceFromTarget = directionFromTarget.magnitude;
        Vector3 normalizedDirection = directionFromTarget.normalized;

        // 5. 限界値を超えていたら、座標を強制ロックする
        if (distanceFromTarget > maxDistance)
        {
            // 離れすぎ：最大距離の位置に
            nextPosition = targetBasePos + normalizedDirection * maxDistance;
        }
        else if (distanceFromTarget < minDistance)
        {
            // 近づきすぎ：最小距離の位置に
            nextPosition = targetBasePos + normalizedDirection * minDistance;
        }

        // 6. 確定した座標を適用
        transform.position = nextPosition;

        // 7. 常に球体の方を向く
        transform.LookAt(targetBasePos);
    }
}
