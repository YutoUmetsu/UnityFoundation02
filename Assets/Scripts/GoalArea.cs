using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalArea : MonoBehaviour
{
    [Header("遷移先設定")]
    [SerializeField] private string nextSceneName = "NextStage"; // 次のステージのシーン名

    [Header("演出設定")]
    [SerializeField] private ParticleSystem clearEffect; // クリア時のエフェクト（任意）

    private bool isCleared = false;

    // Is Triggerにチェックが入ったコライダーに何かが侵入したときに呼ばれる
    private void OnTriggerEnter(Collider other)
    {
        // すでにクリア済みの場合は二重処理を防ぐ
        if (isCleared) return;

        // 触れたオブジェクトが「Player」というタグを持っているか判定
        if (other.CompareTag("Player"))
        {
            isCleared = true;
            Debug.Log("ゴール！クリアしました！");

            // エフェクトが設定されていれば再生
            if (clearEffect != null)
            {
                clearEffect.Play();
            }

            // 次のシーンへ遷移
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
