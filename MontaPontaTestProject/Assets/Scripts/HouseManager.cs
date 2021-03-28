using UnityEngine;

public class HouseManager : MonoBehaviour
{
    #region Singletone
    public static HouseManager instance;

    private void Awake()
    {
        instance = this;
    }
    #endregion

    public GameObject house;
}
