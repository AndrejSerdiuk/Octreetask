using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace OTreeTask
{
    public class Octree
    {
        private uint level;
        private Rect3D boundary;
        private List<Point3D> points;
        private bool bIsSubdivided;
        public static uint PointSum;

        private Octree UZUYUX;
        private Octree UZUYLX;
        private Octree UZLYUX;
        private Octree UZLYLX;
        private Octree LZUYUX;
        private Octree LZUYLX;
        private Octree LZLYUX;
        private Octree LZLYLX;

        public Octree() { }

        public Octree(Rect3D Boundary, uint Level)
        {
            this.level = Level;
            this.boundary = Boundary;
        }

        public bool AddPoint(Point3D newPoint)
        {
            if (boundary.Contains(newPoint))
            {
                if (level <= 0)
                {
                    if (points == null) points = new List<Point3D>();
                    points.Add(newPoint);
                    PointSum++;
                    return true;
                }
                else
                {
                    if (!bIsSubdivided)
                    {
                        Subdivide();
                        bIsSubdivided = true;
                    }

                    if (this.LZUYUX.AddPoint(newPoint)) { return true; }
                    else if (this.LZUYLX.AddPoint(newPoint)) { return true; }
                    else if (this.LZLYUX.AddPoint(newPoint)) { return true; }
                    else if (this.LZLYLX.AddPoint(newPoint)) { return true; }
                    else if (this.UZUYUX.AddPoint(newPoint)) { return true; }
                    else if (this.UZUYLX.AddPoint(newPoint)) { return true; }
                    else if (this.UZLYUX.AddPoint(newPoint)) { return true; }
                    else if (this.UZLYLX.AddPoint(newPoint)) { return true; }
                    else { return false; }

                }
            }
            else
            {
                return false;
            }
        }

        private void Subdivide()
        {
            double width = DecimalDivision(this.boundary.SizeX, 2);
            double height = DecimalDivision(this.boundary.SizeY, 2);
            double depth = DecimalDivision(this.boundary.SizeZ, 2);

            Rect3D rLZUYUX = new Rect3D(
                this.boundary.X + width,
                this.boundary.Y,
                this.boundary.Z,
                width, height, depth);
            this.LZUYUX = new Octree(rLZUYUX, this.level - 1);

            Rect3D rLZUYLX = new Rect3D(
                this.boundary.X,
                this.boundary.Y,
                this.boundary.Z,
                width, height, depth);
            this.LZUYLX = new Octree(rLZUYLX, this.level - 1);

            Rect3D rLZLYUX = new Rect3D(
                this.boundary.X + width,
                this.boundary.Y + height,
                this.boundary.Z,
                width, height, depth);
            this.LZLYUX = new Octree(rLZLYUX, this.level - 1);

            Rect3D rLZLYLX = new Rect3D(
                this.boundary.X,
                this.boundary.Y + height,
                this.boundary.Z,
                width, height, depth);
            this.LZLYLX = new Octree(rLZLYLX, this.level - 1);

            Rect3D rUZUYUX = new Rect3D(
                this.boundary.X + width,
                this.boundary.Y,
                this.boundary.Z + depth,
                width, height, depth);
            this.UZUYUX = new Octree(rUZUYUX, this.level - 1);

            Rect3D rUZUYLX = new Rect3D(
                this.boundary.X,
                this.boundary.Y,
                this.boundary.Z + depth,
                width, height, depth);
            this.UZUYLX = new Octree(rUZUYLX, this.level - 1);

            Rect3D rUZLYUX = new Rect3D(
                this.boundary.X + width,
                this.boundary.Y + height,
                this.boundary.Z + depth,
                width, height, depth);
            this.UZLYUX = new Octree(rUZLYUX, this.level - 1);

            Rect3D rUZLYLX = new Rect3D(
                this.boundary.X,
                this.boundary.Y + height,
                this.boundary.Z + depth,
                width, height, depth);
            this.UZLYLX = new Octree(rUZLYLX, this.level - 1);
        }

        public static double DecimalDivision(double FirstVal, double SecondVal)
        {
            Decimal dVal1 = new Decimal(FirstVal);
            Decimal dVal2 = new Decimal(SecondVal);
            return Decimal.ToDouble(Decimal.Divide(dVal1, dVal2));
        }
    }
}
