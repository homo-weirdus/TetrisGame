using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;
using System.Reflection.Emit;

namespace TetrisGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    //COLIN 11/8: this is the class for the main window. a lot of this is auto generated from the xaml designer
    public partial class MainWindow : Window
    {
        public bool leftheld = false;
        public bool rightheld = false;
        public static bool gameover = false;
        public static bool paused = false;
        public GameLogic mainLogic;
        public Timer righttime;
        public Timer lefttime;
        public AutoResetEvent rightreset;
        public AutoResetEvent leftreset;
        public static bool downheld = false;
        public MainWindow()
        {

            InitializeComponent();

            mainLogic = new GameLogic();
            mainLogic.startGame();
            rightreset = new AutoResetEvent(false);
            leftreset = new AutoResetEvent(false);
            

        }
        //COLIN 11/8: keydown event. need to change how this works in the future but its good enough for now.
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            int rottest;

            if (gameover == false && paused == false)
            {
                if (e.Key == Key.Left || e.Key == Key.A)
                {
                    if (leftheld == false)
                    {
                        leftheld = true;
                        if (((MainWindow)Application.Current.MainWindow).mainLogic.curtet.TryMoveX(true) == true)
                        {
                            ((MainWindow)Application.Current.MainWindow).mainLogic.curtet.MoveX(true);
                        }
                        lefttime = new Timer(movle, leftreset, 200, 50);
                    }
                }
                else if (e.Key == Key.Right || e.Key == Key.D)
                {
                    if (rightheld == false)
                    {
                        rightheld = true;

                        if (((MainWindow)Application.Current.MainWindow).mainLogic.curtet.TryMoveX(false) == true)
                        {
                            ((MainWindow)Application.Current.MainWindow).mainLogic.curtet.MoveX(false);
                        }
                        righttime = new Timer(movri, rightreset, 200, 50);
                        //Task.Delay(200, rightcan.Token).ContinueWith(t => movri());
                        //rightcan.Cancel();

                    }
                }
                else if (e.Key == Key.Down || e.Key == Key.S)
                {
                    if (downheld == false)
                    {
                        downheld = true;
                        ((MainWindow)Application.Current.MainWindow).mainLogic.gravtimer.Interval = TimeSpan.FromMilliseconds(GameLogic.gravinterval / 12);
                    }
                    // if (((MainWindow)Application.Current.MainWindow).mainLogic.curtet.TrySoftDrop() == true)
                    // {
                    //    ((MainWindow)Application.Current.MainWindow).mainLogic.curtet.softDrop();
                    //}
                }
                else if (e.Key == Key.Space)
                {
                    ((MainWindow)Application.Current.MainWindow).mainLogic.curtet.HardDrop();
                    ((MainWindow)Application.Current.MainWindow).mainLogic.curtet.NextPiece(mainLogic.randomizer.NextPiece());
                }
                else if (e.Key == Key.X)
                {
                    rottest = ((MainWindow)Application.Current.MainWindow).mainLogic.curtet.TryRotate(true);
                    if (rottest != 9)
                    {
                        //Debug.WriteLine("rotaaate");
                        ((MainWindow)Application.Current.MainWindow).mainLogic.curtet.RotateDir(true, rottest);
                    }
                }
                else if (e.Key == Key.Z)
                {
                    rottest = ((MainWindow)Application.Current.MainWindow).mainLogic.curtet.TryRotate(false);
                    if (rottest != 9)
                    {
                        ((MainWindow)Application.Current.MainWindow).mainLogic.curtet.RotateDir(false, rottest);
                    }
                }
                else if (e.Key == Key.C)
                {
                    if (GameLogic.canhold == true)
                    {
                        ((MainWindow)Application.Current.MainWindow).mainLogic.curtet.HoldTet();
                        ((MainWindow)Application.Current.MainWindow).holdimg.Source = new BitmapImage(DrawingStuff.urishortcut[GameLogic.holdtype]);
                    }
                }
            }
            if(e.Key == Key.Escape)
            {
                if(MainWindow.paused == true)
                {
                    if(MainWindow.gameover == false)
                    {
                        ((MainWindow)Application.Current.MainWindow).mainLogic.gravtimer.Start();

                        ((MainWindow)Application.Current.MainWindow).savebutton.Visibility = Visibility.Hidden;
                    ((MainWindow)Application.Current.MainWindow).LoadButton.Visibility = Visibility.Hidden;
                    ((MainWindow)Application.Current.MainWindow).newbut.Visibility = Visibility.Hidden;
                    ((MainWindow)Application.Current.MainWindow).savebutton.IsEnabled = false;
                    ((MainWindow)Application.Current.MainWindow).LoadButton.IsEnabled = false;
                    ((MainWindow)Application.Current.MainWindow).newbut.IsEnabled = false;
                        paused = false;
                }
                }
                else
                {
                    MainWindow.paused = true;
                    MainWindow.pausegame();
                }
            }
        }
        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (gameover == false && paused == false)
            {
                //Debug.WriteLine("keyup");
                if (e.Key == Key.Right || e.Key == Key.D)
                {
                    rightheld = false;
                    //Debug.WriteLine("canceled");
                    righttime.Dispose();

                }
                else if (e.Key == Key.Left || e.Key == Key.A)
                {
                    leftheld = false;
                    //Debug.WriteLine("canceled");
                    lefttime.Dispose();
                }
                else if (e.Key == Key.Down || e.Key == Key.S)
                {
                    downheld = false;
                    //Debug.WriteLine("canceled");
                    //lefttime.Dispose();
                    ((MainWindow)Application.Current.MainWindow).mainLogic.gravtimer.Interval = TimeSpan.FromMilliseconds(GameLogic.gravinterval);
                }
            }
        }
        public void movri(Object stateInfo)
        {
            this.Dispatcher.Invoke(() =>
            {
                if (rightheld)
                {
                    if (((MainWindow)Application.Current.MainWindow).mainLogic.curtet.TryMoveX(false) == true)
                    {
                        ((MainWindow)Application.Current.MainWindow).mainLogic.curtet.MoveX(false);
                    }
                    //Task.Delay(66, rightcan.Token).ContinueWith(t => movri());
                }
            });
        }
        public void movle(Object stateInfo)
        {
            this.Dispatcher.Invoke(() =>
            {
                if (leftheld)
                {
                    if (((MainWindow)Application.Current.MainWindow).mainLogic.curtet.TryMoveX(true) == true)
                    {
                        ((MainWindow)Application.Current.MainWindow).mainLogic.curtet.MoveX(true);
                    }
                    //Task.Delay(66, rightcan.Token).ContinueWith(t => movri());
                }
            });
        }

        private void savebutton_Click(object sender, RoutedEventArgs e)
        {
            int baglef;
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "data"; // Default file name
            dlg.DefaultExt = ".sav"; // Default file extension
            dlg.Filter = "save data (.sav)|*.sav"; // Filter files by extension
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result.Value)
            {
                // Save document
                string filename = dlg.FileName;
                //FileStream fil = dlg.OpenFile();
                BinaryWriter rit = new BinaryWriter(dlg.OpenFile());
                rit.Write(GameLogic.score);
                rit.Write(GameLogic.totalcleared);
                rit.Write(GameLogic.level);
                rit.Write(((MainWindow)Application.Current.MainWindow).mainLogic.curtet.minotype);
                rit.Write(GameLogic.holdtype);
                for(int i = 0; i < 5; i++)
                {
                    rit.Write(((MainWindow)Application.Current.MainWindow).mainLogic.randomizer.nextQueue.ElementAt(i));
                }
                baglef = ((MainWindow)Application.Current.MainWindow).mainLogic.randomizer.bag.Count;
                rit.Write(baglef);
                for(int j = 0; j < baglef; j++)
                {
                    rit.Write(((MainWindow)Application.Current.MainWindow).mainLogic.randomizer.bag.ElementAt(j));
                }
                for(int k = 0; k < 20; k++)
                {
                    rit.Write(GameLogic.board[k]);
                }
            }
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            bool gothighest = false;
            int baglef;
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "data"; // Default file name
            dlg.DefaultExt = ".sav"; // Default file extension
            dlg.Filter = "save data (.sav)|*.sav"; // Filter files by extension

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result.Value)
            {
                byte minotsav;
                // Open document
                string filename = dlg.FileName;
                BinaryReader rit = new BinaryReader(dlg.OpenFile());
                GameLogic.score = rit.ReadInt32();
                GameLogic.totalcleared = rit.ReadInt32();
                GameLogic.level = rit.ReadInt32();
                minotsav = rit.ReadByte();
                GameLogic.holdtype = rit.ReadByte();
                ((MainWindow)Application.Current.MainWindow).holdimg.Source = new BitmapImage(DrawingStuff.urishortcut[GameLogic.holdtype]);
                ((MainWindow)Application.Current.MainWindow).mainLogic.randomizer.nextQueue.Clear();
                for (int i = 0; i < 5; i++)
                {
                    ((MainWindow)Application.Current.MainWindow).mainLogic.randomizer.nextQueue.Enqueue(rit.ReadByte());
                }
                baglef = rit.ReadInt32();
                ((MainWindow)Application.Current.MainWindow).mainLogic.randomizer.bag.Clear();
                for (int j = 0; j < baglef; j++)
                {
                    ((MainWindow)Application.Current.MainWindow).mainLogic.randomizer.bag.Add(rit.ReadByte());
                }
                for (int k = 0; k < 20; k++)
                {
                    GameLogic.board[k] = rit.ReadBytes(10);
                    //rit.Write(GameLogic.board[k]);
                }
                GameLogic.highestfillrow = 0;
                DrawingStuff.updateboard();
                for(int m = 0; m < 20; m++)
                {
                    for(int l = 0; l < 10; l++)
                    {
                        if (GameLogic.board[m][l] != 0)
                        {
                            gothighest = true;
                            GameLogic.highestfillrow = (byte)m;
                            break;
                        }
                        
                    }
                    if (gothighest == true)
                    {
                        break;
                    }
                }
                GameLogic.gravinterval = (int)((Math.Pow((float)(800 - (GameLogic.level * 7)) / 1000, (GameLogic.level)) * 1000));
                ((MainWindow)Application.Current.MainWindow).mainLogic.gravtimer.Interval = TimeSpan.FromMilliseconds(GameLogic.gravinterval);
                ((MainWindow)Application.Current.MainWindow).mainLogic.curtet.NextPiece(minotsav);
            }
        }

        private void newbut_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).mainLogic.startGame();
        }
        public static void pausegame()
        {
            if(gameover == true)
            {
            }
            else
            {
                ((MainWindow)Application.Current.MainWindow).mainLogic.lockdowntimer.Stop();
                ((MainWindow)Application.Current.MainWindow).mainLogic.gravtimer.Stop();
                if(((MainWindow)Application.Current.MainWindow).leftheld == true)
                {
                    ((MainWindow)Application.Current.MainWindow).lefttime.Dispose();
                    ((MainWindow)Application.Current.MainWindow).leftheld = false;
                }
                if (((MainWindow)Application.Current.MainWindow).rightheld == true)
                {
                    ((MainWindow)Application.Current.MainWindow).righttime.Dispose();
                    ((MainWindow)Application.Current.MainWindow).rightheld = false;
                }
                downheld = false;
            }
            ((MainWindow)Application.Current.MainWindow).savebutton.Visibility = Visibility.Visible;
            ((MainWindow)Application.Current.MainWindow).LoadButton.Visibility = Visibility.Visible;
            ((MainWindow)Application.Current.MainWindow).newbut.Visibility = Visibility.Visible;
            ((MainWindow)Application.Current.MainWindow).savebutton.IsEnabled = true;
            ((MainWindow)Application.Current.MainWindow).LoadButton.IsEnabled = true;
            ((MainWindow)Application.Current.MainWindow).newbut.IsEnabled = true;
        }
    }
}
