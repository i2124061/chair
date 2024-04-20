using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoicevoxBridge; // VOICEVOXクラスの名前空間をインポート

public class MainControl : MonoBehaviour
{
    private static MainControl instance;
    public static MainControl Instance => instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    //CountScript.CSから送られてきた値を振り分ける
    //VOICEVOX.CSに値を送信
    public void ReceiveKeyCount(int keyCount)
    {
        //0以上、10未満の場合（少なすぎ）ver
        if (keyCount >= 0 && keyCount < 10)
        {
            Debug.Log("スクリプト1から受け取った値 :" + keyCount);
            Debug.Log("少なすぎver");
            VOICEVOX.Instance.ReceiveKeyCount_1(keyCount);
        }

        //10以上、30未満の場合（少なめ）ver
        if (keyCount >= 10 && keyCount < 30)
        {
            Debug.Log("スクリプト1から受け取った値 :" + keyCount);
            Debug.Log("少なめver");
            VOICEVOX.Instance.ReceiveKeyCount_2(keyCount);
        }

        //30以上、50未満の場合（まあまあ、平均値）ver
        if (keyCount >= 30 && keyCount < 50)
        {
            Debug.Log("スクリプト1から受け取った値 :" + keyCount);
            Debug.Log("まあまあ、平均値ver");
            VOICEVOX.Instance.ReceiveKeyCount_3(keyCount);
        }

        //50以上、70未満の場合（まあまあすごい）ver
        if (keyCount >= 50 && keyCount < 70)
        {
            Debug.Log("スクリプト1から受け取った値 :" + keyCount);
            Debug.Log("まあまあすごいver");
            VOICEVOX.Instance.ReceiveKeyCount_4(keyCount);
        }

        //70以上、100未満の場合（すごい！）ver
        if (keyCount >= 70 && keyCount < 100)
        {
            Debug.Log("スクリプト1から受け取った値 :" + keyCount);
            Debug.Log("すごいver");
            VOICEVOX.Instance.ReceiveKeyCount_5(keyCount);
        }

        //100以上（不正疑惑）
        if (keyCount >= 100)
        {
            Debug.Log("スクリプト1から受け取った値 :" + keyCount);
            Debug.Log("不正疑惑ver");
            VOICEVOX.Instance.ReceiveKeyCount_6(keyCount);
        }
    }
}
