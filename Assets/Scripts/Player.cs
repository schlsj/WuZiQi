using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{

    public PlayerType PlayerType;
	
	// Update is called once per frame
	void Update () {
	    PlayerChess(); 
	}

    void PlayerChess()
    {
        if (Input.GetMouseButtonUp(0)&&!EventSystem.current.IsPointerOverGameObject())
        {
            Vector2 clickDot = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            int xPosition = (int) (7.5 + clickDot.x);
            int yPosition = (int) (7.5 + clickDot.y);
            //print(string.Format("PlayerType:{0}   xPosition:{1}   yPosition:{2}", PlayerType, xPosition, yPosition));
            Dealer.Instance.PlacePiece(PlayerType,new ChessCoordinate(){XPosition = xPosition,YPosition = yPosition});
        }  
    }
}
