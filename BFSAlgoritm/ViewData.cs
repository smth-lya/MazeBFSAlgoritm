using System;
using System.Threading;
using static BFSAlgoritm.LabirintAlgoritm;

namespace BFSAlgoritm
{
    public class ViewData
    {
        private BFSData _bfs;

        public ViewData(BFSData bfs)
        {
            _bfs = bfs;
        }

        public static ConsoleColor GetCellColor(Cell cellType)
        {
            return cellType switch
            {
                Cell.Empty => ConsoleColor.Gray,
                Cell.Start => ConsoleColor.DarkCyan,
                Cell.Finish => ConsoleColor.DarkGreen,
                Cell.Path => ConsoleColor.Green,
                Cell.Obstacle => ConsoleColor.DarkGray,
                _ => ConsoleColor.Black,
            };
        }

        public void DrawMaze()
        {
            var size = _bfs.Size;
            var adj = _bfs.adj;

            for (int i = 0; i < size.y; i++)
            {
                for (int j = 0; j < size.x; j++)
                {
                    Console.BackgroundColor = GetCellColor(adj[i, j]);
                    Console.Write("  ");
                }
                Console.ResetColor();
                Console.WriteLine();
            }
        }
        public void PathDrawing(bool hasAnimation = true)
        {
            if (OperatingSystem.IsWindows())
            {
                Console.SetWindowSize(150, 50);
            }

            if (!hasAnimation)
            {
                ReconstructPath();
                DrawMaze();
            }
            else
            {
                AnimatePathDrawing();
            }
        }

        void ReconstructPath((int x, int y)? endPosition = null)
        {
            var dist = _bfs.dist;

            var endPos = endPosition ?? _bfs.EndPos;

            int s = dist[endPos.y, endPos.x];

            ReconstructPath(countSteps: s, endPosition);
        }
        void ReconstructPath(int countSteps, (int x, int y)? endPosition = null)
        {
            var size = _bfs.Size;

            var startPos = _bfs.StartPos;
            var endPos = endPosition ?? _bfs.EndPos;

            var adj = _bfs.adj;
            var dist = _bfs.dist;

            int s = dist[endPos.y, endPos.x];

            int x = endPos.x,
                y = endPos.y;

            adj[endPos.y, endPos.x] = Cell.Finish;
            adj[startPos.y, startPos.x] = Cell.Start;

            while (dist[endPos.y, endPos.x] - s < countSteps)
            {
                var dir = new (int x, int y)[] { (x + 1, y),
                                                 (x - 1, y),
                                                 (x, y + 1),
                                                 (x, y - 1) };
                foreach (var p in dir)
                {
                    int x1 = p.x,
                        y1 = p.y;

                    if (0 <= x1 && x1 < size.x && 0 <= y1 && y1 < size.y)
                    {
                        if (dist[y1, x1] + 1 == s)
                        {
                            s--;

                            x = x1;
                            y = y1;

                            if (x1 == startPos.x && y1 == startPos.y) adj[y1, x1] = Cell.Start;
                            else adj[y1, x1] = Cell.Path;
                        }
                    }
                }
            }
        }

        void AnimatePathDrawing()
        {
            var dist = _bfs.dist;
            var endPos = _bfs.EndPos;

            Console.CursorVisible = false;

            for (int currentStep = 0; currentStep < dist[endPos.y, endPos.x]; currentStep++)
            {
                Console.SetCursorPosition(0, 0);

                ReconstructPath(countSteps: currentStep);
                DrawMaze();

                Thread.Sleep(50);
            }
        }
    }
}