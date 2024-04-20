using System;
using System.Threading.Tasks;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Random;
using VoicevoxBridge;

namespace VoicevoxBridge
{

    [System.Serializable]
    public class VoiceRandomCelect
    {
        public AudioClip voiceClip;
        public float volume = 1.0f;
        public float pitch = 1.0f;
    }

    public class VOICEVOX : MonoBehaviour
    {
        [SerializeField] AudioSource audioSource = null;
        [SerializeField] string voicevoxEngineURL = "http://localhost:50021/";
        [SerializeField] bool enableLog = false;
        [SerializeField] VOICEVOX voicevox;

        VoicevoxPlayer player = null;

        private static VOICEVOX instance;
        public static VOICEVOX Instance => instance;
        private int speaker = 2; //アナウンスの声を“四国めたん”に指定

        public AudioClip audioClip_0;

        //結果に応じたアナウンスからのコメント
        //Unity内で自由に追加、編集ができる。
        //リアルタイムで音声を生成するため少しラグあり。
        public string[] randomTexts_1;
        public string[] randomTexts_2;
        public string[] randomTexts_3;
        public string[] randomTexts_4;
        public string[] randomTexts_5;
        public string[] randomTexts_6;

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }

        void Awake()
        {
            voicevox = this;
            if (audioSource == null) 
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }

            player = new VoicevoxPlayer(voicevoxEngineURL, audioSource);
            player.EnableLog = enableLog;

            if (instance != null && instance != this)
            {
                Destroy(gameObject);
            }else{
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

//少なすぎ(0〜9回)ver
        public void ReceiveKeyCount_1(int keyCount)
        {
            _ = PlayVoice_1(keyCount); // PlayVoiceをTask.Runで実行　//PlayVoice_1に値を送信
        }
       //記録をVOICEVOXに喋ってもらう
        public async Task PlayVoice_1(int keyCount)
        {
            int randomIndex  = Range(0,randomTexts_1.Length);
            string selectedText = randomTexts_1[randomIndex];
            //textにキーボードの「４」が押された回数(記録)が代入される
            string text = "ただいまの記録、" + keyCount + "回。" + selectedText;
            await voicevox.PlayOneShot(speaker, text);
            StartCoroutine(ShutDown());
       }

//少なめ(10〜29回)ver
        public void ReceiveKeyCount_2(int keyCount)
        {
            _ = PlayVoice_2(keyCount); //PlayVoice_2に値を送信
        }
        public async Task PlayVoice_2(int keyCount)
        {
            int randomIndex  = Range(0,randomTexts_2.Length);
            string selectedText = randomTexts_2[randomIndex];
            string text = "ただいまの記録、" + keyCount + "回。" + selectedText;
            await voicevox.PlayOneShot(speaker, text);
            StartCoroutine(ShutDown());
        }

//まあまあ、平均値(30〜49回)ver
        public void ReceiveKeyCount_3(int keyCount)
        {
            _ = PlayVoice_3(keyCount); //PlayVoice_3に値を送信
        }
        public async Task PlayVoice_3(int keyCount)
        {
            int randomIndex = Range(0,randomTexts_3.Length);
            string selectedText = randomTexts_3[randomIndex];
            string text = "ただいまの記録、" + keyCount + "回。" + selectedText; 
            await voicevox.PlayOneShot(speaker, text);
            StartCoroutine(ShutDown());
        }

//まあまあすごい(50〜69回)ver
        public void ReceiveKeyCount_4(int keyCount)
        {
            _ = PlayVoice_4(keyCount); //PlayVoice_4に値を送信
        }
        public async Task PlayVoice_4(int keyCount)
        {
            int randomIndex = Range(0,randomTexts_4.Length);
            string selectedText = randomTexts_4[randomIndex];
            string text = "ただいまの記録、" + keyCount + "回。" + selectedText; 
            await voicevox.PlayOneShot(speaker, text);
            StartCoroutine(ShutDown());
        }

//すごい！(70〜99回)ver
        public void ReceiveKeyCount_5(int keyCount)
        {
            _ = PlayVoice_5(keyCount);　//PlayVoice_5に値を送信
        }
        public async Task PlayVoice_5(int keyCount)
        {
            int randomIndex = Range(0,randomTexts_5.Length);
            string selectedText =randomTexts_5[randomIndex];
            string text = "ただいまの記録、" + keyCount + "回。" + selectedText; 
            await voicevox.PlayOneShot(speaker, text);
            StartCoroutine(ShutDown());
        }

//不正疑惑(100回〜)ver
        public void ReceiveKeyCount_6(int keyCount)
        {
            _ = PlayVoice_6(keyCount); //PlayVoice_6に値を送信
        }
        public async Task PlayVoice_6(int keyCount)
        {
            int randomIndex = Range(0,randomTexts_6.Length);
            string selectedText =randomTexts_6[randomIndex];
            string text = "ただいまの記録、" + keyCount + "回。" + selectedText; 
            await voicevox.PlayOneShot(speaker, text);
            StartCoroutine(ShutDown());
        }

        private IEnumerator ShutDown()
        {
            yield return new WaitForSeconds(2f);
            PlaySound(audioClip_0);
        }

        private void PlaySound(AudioClip clip)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }

///////////////////////ここから下は基本いじらない。///////////////////////////////////

        public async Task<Voice> CreateVoice(int speaker, string text, CancellationToken cancellationToken = default)
        {
            return await player.CreateVoice(speaker, text, cancellationToken);
        }

        public async Task Play(Voice voice, CancellationToken cancellationToken = default, bool autoReleaseVoice = true)
        {
            await player.Play(voice, cancellationToken, autoReleaseVoice);
        }

        public async Task PlayOneShot(int speaker, string text, CancellationToken cancellationToken = default)
        {
            await player.PlayOneShot(speaker, text, cancellationToken);
        }

        void OnDestroy()
        {
            if (player != null) player.Dispose();
        }
    }
}
