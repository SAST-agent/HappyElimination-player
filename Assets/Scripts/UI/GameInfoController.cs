using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 信息面板控制器
/// </summary>
public class GameInfoController : MonoBehaviour
{
    public Text gameRound;
    public Text playerScore;
    public Text opponentScore;

    public Text skillRound;
    private int round, player, opponent, skillround;
    // Start is called before the first frame update
    void Start()
    {
        if (ModeController.IsInteractMode())
        {
            gameRound.text = "当前回合: " + 1.ToString();
            playerScore.text = "你的分数: " + 0.ToString();
            opponentScore.text = "对手分数: " + 0.ToString();
            skillRound.text = "技能持续时间: " + 0.ToString();
        }
        else
        {
            gameRound.text = "当前回合: " + 1.ToString();
            playerScore.text = "0号玩家分数: " + 0.ToString();
            opponentScore.text = "1号玩家分数: " + 0.ToString();
            skillRound.text = "技能持续时间: " + 0.ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (ModeController.IsInteractMode())
        {
            gameRound.text = "当前回合: " + (round + 1).ToString();
            playerScore.text = "你的分数: " + player.ToString();
            opponentScore.text = "对手分数: " + opponent.ToString();
            skillRound.text = "技能持续时间: " + StateController.getSkill().ToString();
        }
        else
        {
            gameRound.text = "当前回合: " + (round + 1).ToString();
            playerScore.text = "0号玩家分数: " + player.ToString();
            opponentScore.text = "1号玩家分数: " + opponent.ToString();
            skillRound.text = "技能持续时间: " + StateController.getSkill().ToString();
        }
    }

    public void SetGameInfo(int _round, int _player, int _opponent, int _skillround)
    {
        if (_round >= 0)
        {
            round = _round;
            player = _player;
            opponent = _opponent;
            skillround = _skillround;
        }
    }

    public void UpdateRound()
    {
        round = StateController.getRound();
    }

    public void UpdateScore()
    {
        if (ModeController.IsInteractMode())
        {
            player = StateController.getScores()[StateController.getPlayer()];
            opponent = StateController.getScores()[1 - StateController.getPlayer()];
        }
        else if (ModeController.IsReplayMode())
        {
            // 回放模式不会连后端，因此StateController里不会存player
            // 而且在回放模式下“当前玩家”没有意义，不应当去访问这个 ↑ 变量
            player = StateController.getScores()[0];
            opponent = StateController.getScores()[1];
        }
    }

    public void UpdateSkillRound()
    {
        skillround = StateController.getSkillLastRound()[player];
    }
}
