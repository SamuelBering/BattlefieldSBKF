using System;

namespace BattlefieldSBKF.Models
{
    public class TargetGridBoard : GridBoard
    {
        private readonly char MissSymbol = 'M';
        private readonly char HitSymbol = 'H';

        public TargetGridBoard(int gridSide, IBattleShipProtocol batProto) : base(gridSide, batProto)
        {
      
        }

        public void MarkShot(string yCoord, string xCoord, bool hit)
        {
            var gridIndex = BoardCoordinateToIndex(yCoord, xCoord);
            if (hit)
            {
                base.Grid[gridIndex] = HitSymbol;
            }

            else if (Grid[gridIndex] == OceanSymbol) 
            {
                base.Grid[gridIndex] = MissSymbol;
            }

        }

        public override void ShowBoard()
        {
            Console.WriteLine(" -- Targetgrid --");
            base.ShowBoard();
        }
    }
}