using System;
using MastersThesisMarianNikolov0124IMD.Common;
using MastersThesisMarianNikolov0124IMD.Contracts;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using MastersThesisMarianNikolov0124IMD.GameObjects;
using System.Windows;
using System.Linq;
using System.Windows.Shapes;
using MastersThesisMarianNikolov0124IMD.Global;
using System.Windows.Media.Effects;

namespace MastersThesisMarianNikolov0124IMD.Renderers
{
    public delegate void KeyDownEventHandler(object obj, KeyDownEventArgs args);

    public class WpfGameRenderer : IRenderer
    {
        private Canvas canvas;

        public int ScreenHeigth
        {
            get
            {
                return (int)(this.canvas.Parent as MainWindow).Width;
            }
        }

        public int ScreenWidth
        {
            get
            {
                return (int)(this.canvas.Parent as MainWindow).Height;
            }
        }

        public event KeyDownEventHandler presingKey;

        public WpfGameRenderer(Canvas gameCanvas)
        {
            string pathToBackground = System.IO.Path.GetFullPath(@"..\..\Images\gameBackground.jpg");
            this.canvas = gameCanvas;
            ImageBrush myBrush = new ImageBrush();
            Image image = new Image();
            image.Source = new BitmapImage(new Uri(pathToBackground));
            myBrush.ImageSource = image.Source;
            this.canvas.Background = myBrush;

            (this.canvas.Parent as MainWindow).KeyDown += WpfGameRenderer_KeyDown;
        }

        private void WpfGameRenderer_KeyDown(object sender, KeyEventArgs args)
        {
            var key = args.Key;
            var isLeft = key == Key.Left;
            var isRihgt = key == Key.Right;
            var isEnter = key == Key.Enter;

            if (isLeft)
            {
                this.presingKey(this, new KeyDownEventArgs(GameCommandType.MoveLeft));
            }
            else if (isRihgt)
            {
                this.presingKey(this, new KeyDownEventArgs(GameCommandType.MoveRight));
            }
            else if (isEnter)
            {
                this.presingKey(this, new KeyDownEventArgs(GameCommandType.Falling));
            }
            else
            {
                this.presingKey(this, new KeyDownEventArgs(GameCommandType.WrongKey));
            }
        }

        public void RefreshGame()
        {
            this.canvas.Refresh();
        }

        public void Clear()
        {
            this.canvas.Children.Clear();
        }

        public void Draw(params IGameObject[] drawObjects)
        {
            foreach (var objectForDraw in drawObjects)
            {
                if (objectForDraw is CheckerRed)
                {
                    IGameObject drawingIMovable = objectForDraw as IGameObject;
                    DrawRedCheckers(objectForDraw);
                }
                if (objectForDraw is CheckerBlue)
                {
                    IGameObject drawingIMovable = objectForDraw as IGameObject;
                    DrawBlueCheckers(objectForDraw);
                }
                if (objectForDraw is GameBoard)
                {
                    IGameObject drawingIMovable = objectForDraw as IGameObject;
                    DrawGameBoard(objectForDraw);
                }
                if (objectForDraw is TextGameObject)
                {
                    if (objectForDraw is WrongKeyMessage)
                    {
                        WrongKeyMessage drawing = objectForDraw as WrongKeyMessage;
                        DrawText(drawing);
                    }
                    else
                    {
                        NextTurnMessage drawing = objectForDraw as NextTurnMessage;
                        DrawText(drawing as NextTurnMessage);
                    }
                }
            }
        }

        public void DrawPleyField(IGameObject[,] drawObjects)
        {
            for (int row = 0; row < drawObjects.GetLength(0); row++)
            {
                for (int col = 0; col < drawObjects.GetLength(1); col++)
                {
                    if (drawObjects[row, col] != null)
                    {
                        //IMovable drawingIMovable = drawObjects[row, col] as IMovable;
                        if (drawObjects[row, col] is CheckerRed)
                        {
                            IMovable drawingIMovable = drawObjects[row, col] as IMovable;
                            DrawRedCheckers(drawObjects[row, col]);
                        }
                        if (drawObjects[row, col] is CheckerBlue)
                        {
                            IMovable drawingIMovable = drawObjects[row, col] as IMovable;
                            DrawBlueCheckers(drawObjects[row, col]);
                        }
                    }
                }
            }
        }

        public void DrawGameBoard(IGameObject boardForDraw)
        {
            BitmapImage boardSource = new BitmapImage();
            boardSource.BeginInit();
            string path = System.IO.Path.GetFullPath(@"..\..\Images\gameBoard.png");
            boardSource.UriSource = new Uri(path);
            boardSource.EndInit();

            Image board = new Image();
            board.Source = boardSource;
            board.Height = boardForDraw.Bounds.Height;
            board.Width = boardForDraw.Bounds.Width;

            Canvas.SetLeft(board, boardForDraw.Position.Left);
            Canvas.SetTop(board, boardForDraw.Position.Top);
            board.Uid = "5";
            this.canvas.Children.Add(board);
        }

        private void DrawRedCheckers(IGameObject objectForDraw)
        {
            BitmapImage ballFacetSource = new BitmapImage();
            ballFacetSource.BeginInit();
            string path = System.IO.Path.GetFullPath(@"..\..\Images\redBall.png");
            ballFacetSource.UriSource = new Uri(path);
            ballFacetSource.EndInit();

            Image checker = new Image();
            checker.Source = ballFacetSource;
            checker.Height = objectForDraw.Bounds.Height;
            checker.Width = objectForDraw.Bounds.Width;

            Canvas.SetLeft(checker, objectForDraw.Position.Left);
            Canvas.SetTop(checker, objectForDraw.Position.Top);
            this.canvas.Children.Add(checker);
        }

        private void DrawBlueCheckers(IGameObject objectForDraw)
        {
            BitmapImage ballFacetSource = new BitmapImage();
            ballFacetSource.BeginInit();
            string path = System.IO.Path.GetFullPath(@"..\..\Images\blueBall.png");
            ballFacetSource.UriSource = new Uri(path);
            ballFacetSource.EndInit();

            Image checker = new Image();
            checker.Source = ballFacetSource;
            checker.Height = objectForDraw.Bounds.Height;
            checker.Width = objectForDraw.Bounds.Width;

            Canvas.SetLeft(checker, objectForDraw.Position.Left);
            Canvas.SetTop(checker, objectForDraw.Position.Top);
            this.canvas.Children.Add(checker);
        }

        public void DrawText(ITextGameObject drawing)
        {
            var text = new TextBlock()
            {
                Width = drawing.Bounds.Width,
                Height = drawing.Bounds.Height,
                Foreground = Brushes.White,
                Text = drawing.Text,
                FontSize = 33,
            };

            if (drawing is WrongKeyMessage)
            {
                text.Foreground = Brushes.Silver;
                text.FontSize = 55;
            }

            Canvas.SetLeft(text, drawing.Position.Left);
            Canvas.SetTop(text, drawing.Position.Top);
            this.canvas.Children.Add(text);
        }

        public void DrawWinLine(Position startPosition, Position endPosition)
        {
            Line myLine = new Line();

            myLine.X1 = startPosition.Left + GlobalConstants.checkerWidth / 2;
            myLine.Y1 = startPosition.Top + GlobalConstants.checkerWidth / 2;
            myLine.X2 = endPosition.Left + GlobalConstants.checkerWidth / 2;
            myLine.Y2 = endPosition.Top + GlobalConstants.checkerWidth / 2;

            myLine.Stroke = Brushes.AliceBlue;
            myLine.StrokeThickness = 20;
            //Rectangle myLine = new Rectangle();
            //myLine.Width = 222;
            //myLine.Width = 222;

            //SolidColorBrush blueBrush = new SolidColorBrush();
            //blueBrush.Color = Colors.Blue;
            //myLine.Fill = blueBrush;
            //myLine.RadiusX = 5;
            //myLine.RadiusY = 5;
            //Canvas.SetLeft(myLine, 0);
            //Canvas.SetRight(myLine, 222);
            this.canvas.Children.Add(myLine);
        }

        public void ShowStartGameScreen()
        {
            string pathBackground = System.IO.Path.GetFullPath(@"..\..\Images\gameBackground.jpg");
            ImageBrush myBrush = new ImageBrush();
            Image image = new Image();
            image.Source = new BitmapImage(new Uri(pathBackground));
            myBrush.ImageSource = image.Source;

            var window = new Window
            {
                WindowState = WindowState.Maximized,
                WindowStyle = WindowStyle.None,
                ResizeMode = ResizeMode.NoResize,
                Background = myBrush
            };
            window.Show();
            System.Threading.Thread.Sleep(3000);
            window.Close();
        }

        public void ShowEndGameScreen()
        {
            string pathBackground = System.IO.Path.GetFullPath(@"..\..\Images\gameBackground.jpg");
            ImageBrush myBrush = new ImageBrush();
            Image image = new Image();
            image.Source = new BitmapImage(new Uri(pathBackground));
            myBrush.ImageSource = image.Source;

            var endGameWindow = new Window
            {
                WindowState = WindowState.Maximized,
                WindowStyle = WindowStyle.None,
                ResizeMode = ResizeMode.NoResize,
                Background = myBrush
            };

          
            // prosto taka raboti WPF
            var parent = this.canvas.Parent;
            while (!(parent is Window))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }

            StackPanel panel = new StackPanel
            {
            };

            var buttonNewGame = new Button
            {
                FontSize = 20,
                Content = "PLAY AGAIN",
                Width = 250,
                Height = 50,
                Background = Brushes.Transparent,
                Foreground = Brushes.LightSkyBlue,
            };

            var buttonEnd = new Button
            {
                FontSize = 20,
                Content = "EXIT GAME",
                Width = 250,
                Height = 50,
                Background = Brushes.Transparent,
                Foreground = Brushes.LightSkyBlue,

            };

            buttonNewGame.Click += (snd, ev) =>
            {
                new MainWindow().Show();
                endGameWindow.Close();
            };

            buttonEnd.Click += (snd, ev) =>
            {
                Environment.Exit(0);
            };

            panel.Children.Add(buttonNewGame);
            panel.Children.Add(buttonEnd);

            endGameWindow.Content = panel;
            endGameWindow.Show();
            (parent as Window).Close();
        }

        public void StopEventHandler()
        {
            (this.canvas.Parent as MainWindow).KeyDown -= WpfGameRenderer_KeyDown;
        }

    }
}
