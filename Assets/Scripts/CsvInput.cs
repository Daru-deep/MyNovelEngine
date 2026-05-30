using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

[System.Serializable]
public class DialogueData
{
    public int id;
    
    // 3人分の「名前」と「表情」の変数を用意！
    public string character1;
    public EmotionType emotion1;

    public string character2;
    public EmotionType emotion2;

    public string character3;
    public EmotionType emotion3;
    
    public string text;
    public int bgImageNum;
    public int bgmNum;
    public int seNum;
    public int nextID;
}

public class CsvInput
{
    private List<DialogueData> dialogues = new List<DialogueData>();

    public void LoadStory(StoryDataSO storyData)
    {
        if (storyData != null && storyData.csvFile != null)
        {
            LoadCSV(storyData.csvFile);
        }
    }

    void LoadCSV(TextAsset csvFile)
    {
        dialogues.Clear();
        string[] lines = csvFile.text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
 
        for (int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i])) continue;
 
            string[] values = lines[i].Split(',');
 
            // 列数が足りない行はスキップ（今回は12列必要だよ）
            if (values.Length < 12) continue;

            DialogueData data = new DialogueData();

            if (!int.TryParse(values[0].Trim(), out data.id)) continue;

            // 1人目の名前と表情
            data.character1 = !string.IsNullOrEmpty(values[1]) ? values[1] : "empty";
            data.emotion1 = ParseEmotion(values[2]);

            // 2人目の名前と表情
            data.character2 = !string.IsNullOrEmpty(values[3]) ? values[3] : "empty";
            data.emotion2 = ParseEmotion(values[4]);

            // 3人目の名前と表情
            data.character3 = !string.IsNullOrEmpty(values[5]) ? values[5] : "empty";
            data.emotion3 = ParseEmotion(values[6]);

            // 残りのデータ（列がずれたので番号を修正）
            data.text = values[7];
            int.TryParse(values[8].Trim(), out data.bgImageNum);
            int.TryParse(values[9].Trim(), out data.bgmNum);
            int.TryParse(values[10].Trim(), out data.seNum);
            int.TryParse(values[11].Trim(), out var nextID);
            data.nextID = string.IsNullOrWhiteSpace(values[11]) ? -1 : nextID;
 
            dialogues.Add(data);
        }
    }

    // 文字列からEmotionTypeに安全に変換するためのヘルパー関数
    private EmotionType ParseEmotion(string value)
    {
        if (!string.IsNullOrEmpty(value) && Enum.TryParse(value, true, out EmotionType parsedEmotion))
        {
            return parsedEmotion;
        }
        return EmotionType.Normal; // 空っぽか変換失敗なら通常顔
    }

    public DialogueData GetDialogue(int id)
    {
        return dialogues.FirstOrDefault(d => d.id == id);
    }
}