using System.Collections;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PhaseManager : MonoBehaviourPunCallbacks
{
    public static PhaseManager instance;

    public TextMeshProUGUI timerText;
    public TextMeshProUGUI currentPhaseText;
    public GameObject[] phase1EnableObjects;
    public GameObject[] phase2EnableObjects;
    public GameObject[] phase3EnableObjects;
    public GameObject[] phase3DisableObjects;
    public GameObject[] phase4DisableObjects;

    private float phaseDuration = 150f;
    private float timer;

    public int currentPhase = 1;

    private void Start()
    {
        instance = this;

        // Eğer MasterClient'sen timer başlatılır
        if (PhotonNetwork.IsMasterClient)
        {
            timer = phaseDuration;
            StartCoroutine(PhaseTimer());
        }
    }

    private void Update()
    {
        // Diğer oyuncular sadece timer ve phase UI'ını günceller
        UpdateTimerUI();
    }

    private void UpdateTimerUI()
    {
        // Timer ve phase bilgilerini UI'a yazdır
        int displaySeconds = Mathf.CeilToInt(timer % 60);
        int displayMinutes = Mathf.FloorToInt(timer / 60);
        timerText.text = $"Next Phase in: {displayMinutes:00}:{displaySeconds:00}";
        currentPhaseText.text = "Phase: " + currentPhase;
    }

    private IEnumerator PhaseTimer()
    {
        while (true)
        {
            yield return null;  // Frame bazlı kontrol
            if (!PhotonNetwork.IsMasterClient) yield break;  // Eğer MasterClient değilse coroutine durur

            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                timer = 0;
                ChangePhase();  // Phase değişir
                timer = phaseDuration;  // Timer tekrar başlar
            }

            // Faz ve timer'ı diğer oyunculara senkronize et
            photonView.RPC("SyncPhaseAndTimer", RpcTarget.All, timer, currentPhase);
        }
    }

    private void ChangePhase()
    {
        currentPhase++;
        if (currentPhase > 4)
        {
            currentPhase = 4;  // Faz sınırı 4
        }
        SkillManager.instance.UpdateShopUI();
        UpdatePhase();
    }

    private void UpdatePhase()
    {
        switch (currentPhase)
        {
            case 1:
                ActivatePhase1();
                break;
            case 2:
                ActivatePhase2();
                break;
            case 3:
                ActivatePhase3();
                break;
            case 4:
                ActivatePhase4();
                break;
        }
    }

    [PunRPC]
    public void SyncPhaseAndTimer(float syncedTimer, int syncedPhase)
    {
        // Diğer oyuncular timer ve phase'ı senkronize eder
        timer = syncedTimer;
        currentPhase = syncedPhase;
        UpdatePhase();
    }

    private void ActivatePhase1()
    {
        foreach (GameObject gameObject in phase1EnableObjects)
        {
            gameObject.SetActive(true);
        }
    }

    private void ActivatePhase2()
    {
        foreach (GameObject gameObject in phase2EnableObjects)
        {
            gameObject.SetActive(true);
        }
    }

    private void ActivatePhase3()
    {
        foreach (GameObject gameObject in phase3DisableObjects)
        {
            gameObject.SetActive(false);
        }

        foreach (GameObject gameObject in phase3EnableObjects)
        {
            gameObject.SetActive(true);
        }
    }

    private void ActivatePhase4()
    {
        foreach (GameObject gameObject in phase4DisableObjects)
        {
            Destroy(gameObject);
        }
    }

    // Eğer master client çıkarsa timer duracak ve yeni master devralacak
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        Debug.Log("Yeni MasterClient: " + newMasterClient.NickName);
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("SyncPhaseAndTimer",RpcTarget.AllBuffered, timer, currentPhase);
            StartCoroutine(PhaseTimer());  // Yeni MasterClient timer'ı başlatır
        }
    }
}