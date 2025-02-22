using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MissionManager : MonoBehaviour
{
    public TextMeshProUGUI mission1Text;
    public TextMeshProUGUI mission2Text;
    public TextMeshProUGUI mission3Text;

    public AreaEnemyChecker area1;
    public AreaEnemyChecker area2;
    public AreaEnemyChecker area3;

    private bool mission1Completed = false;
    private bool mission2Completed = false;
    private bool mission3Completed = false;

    private int completedMissions = 0; // Tamamlanan görev sayısı

    void Start()
    {
        if (area1 == null || area2 == null || area3 == null)
        {
            Debug.LogError("Missing area references in MissionManager!");
        }

        // Ayasofya görevi ilk başta solgun olacak
        mission3Text.color = new Color(mission3Text.color.r, mission3Text.color.g, mission3Text.color.b, 0.5f);
    }

    void Update()
    {
        // Check Area 1 independently
        if (!mission1Completed && area1 != null && area1.IsAreaCleared())
        {
            MarkMissionCompleted1(mission1Text);
            mission1Completed = true;
            Debug.Log("Mission 1 completed!");
        }

        // Check Area 2 independently
        if (!mission2Completed && area2 != null && area2.IsAreaCleared())
        {
            MarkMissionCompleted2(mission2Text);
            mission2Completed = true;
            Debug.Log("Mission 2 completed!");
        }

        // Enable mission 3 if both mission 1 and 2 are completed
        if (mission1Completed && mission2Completed && !mission3Completed)
        {
            // Make mission 3 visible
            mission3Text.color = new Color(mission3Text.color.r, mission3Text.color.g, mission3Text.color.b, 1f);

            // Check Area 3
            if (area3 != null && area3.IsAreaCleared())
            {
                MarkMissionCompleted3(mission3Text);
                mission3Completed = true;
                Debug.Log("Mission 3 completed!");
            }
        }
    }

    void MarkMissionCompleted1(TextMeshProUGUI text)
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0.5f);
        text.fontStyle = FontStyles.Strikethrough;
    }

    void MarkMissionCompleted2(TextMeshProUGUI text)
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0.5f);
        text.fontStyle = FontStyles.Strikethrough;
    }

    void MarkMissionCompleted3(TextMeshProUGUI text)
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0.5f);
        text.fontStyle = FontStyles.Strikethrough;
    }
}
