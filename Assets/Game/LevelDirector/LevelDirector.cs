using NaughtyAttributes;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class LevelDirector : MonoBehaviour
{
    public static int soulsGoal { get { return 5; } }
    public static int deliveredSoulsCounter {  get; private set; }
    public static int keysCounter { get; private set; }
    public static Transform lastPier { get; private set; }
    public static Transform firstPier { get; private set; }

    public static event Action OnSoulDeliveredEvent;
    public static event Action OnKeysValueChangedEvent;
    public static event Action<Transform> OnQuestTargetChangedEvent;
    public static event Action OnGameFinishedEvent;
    public static event Action OnRespawn;

    private static LevelDirector instance;
    private static Transform nextPier = null;

    private void Awake()
    {
        if (instance != null) Destroy(gameObject);
        instance = this;
        deliveredSoulsCounter = 0;
    }
    private void OnEnable()
    {
        Shop.OnItemPurchasedEvent += OnItemPurchased;
    }
    private void OnDisable()
    {
        Shop.OnItemPurchasedEvent -= OnItemPurchased;
    }
    private void Start()
    {
        SendNewQuestTarget(firstPier);
    }
    public static void Respawn()
    {
        OnRespawn?.Invoke();
    }
    public static void SetStartPier(Transform pier)
    {
        firstPier = pier;
        lastPier = firstPier;
    }
    public static void SendNewQuestTarget(Transform target)
    {
        if (nextPier != null) lastPier = nextPier;
        nextPier = target;
        OnQuestTargetChangedEvent?.Invoke(target);
    }
    [Button]
    public static void OnSoulDelivered()
    {
        deliveredSoulsCounter++;
        if (deliveredSoulsCounter >= soulsGoal)
        {
            FinishGame();
        }
        OnSoulDeliveredEvent?.Invoke();
    }
    private void OnItemPurchased(Item item)
    {
        if (item == Item.Key) GetKey();
    }
    [Button]
    private void GetKey()
    {
        keysCounter++;
        OnKeysValueChangedEvent?.Invoke();
    }
    [Button]
    public static void WasteKey()
    {
        if (!GotKey()) return;
        keysCounter--;
        OnKeysValueChangedEvent?.Invoke();
    }
    public static bool GotKey()
    {
        return keysCounter > 0;
    }
    private static void FinishGame()
    {
        OnGameFinishedEvent?.Invoke();
    }
    public static void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
