using UnityEngine;

public class Jumpy : Block
{

    private bool hasJumped = false;

    protected override void FinishMove()
    {
        base.FinishMove();

        if (!hasJumped)
        {
            hasJumped = true;
            Jump(moveChange.x, moveChange.y);
        }
    }

    private void Jump(int _deltaX, int _deltaY)
    {
        base.CheckMove(_deltaX, _deltaY);
    }

    public override bool CheckMove(int _deltaX, int _deltaY)
    {
        hasJumped = false;
        return base.CheckMove(_deltaX, _deltaY);
    }

}
