using MastersThesisMarianNikolov0124IMD.Contracts;
using MastersThesisMarianNikolov0124IMD.Engine;
using MastersThesisMarianNikolov0124IMD.Renderers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MastersThesisMarianNikolov0124IMD
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            IRenderer renderer = new WpfGameRenderer(this.GameCanvas);
            this.Engine = new GameEngine(renderer);
            this.Engine.InitGame();
            this.Engine.StartGame();
        }

        public IGameEngine Engine { get; set; }
    }

    public static class ExtensionMethods
    {
        // for refresh UI
        // https://social.msdn.microsoft.com/Forums/vstudio/en-US/878ea631-c76e-49e8-9e25-81e76a3c4fe3/refresh-the-gui-in-a-wpf-application?forum=wpf
        private static Action EmptyDelegate = delegate () { };

        public static void Refresh(this UIElement uiElement)
        {
            uiElement.Dispatcher.Invoke(DispatcherPriority.Render, EmptyDelegate);
        }
    }
}
