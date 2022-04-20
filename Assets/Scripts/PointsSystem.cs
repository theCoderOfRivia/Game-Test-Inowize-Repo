using UnityEngine;
using TMPro;

public class PointsSystem : MonoBehaviour
{
    public static PointsSystem instance;

    int currentPoints = 0;

    [Header("References"), Space(5)]
    [SerializeField] TextMeshProUGUI pointsText;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        pointsText.SetText(currentPoints.ToString());
    }

    public void IncreasePoints(EnemyData.EnemyType enemyType)
    {
        if(enemyType == EnemyData.EnemyType.Melee)
        {
            currentPoints++;
            pointsText.SetText(currentPoints.ToString());
        }
        else
        {
            currentPoints += 3;
            pointsText.SetText(currentPoints.ToString());
        }
    }
}
