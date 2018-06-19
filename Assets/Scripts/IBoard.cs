public interface IBoard
{
    void PlacePiece(ChessCoordinate coordinate,bool isOffensiveTurn);
    void Retract();
    void ShowEnd(bool isOffensiveTurn);
}
