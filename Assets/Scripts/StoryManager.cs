using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class StoryManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image background;
    [SerializeField] private TextMeshProUGUI storyText;
    [SerializeField] private TextMeshProUGUI characterNameDisplay; // 喋っている人の名前を出すUI

    // 3人分の立ち絵を表示するImageコンポーネント
    [Header("Character Images")]
    [SerializeField] private Image characterImage1;
    [SerializeField] private Image characterImage2;
    [SerializeField] private Image characterImage3;

    [Header("Systems & Data")]
    [SerializeField] private CsvInput csvInput;
    [SerializeField] private List<CharacterData> characterDataList;
    [SerializeField] private Sprite[] backgroundSprites;

    public int currentID { get; private set; } = 1;

    private void Start()
    {
        csvInput.StartLoadCSV(0);
        SetStoryElement(currentID);
    }

    private void SetStoryElement(int id)
    {
        var dialogue = csvInput.GetDialogue(id);
        if (dialogue == null) return;

        // 1. セリフテキストの反映
        storyText.text = dialogue.text;

        // 2. 背景画像の反映
        if (dialogue.bgImageNum >= 0 && dialogue.bgImageNum < backgroundSprites.Length)
        {
            background.sprite = backgroundSprites[dialogue.bgImageNum];
        }

        // 3. 3人のキャラクターの立ち絵と表情をそれぞれ更新
        UpdateCharacterSlot(dialogue.character1, dialogue.emotion1, characterImage1);
        UpdateCharacterSlot(dialogue.character2, dialogue.emotion2, characterImage2);
        UpdateCharacterSlot(dialogue.character3, dialogue.emotion3, characterImage3);

        // 4. 今だれが喋っているかを名前に表示（ここでは仮に1人目の名前を出しているよ）
        // もし「ナレーション」や「誰もいない」なら空文字にする
        if (dialogue.character1 != "empty")
        {
            characterNameDisplay.text = dialogue.character1;
        }
        else
        {
            characterNameDisplay.text = "";
        }

        currentID = id;
    }

    // 特定のスロット（Image）に対して、キャラ名と表情からSpriteを探して適用する共通処理
    private void UpdateCharacterSlot(string charName, EmotionType emotion, Image targetImage)
    {
        if (charName != "empty")
        {
            CharacterData characterData = characterDataList.Find(c => c.characterName == charName);

            if (characterData != null)
            {
                Sprite faceSprite = characterData.GetFace(emotion);
                if (faceSprite != null)
                {
                    targetImage.gameObject.SetActive(true);
                    targetImage.sprite = faceSprite;
                    return;
                }
            }
        }

        // キャラ名が empty、または画像が見つからない場合はスロットを非表示にする
        targetImage.gameObject.SetActive(false);
    }

    public void OnClickNextButton()
    {
        var dialogue = csvInput.GetDialogue(currentID);
        if (dialogue != null && dialogue.nextID != 0)
        {
            SetStoryElement(dialogue.nextID);
        }
    }
}