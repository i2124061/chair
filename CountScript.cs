/*
3つのスクリプトに関して

（CountScript.CS = ゲームスクリプト）
→キー２入力を確認後、開始前アナウンスから最終的なキー４入力回数の計測までを行います。

（MainControl.CS = メインスクリプト）
→ゲームスクリプトから取得した値を段階で振り分けて、VOICEVOX.CSに送ります。

(VOICEVOX.CS = おしゃべりスクリプト)
→結果発表アナウンスを喋らせるためのスクリプトです。
結果発表はリアルタイムで音声を生成するので、VOICEVOXを起動させた状態出ないと音声が再生されません。

現状のスクリプトだとCountScript.CS → MainControl.CS → VOICEVOX.CSの順に動きます。
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoicevoxBridge; //VOICEVOXクラスの名前空間をインポート

[System.Serializable]
public class ChairSoundEffects
{
    public AudioClip soundClip;
    public float volume = 1.0f;
    public float pitch = 1.0f;
}

public class CountScript : MonoBehaviour
{

    private AudioSource audioSource;

    private int keyCount;
    //AudioClipをInspectorウィンドウで登録出来るように設定
    public AudioClip soundClip_0;
    public AudioClip soundClip_1_1;
    public AudioClip soundClip_1_2;
    public AudioClip soundClip_2_1;
    public AudioClip soundClip_2_2;
    public AudioClip soundClip_2_3;
    public AudioClip soundClip_2_4;
    public AudioClip soundClip_2_5;
    public AudioClip soundClip_2_6;
    public AudioClip soundClip_3;
    public AudioClip soundClip_4;
    public AudioClip soundClip_5;
    public AudioClip soundClip_6;
    public AudioClip soundClip_7;
    public AudioClip soundClip_8;
    public AudioClip soundClip_9;
    public ChairSoundEffects[] chairSoundEffects; //インタラクションの効果音の配列

    public AudioClip soundEffect; ////残り３秒カウントダウン
    private float interval = 1.0f;

    private bool isStart_01 = false; //繰り返し２が押されてもアナウンスが被らないようにするため
    private bool isCounting = false; //カウントダウンが始まるまでキー４入力が反映されないようにするため
    private bool isPaused = false; //一時停止中のフラグ
    private bool isCoolingDown = false; //クールダウン中のフラグ
    private float cooldownEndTime; //クールダウンの終了時刻
    private float timer_2 = 0.0f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        //クールダウン中でないかつ「2」キーが押されたとき
        if (!isCoolingDown && Input.GetKeyDown(KeyCode.Alpha2) && !isStart_01)
        {
            isStart_01 = true;
            StartCoroutine(PlaySecondAndWait());
        }
    }

    private IEnumerator PlaySecondAndWait() //開始アナウンス
    {
        PlaySound(soundClip_0); //起動音
        yield return new WaitForSeconds(soundClip_0.length);
        PlaySound(soundClip_1_1);//「ただいまより、インタラクティブアート研究室主催」
        StartCoroutine(PlaySecondSound());
    }

    private IEnumerator PlaySecondSound()
    {
        yield return new WaitForSeconds(soundClip_1_1.length + 0.6f);
        PlaySound(soundClip_1_2); //第一回椅子遊戯大会　着座　団体の部を開催いたします。
        yield return new WaitForSeconds(soundClip_1_2.length + 1f);
        PlaySound(soundClip_8); //本大会では30秒間で何回立ったり座ったりできるか、計測しています。
        yield return new WaitForSeconds(6f);
        PlaySound(soundClip_2_1);//「まさに今、椅子に座ったあなた！」
        yield return new WaitForSeconds(2.5f);
        PlaySound(soundClip_2_2);//「位置につきましたか。」
        yield return new WaitForSeconds(2f);
        PlaySound(soundClip_2_3);//「始めちゃいますよ。」
        yield return new WaitForSeconds(2f);
        PlaySound(soundClip_2_4);//「よーーーい」
        yield return new WaitForSeconds(3f);
        PlaySound(soundClip_2_5);//「スタート！」
        yield return new WaitForSeconds(soundClip_2_5.length);
        PlaySound(soundClip_2_6);//開始の合図
        yield return new WaitForSeconds(soundClip_2_6.length - 0.5f);
        StartCoroutine(GetKeyCount());
    }

    private IEnumerator GetKeyCount()
    {

        isCounting = true;
        keyCount = 0;
        float startTime = Time.time;
        float timer = 0f;

        while (timer <= 30f && !isPaused)
        {
            timer = Time.time - startTime;

            timer_2 += Time.deltaTime;

            if (timer >= 27f && timer_2 >= interval)
            {
                PlaySoundEffect();
                timer_2 = 0.0f;// タイマーをリセット
            }

            if (Input.GetKeyDown(KeyCode.Alpha4) && isCounting)
            {
                keyCount++;
                PlayRandomChairSoundEffects(); //座るたびに３種類の効果音がランダムで鳴る
            }
            yield return null;

            //カウント終了後、待機時間のアナウンス
            if (timer >= 30f && timer <= 32f && isStart_01)//カウント終了後、待機時間のアナウンス
            {
                PlaySound(soundClip_9);
                yield return new WaitForSeconds(soundClip_9.length);
                PlaySound(soundClip_7);//「集計中。しばらくお待ちくださいね。」
                yield return new WaitForSeconds(soundClip_7.length);
            }
        }


        //MainControl.CSに値を送信
        MainControl.Instance.ReceiveKeyCount(keyCount);

        isPaused = true; //カウント終了後に一旦一切のキー入力を受け付けないようにする
        cooldownEndTime = Time.time + 25f; //クールダウン終了時刻を設定
        isCoolingDown = true; //クールダウン中のフラグを設定
        yield return new WaitForSeconds(25f); // 待機

        ResetGame();
    }

    private void PlaySound(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    private void PlaySoundEffect()//残り３秒カウントダウン
    {
        if (soundEffect != null)
        {
            audioSource.PlayOneShot(soundEffect);
        }
    }

    private void PlayRandomChairSoundEffects() //椅子に座るたびに鳴る効果音。追加も可能。
    {
        if (chairSoundEffects.Length == 0)
        {
            Debug.LogWarning("No sound effect settings available.");
            return;
        }

        int randomIndex = UnityEngine.Random.Range(0, chairSoundEffects.Length);
        ChairSoundEffects selectedSettings = chairSoundEffects[randomIndex];

        audioSource.clip = selectedSettings.soundClip;
        audioSource.volume = selectedSettings.volume;
        audioSource.pitch = selectedSettings.pitch;
        audioSource.Play();
    }

    private void ResetGame()//初期に戻す処理をここに記述
    {
        isStart_01 = false;
        isCounting = false;
        isPaused = false;
        isCoolingDown = false;
        cooldownEndTime = 0.0f;
        timer_2 = 0.0f;
        keyCount = 0;
    }

}
