using UnityEngine;
using UnityEngine.UI;

public class WoodCounter : MonoBehaviour
{
    public static WoodCounter wood;
    
    public Text scoreText;
    public int score = 0;

    private void OnEnable()
    {
        wood = this;
    }
    public void Count()
    {
        score += 3;
        scoreText.GetComponent<Text>().text = "Woods: " + score;
    }
}
