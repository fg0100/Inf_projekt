using UnityEngine;

public class Reacher_GUI : MonoBehaviour
{
    [SerializeField] private ReacherRobot _reacherAgent;

    private GUIStyle _defaultStyle = new GUIStyle();
    private GUIStyle _positiveStyle = new GUIStyle();
    private GUIStyle _negativeStyle = new GUIStyle();

    void Start()
    {
        _defaultStyle.fontSize = 20;
        _defaultStyle.normal.textColor = Color.yellow;

        _positiveStyle.fontSize = 20;
        _positiveStyle.normal.textColor = Color.green;

        _negativeStyle.fontSize = 20;
        _negativeStyle.normal.textColor = Color.red;
    }
    private void OnGUI()
    {
        string debugEpisode = "Episode: " + _reacherAgent.CurrentEpisode + " - Step: " + _reacherAgent.StepCount;
        string debugReward = "Reward: " + _reacherAgent.CumulativeReward.ToString();

        // Select style based on reward value
        GUIStyle rewardStyle = _reacherAgent.CumulativeReward < 0 ? _negativeStyle : _positiveStyle;

        // Display the debug text
        GUI.Label(new Rect(20, 20, 500, 30), debugEpisode, _defaultStyle);
        GUI.Label(new Rect(20, 60, 500, 30), debugReward, rewardStyle);
    }

    void Update()
    {
        
    }
}
