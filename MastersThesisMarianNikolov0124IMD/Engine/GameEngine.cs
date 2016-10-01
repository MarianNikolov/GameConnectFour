using System;
using MastersThesisMarianNikolov0124IMD.Contracts;
using System.Collections.Generic;
using System.Windows.Threading;
using MastersThesisMarianNikolov0124IMD.Common;
using MastersThesisMarianNikolov0124IMD.Factories;
using MastersThesisMarianNikolov0124IMD.Global;
using MastersThesisMarianNikolov0124IMD.GameObjects;
using System.Windows.Interop;

namespace MastersThesisMarianNikolov0124IMD.Engine
{
    public class GameEngine : IGameEngine
    {
        private IRenderer renderer;
        private IGameObject checker;
        private DispatcherTimer timer;
        private Position[] positionsForDropCheckers;
        private Position[,] positionsGameField;
        private IGameObject[] upGameField;
        private IGameObject[,] gameField;
        private IGameObject board;
        private Position[] positionRemainingRedCheckers;
        private Position[] positionRemainingBlueCheckers;
        private IGameObject[] remainingRedCheckers;
        private IGameObject[] remainingBlueCheckers;
        private Position startPositionForWinGame;
        private Position endPositionForWinGame;

        bool isRedTurn = true;

        public GameEngine(IRenderer renderer)
        {
            this.Renderer = renderer;
            this.timer = new DispatcherTimer();
            this.Renderer.presingKey += HandleKeyPressed;
        }

        public IRenderer Renderer
        {
            get
            {
                return this.renderer;
            }
            set
            {
                this.renderer = value;
            }
        }

        public Position[] PositionsForDropCheckers
        {
            get { return this.positionsForDropCheckers; }
            set { this.positionsForDropCheckers = value; }
        }

        public Position[,] PositionsGameField
        {
            get
            {
                return this.positionsGameField;
            }
            set
            {
                this.positionsGameField = value;
            }

        }

        public IGameObject[] UpGameField
        {
            get
            {
                return this.upGameField;
            }

            set
            {
                this.upGameField = value;
            }
        }

        public IGameObject[,] GameField
        {
            get
            {
                return this.gameField;
            }

            set
            {
                this.gameField = value;
            }
        }

        public IGameObject Checker
        {
            get
            {
                return this.checker;
            }

            set
            {
                this.checker = value;
            }
        }

        public IGameObject Board
        {
            get
            {
                return this.board;
            }

            set
            {
                this.board = value;
            }
        }

        public IGameObject[] RemainingRedCheckers
        {
            get
            {
                return this.remainingRedCheckers;
            }

            set
            {
                this.remainingRedCheckers = value;
            }
        }

        public IGameObject[] RemainingBlueCheckers
        {
            get
            {
                return this.remainingBlueCheckers;
            }

            set
            {
                this.remainingBlueCheckers = value;
            }
        }

        public Position[] PositionRemainingRedCheckers
        {
            get
            {
                return this.positionRemainingRedCheckers;
            }

            set
            {
                this.positionRemainingRedCheckers = value;
            }
        }

        public Position[] PositionRemainingBlueCheckers
        {
            get
            {
                return this.positionRemainingBlueCheckers;
            }

            set
            {
                this.positionRemainingBlueCheckers = value;
            }
        }

        public Position StartPositionForWinGame
        {
            get
            {
                return this.startPositionForWinGame;
            }

            set
            {
                this.startPositionForWinGame = value;
            }
        }

        public Position EndPositionForWinGame
        {
            get
            {
                return this.endPositionForWinGame;
            }

            set
            {
                this.endPositionForWinGame = value;
            }
        }

        private void HandleKeyPressed(object sender, KeyDownEventArgs key)
        {
            bool isLeft = key.Command == GameCommandType.MoveLeft;
            bool isRight = key.Command == GameCommandType.MoveRight;
            bool isFalling = key.Command == GameCommandType.Falling;

            try
            {
                int change = isLeft ? -1 : 1;
                int currentColumnIndex = FindCurrentIndexInUpGameField();

                if (isLeft || isRight)
                {
                    if (IsInRangeUpGameFiled(currentColumnIndex, change))
                    {
                        MoveUpCheckers(currentColumnIndex, change);
                    }
                }
                else if (isFalling)
                {
                    bool isFreeCellsInColumn = IsFreeCellsInColumn(currentColumnIndex);

                    DropChecker(currentColumnIndex);

                    if (isFreeCellsInColumn)
                    {
                        RemoveReminderChecker();

                        this.renderer.Clear();

                        this.Renderer.DrawPleyField(this.GameField);
                        this.Renderer.Draw(this.Board);
                        this.Renderer.Draw(this.RemainingRedCheckers);
                        this.Renderer.Draw(this.RemainingBlueCheckers);
                        this.Renderer.Draw();

                        // generate current turn message
                        Position turnMessagePosition = GameObjectFactory.GeneratePosition(420, 60);
                        Size turnMessageSize = GameObjectFactory.GenerateSize(230, 45);
                        string turnMessageText = isRedTurn ? GlobalConstants.redTurnMsg : GlobalConstants.blueTurnMsg;
                        ITextGameObject turnMessage = GameObjectFactory.GenerateNextTurnMessage(turnMessagePosition, turnMessageSize, turnMessageText);
                        this.Renderer.Draw(turnMessage);
                        this.Renderer.RefreshGame();

                        System.Threading.Thread.Sleep(500);
                    }
                }
                else
                {
                    throw new KeyNotFoundException("Wrong key pressed");
                }
            }
            catch (KeyNotFoundException ex)
            {
                Position wrongKeyMessagePosition = GameObjectFactory.GeneratePosition(333, 333);
                Size wrongKeyMessageSize = GameObjectFactory.GenerateSize(666, 222);
                ITextGameObject wrongKeyMsg = GameObjectFactory.GenerateWrongKeyMessage(wrongKeyMessagePosition, wrongKeyMessageSize, ex.Message);
                this.Renderer.Draw(wrongKeyMsg);
                this.Renderer.RefreshGame();

                System.Threading.Thread.Sleep(500);
            }
            finally
            {
                this.renderer.Clear();

                this.Renderer.Draw(this.upGameField);
                this.Renderer.DrawPleyField(this.GameField);
                this.Renderer.Draw(this.Board);
                this.Renderer.Draw(this.RemainingRedCheckers);
                this.Renderer.Draw(this.RemainingBlueCheckers);
                this.Renderer.Draw();
                this.Renderer.RefreshGame();
            }
        }

        private bool IsFreeCellsInColumn(int currentColumnIndex)
        {
            if (this.GameField[0, currentColumnIndex] == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void RemoveReminderChecker()
        {
            if (isRedTurn)
            {
                for (int i = this.RemainingRedCheckers.Length - 1; i >= 0; i--)
                {
                    bool isObjFound = this.RemainingRedCheckers[i] != null;
                    if (isObjFound)
                    {
                        this.RemainingRedCheckers[i] = null;
                        break;
                    }
                }
            }
            else
            {
                for (int i = this.RemainingBlueCheckers.Length - 1; i >= 0; i--)
                {
                    bool isObjFound = this.RemainingBlueCheckers[i] != null;
                    if (isObjFound)
                    {
                        this.RemainingBlueCheckers[i] = null;
                        break;
                    }
                }
            }
        }

        private void MoveUpCheckers(int currentIndex, int change)
        {
            Size checkerSizeForUpField = GameObjectFactory.GenerateSize(GlobalConstants.startCheckerWidth, GlobalConstants.startCheckerHeight);
            if (isRedTurn)
            {
                IGameObject checkerRedForUpFiled = GameObjectFactory.GenerateRedChecker(PositionsForDropCheckers[currentIndex + change], checkerSizeForUpField);
                this.UpGameField[currentIndex] = null;
                this.UpGameField[currentIndex + change] = checkerRedForUpFiled;
            }
            else
            {
                IGameObject checkerBlueForUpFiled = GameObjectFactory.GenerateBlueChecker(PositionsForDropCheckers[currentIndex + change], checkerSizeForUpField);
                this.UpGameField[currentIndex] = null;
                this.UpGameField[currentIndex + change] = checkerBlueForUpFiled;
            }
        }

        private void DropChecker(int currentIndex)
        {
            var col = currentIndex;
            for (int row = 0; row < this.GameField.GetLength(0); row++)
            {
                if (row <= this.GameField.GetLength(0) - 1)
                {
                    var isValidDropParameters = IsValidDropParameters(row, col);
                    if (isValidDropParameters)
                    {
                        Size checkerSizeForField = GameObjectFactory.GenerateSize(GlobalConstants.checkerWidth, GlobalConstants.checkerHeight);
                        if (isRedTurn)
                        {
                            IGameObject checkerForFiled = GameObjectFactory.GenerateRedChecker(this.PositionsGameField[row, col], checkerSizeForField);
                            this.GameField[row, col] = checkerForFiled;
                            IGameObject checkerForUpFiled = GameObjectFactory.GenerateBlueChecker(this.UpGameField[currentIndex].Position, checkerSizeForField);
                            this.UpGameField[currentIndex] = null;
                            this.UpGameField[currentIndex] = checkerForUpFiled;
                            isRedTurn = !isRedTurn;
                        }
                        else
                        {
                            IGameObject checkerForFiled = GameObjectFactory.GenerateBlueChecker(this.PositionsGameField[row, col], checkerSizeForField);
                            this.GameField[row, col] = checkerForFiled;
                            IGameObject checkerForUpFiled = GameObjectFactory.GenerateRedChecker(this.UpGameField[currentIndex].Position, checkerSizeForField);
                            this.UpGameField[currentIndex] = null;
                            this.UpGameField[currentIndex] = checkerForUpFiled;
                            isRedTurn = !isRedTurn;
                        }
                        break;
                    }
                }

            }
        }

        private bool IsValidDropParameters(int row, int col)
        {
            //var isОnТheBottom = row == this.GameField.GetLength(0) - 1;
            //var isNextFieldIsОccupied = this.GameField[row + 1, col] != null;
            if (row == this.GameField.GetLength(0) - 1 || this.GameField[row + 1, col] != null)
            {
                var isThereAreFreeCell = IsFreeCellsInColumn(col);
                if (isThereAreFreeCell)
                {
                    return true;
                }
            }

            return false;
        }

        private int FindCurrentIndexInUpGameField()
        {
            var number = -1;
            for (int i = 0; i < this.UpGameField.Length; i++)
            {
                if (this.UpGameField[i] != null)
                {
                    number = i;
                    break;
                }
            }
            return number;
        }

        private bool IsInRangeUpGameFiled(int currentIndex, int change)
        {
            bool isCorrectlyLeftPosition;
            bool isCorrectlyRightPosition;

            if (change == 1)
            {
                isCorrectlyLeftPosition = currentIndex >= 0;
                isCorrectlyRightPosition = currentIndex < this.PositionsForDropCheckers.Length - 1;
            }
            else
            {
                isCorrectlyLeftPosition = currentIndex > 0;
                isCorrectlyRightPosition = currentIndex <= this.PositionsForDropCheckers.Length - 1;
            }

            if (isCorrectlyLeftPosition && isCorrectlyRightPosition)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void InitGame()
        {
            InitPositionGameField();
            InitPositionUpField();
            InitStartChecker();
            InitRemindCheckers();
            InitBoard();

            //this.renderer.ShowStartGameScreen();
        }

        public void StartGame()
        {
            this.Renderer.Draw(this.UpGameField);
            this.Renderer.DrawPleyField(this.GameField);
            this.Renderer.Draw(this.Board);
            this.Renderer.Draw(this.RemainingRedCheckers);
            this.Renderer.Draw(this.RemainingBlueCheckers);


            timer.Interval = TimeSpan.FromMilliseconds(GlobalConstants.timerFramesIntervalInMiliSeconds);
            timer.Tick += (sender, args) =>
            {
                bool isWinner = CheckForWinner();
                if (isWinner)
                {
                    this.renderer.Clear();
                    this.Renderer.Draw(this.Board);
                    this.Renderer.Draw(this.upGameField);
                    this.Renderer.DrawPleyField(this.GameField);
                    this.Renderer.Draw(this.Board);
                    this.Renderer.Draw(this.RemainingRedCheckers);
                    this.Renderer.Draw(this.RemainingBlueCheckers);
                    this.Renderer.DrawWinLine(this.StartPositionForWinGame, this.EndPositionForWinGame);
                    this.Renderer.RefreshGame();

                    System.Threading.Thread.Sleep(4000);
                    this.Renderer.StopEventHandler();
                    timer.Stop();

                    Position winnerTextPoition = GameObjectFactory.GeneratePosition(500, 500);
                    Size winnerTextSize = GameObjectFactory.GenerateSize(350, 200);
                    string winner = isRedTurn ? GlobalConstants.PlayerBlue : GlobalConstants.PlayerRed;
                    string winnerMessege = string.Format(GlobalConstants.WinnerMessage, winner);
                    ITextGameObject winnerText = GameObjectFactory.GenerateMessage(winnerTextPoition, winnerTextSize, winnerMessege);

                    this.Renderer.ShowEndGameScreen(winnerText);

                    //Environment.Exit(0);
                }
            };
            timer.Start();
        }

        private void InitBoard()
        {
            Position boardPosition = new Position(150, 50);
            Size boardSize = new Size(GlobalConstants.boardWidth, GlobalConstants.boardHeight);
            this.Board = GameObjectFactory.GenerateBoard(boardPosition, boardSize);
        }

        private void InitStartChecker()
        {
            var startPosition = PositionsForDropCheckers[3];
            var checkerSize = GameObjectFactory.GenerateSize(GlobalConstants.startCheckerWidth, GlobalConstants.startCheckerHeight);
            this.Checker = GameObjectFactory.GenerateRedChecker(startPosition, checkerSize);
            this.UpGameField[3] = this.Checker;
        }

        private void InitPositionUpField()
        {
            this.UpGameField = new GameObject[7];
            this.PositionsForDropCheckers = new Position[7];

            var startLeftPosition = GlobalConstants.startLeftPositionForUpCheckers;
            var startTopPosition = GlobalConstants.startTopPositionForUpCheckers;
            for (int i = 0; i < PositionsForDropCheckers.Length; i++)
            {
                PositionsForDropCheckers[i] = GameObjectFactory.GeneratePosition(startLeftPosition, startTopPosition);
                startLeftPosition += GlobalConstants.spacingBetweenCells;
            }
        }

        private void InitPositionGameField()
        {
            this.GameField = new GameObject[6, 7];
            this.PositionsGameField = new Position[6, 7];

            int currentLeftPosition = GlobalConstants.startLeftPositionForUpCheckers;
            int currentTopPosition = GlobalConstants.leftSpacing;
            for (int row = 0; row < PositionsGameField.GetLength(0); row++)
            {
                currentTopPosition += GlobalConstants.topSpacing;
                for (int col = 0; col < PositionsGameField.GetLength(1); col++)
                {
                    PositionsGameField[row, col] = GameObjectFactory.GeneratePosition(currentLeftPosition, currentTopPosition);
                    currentLeftPosition += GlobalConstants.spacingBetweenCells;
                }

                currentLeftPosition = GlobalConstants.startLeftPositionForUpCheckers;
            }
        }

        private void InitRemindCheckers()
        {
            this.RemainingRedCheckers = new CheckerRed[20];
            this.PositionRemainingRedCheckers = new Position[20];
            this.RemainingBlueCheckers = new CheckerBlue[21];
            this.PositionRemainingBlueCheckers = new Position[21];

            FillPositionsForReminderCheckers();
        }

        private void FillPositionsForReminderCheckers()
        {
            Size sizeForReminderCheckers = GameObjectFactory.GenerateSize(GlobalConstants.remindCheckerWidth, GlobalConstants.remindCheckerHeight);

            // fill positions for red
            var startPositionLeftReminderCheckerRed = GlobalConstants.remindCheckerStartPosLeft;
            var startPositionTopReminderCheckerRed = GlobalConstants.remindCheckerStartPosTop;
            bool isBeginSecondLineForRedReminderCheckers = true;
            for (int i = 0; i < this.PositionRemainingRedCheckers.Length; i++)
            {
                if (i > this.PositionRemainingRedCheckers.Length / 2 && isBeginSecondLineForRedReminderCheckers)
                {
                    startPositionLeftReminderCheckerRed = GlobalConstants.remindCheckerStartPosLeft + GlobalConstants.spacingBetweenRemindCheckers;
                    startPositionTopReminderCheckerRed = GlobalConstants.remindCheckerStartPosTop;
                    isBeginSecondLineForRedReminderCheckers = false;
                }

                this.PositionRemainingRedCheckers[i] = GameObjectFactory.GeneratePosition(startPositionLeftReminderCheckerRed, startPositionTopReminderCheckerRed);
                startPositionTopReminderCheckerRed += GlobalConstants.spacingBetweenRemindCheckers;
            }

            for (int i = 0; i < this.RemainingRedCheckers.Length; i++)
            {
                var currentPosition = this.PositionRemainingRedCheckers[i];
                this.RemainingRedCheckers[i] = GameObjectFactory.GenerateRedChecker(currentPosition, sizeForReminderCheckers);
            }

            // fill positions for blue
            var startPositionLeftReminderCheckerBlue = GlobalConstants.remindCheckerStartPosLeft + GlobalConstants.spacingBetweenRemindCheckers * 3;
            var startPositionTopReminderCheckerBlue = GlobalConstants.remindCheckerStartPosTop;
            bool isBeginSecondLineForBlueReminderCheckers = true;
            for (int i = 0; i < this.PositionRemainingBlueCheckers.Length; i++)
            {
                if (i > this.PositionRemainingBlueCheckers.Length / 2 && isBeginSecondLineForBlueReminderCheckers)
                {
                    startPositionLeftReminderCheckerBlue = GlobalConstants.remindCheckerStartPosLeft + GlobalConstants.spacingBetweenRemindCheckers * 3 + GlobalConstants.spacingBetweenRemindCheckers;
                    startPositionTopReminderCheckerBlue = GlobalConstants.remindCheckerStartPosTop;
                    isBeginSecondLineForBlueReminderCheckers = false;
                }

                this.PositionRemainingBlueCheckers[i] = GameObjectFactory.GeneratePosition(startPositionLeftReminderCheckerBlue, startPositionTopReminderCheckerBlue);
                startPositionTopReminderCheckerBlue += GlobalConstants.spacingBetweenRemindCheckers;
            }

            for (int i = 0; i < this.RemainingBlueCheckers.Length; i++)
            {
                var currentPosition = this.PositionRemainingBlueCheckers[i];
                this.RemainingBlueCheckers[i] = GameObjectFactory.GenerateBlueChecker(currentPosition, sizeForReminderCheckers);
            }
        }

        private bool CheckForWinner()
        {
            for (int row = 0; row < this.GameField.GetLength(0); row++)
            {
                for (int col = 0; col < this.GameField.GetLength(1); col++)
                {
                    if (this.GameField[row, col] == null)
                    {
                        continue;
                    }

                    IGameObject currentPlayer = this.GameField[row, col];

                    bool isUpLine = CheckUp(row, col, currentPlayer.GetType());
                    bool isUpRightLine = CheckUpRight(row, col, currentPlayer.GetType());
                    bool isRightLine = CheckRihgt(row, col, currentPlayer.GetType());
                    bool isDownRightLine = CheckDownRight(row, col, currentPlayer.GetType());
                    bool isDownLine = CheckDown(row, col, currentPlayer.GetType());
                    bool isDownLeftLine = CheckDownLeft(row, col, currentPlayer.GetType());
                    bool isLeftLine = CheckLeft(row, col, currentPlayer.GetType());
                    bool isUpLeftLine = CheckUpLeft(row, col, currentPlayer.GetType());

                    bool isWinner =
                        isUpLine ||
                        isUpRightLine ||
                        isRightLine ||
                        isDownRightLine ||
                        isDownLine ||
                        isDownLeftLine ||
                        isLeftLine ||
                        isUpLeftLine;

                    if (isWinner)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool CheckUp(int row, int col, Type currentPlayer)
        {
            bool isOutOfUpRange = row - 3 < 0;
            if (isOutOfUpRange)
            {
                return false;
            }
            else if (this.GameField[row - 1, col] == null ||
                this.GameField[row - 2, col] == null ||
                this.GameField[row - 3, col] == null ||
                this.GameField[row - 1, col].GetType() != currentPlayer ||
                this.GameField[row - 2, col].GetType() != currentPlayer ||
                this.GameField[row - 3, col].GetType() != currentPlayer)
            {
                return false;
            }
            else
            {
                this.StartPositionForWinGame = this.PositionsGameField[row, col];
                this.EndPositionForWinGame = this.PositionsGameField[row - 3, col];
                return true;
            }
        }

        private bool CheckUpRight(int row, int col, Type currentPlayer)
        {
            bool isOutOfUpRange = row - 3 < 0;
            bool isOutOfRightRange = col + 3 > this.GameField.GetLength(1) - 1;
            if (isOutOfUpRange || isOutOfRightRange)
            {
                return false;
            }

            bool firstUpRightIsNull = this.GameField[row - 1, col + 1] == null;
            bool secondUpRightIsNull = this.GameField[row - 2, col + 2] == null;
            bool thirdUpRightIsNull = this.GameField[row - 3, col + 3] == null;
            bool isНeighborsCellAreNull = firstUpRightIsNull || secondUpRightIsNull || thirdUpRightIsNull;
            if (isНeighborsCellAreNull)
            {
                return false;
            }

            bool firstUpRight = this.GameField[row - 1, col + 1].GetType() != currentPlayer;
            bool secondUpRight = this.GameField[row - 2, col + 2].GetType() != currentPlayer;
            bool thirdUpRight = this.GameField[row - 3, col + 3].GetType() != currentPlayer;
            bool isНeighborsCellAreDifferentChecker = firstUpRight || secondUpRight || thirdUpRight;
            if (isНeighborsCellAreDifferentChecker)
            {
                return false;
            }
            else
            {
                this.StartPositionForWinGame = this.PositionsGameField[row, col];
                this.EndPositionForWinGame = this.PositionsGameField[row - 3, col + 3];
                return true;
            }
        }

        private bool CheckRihgt(int row, int col, Type currentPlayer)
        {
            bool isOutOfRightRange = col + 3 > this.GameField.GetLength(1) - 1;
            if (isOutOfRightRange)
            {
                return false;
            }

            bool firstRightIsNull = this.GameField[row, col + 1] == null;
            bool secondightIsNull = this.GameField[row, col + 2] == null;
            bool thirdRightIsNull = this.GameField[row, col + 3] == null;
            bool isНeighborsCellAreNull = firstRightIsNull || secondightIsNull || thirdRightIsNull;
            if (isНeighborsCellAreNull)
            {
                return false;
            }

            bool firstRight = this.GameField[row, col + 1].GetType() != currentPlayer;
            bool secondRight = this.GameField[row, col + 2].GetType() != currentPlayer;
            bool thirdRight = this.GameField[row, col + 3].GetType() != currentPlayer;
            bool isНeighborsCellAreDifferentChecker = firstRight || secondRight || thirdRight;
            if (isНeighborsCellAreDifferentChecker)
            {
                return false;
            }
            else
            {
                this.StartPositionForWinGame = this.PositionsGameField[row, col];
                this.EndPositionForWinGame = this.PositionsGameField[row, col + 3];
                return true;
            }
        }

        private bool CheckDownRight(int row, int col, Type currentPlayer)
        {
            bool isOutOfDownRange = row + 3 > this.GameField.GetLength(0) - 1;
            bool isOutOfRightRange = col + 3 > this.GameField.GetLength(1) - 1;
            if (isOutOfDownRange || isOutOfRightRange)
            {
                return false;
            }

            bool firstDownRightIsNull = this.GameField[row + 1, col + 1] == null;
            bool secondDownRightIsNull = this.GameField[row + 2, col + 2] == null;
            bool thirdDownRightIsNull = this.GameField[row + 3, col + 3] == null;
            bool isНeighborsCellAreNull = firstDownRightIsNull || secondDownRightIsNull || thirdDownRightIsNull;
            if (isНeighborsCellAreNull)
            {
                return false;
            }

            bool firstDownRight = this.GameField[row + 1, col + 1].GetType() != currentPlayer;
            bool secondDownRight = this.GameField[row + 2, col + 2].GetType() != currentPlayer;
            bool thirdDownRight = this.GameField[row + 3, col + 3].GetType() != currentPlayer;
            bool isНeighborsCellAreDifferentChecker = firstDownRight || secondDownRight || thirdDownRight;
            if (isНeighborsCellAreDifferentChecker)
            {
                return false;
            }
            else
            {
                this.StartPositionForWinGame = this.PositionsGameField[row, col];
                this.EndPositionForWinGame = this.PositionsGameField[row + 3, col + 3];
                return true;
            }
        }

        private bool CheckDown(int row, int col, Type currentPlayer)
        {
            bool isOutOfDownRange = row + 3 > this.GameField.GetLength(0) - 1;
            if (isOutOfDownRange)
            {
                return false;
            }

            bool firstDownIsNull = this.GameField[row + 1, col] == null;
            bool secondDownIsNull = this.GameField[row + 2, col] == null;
            bool thirdDownIsNull = this.GameField[row + 3, col] == null;
            bool isНeighborsCellAreNull = firstDownIsNull || secondDownIsNull || thirdDownIsNull;
            if (isНeighborsCellAreNull)
            {
                return false;
            }

            bool firstDown = this.GameField[row + 1, col].GetType() != currentPlayer;
            bool secondDown = this.GameField[row + 2, col].GetType() != currentPlayer;
            bool thirdDown = this.GameField[row + 3, col].GetType() != currentPlayer;
            bool isНeighborsCellAreDifferentChecker = firstDown || secondDown || thirdDown;
            if (isНeighborsCellAreDifferentChecker)
            {
                return false;
            }
            else
            {
                this.StartPositionForWinGame = this.PositionsGameField[row, col];
                this.EndPositionForWinGame = this.PositionsGameField[row + 3, col];
                return true;
            }
        }

        private bool CheckDownLeft(int row, int col, Type currentPlayer)
        {
            bool isOutOfDownRange = row + 3 > this.GameField.GetLength(0) - 1;
            bool isOutOfLeftRange = col - 3 < 0;
            if (isOutOfDownRange || isOutOfLeftRange)
            {
                return false;
            }

            bool firstDownLeftIsNull = this.GameField[row + 1, col - 1] == null;
            bool secondDownLeftIsNull = this.GameField[row + 2, col - 2] == null;
            bool thirdDownLeftIsNull = this.GameField[row + 3, col - 3] == null;
            bool isНeighborsCellAreNull = firstDownLeftIsNull || secondDownLeftIsNull || thirdDownLeftIsNull;
            if (isНeighborsCellAreNull)
            {
                return false;
            }

            bool firstDownLeft = this.GameField[row + 1, col - 1].GetType() != currentPlayer;
            bool secondDownLeft = this.GameField[row + 2, col - 2].GetType() != currentPlayer;
            bool thirdDownLeft = this.GameField[row + 3, col - 3].GetType() != currentPlayer;
            bool isНeighborsCellAreDifferentChecker = firstDownLeft || secondDownLeft || thirdDownLeft;
            if (isНeighborsCellAreDifferentChecker)
            {
                return false;
            }
            else
            {
                this.StartPositionForWinGame = this.PositionsGameField[row, col];
                this.EndPositionForWinGame = this.PositionsGameField[row + 3, col - 3];
                return true;
            }
        }

        private bool CheckLeft(int row, int col, Type currentPlayer)
        {
            bool isOutOfLeftRange = col - 3 < 0;
            if (isOutOfLeftRange)
            {
                return false;
            }

            bool firstDownLeftIsNull = this.GameField[row, col - 1] == null;
            bool secondDownLeftIsNull = this.GameField[row, col - 2] == null;
            bool thirdDownLeftIsNull = this.GameField[row, col - 3] == null;
            bool isНeighborsCellAreNull = firstDownLeftIsNull || secondDownLeftIsNull || thirdDownLeftIsNull;
            if (isНeighborsCellAreNull)
            {
                return false;
            }

            bool firstDownLeft = this.GameField[row, col - 1].GetType() != currentPlayer;
            bool secondDownLeft = this.GameField[row, col - 2].GetType() != currentPlayer;
            bool thirdDownLeft = this.GameField[row, col - 3].GetType() != currentPlayer;
            bool isНeighborsCellAreDifferentChecker = firstDownLeft || secondDownLeft || thirdDownLeft;
            if (isНeighborsCellAreDifferentChecker)
            {
                return false;
            }
            else
            {
                this.StartPositionForWinGame = this.PositionsGameField[row, col];
                this.EndPositionForWinGame = this.PositionsGameField[row, col - 3];
                return true;
            }
        }

        private bool CheckUpLeft(int row, int col, Type currentPlayer)
        {
            bool isOutOfUpRange = row - 3 < 0;
            bool isOutOfLeftRange = col - 3 < 0;
            if (isOutOfUpRange || isOutOfLeftRange)
            {
                return false;
            }

            bool firstDownLeftIsNull = this.GameField[row - 1, col - 1] == null;
            bool secondDownLeftIsNull = this.GameField[row - 2, col - 2] == null;
            bool thirdDownLeftIsNull = this.GameField[row - 3, col - 3] == null;
            bool isНeighborsCellAreNull = firstDownLeftIsNull || secondDownLeftIsNull || thirdDownLeftIsNull;
            if (isНeighborsCellAreNull)
            {
                return false;
            }

            bool firstDownLeft = this.GameField[row - 1, col - 1].GetType() != currentPlayer;
            bool secondDownLeft = this.GameField[row - 2, col - 2].GetType() != currentPlayer;
            bool thirdDownLeft = this.GameField[row - 3, col - 3].GetType() != currentPlayer;
            bool isНeighborsCellAreDifferentChecker = firstDownLeft || secondDownLeft || thirdDownLeft;
            if (isНeighborsCellAreDifferentChecker)
            {
                return false;
            }
            else
            {
                this.StartPositionForWinGame = this.PositionsGameField[row, col];
                this.EndPositionForWinGame = this.PositionsGameField[row - 3, col - 3];
                return true;
            }
        }
    }
}
