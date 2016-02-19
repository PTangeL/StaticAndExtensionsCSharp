namespace Library.Images
{
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Linq;
    using System.Runtime.InteropServices;

    public class Quantizer
    {
        [StructLayout(LayoutKind.Explicit)]
        public struct Color32
        {
            [FieldOffset(0)]
            public byte Blue;

            [FieldOffset(1)]
            public byte Green;

            [FieldOffset(2)]
            public byte Red;

            [FieldOffset(3)]
            public byte Alpha;

            [FieldOffset(0)]
            public int ARGB;

            public Color Color => Color.FromArgb(Alpha, Red, Green, Blue);
        }

        private bool blnSinglePass;
        private int intMaxColors;
        private int intMaxColorBits;
        private Octree octree;

        public Quantizer()
            : this(255, 8)
        {
        }

        public Quantizer(int intMaxColors, int intMaxColorBits)
        {
            if (intMaxColors > 255)
            {
                throw new ArgumentOutOfRangeException(
                    "intMaxColors",
                    intMaxColors,
                    "The number of colors should be less than 256."
                );
            }

            if ((intMaxColorBits < 1) | (intMaxColorBits > 8))
            {
                throw new ArgumentOutOfRangeException(
                    "intMaxColorBits",
                    intMaxColorBits,
                    "The number of color bits should be between 1 and 8."
                );
            }

            blnSinglePass = false;
            this.intMaxColors = intMaxColors;
            this.intMaxColorBits = intMaxColorBits;

            octree = new Octree(intMaxColorBits);
        }

        public Bitmap Quantize(Image source)
        {
            int intWidth = source.Width;
            int intHeight = source.Height;
            Rectangle bounds = new Rectangle(0, 0, intWidth, intHeight);
            Bitmap bmpCopy = new Bitmap(intWidth, intHeight, PixelFormat.Format32bppArgb);
            Bitmap bmpOutput = new Bitmap(intWidth, intHeight, PixelFormat.Format8bppIndexed);
            Graphics g = Graphics.FromImage(bmpCopy);
            BitmapData sourceData = null;

            g.PageUnit = GraphicsUnit.Pixel;
            g.DrawImageUnscaled(source, bounds);

            try
            {
                sourceData = bmpCopy.LockBits(bounds, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

                if (!blnSinglePass) FirstPass(sourceData, intWidth, intHeight);

                bmpOutput.Palette = this.GetPalette(bmpOutput.Palette);

                SecondPass(sourceData, bmpOutput, intWidth, intHeight, bounds);
            }
            finally
            {
                bmpCopy.UnlockBits(sourceData);
            }

            source.Dispose();

            return bmpOutput;
        }

        protected unsafe void FirstPass(BitmapData sourceData, int intWidth, int intHeight)
        {
            byte* pSourceRow = (byte*)sourceData.Scan0.ToPointer();
            Int32* pSourcePixel;

            for (int intRow = 0; intRow < intHeight; intRow++)
            {
                pSourcePixel = (Int32*)pSourceRow;

                for (int intCol = 0; intCol < intWidth; intCol++, pSourcePixel++)
                {
                    InitialQPixel((Color32*)pSourcePixel);
                }

                pSourceRow += sourceData.Stride;
            }
        }

        protected unsafe void SecondPass(BitmapData sourceData, Bitmap bmpOutput,
            int intWidth, int intHeight, Rectangle bounds)
        {
            BitmapData outputData = null;
            byte* pSourceRow, pDestinationRow, pDestinationPixel;
            Int32* pSourcePixel, pPreviousPixel;

            try
            {
                outputData = bmpOutput.LockBits(
                    bounds,
                    ImageLockMode.WriteOnly,
                    PixelFormat.Format8bppIndexed
                );

                pSourceRow = (byte*)sourceData.Scan0.ToPointer();
                pSourcePixel = (Int32*)pSourceRow;
                pPreviousPixel = pSourcePixel;

                pDestinationRow = (byte*)outputData.Scan0.ToPointer();
                pDestinationPixel = pDestinationRow;

                byte intPixelValue = QPixel((Color32*)pSourcePixel);

                *pDestinationPixel = intPixelValue;

                for (int intRow = 0; intRow < intHeight; intRow++)
                {
                    pSourcePixel = (Int32*)pSourceRow;
                    pDestinationPixel = pDestinationRow;

                    for (int intCol = 0; intCol < intWidth; intCol++, pSourcePixel++, pDestinationPixel++)
                    {
                        if (*pPreviousPixel != *pSourcePixel)
                        {
                            intPixelValue = QPixel((Color32*)pSourcePixel);
                            pPreviousPixel = pSourcePixel;
                        }

                        *pDestinationPixel = intPixelValue;
                    }

                    pSourceRow += sourceData.Stride;

                    pDestinationRow += outputData.Stride;
                }
            }
            finally
            {
                bmpOutput.UnlockBits(outputData);
            }
        }

        protected unsafe void InitialQPixel(Color32* pixel)
        {
            octree.AddColor(pixel);
        }

        protected unsafe byte QPixel(Color32* pixel)
        {
            byte intPaletteIndex = (byte)intMaxColors;

            if (pixel->Alpha > 0)
            {
                intPaletteIndex = (byte)octree.GetPaletteIndex(pixel);
            }

            return intPaletteIndex;
        }

        protected ColorPalette GetPalette(ColorPalette original)
        {
            ArrayList arrPalette = octree.palletize(intMaxColors - 1);

            for (int intIndex = 0; intIndex < arrPalette.Count; intIndex++)
            {
                original.Entries[intIndex] = (Color)arrPalette[intIndex];
            }

            original.Entries[intMaxColors] = Color.FromArgb(0, 0, 0, 0);

            return original;
        }

        private class Octree
        {
            private static int[] mask = new int[8] { 0x80, 0x40, 0x20, 0x10, 0x08, 0x04, 0x02, 0x01 };
            private OctreeNode root;
            private OctreeNode previousNode;
            private OctreeNode[] arrReducibleNodes;
            private int intMaxColorBits;
            private int intPreviousColor;
            private int intLeafCount;

            public Octree(int intMaxColorBits)
            {
                this.intMaxColorBits = intMaxColorBits;
                intLeafCount = 0;
                arrReducibleNodes = new OctreeNode[9];
                root = new OctreeNode(0, intMaxColorBits, this);
                intPreviousColor = 0;
                previousNode = null;
            }

            public unsafe void AddColor(Color32* pixel)
            {
                if (intPreviousColor == pixel->ARGB)
                {
                    if (null == previousNode)
                    {
                        intPreviousColor = pixel->ARGB;
                        root.AddColor(pixel, intMaxColorBits, 0, this);
                    }
                    else
                    {
                        previousNode.Increment(pixel);
                    }
                }
                else
                {
                    intPreviousColor = pixel->ARGB;
                    root.AddColor(pixel, intMaxColorBits, 0, this);
                }
            }

            public void Reduce()
            {
                int intIndex;
                OctreeNode node;

                for (intIndex = intMaxColorBits - 1; (intIndex > 0) && (null == arrReducibleNodes[intIndex]); intIndex--) ;

                node = arrReducibleNodes[intIndex];
                arrReducibleNodes[intIndex] = node.NextReducible;

                intLeafCount -= node.Reduce();
                previousNode = null;
            }

            public int Leaves
            {
                get { return intLeafCount; }
                set { intLeafCount = value; }
            }

            protected OctreeNode[] reducibleNodes => arrReducibleNodes;

            protected void trackPrevious(OctreeNode node)
            {
                previousNode = node;
            }

            public ArrayList palletize(int colorCount)
            {
                while (Leaves > colorCount) Reduce();

                ArrayList arrPalette = new ArrayList(Leaves);
                int intPaletteIndex = 0;

                root.ConstructPalette(arrPalette, ref intPaletteIndex);

                return arrPalette;
            }

            public unsafe int GetPaletteIndex(Color32* pixel) => root.GetPaletteIndex(pixel, 0);

            protected class OctreeNode
            {
                private bool blnLeaf;
                private int intPixelCount;
                private int intRed;
                private int intGreen;
                private int intBlue;
                private int intPaletteIndex;
                private OctreeNode nextReducible;
                private OctreeNode[] arrChildren;

                public OctreeNode(int intLevel, int intColorBits, Octree octree)
                {
                    blnLeaf = (intLevel == intColorBits);

                    intRed = intGreen = intBlue = 0;
                    intPixelCount = 0;

                    if (blnLeaf)
                    {
                        octree.Leaves++;
                        nextReducible = null;
                        arrChildren = null;
                    }
                    else
                    {
                        nextReducible = octree.reducibleNodes[intLevel];
                        octree.reducibleNodes[intLevel] = this;
                        arrChildren = new OctreeNode[8];
                    }
                }

                public unsafe void AddColor(Color32* pixel, int intColorbits, int intLevel, Octree octree)
                {
                    int intShift, intIndex;

                    if (blnLeaf)
                    {
                        Increment(pixel);
                        octree.trackPrevious(this);
                    }
                    else
                    {
                        intShift = 7 - intLevel;
                        intIndex = ((pixel->Red & mask[intLevel]) >> (intShift - 2)) |
                                    ((pixel->Green & mask[intLevel]) >> (intShift - 1)) |
                                    ((pixel->Blue & mask[intLevel]) >> (intShift));

                        OctreeNode child = arrChildren[intIndex];

                        if (null == child)
                        {
                            child = new OctreeNode(intLevel + 1, intColorbits, octree);
                            arrChildren[intIndex] = child;
                        }

                        child.AddColor(pixel, intColorbits, intLevel + 1, octree);
                    }
                }

                public OctreeNode NextReducible
                {
                    get { return nextReducible; }
                    set { nextReducible = value; }
                }

                public OctreeNode[] Children => arrChildren;

                public int Reduce()
                {
                    int intChildren = 0;
                    OctreeNode child;

                    intRed = intGreen = intBlue = 0;

                    for (int intIndex = 0; intIndex < 8; intIndex++)
                    {
                        child = arrChildren[intIndex];

                        if (null != child)
                        {
                            intRed += child.intRed;
                            intGreen += child.intGreen;
                            intBlue += child.intBlue;
                            intPixelCount += child.intPixelCount;

                            ++intChildren;

                            arrChildren[intIndex] = null;
                        }
                    }

                    blnLeaf = true;

                    return intChildren - 1;
                }

                public void ConstructPalette(ArrayList palette, ref int intPaletteIndex)
                {
                    if (blnLeaf)
                    {
                        this.intPaletteIndex = intPaletteIndex++;

                        palette.Add(
                            Color.FromArgb(
                                intRed / intPixelCount,
                                intGreen / intPixelCount,
                                intBlue / intPixelCount
                            )
                        );
                    }
                    else
                    {
                        for (int intIndex = 0; intIndex < 8; intIndex++)
                        {
                            if (null != arrChildren[intIndex])
                            {
                                arrChildren[intIndex].ConstructPalette(palette, ref intPaletteIndex);
                            }
                        }
                    }
                }

                public unsafe int GetPaletteIndex(Color32* pixel, int intLevel)
                {
                    int intPaletteIndex = this.intPaletteIndex;
                    int intShift, intIndex;

                    if (!blnLeaf)
                    {
                        intShift = 7 - intLevel;
                        intIndex = ((pixel->Red & mask[intLevel]) >> (intShift - 2)) |
                                    ((pixel->Green & mask[intLevel]) >> (intShift - 1)) |
                                    ((pixel->Blue & mask[intLevel]) >> (intShift));

                        if (null != arrChildren[intIndex])
                        {
                            intPaletteIndex = arrChildren[intIndex].GetPaletteIndex(pixel, intLevel + 1);
                        }
                        else
                        {
                            intPaletteIndex = 0;
                        }
                    }

                    return intPaletteIndex;
                }

                public unsafe void Increment(Color32* pixel)
                {
                    intPixelCount++;
                    intRed += pixel->Red;
                    intGreen += pixel->Green;
                    intBlue += pixel->Blue;
                }
            }
        }
    }
}