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
        private bool isRedMessage;

        private Canvas canvas;

        public int ScreenHeigth
        {
            get
            {
                return (int)(this.Canvas.Parent as MainWindow).Width;
            }
        }

        public int ScreenWidth
        {
            get
            {
                return (int)(this.Canvas.Parent as MainWindow).Height;
            }
        }

        public bool IsRedMessege
        {
            get
            {
                return this.isRedMessage;
            }

            set
            {
                this.isRedMessage = value;
            }
        }

        public Canvas Canvas
        {
            get
            {
                return this.canvas;
            }

            set
            {
                this.canvas = value;
            }
        }

        public event KeyDownEventHandler presingKey;

        public WpfGameRenderer(Canvas gameCanvas)
        {
            this.IsRedMessege = false;
            this.Canvas = gameCanvas;

            string pathToBackground = System.IO.Path.GetFullPath(@"..\..\Images\gameBackground.jpg");
            Image image = new Image();
            ImageBrush myBrush = new ImageBrush();
            image.Source = new BitmapImage(new Uri(pathToBackground));
            myBrush.ImageSource = image.Source;
            this.Canvas.Background = myBrush;

            (this.Canvas.Parent as MainWindow).KeyDown += WpfGameRenderer_KeyDown;
        }

        public void Draw(params IGameObject[] drawObjects)
        {
            foreach (var objectForDraw in drawObjects)
            {
                if (objectForDraw is CheckerRed)
                {
                    DrawRedCheckers(objectForDraw);
                }
                if (objectForDraw is CheckerBlue)
                {
                    DrawBlueCheckers(objectForDraw);
                }
                if (objectForDraw is GameBoard)
                {
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
                        if (drawObjects[row, col] is CheckerRed)
                        {
                            IGameObject drawingIMovable = drawObjects[row, col] as IGameObject;
                            DrawRedCheckers(drawObjects[row, col]);
                        }
                        if (drawObjects[row, col] is CheckerBlue)
                        {
                            IGameObject drawingIMovable = drawObjects[row, col] as IGameObject;
                            DrawBlueCheckers(drawObjects[row, col]);
                        }
                    }
                }
            }
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
            
            this.Canvas.Children.Add(myLine);
        }

        public void RefreshGame()
        {
            this.Canvas.Refresh();
        }

        public void Clear()
        {
            this.Canvas.Children.Clear();
        }

        public void ShowStartGameScreen()
        {
            string pathBackground = System.IO.Path.GetFullPath(@"..\..\Images\startGameBackground.jpg");
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
            System.Threading.Thread.Sleep(7000);
            window.Close();
        }

        public void ShowEndGameScreen(ITextGameObject winnerText)
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

            var parent = this.Canvas.Parent;
            while (!(parent is Window))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }

            StackPanel panel = new StackPanel
            {
            };

            var buttonNewGame = new Button
            {
                FontSize = 44,
                Content = "PLAY AGAIN",
                Width = 400,
                Height = 100,
                Background = Brushes.Transparent,
                Foreground = Brushes.LightSkyBlue,
                HorizontalAlignment = HorizontalAlignment.Center,
                BorderThickness = new Thickness(5),
            };

            var buttonEnd = new Button
            {
                FontSize = 40,
                Content = "EXIT GAME",
                Width = 400,
                Height = 100,
                Background = Brushes.Transparent,
                Foreground = Brushes.LightSkyBlue,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 33, 0, 0),
                BorderThickness = new Thickness(5),
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

            var textForPushDownMessageAndButtons = new TextBlock()
            {
                Width = panel.Width,
                Height = 150,
            };

            var text = new TextBlock()
            {
                Width = 700,
                Height = 300,
                Foreground = Brushes.White,
                Text = winnerText.Text,
                FontSize = 80,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(100, 0, 0, 0),
                FontWeight = FontWeights.ExtraBold,
                FontStyle = FontStyles.Oblique
            };

            panel.Children.Add(textForPushDownMessageAndButtons);
            panel.Children.Add(text);

            panel.Children.Add(buttonNewGame);
            panel.Children.Add(buttonEnd);

            endGameWindow.Content = panel;
            endGameWindow.Show();
            (parent as Window).Close();
        }

        public void StopEventHandler()
        {
            (this.Canvas.Parent as MainWindow).KeyDown -= WpfGameRenderer_KeyDown;
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
            this.Canvas.Children.Add(checker);
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
            this.Canvas.Children.Add(checker);
        }

        private void DrawGameBoard(IGameObject boardForDraw)
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
            this.Canvas.Children.Add(board);
        }

        private void DrawText(ITextGameObject drawing)
        {
            var text = new TextBlock()
            {
                Width = drawing.Bounds.Width,
                Height = drawing.Bounds.Height,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Text = drawing.Text,
                FontSize = 33,
            };

            if (drawing is NextTurnMessage)
            {
                if (IsRedMessege)
                {
                    text.Foreground = Brushes.White;
                    text.Background = Brushes.Red;
                    this.IsRedMessege = !this.IsRedMessege;
                }
                else
                {
                    text.Foreground = Brushes.White;
                    text.Background = Brushes.Blue;
                    this.IsRedMessege = !this.IsRedMessege;
                }
            }

            if (drawing is WrongKeyMessage)
            {
                text.Width = 470;
                text.Height = 80;
                text.Foreground = Brushes.Black;
                text.Background = Brushes.White;
                text.HorizontalAlignment = HorizontalAlignment.Center;
                text.VerticalAlignment = VerticalAlignment.Center;
                text.FontSize = 55;
            }

            Canvas.SetLeft(text, drawing.Position.Left);
            Canvas.SetTop(text, drawing.Position.Top);
            this.Canvas.Children.Add(text);
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
    }
}
