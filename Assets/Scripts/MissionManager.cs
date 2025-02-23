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

    private bool mission1Completed = false;
    private bool mission2Completed = false;
    private bool mission3Completed = false;

    private int completedMissions = 0; // Tamamlanan görev sayısı

    void Start()
    {
        // Ayasofya görevi ilk başta solgun olacak
        mission3Text.color = new Color(mission3Text.color.r, mission3Text.color.g, mission3Text.color.b, 0.5f);
    }

    public void CompleteMission(int missionID)
    {
        switch (missionID)
        {
            case 1:
                mission1Completed = true;
                MarkMissionCompleted(mission1Text);
                break;
            case 2:
                mission2Completed = true;
                MarkMissionCompleted(mission2Text);
                break;
            case 3:
                if (!mission3Completed && mission1Completed && mission2Completed) // 1 ve 2 tamamlanmışsa 3 açılabilir
                {
                    mission3Completed = true;
                    MarkMissionCompleted(mission3Text);
                }
                break;
        }

        completedMissions++;

        // Eğer ilk iki görev tamamlandıysa Ayasofya görevinin rengini düzelt
        if (mission1Completed && mission2Completed)
        {
            mission3Text.color = new Color(mission3Text.color.r, mission3Text.color.g, mission3Text.color.b, 1f);
        }
        if (mission3Completed)
        {
            mission3Completed = true;
            MarkMissionCompleted(mission3Text);
        }
    }

    void MarkMissionCompleted(TextMeshProUGUI text)
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0.5f); // Soluk yap
        text.fontStyle = FontStyles.Strikethrough; // Üzerini çiz
    }
}
