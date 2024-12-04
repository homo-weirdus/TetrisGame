using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace TetrisGame
{
    //COLIN 11/8: Randomizer class. for randomization and the next queue.  currently empty
    /*TODO:
     * class constructor
     * next queue array
     * bag remanining array
     * method to set the next active tetromino
     * method to generate the next tetromino in the queue
     * update drawing of next queue
     * seed the randomizer for if multiplayer is added
     * */

    //Rebeca 11/12: Added the randomizer class and integrated with GameLogic.cs
    public class Randomizer
    {
        public List<byte> bag;
        public Queue<byte> nextQueue;
        private const int QueueSize = 5; // Change this if you want more pieces in the queue
        private static readonly Random random = new Random();
        public Randomizer()
        {
            bag = new List<byte>();
            nextQueue = new Queue<byte>();
            RefillBag();
            InitializeQueue();
        }
        private void RefillBag()
        {
            
            bag.Clear();
            for (byte i = 1; i <= 7; i++)
            {
                bag.Add(i);
            }
            for (int i = bag.Count - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                (bag[i], bag[j]) = (bag[j], bag[i]);
            }
        }
        public byte GetNextPiece()
        {
            byte gotten;

            if (bag.Count == 0)
            {
                RefillBag();
            }
            gotten = bag[0];
            bag.RemoveAt(0);
            return gotten;
        }
        private void InitializeQueue()
        {
            for (int i = 0; i < QueueSize; i++)
            {
                nextQueue.Enqueue(GetNextPiece());
            }
        }
        public byte NextPiece()
        {
            byte nextPiece = nextQueue.Dequeue();
            nextQueue.Enqueue(GetNextPiece());
            updateDraw();
            return nextPiece;
        }
        public byte[] GetNextQueue()
        {
            return nextQueue.ToArray();
        }
        public void updateDraw()
        {
            if (DrawingStuff.urishortcut[3] != null)
            {
                for (int i = 0; i < 5; i++)
                {
                    DrawingStuff.nextdrawarr[i].Source = new BitmapImage(DrawingStuff.urishortcut[nextQueue.ElementAt(i)]);
                }
            }
        }
    }

}

