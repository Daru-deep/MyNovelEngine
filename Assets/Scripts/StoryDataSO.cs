using UnityEngine;

[CreateAssetMenu(fileName = "NewStoryData", menuName = "Story/StoryData")]
public class StoryDataSO : ScriptableObject
{
    public string storyTitle;
    public TextAsset csvFile;
    public Sprite[] backgroundSprites;
}
