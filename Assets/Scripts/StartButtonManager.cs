using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButtonManager : MonoBehaviour {
    public void ChessManual()
    {
        SceneManager.LoadScene(1);
    }

    public void ChessPersonSVEnvironment()
    {
        //TODO
        print("sorry，人机对战正在开发，请耐心等待!");
    }

    public void ChessPersonSVPersion()
    {
        //todo
        print("抱歉，逐鹿中原正在开发，请耐心等待！");
    }
}
