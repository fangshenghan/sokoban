using UnityEngine;

public class Jumpy : Block
{

    public override bool CheckMove(int _deltaX, int _deltaY)
    {
        if (State == MoveStates.idle)
        {
            State = MoveStates.attemptingMove;
            if (canMove)
            {
                if (InGrid(gridPos.x + _deltaX, gridPos.y + _deltaY))
                {
                    Cell checkCell = gridManager.gridList[gridPos.x + _deltaX][gridPos.y + _deltaY].GetComponent<Cell>();
                    if (!checkCell.CheckContainObj())
                    {
                        // try move two cells if first is empty
                        int dX = _deltaX * 2, dY = _deltaY * 2;
                        if (InGrid(gridPos.x + dX, gridPos.y + dY))
                        {
                            Cell checkCell2 = gridManager.gridList[gridPos.x + dX][gridPos.y + dY].GetComponent<Cell>();
                            if (!checkCell2.CheckContainObj())
                            {
                                StartMove(checkCell2, dX, dY);
                                BroadcastMessage("BlockMoved", new Vector2Int(dX, dY), SendMessageOptions.DontRequireReceiver);
                                return true;
                            }
                        }
                        
                    } 
                    
                    if (!checkCell.CheckContainObj() || CheckHit(checkCell.ContainObj.GetComponent<Block>(), _deltaX, _deltaY))
                    {
                        StartMove(checkCell, _deltaX, _deltaY);
                        BroadcastMessage("BlockMoved", new Vector2Int(_deltaX, _deltaY), SendMessageOptions.DontRequireReceiver);
                        return true;
                    }
                }
            }
            State = MoveStates.idle;
        }
        return false;
    }

    private bool InGrid(int _newX, int _newY)
    {
        if (_newX >= 0 && _newX < gridManager.gridList.Count &&
            _newY >= 0 && _newY < gridManager.gridList[0].Count) return true;
        return false;
    }

    private bool CheckHit(Block _hitObj, int _deltaX, int _deltaY)
    {
        if (GetComponent<Slidey>() == null && _hitObj.CheckMove(_deltaX, _deltaY)) return true;
        return false;
    }

}
