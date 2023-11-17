using System;
using System.Collections.Generic;
using System.Threading;

namespace BFSAlgoritm
{
    public static class LabirintAlgoritm
    {
        private static Random _random = new Random();

        public static void BFS(BFSData data = null)
        {
            BFSData bfs = data ?? new BFSData();

            ViewData viewData = new ViewData(bfs);
            Action<(int x, int y)> action = ((int x, int y) endPoint) => { bfs.EndPos = endPoint; viewData.PathDrawing(false); Console.SetCursorPosition(0, 0); Console.CursorVisible = false ; Thread.Sleep(0); };

            bool isPassed = bfs.PerformBFS(OnVisitedCell: action);
            if (isPassed)
            {

                viewData.PathDrawing();
            }
        }

        private static bool PerformBFS(this BFSData _bfs, Action<(int x, int y)> OnVisitedCell)
        {
            var size = _bfs.Size;
            var adj = _bfs.adj;
            var dist = _bfs.dist;
            var startPos = _bfs.StartPos;
            var endPos = _bfs.EndPos;
            var obstacle = _bfs.HasObstacles;

            Queue<(int x, int y)> v = new Queue<(int, int)>();
            v.Enqueue(startPos);

            bool[,] visited = new bool[size.y, size.x];
            visited[startPos.y, startPos.x] = true;

            dist[startPos.y, startPos.x] = 0;

            while (v.Count > 0)
            {
                var pos = v.Dequeue();

                int x = pos.x,
                    y = pos.y;

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
                        if (adj[y1, x1] == Cell.Obstacle) continue;
                        if (!visited[y1, x1])
                        {
                            visited[y1, x1] = true;
                            dist[y1, x1] = Math.Min(dist[y1, x1], dist[y, x] + 1);
                            v.Enqueue(p);

                            OnVisitedCell?.Invoke(p);

                        }
                    }
                }
            }

            return visited[endPos.y, endPos.x];
        }

        public static T[,] Initialize2DArray<T>(this T[,] array, Func<T> initializer)
        {
            int rows = array.GetLength(0);
            int cols = array.GetLength(1);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    array[i, j] = initializer();
                }
            }

            return array;
        }
        public static Cell[,] GenerateLabirint((int x, int y) size, float obstaclesChance, bool hasObstacles = true)
        {
            int rows = size.y,
                columns = size.x;

            var adj = new Cell[rows, columns];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    int randomValue = _random.Next(0, 100);

                    Cell cell = (hasObstacles && randomValue > (int)(obstaclesChance * 100)) ? Cell.Obstacle : Cell.Empty;

                    adj[i, j] = cell;
                }
            }

            return adj;
        }
        public static (int X, int Y) GenerateRandomPoint((int MinX, int MaxX) XRange, (int MinY, int MaxY) YRange)
        {
            int randomX = _random.Next(XRange.MinX, XRange.MaxX);
            int randomY = _random.Next(YRange.MinY, YRange.MaxY);

            return (X: randomX, Y: randomY);
        }
        public static (int X, int Y) GenerateRandomPoint((int x, int y) size)
        {
            int randomX = _random.Next(0, size.x);
            int randomY = _random.Next(0, size.y);

            return (X: randomX, Y: randomY);
        }
    }
}
