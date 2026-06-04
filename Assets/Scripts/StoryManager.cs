using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

public class StoryManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image background;
    [SerializeField] private TextMeshProUGUI storyText;

    // 3人分の立ち絵を表示するImageコンポーネント
    [Header("Character Images")]
    [SerializeField] private Image characterImage1;
    [SerializeField] private Image characterImage2;
    [SerializeField] private Image characterImage3;

    [Header("Systems & Data")]
    [SerializeField] private CharacterRegistrySO characterRegistry;

    private readonly CsvInput csvInput = new();
    private Sprite[] backgroundSprites;
    public int currentID { get; private set; } = 1;
    public System.Action OnStoryEnd;

    public void LoadStory(StoryDataSO storyData)
    {
        if (storyData == null)            { Debug.LogError("[StoryManager] LoadStory: storyData が null です。GameFlowManager の Stories リストを確認してください。"); return; }
        if (storyText == null)            { Debug.LogError("[StoryManager] storyText が未設定です。Inspector を確認してください。"); return; }
        if (background == null)           { Debug.LogError("[StoryManager] background が未設定です。Inspector を確認してください。"); return; }

        backgroundSprites = storyData.backgroundSprites;
        csvInput.LoadStory(storyData);
        currentID = 1;
        SetStoryElement(currentID);
    }

    private void SetStoryElement(int id)
    {
        var dialogue = csvInput.GetDialogue(id);
        if (dialogue == null)
        {
            Debug.LogError($"[StoryManager] id={id} の DialogueData が見つかりません。CSV の内容を確認してください。");
            return;
        }

        storyText.text = dialogue.text;

        if (dialogue.bgImageNum >= 0 && dialogue.bgImageNum < backgroundSprites.Length)
        {
            background.sprite = backgroundSprites[dialogue.bgImageNum];
        }
        else
        {
            Debug.LogWarning($"[StoryManager] bgImageNum={dialogue.bgImageNum} が backgroundSprites の範囲外です。");
        }

        UpdateCharacterSlot(dialogue.character1, dialogue.emotion1, characterImage1);
        UpdateCharacterSlot(dialogue.character2, dialogue.emotion2, characterImage2);
        UpdateCharacterSlot(dialogue.character3, dialogue.emotion3, characterImage3);

        currentID = id;
    }

    private void UpdateCharacterSlot(string charName, EmotionType emotion, Image targetImage)
    {
        if (targetImage == null) { Debug.LogWarning("[CS] targetImage が null"); return; }

        if (charName != null && charName != "empty")
        {
            CharacterData characterData = characterRegistry != null ? characterRegistry.Find(charName) : null;
            Debug.Log($"[CS] charName={charName} / characterData={characterData} / registry={characterRegistry}");

            if (characterData != null)
            {
                Sprite faceSprite = characterData.GetFace(emotion);
                Debug.Log($"[CS] faceSprite={faceSprite}");
                if (faceSprite != null)
                {
                    targetImage.gameObject.SetActive(true);
                    targetImage.sprite = faceSprite;
                    targetImage.preserveAspect = true;
                    return;
                }
            }
        }

        targetImage.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            OnClickNextButton();
        }
    }

    public void OnClickNextButton()
    {
        var dialogue = csvInput.GetDialogue(currentID);
        if (dialogue == null) return;

        if (dialogue.nextID == -1)
        {
            OnStoryEnd?.Invoke();
        }
        else
        {
            SetStoryElement(dialogue.nextID);
        }
    }
}