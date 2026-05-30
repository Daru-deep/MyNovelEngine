using System.Collections.Generic;
using UnityEngine;

public class GameFlowManager : MonoBehaviour
{
    [SerializeField] private StoryManager storyManager;
    [SerializeField] private List<StoryDataSO> stories;

    private int currentIndex = 0;

    private void Start()
    {
        storyManager.OnStoryEnd = LoadNextStory;
        LoadStoryAt(0);
    }

    public void LoadStoryAt(int index)
    {
        if (index < 0 || index >= stories.Count) return;
        currentIndex = index;
        storyManager.LoadStory(stories[index]);
    }

    private void LoadNextStory()
    {
        LoadStoryAt(currentIndex + 1);
    }
}
