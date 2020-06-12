using UnityEngine;

public class CannonBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject m_cannonBallSpawnPosition;
    [SerializeField] private GameObject m_cannonBallPrefab;

    private int m_cannonInventory = 0;

    public void OnCannonLoaded(string cannonId)
    {
        if (cannonId != this.tag)
        {
            return;
        }

        m_cannonInventory++;
    }

    public void OnCannonFired(string cannonId)
    {
        if (cannonId != this.tag || m_cannonInventory == 0)
        {
            return;
        }

        GameObject.Instantiate(m_cannonBallPrefab, m_cannonBallSpawnPosition.transform);
        m_cannonInventory--;

        Debug.Log($"{this.tag} fired!");
    }
}
