using UnityEngine;
using UnityEngine.UI;

public class GameInfoController : MonoBehaviour
{
    public Text gameRound;
    public Text playerScore;
    public Text opponentScore;
    private int round, player, opponent;
    // Start is called before the first frame update
    void Start()
    {
        if (ModeController.IsInteractMode())
        {
            gameRound.text = "当前回合: " + 100.ToString();
            playerScore.text = "你的分数: " + 0.ToString();
            opponentScore.text = "对手分数: " + 0.ToString();
        }
        else
        {
            gameRound.text = "当前回合: " + 100.ToString();
            playerScore.text = "0号玩家分数: " + 0.ToString();
            opponentScore.text = "1号玩家分数: " + 0.ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (ModeController.IsInteractMode())
        {
            gameRound.text = "当前回合: " + round.ToString();
            playerScore.text = "你的分数: " + player.ToString();
            opponentScore.text = "对手分数: " + opponent.ToString();
        }
        else
        {
            gameRound.text = "当前回合: " + round.ToString();
            playerScore.text = "0号玩家分数: " + player.ToString();
            opponentScore.text = "1号玩家分数: " + opponent.ToString();
        }
    }

    public void SetGameInfo(int _round, int _player, int _opponent)
    {
        if (_round >= 0)
        {
            round = _round;
            player = _player;
            opponent = _opponent;
        }
    }
}
