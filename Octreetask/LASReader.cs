using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Media.Media3D;

namespace OTreeTask
{
    internal class LASReader
    {
        bool bGotHeader;
        long offsetPData;
        ushort PointDataSize;
        ulong nPoints;

        double scalex;
        double scaley;
        double scalez;

        double offsetx;
        double offsety;
        double offsetz;

        double maxx;
        double minx;
        double maxy;
        double miny;
        double maxz;
        double minz;

        private string sFilePath;

        public List<Point3D> points;
        public Rect3D bounds;

        public LASReader() { }
        public LASReader(string FilePath)
        {
            if (File.Exists(FilePath))
            {
                sFilePath = FilePath;
                if (ReadHeader())
                    ReadPoints();
            }
        }

        private bool ReadHeader()
        {
            using (FileStream fs = File.Open(sFilePath, FileMode.Open))
            {
                byte[] bdata = new byte[4];
                int nbytes = fs.Read(bdata, 0, 4);
                if (System.Text.Encoding.Default.GetString(bdata) != "LASF")
                {
                    return false;
                }
                Array.Clear(bdata, 0, bdata.Length);
                fs.Seek(96, SeekOrigin.Begin);
                if (fs.Read(bdata, 0, 4) == 4)
                {
                    offsetPData = BitConverter.ToUInt32(bdata, 0);
                }
                fs.Seek(107, SeekOrigin.Begin);
                Array.Clear(bdata, 0, bdata.Length);
                if (fs.Read(bdata, 0, 4) == 4)
                {
                    nPoints = BitConverter.ToUInt32(bdata, 0);
                    if (nPoints == 0)
                    {
                        fs.Seek(247, SeekOrigin.Begin);
                        bdata = new byte[8];
                        if (fs.Read(bdata, 0, 8) == 8)
                        {
                            nPoints = BitConverter.ToUInt64(bdata, 0);
                        }
                    }
                }
                fs.Seek(105, SeekOrigin.Begin);
                Array.Clear(bdata, 0, bdata.Length);
                if (fs.Read(bdata, 0, 2) == 2)
                {
                    PointDataSize = BitConverter.ToUInt16(bdata, 0);
                }
                fs.Seek(131, SeekOrigin.Begin);
                bdata = new byte[96];
                if (fs.Read(bdata, 0, 96) == 96)
                {
                    scalex = BitConverter.ToDouble(bdata, 0);
                    scaley = BitConverter.ToDouble(bdata, 8);
                    scalez = BitConverter.ToDouble(bdata, 16);
                    offsetx = BitConverter.ToDouble(bdata, 24);
                    offsety = BitConverter.ToDouble(bdata, 32);
                    offsetz = BitConverter.ToDouble(bdata, 40);
                    maxx = BitConverter.ToDouble(bdata, 48);
                    minx = BitConverter.ToDouble(bdata, 56);
                    maxy = BitConverter.ToDouble(bdata, 64);
                    miny = BitConverter.ToDouble(bdata, 72);
                    maxz = BitConverter.ToDouble(bdata, 80);
                    minz = BitConverter.ToDouble(bdata, 88);
                }
                bGotHeader = true;
                bounds.X = minx;
                bounds.Y = miny;
                bounds.Z = minz;
                bounds.SizeX = maxx - minx;
                bounds.SizeY = maxy - miny;
                bounds.SizeZ = maxz - minz;
            }
            return true;
        }

        private bool ReadPoints()
        {
            using (FileStream fs = File.Open(sFilePath, FileMode.Open))
            {
                fs.Seek(this.offsetPData, SeekOrigin.Begin);

                points = new List<Point3D>();
                byte[] bdata = new byte[PointDataSize];
                for (ulong i = 0; i < this.nPoints; i++)
                {
                    Array.Clear(bdata, 0, PointDataSize);
                    int nbytes = fs.Read(bdata, 0, PointDataSize);
                    if (nbytes == PointDataSize)
                    {
                        Point3D p = new Point3D();
                        p.X = BitConverter.ToInt32(bdata, 0);
                        p.X = p.X * scalex + offsetx;
                        p.Y = BitConverter.ToInt32(bdata, 4);
                        p.Y = p.Y * scaley + offsety;
                        p.Z = BitConverter.ToInt32(bdata, 8);
                        p.Z = p.Z * scalez + offsetz;
                        points.Add(p);
                    }
                }
            }
            return true;
        }

        public void WriteHeaderSummary()
        {
            if (!bGotHeader)
            {
                return;
            }
            Console.Write("LASF - Header summary \n");
            Console.Write("offsetPData: ");
            Console.Write(offsetPData.ToString() + "\n");
            Console.Write("PointDataSize: ");
            Console.Write(PointDataSize.ToString() + "\n");
            Console.Write("scalexyz: ");
            Console.Write(scalex.ToString() + " ");
            Console.Write(scaley.ToString() + " ");
            Console.Write(scalez.ToString() + "\n");
            Console.Write("nPoints: ");
            Console.Write(nPoints.ToString() + "\n");
            Console.Write("offsetxyz: ");
            Console.Write(offsetx.ToString() + " ");
            Console.Write(offsety.ToString() + " ");
            Console.Write(offsetz.ToString() + "\n");
            Console.Write("maxxyz: ");
            Console.Write(maxx.ToString() + " ");
            Console.Write(maxy.ToString() + " ");
            Console.Write(maxz.ToString() + "\n");
            Console.Write("minxyz: ");
            Console.Write(minx.ToString() + " ");
            Console.Write(miny.ToString() + " ");
            Console.Write(minz.ToString() + "\n");
        }

    }
}
