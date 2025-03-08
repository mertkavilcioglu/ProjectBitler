using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MissionManager : MonoBehaviour
{
    public static MissionManager Instance { get; private set; }

    public TextMeshProUGUI mission1Text;
    public TextMeshProUGUI mission2Text;
    public TextMeshProUGUI mission3Text;

    public AreaEnemyChecker area1;
    public AreaEnemyChecker area2;
    public AreaEnemyChecker area3;

    public GameObject ayasofya;

    // Dictionary to track which area IDs have been completed
    private Dictionary<int, bool> completedAreas = new Dictionary<int, bool>();

    private bool mission1Completed = false;
    private bool mission2Completed = false;
    private bool mission3Completed = false;    

    private int completedMissions = 0;

    void Awake()
    {
        // Simple singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {
        if (area1 == null || area2 == null || area3 == null)
        {
            Debug.LogError("Missing area references in MissionManager!");
        }

        // Ayasofya görevi ilk başta solgun olacak
        if (mission3Text != null)
        {
            mission3Text.color = new Color(mission3Text.color.r, mission3Text.color.g, mission3Text.color.b, 0.5f);
        }

        LoadMissionStatus();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene loaded: " + scene.name);
        StartCoroutine(DisableEnemiesInCompletedAreas());
    }

    void Update()
    {
        CheckMissionStatus();
    }

    private void CheckMissionStatus()
    {
        if (!mission1Completed && area1 != null && area1.IsAreaCleared())
        {
            MarkMissionCompleted1(mission1Text);
            mission1Completed = true;
            completedMissions++;
            completedAreas[area1.areaID] = true;
            SaveMissionStatus();
            Debug.Log("Mission 1 completed! Area ID: " + area1.areaID);
        }

        if (!mission2Completed && area2 != null && area2.IsAreaCleared())
        {
            MarkMissionCompleted2(mission2Text);
            mission2Completed = true;
            completedMissions++;
            completedAreas[area2.areaID] = true;
            SaveMissionStatus();
            Debug.Log("Mission 2 completed! Area ID: " + area2.areaID);
        }

        if (mission1Completed && mission2Completed && !mission3Completed && mission3Text != null)
        {
            // Make mission 3 visible
            mission3Text.color = new Color(mission3Text.color.r, mission3Text.color.g, mission3Text.color.b, 1f);
            ayasofya.SetActive(false);//ilk2gorevi bitirmeden ayasofyaya giremesin diye

            if (area3 != null && area3.IsAreaCleared())
            {
                MarkMissionCompleted3(mission3Text);
                mission3Completed = true;
                completedMissions++;
                completedAreas[area3.areaID] = true;
                SaveMissionStatus();
                Debug.Log("Mission 3 completed! Area ID: " + area3.areaID);
            }
        }
    }

    // Mark missions as completed (UI changes)
    void MarkMissionCompleted1(TextMeshProUGUI text)
    {
        if (text != null)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, 0.5f);
            text.fontStyle = FontStyles.Strikethrough;
        }
    }

    void MarkMissionCompleted2(TextMeshProUGUI text)
    {
        if (text != null)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, 0.5f);
            text.fontStyle = FontStyles.Strikethrough;
        }
    }

    void MarkMissionCompleted3(TextMeshProUGUI text)
    {
        if (text != null)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, 0.5f);
            text.fontStyle = FontStyles.Strikethrough;
        }
    }

    private IEnumerator DisableEnemiesInCompletedAreas()
    {
        yield return new WaitForSeconds(0.1f);

        Debug.Log("Checking for enemies in completed areas. Completed areas: " + string.Join(", ", completedAreas.Keys));

        foreach (var areaEntry in completedAreas)
        {
            int areaID = areaEntry.Key;
            bool isCompleted = areaEntry.Value;

            if (isCompleted)
            {
                AreaEnemyChecker areaChecker = FindAreaByID(areaID);

                if (areaChecker != null)
                {
                    Debug.Log("Found area checker for area ID: " + areaID);

                    GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                    int disabledCount = 0;

                    foreach (GameObject enemy in enemies)
                    {
                        if (IsEnemyInArea(enemy, areaChecker))
                        {
                            enemy.SetActive(false);
                            disabledCount++;
                        }
                    }

                    Debug.Log("Disabled " + disabledCount + " enemies in area " + areaID);
                }
                else
                {
                    Debug.LogWarning("Could not find area checker for area ID: " + areaID);
                }
            }
        }
    }

    private bool IsEnemyInArea(GameObject enemy, AreaEnemyChecker areaChecker)
    {
        BoxCollider2D areaCollider = areaChecker.GetComponent<BoxCollider2D>();
        if (areaCollider == null)
        {
            Debug.LogWarning("Area does not have a BoxCollider2D: " + areaChecker.areaID);
            return false;
        }

        // Get area bounds
        Vector2 areaPos = (Vector2)areaChecker.transform.position + areaCollider.offset;
        Vector2 areaSize = areaCollider.size * areaChecker.transform.localScale;

        // Check if enemy is within area bounds
        return IsPointInBox(enemy.transform.position, areaPos, areaSize);
    }

    // Helper method to check if a point is inside a box
    private bool IsPointInBox(Vector2 point, Vector2 boxCenter, Vector2 boxSize)
    {
        return (point.x >= boxCenter.x - boxSize.x / 2 &&
                point.x <= boxCenter.x + boxSize.x / 2 &&
                point.y >= boxCenter.y - boxSize.y / 2 &&
                point.y <= boxCenter.y + boxSize.y / 2);
    }

    private AreaEnemyChecker FindAreaByID(int areaID)
    {
        if (area1 != null && area1.areaID == areaID) return area1;
        if (area2 != null && area2.areaID == areaID) return area2;
        if (area3 != null && area3.areaID == areaID) return area3;

        AreaEnemyChecker[] allAreas = FindObjectsOfType<AreaEnemyChecker>();
        foreach (AreaEnemyChecker area in allAreas)
        {
            if (area.areaID == areaID)
                return area;
        }

        return null;
    }

    public void SaveMissionStatus()
    {
        PlayerPrefs.SetInt("Mission1Completed", mission1Completed ? 1 : 0);
        PlayerPrefs.SetInt("Mission2Completed", mission2Completed ? 1 : 0);
        PlayerPrefs.SetInt("Mission3Completed", mission3Completed ? 1 : 0);
        PlayerPrefs.SetInt("CompletedMissions", completedMissions);

        string completedAreasStr = "";
        foreach (var area in completedAreas)
        {
            if (area.Value)
            {
                completedAreasStr += area.Key + ",";
            }
        }
        PlayerPrefs.SetString("CompletedAreas", completedAreasStr);

        PlayerPrefs.Save();

        Debug.Log("Mission status saved: M1=" + mission1Completed +
                  ", M2=" + mission2Completed +
                  ", M3=" + mission3Completed +
                  "CompletedAreas=" + completedAreasStr);
    }

    public void LoadMissionStatus()
    {
        mission1Completed = PlayerPrefs.GetInt("Mission1Completed", 0) == 1;
        mission2Completed = PlayerPrefs.GetInt("Mission2Completed", 0) == 1;
        mission3Completed = PlayerPrefs.GetInt("Mission3Completed", 0) == 1;
        completedMissions = PlayerPrefs.GetInt("CompletedMissions", 0);

        string completedAreasStr = PlayerPrefs.GetString("CompletedAreas", "");
        completedAreas.Clear();

        if (!string.IsNullOrEmpty(completedAreasStr))
        {
            string[] areaIDs = completedAreasStr.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);
            foreach (string areaIDStr in areaIDs)
            {
                if (int.TryParse(areaIDStr, out int areaID))
                {
                    completedAreas[areaID] = true;
                    Debug.Log("Loaded completed area: " + areaID);
                }
            }
        }

        Debug.Log("Mission status loaded: M1=" + mission1Completed +
                  ", M2=" + mission2Completed +
                  ", M3=" + mission3Completed +
                  "Completed areas: " + completedAreasStr);

        if (mission1Completed && mission1Text != null)
        {
            MarkMissionCompleted1(mission1Text);
        }

        if (mission2Completed && mission2Text != null)
        {
            MarkMissionCompleted2(mission2Text);
        }

        if (mission3Completed && mission3Text != null)
        {
            // Make mission 3 visible if 1 and 2 are completed
            if (mission3Text != null)
            {
                mission3Text.color = new Color(mission3Text.color.r, mission3Text.color.g, mission3Text.color.b, 1f);
                MarkMissionCompleted3(mission3Text);
            }
        }
    }

    public bool IsAreaCompleted(int areaID)
    {
        return completedAreas.ContainsKey(areaID) && completedAreas[areaID];
    }

    // Reset all mission data
    public void ResetAllMissionData()
    {
        PlayerPrefs.DeleteKey("Mission1Completed");
        PlayerPrefs.DeleteKey("Mission2Completed");
        PlayerPrefs.DeleteKey("Mission3Completed");
        PlayerPrefs.DeleteKey("CompletedMissions");
        PlayerPrefs.DeleteKey("CompletedAreas");
        PlayerPrefs.Save();

        mission1Completed = false;
        mission2Completed = false;
        mission3Completed = false;
        completedMissions = 0;
        completedAreas.Clear();

        Debug.Log("All mission data has been reset");
    }
}