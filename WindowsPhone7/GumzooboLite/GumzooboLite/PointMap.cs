using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BubbleGame
{
    class MPoint
    {
        Vector2 location;
        String name;

        public MPoint(Vector2 location)
        {
            this.location = location;
            name = null;
        }

        public MPoint(Vector2 location, String name)
        {
            this.location = location;
            this.name = name;
        }

        public Vector2 Location
        {
            get { return location; }
        }

        public String Name
        {
            get { return name; }
        }

        public bool Equal(MPoint point)
        {
            if (location.X == point.location.X && location.Y == point.location.Y)
            {
                return true;
            }
            return false;
        }
    }

    class Connection
    {
        MPoint[] points = new MPoint[2];
        public Connection(MPoint point1, MPoint point2)
        {
            points[0] = point1;
            points[1] = point2;
        }

        public MPoint GetPoint(int offset)
        {
            return points[offset];
        }
    }


    class MPath
    {
        List<MPoint> points = new List<MPoint>();

        public MPath(MPoint point)
        {
            points.Add(point);
        }

        public MPath(List<MPoint> points)
        {
            for (int i = 0; i < points.Count; i++)
            {
                this.points.Add(points[i]);
            }
        }

        public MPath(List<MPoint> points, MPoint point)
        {
            for (int i = 0; i < points.Count; i++)
            {
                this.points.Add(points[i]);
            }
            points.Add(point);
        }

        public MPath(MPath path, MPoint point)
        {
            for (int i = 0; i < path.Length; i++)
            {
                this.points.Add(path.GetPoint(i));
            }
            points.Add(point);
        }

        public String Destination
        {
            get { return points[points.Count - 1].Name; }
        }

        public MPoint LastPoint
        {
            get { return points[points.Count - 1]; }
        }

        public MPoint GetPoint(int offset)
        {
            return points[offset];
        }

        public int Length
        {
            get { return points.Count; }
        }
    }


    class PointMap
    {
        List<MPoint> points = new List<MPoint>();
        List<Connection> connecitons = new List<Connection>();

        // async path find variables
        Thread pathFindingThread;
        bool isThreadRunning = false;
        bool isResultReady = false;
        MPath pathResult = null;
        MPoint startPoint = null;
        Connection startConnection = null;
        bool usePoint = true;
        String destination;

        public PointMap()
        {

        }

        ~PointMap()
        {
            if (pathFindingThread != null)
            {
                pathFindingThread.Abort();
            }
        }

        public void AddPoint(MPoint point)
        {
            points.Add(point);
        }

        public void SetPoints(List<MPoint> points)
        {
            this.points = points;
        }

        public void AddConnection(Connection connection)
        {
            connecitons.Add(connection);
        }

        public MPath FindShortestPath(MPoint currentPoint, String destination)
        {
            List<MPath> pathList = new List<MPath>();
            MPath path = null;
            pathList.Add(new MPath(currentPoint));

            while (path == null)
            {
                // expand paths
                pathList = ExpandPathList(pathList);

                // check for success
                for (int i = 0; i < pathList.Count; i++)
                {
                    if (pathList[i].Destination == destination)
                    {
                        path = pathList[i];
                        break;
                    }
                }
            }

            return path;
        }

        public MPath FindShortestPath(Connection currentConnection, String destination)
        {
            MPath path = FindShortestPath(currentConnection.GetPoint(0), destination);
            MPath path2 = FindShortestPath(currentConnection.GetPoint(1), destination);

            if (path2.Length < path.Length)
            {
                path = path2;
            }

            return path;
        }

        public List<MPath> ExpandPathList(List<MPath> pathList)
        {
            List<MPath> newPathList = new List<MPath>();

            foreach (MPath path in pathList)
            {
                foreach (Connection connection in connecitons)
                {
                    if (connection.GetPoint(0).Equal(path.LastPoint))
                    {
                        newPathList.Add(new MPath(path, connection.GetPoint(1)));
                    }
                    else if (connection.GetPoint(1).Equal(path.LastPoint))
                    {
                        newPathList.Add(new MPath(path, connection.GetPoint(0)));
                    }
                }
            }

            return newPathList;
        }

        public void RunPathFindingThread(MPoint point, Connection connection, bool usepoint, String destination)
        {
            if (isThreadRunning)
            {
                pathFindingThread.Abort();
            }
            isResultReady = false;
            startPoint = point;
            startConnection = connection;
            usePoint = usepoint;
            this.destination = destination;

            pathFindingThread = new Thread(ThreadProc);
            isThreadRunning = true;
            pathFindingThread.Start();
        }


        private void ThreadProc()
        {
#if XBOX
            Thread.CurrentThread.SetProcessorAffinity(4);
#endif
            try
            {
                if (usePoint)
                {
                    pathResult = FindShortestPath(startPoint, destination);
                }
                else
                {
                    pathResult = FindShortestPath(startConnection, destination);
                }
                isResultReady = true;
                isThreadRunning = false;
            }
            catch (Exception)
            {

            }

        }

        public bool IsResultReady
        {
            get { return isResultReady; }
        }

        public MPath Result
        {
            get { return pathResult; }
        }
    }
}