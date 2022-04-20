using UnityEngine;

public class IconRotator : MonoBehaviour
{
    Transform player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void Update()
    {
        transform.LookAt(player.position);
    }
}
