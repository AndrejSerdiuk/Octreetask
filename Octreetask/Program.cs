using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace OTreeTask
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string FilePath = "2743_1234.las";
            LASReader reader = new LASReader(FilePath);
            reader.WriteHeaderSummary();
            int nRPoints = reader.points.Count();
            Console.WriteLine(" Points read: " + reader.points.Count().ToString());
            Octree tree = new Octree(reader.bounds, 10);
            for (int i = 0; i < nRPoints; i++)
            {
                tree.AddPoint(reader.points[i]);
            }

            Console.WriteLine("Stored points: " + Octree.PointSum.ToString());
            Console.ReadKey();
        }
    }
}
