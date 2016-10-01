using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MastersThesisMarianNikolov0124IMD.Global
{
    public class GlobalConstants
    {
        // game speed   10 wasgood
        public const double timerFramesIntervalInMiliSeconds = 30; 

        // size of up checkers
        public const int startCheckerWidth = 90;
        public const int startCheckerHeight = 90;
        public const int startLeftPositionForUpCheckers = 165;
        public const int startTopPositionForUpCheckers = 40;

        // size of board checkers
        public const int checkerWidth = 95;
        public const int checkerHeight = 95;
        public const int topSpacing = 107;
        public const int leftSpacing = 45;
        //public const int checkerSpeed = 2;

        // size of reminding board checkers
        public const int remindCheckerWidth = 60;
        public const int remindCheckerHeight = 60;
        public const int remindCheckerStartPosLeft = 1000;
        public const int remindCheckerStartPosTop = 30;
        public const int spacingBetweenRemindCheckers = 70;


        // size of board
        public const int boardWidth = 784;  
        public const int boardHeight = 802;
        public const int spacingBetweenCells = 110;

        // turn message 
        public const string redTurnMsg = "Red turn";
        public const string blueTurnMsg = "Blue turn";


    }
}
