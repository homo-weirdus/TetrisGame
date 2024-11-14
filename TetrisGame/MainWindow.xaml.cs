using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TetrisGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    //COLIN 11/8: this is the class for the main window. a lot of this is auto generated from the xaml designer
    public partial class MainWindow : Window
    {
        public GameLogic mainLogic;
        public MainWindow()
        {

            InitializeComponent();
            mainLogic = new GameLogic();
            mainLogic.startGame();
        }
        //COLIN 11/8: keydown event. need to change how this works in the future but its good enough for now.
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            int rottest;
            if(e.Key == Key.Left)
            {
                if(((MainWindow)Application.Current.MainWindow).mainLogic.curtet.TryMoveX(true) == true)
                {
                    ((MainWindow)Application.Current.MainWindow).mainLogic.curtet.MoveX(true);
                }
            }
            else if (e.Key == Key.Right)
            {
                if (((MainWindow)Application.Current.MainWindow).mainLogic.curtet.TryMoveX(false) == true)
                {
                    ((MainWindow)Application.Current.MainWindow).mainLogic.curtet.MoveX(false);
                }
            }
            else if (e.Key == Key.Down)
            {
                if (((MainWindow)Application.Current.MainWindow).mainLogic.curtet.TrySoftDrop() == true)
                {
                    ((MainWindow)Application.Current.MainWindow).mainLogic.curtet.softDrop();
                }
            }
            else if(e.Key == Key.Space)
            {
                ((MainWindow)Application.Current.MainWindow).mainLogic.curtet.LockActive();
                ((MainWindow)Application.Current.MainWindow).mainLogic.curtet.NextPiece(mainLogic.randomizer.NextPiece());
            }
            else if(e.Key == Key.X)
            {
                rottest = ((MainWindow)Application.Current.MainWindow).mainLogic.curtet.TryRotate(true);
                if(rottest != 9)
                {
                    Debug.WriteLine("rotaaate");
                    ((MainWindow)Application.Current.MainWindow).mainLogic.curtet.RotateDir(true, rottest);
                }
            }
            else if(e.Key == Key.Z)
            {
                rottest = ((MainWindow)Application.Current.MainWindow).mainLogic.curtet.TryRotate(false);
                if (rottest != 9)
                {
                    ((MainWindow)Application.Current.MainWindow).mainLogic.curtet.RotateDir(false, rottest);
                }
            }
        }
    }
}
