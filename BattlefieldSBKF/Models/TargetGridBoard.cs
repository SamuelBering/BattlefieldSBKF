namespace BattlefieldSBKF.Models
{
    public class TargetGridBoard : GridBoard
    {
        private readonly char MissSymbol = 'M';
        private readonly char HitSymbol = 'H';

        public TargetGridBoard(int gridSide) : base(gridSide)
        {
      
        }

        public void MarkShot(int gridIndex, bool IsAHit)
        {
            if (IsAHit)
            {
                base.Grid[gridIndex] = HitSymbol;
            }
            else base.Grid[gridIndex] = MissSymbol;

        }
    }
}