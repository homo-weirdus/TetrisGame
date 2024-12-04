using System;
using System.Collections.Generic;
using System.Windows.Shapes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace TetrisGame
{
    //COLIN 11/8: Drawing class. a lot of this will probably get replaced but for now it will do
    //COLIN 11/8: not going to bother with a todo list because if it gets rewritten it is pointless
    public class DrawingStuff
    {
        //COLIN 11/8: rectangles are just the easiest way to do the drawing. if we want to add more detail we will need to do something different
        //COLIN 11/8: using a grid and setting the rectangles to strech to the edge of the grid space works well enough
        public static Rectangle[][] boardDrawArr = new Rectangle[20][];
        //COLIN 11/8: active tetromino drawing is just overlayed on top of the rectangle for the board, but with a higher zindex to be on top
        public static Rectangle[] activeDrawArr = new Rectangle[4];
        public static Rectangle[] ghostDrawArr = new Rectangle[4];
        //COLIN 11/8: just an array of colors to use for tetrominoes. colors arent exact and if we change how drawing works we will need to rewrite it anyways
        public static Brush[] brusharr = { Brushes.Black, Brushes.SkyBlue, Brushes.Purple, Brushes.Yellow, Brushes.Blue, Brushes.Orange, Brushes.Green, Brushes.Red };
        public static Image[] nextdrawarr = new Image[5];
        public static Uri[] urishortcut = new Uri[8];
        public static bool maderects = false;
        //COLIN 11/8: generates the rectangles. not much more to say
        public static void initialRects()
        {
            BitmapImage tempbmp;
            for(int i = 0; i < 20; i++)
            {
                boardDrawArr[i] = new Rectangle[10];
                for (int j = 0; j < 10; j++)
                {
                    boardDrawArr[i][j] = new Rectangle();
                    boardDrawArr[i][j].VerticalAlignment = VerticalAlignment.Stretch;
                    boardDrawArr[i][j].HorizontalAlignment = HorizontalAlignment.Stretch;
                    boardDrawArr[i][j].Fill = Brushes.Black;
                    Grid.SetRow(boardDrawArr[i][j], i);
                    Grid.SetColumn(boardDrawArr[i][j], j);
                    ((MainWindow)Application.Current.MainWindow).BoardGrid.Children.Add(boardDrawArr[i][j]);
                }
            }
            //COLIN 11/8: just making the active tetromino rectangles invisible and shoving them in the top left corner at the start. probably needs to be done differently.
            for(int k = 0; k< 4; k++)
            {
                activeDrawArr[k] = new Rectangle();
                activeDrawArr[k].VerticalAlignment = VerticalAlignment.Stretch;
                activeDrawArr[k].HorizontalAlignment = HorizontalAlignment.Stretch;
                activeDrawArr[k].Fill = brusharr[((MainWindow)Application.Current.MainWindow).mainLogic.curtet.minotype];
                activeDrawArr[k].Visibility = Visibility.Hidden;

                Grid.SetColumn(activeDrawArr[k], 0);
                Grid.SetRow(activeDrawArr[k], 0);

                ((MainWindow)Application.Current.MainWindow).BoardGrid.Children.Add(activeDrawArr[k]);
                ghostDrawArr[k] = new Rectangle();
                ghostDrawArr[k].VerticalAlignment = VerticalAlignment.Stretch;
                ghostDrawArr[k].HorizontalAlignment = HorizontalAlignment.Stretch;
                ghostDrawArr[k].Stroke = brusharr[((MainWindow)Application.Current.MainWindow).mainLogic.curtet.minotype];
                ghostDrawArr[k].StrokeThickness = 4;
                //ghostDrawArr[k].str = brusharr[((MainWindow)Application.Current.MainWindow).mainLogic.curtet.minotype];
                ghostDrawArr[k].Visibility = Visibility.Visible;

                Grid.SetColumn(ghostDrawArr[k], 0);
                Grid.SetRow(ghostDrawArr[k], 0);

                ((MainWindow)Application.Current.MainWindow).BoardGrid.Children.Add(ghostDrawArr[k]);
            }
            urishortcut[0] = new Uri("pack://application:,,,/empty.png");
            urishortcut[1] = new Uri("pack://application:,,,/i.png");
            urishortcut[2] = new Uri("pack://application:,,,/t.png");
            urishortcut[3] = new Uri("pack://application:,,,/o.png");
            urishortcut[4] = new Uri("pack://application:,,,/j.png");
            urishortcut[5] = new Uri("pack://application:,,,/l.png");
            urishortcut[6] = new Uri("pack://application:,,,/s.png");
            urishortcut[7] = new Uri("pack://application:,,,/z.png");
            for (int l = 0; l < 5; l++)
            {
                nextdrawarr[l] = new Image();
                nextdrawarr[l].Source = new BitmapImage(urishortcut[0]);
                nextdrawarr[l].VerticalAlignment = VerticalAlignment.Top;
                nextdrawarr[l].HorizontalAlignment = HorizontalAlignment.Left;
                nextdrawarr[l].Margin = new Thickness(0, ((l*75)), 0, 0 );
                nextdrawarr[l].Width = 100;
                nextdrawarr[l].Height = 75;
                nextdrawarr[l].SetValue(RenderOptions.BitmapScalingModeProperty, BitmapScalingMode.NearestNeighbor);
                ((MainWindow)Application.Current.MainWindow).nextgrid.Children.Add(nextdrawarr[l]);
            }
            maderects = true;
        }
        public static void updateboard()
        {
            for(int i = GameLogic.highestfillrow; i < 20; i++)
            {
                for(int j = 0; j < 10; j++)
                {
                    boardDrawArr[i][j].Fill = brusharr[GameLogic.board[i][j]];
                }
            }
        }
        public static void drawghost()
        {
            sbyte drawy;
           
            for (int i = 0; i < 4; i++)
            {
                drawy = (sbyte)(((MainWindow)Application.Current.MainWindow).mainLogic.curtet.minoposy[i] - ((MainWindow)Application.Current.MainWindow).mainLogic.curtet.positiony + ((MainWindow)Application.Current.MainWindow).mainLogic.curtet.ghostpos);
                if(drawy  >= 0)
                {
                    Grid.SetRow(ghostDrawArr[i], drawy);
                    Grid.SetColumn(ghostDrawArr[i], ((MainWindow)Application.Current.MainWindow).mainLogic.curtet.minoposx[i]);
                    ghostDrawArr[i].Visibility = Visibility.Visible;
                }
                else
                {
                    ghostDrawArr[i].Visibility = Visibility.Hidden;
                }
                
            }
        }
        public static void updatescores(int scoresent, int linessent, int lvlsent)
        {
            string scorestr;
            string linestr = "lines cleared: ";
            string lvlstr = "Level: ";
            scorestr = scoresent.ToString("D10");
            linestr += linessent.ToString("D4");
            lvlstr += lvlsent.ToString("D2");
            ((MainWindow)Application.Current.MainWindow).scoretext.Text = scorestr;
            ((MainWindow)Application.Current.MainWindow).linestext.Text = linestr;
            ((MainWindow)Application.Current.MainWindow).leveltext.Text = lvlstr;
        }
    }
}
