using System;
using System.Collections.Generic;
using System.Windows.Shapes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;

namespace TetrisGame
{
    //COLIN 11/8: Drawing class. a lot of this will probably get replaced but for now it will do
    //COLIN 11/8: not going to bother with a todo list because if it gets rewritten it is pointless
    public class DrawingStuff
    {
        //COLIN 11/8: rectangles are just the easiest way to do the drawing. if we want to add more detail we will need to do something different
        //COLIN 11/8: using a grid and setting the rectangles to strech to the edge of the grid space works well enough
        public static Rectangle[][] boardDrawArr = new Rectangle[10][];
        //COLIN 11/8: active tetromino drawing is just overlayed on top of the rectangle for the board, but with a higher zindex to be on top
        public static Rectangle[] activeDrawArr = new Rectangle[4];
        //COLIN 11/8: just an array of colors to use for tetrominoes. colors arent exact and if we change how drawing works we will need to rewrite it anyways
        public static Brush[] brusharr = { Brushes.Black, Brushes.SkyBlue, Brushes.Purple, Brushes.Yellow, Brushes.Blue, Brushes.Orange, Brushes.Green, Brushes.Red };
        //COLIN 11/8: generates the rectangles. not much more to say
        public static void initialRects()
        {
            for(int i = 0; i < 10; i++)
            {
                boardDrawArr[i] = new Rectangle[20];
                for (int j = 0; j < 20; j++)
                {
                    boardDrawArr[i][j] = new Rectangle();
                    boardDrawArr[i][j].VerticalAlignment = VerticalAlignment.Stretch;
                    boardDrawArr[i][j].HorizontalAlignment = HorizontalAlignment.Stretch;
                    boardDrawArr[i][j].Fill = Brushes.Black;
                    Grid.SetColumn(boardDrawArr[i][j], i);
                    Grid.SetRow(boardDrawArr[i][j], j);
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
            }
        }
    }
}
