using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChessBoard : MonoBehaviour,IBoard {
    [SerializeField]
    private GameObject prefabBlackPiece;
    [SerializeField]
    private GameObject prefabWhitePiece;
    [SerializeField]
    private Transform pilotDot;
    [SerializeField]
    private Button btnRetract;
    [SerializeField]
    private GameObject ImgEnd;
    [SerializeField]
    private Text txtEndInfo;

    private Stack<ExtendPiece> pieces;

    void Start()
    {
        pieces = new Stack<ExtendPiece>();
        Dealer.Instance.InitChessManula(this);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="xPosition">以棋盘左下角为(0,0)点的X坐标</param>
    /// <param name="yPosition">以棋盘左下角为(0,0)点的Y坐标</param>
    /// <param name="isOffensiveTurn"></param>
    public void PlacePiece(ChessCoordinate coordinate, bool isOffensiveTurn)
    {
        int xTransform = coordinate.XPosition - 7;
        int yTransform = coordinate.YPosition - 7;
        GameObject newPiece = Instantiate(isOffensiveTurn ? prefabBlackPiece : prefabWhitePiece,
            new Vector3(xTransform,yTransform), Quaternion.identity);
        pilotDot.position = new Vector3(xTransform,yTransform);
        newPiece.transform.parent = transform;
        pieces.Push(new ExtendPiece(){XTransform = xTransform,YTransform = yTransform,Piece = newPiece});
        if (pieces.Count >= 2)
        {
            btnRetract.interactable = true;
        }
    }

    public void Retract()
    {
        for (int i = 0; i < 2; i++)
        {
            ExtendPiece go = pieces.Pop();
            Destroy(go.Piece);
        }
        ExtendPiece topPiece = pieces.Peek();
        if (topPiece != null)
        {
            pilotDot.position = new Vector3(topPiece.XTransform, topPiece.YTransform);
        }
        else
        {
            pilotDot.position = new Vector3(-50, -50);
        }
    }

    public void ShowEnd(bool isOffensiveTurn)
    {
        txtEndInfo.text = (isOffensiveTurn ? "黑" : "白") + "棋赢了!";
        ImgEnd.SetActive(true);
    }

    class ExtendPiece
    {
        public int XTransform { get; set; }
        public int YTransform { get; set; }
        public GameObject Piece { get; set; }
    }
}
