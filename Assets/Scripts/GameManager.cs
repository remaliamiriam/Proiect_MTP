using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int totalTokens = 0;
    private int collectedTokens = 0;

    public Text tokenUIText;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        totalTokens = GameObject.FindGameObjectsWithTag("Token").Length;
        UpdateUI();
    }

    public void CollectToken()
    {
        collectedTokens++;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (tokenUIText != null)
            tokenUIText.text = $"Tokens: {collectedTokens} / {totalTokens}";
    }

    public bool HasCollectedEnoughTokens()
    {
        return collectedTokens >= totalTokens;
    }
}
