using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Dealer:MonoBehaviour
{ 
    #region 荷官的单例实现
    private static volatile Dealer mInstance;
    private static object theLock=new object();
    public static Dealer Instance
    {
        get
        {
            if (mInstance == null)
            {
                lock (theLock)
                {
                    if (mInstance == null)
                    {
                        var instances = FindObjectsOfType<Dealer>();
                        if (instances.Length > 0)
                        {
                            mInstance = instances[0];
                            for (int i = 1; i < instances.Length; i++)
                            {
                                Destroy(instances[i].gameObject);
                            }
                        }
                        else
                        {
                            GameObject go = new GameObject("GODealer");
                            mInstance = go.AddComponent<Dealer>();
                            DontDestroyOnLoad(go);
                        }
                    }   
                }
            }
            return mInstance;
        }
    }
    #endregion

    private IBoard board;
    private int[,] chessStatus;//表示棋局的数组，0空白；1黑棋；2白棋
    private bool isOffensiveTurn;
    public bool IsOffensiveTurn {
        get { return isOffensiveTurn; }
    }
    private bool gameEnded;
    private Stack<ChessCoordinate> stackCoordinate;

    public void InitChessManula(IBoard parmBoard)
    {
        board = parmBoard;
        isOffensiveTurn = true;
        chessStatus = new int[15, 15];
        gameEnded = false;
        stackCoordinate = new Stack<ChessCoordinate>();
        Timer = 0;
    }
  
    public float Timer { get; private set; }

    public void PlacePiece(PlayerType senderType,ChessCoordinate coordinate)
    {
        if (coordinate.XPosition < 0 || coordinate.XPosition > 14 || coordinate.YPosition < 0 || coordinate.YPosition > 14 || 
            Timer < 0.3f || chessStatus[coordinate.XPosition, coordinate.YPosition] !=0 || !ValidateTurn(senderType) || gameEnded)
        {
            return;
        }
        chessStatus[coordinate.XPosition, coordinate.YPosition] = (int)senderType;
        stackCoordinate.Push(coordinate);
        board.PlacePiece(coordinate,isOffensiveTurn);
        if (IsWin(coordinate))
        {
            board.ShowEnd(isOffensiveTurn);
            gameEnded = true;
        }
        isOffensiveTurn = !isOffensiveTurn;
        Timer = 0;
    }

    public void ExtractInManula()
    {
        stackCoordinate.Pop();
        stackCoordinate.Pop();
        board.Retract();
    }

    bool ValidateTurn(PlayerType playerType)
    {
        return (playerType == PlayerType.Black && Dealer.Instance.IsOffensiveTurn) ||
               (playerType == PlayerType.White && !Dealer.Instance.IsOffensiveTurn);
    }

    bool IsWin(ChessCoordinate coordinate)
    {
        if (SamePiece(coordinate.XPosition, coordinate.YPosition, 1, 0) >= 5)
        {
            return true;
        }
        if (SamePiece(coordinate.XPosition, coordinate.YPosition, 0, 1) >= 5)
        {
            return true;
        }
        if (SamePiece(coordinate.XPosition, coordinate.YPosition, 1, 1) >= 5)
        {
            return true;
        }
        if (SamePiece(coordinate.XPosition, coordinate.YPosition, 1, -1) >= 5)
        {
            return true;
        }
        return false;
    }

    private int SamePiece(int xPostion, int yPosition, int xDelta, int yDelta)
    {
        int result = 1;
        int dotStatus = chessStatus[xPostion, yPosition];
        for (int i = 1; i <= 4; i++)
        {
            int xProbe = xPostion + i * xDelta;
            int yProbe = yPosition + i * yDelta;
            if (xProbe < 0 || xProbe > 14 || yProbe < 0 || yProbe > 14)
            {
                break;
            }
            if (chessStatus[xProbe,yProbe] == dotStatus)
            {
                result++;
            }
            else
            {
                break;
            }
        }
        if (result == 5)
        {
            return result;
        }
        for (int i = -1; i >= -4; i--)
        {
            int xProbe = xPostion + i * xDelta;
            int yProbe = yPosition + i * yDelta;
            if (xProbe < 0 || xProbe > 14 || yProbe < 0 || yProbe > 14)
            {
                break;
            }
            if (chessStatus[xPostion + i * xDelta, yPosition + i * yDelta] == dotStatus)
            {
                result++;
            }
            else
            {
                break;
            }
        }
        return result;
    }
    
    void Update()
    {
        Timer += Time.deltaTime;
    }

    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
