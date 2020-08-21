using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AntMe.Plugin.Fna
{
    public class NineSlicedTexture
    {
        private Texture2D texture;
        private Rectangle innerArea;
        private Point overlap;

        public NineSlicedTexture(Texture2D texture, Rectangle innerArea)
        {
            this.texture = texture;
            this.innerArea = innerArea;

            overlap = new Point(texture.Width - innerArea.Width, texture.Height - innerArea.Height);
        }


        public void Draw(SpriteBatch batch, Rectangle destinationRectangle, Color color)
        {
            //    1 |   2   | 3   TOP
            //  ____|_______|____
            //      |       | 
            //    4 |   5   | 6   MIDDLE
            //  ____|_______|____
            //      |       | 
            //    7 |   8   | 9   BOTTOM
            //
            // LEFT  CENTER  RIGHT

            int leftSlice = innerArea.X;
            int rightSlice = innerArea.X + innerArea.Width;

            int topSlice = innerArea.Y;
            int bottomSlice = innerArea.Y + innerArea.Height;

            int rightWidth = texture.Width - rightSlice;
            int bottomHeight = texture.Height - bottomSlice;

            int destInnerWidth = destinationRectangle.Width - overlap.X;
            int destInnerHeight = destinationRectangle.Height - overlap.Y;

            int destRight = destinationRectangle.X + destinationRectangle.Width - rightWidth;
            int destBottom = destinationRectangle.Y + destinationRectangle.Height - bottomHeight;

            // TOP
            // 1. Top left
            Rectangle sourceTopLeft = new Rectangle(0, 0, leftSlice, topSlice);
            Rectangle destTopLeft = new Rectangle(destinationRectangle.X, destinationRectangle.Y, sourceTopLeft.Width, sourceTopLeft.Height);
            batch.Draw(texture, destTopLeft, sourceTopLeft, color);

            // 2. Top center
            Rectangle sourceTopCenter = new Rectangle(leftSlice, 0, innerArea.Width, topSlice);
            Rectangle destTopCenter = new Rectangle(destinationRectangle.X + leftSlice, destinationRectangle.Y, destInnerWidth, sourceTopCenter.Height);
            batch.Draw(texture, destTopCenter, sourceTopCenter, color);

            // 3. Top right
            Rectangle sourceTopRight = new Rectangle(rightSlice, 0, rightWidth, topSlice);
            Rectangle destTopRight = new Rectangle(destRight, destinationRectangle.Y, sourceTopRight.Width, sourceTopRight.Height);
            batch.Draw(texture, destTopRight, sourceTopRight, color);

            // MIDDLE
            // 4. Middle left
            Rectangle sourceMiddleLeft = new Rectangle(0, topSlice, leftSlice, innerArea.Height);
            Rectangle destMiddleLeft = new Rectangle(destinationRectangle.X, destinationRectangle.Y + topSlice, sourceMiddleLeft.Width, destInnerHeight);
            batch.Draw(texture, destMiddleLeft, sourceMiddleLeft, color);

            // 5. Middle center
            Rectangle sourceMiddleCenter = innerArea;
            Rectangle destMiddleCenter = new Rectangle(destinationRectangle.X + leftSlice, destinationRectangle.Y + topSlice, destInnerWidth, destInnerHeight);
            batch.Draw(texture, destMiddleCenter, sourceMiddleCenter, color);

            // 6. Middle right
            Rectangle sourceMiddleRight = new Rectangle(rightSlice, topSlice, rightWidth, innerArea.Height);
            Rectangle destMiddleRight = new Rectangle(destRight, destinationRectangle.Y + topSlice, sourceMiddleRight.Width, destInnerHeight);
            batch.Draw(texture, destMiddleRight, sourceMiddleRight, color);

            // BOTTOM
            // 7. Bottom left
            Rectangle sourceBottomLeft = new Rectangle(0, bottomSlice, leftSlice, bottomHeight);
            Rectangle destBottomLeft = new Rectangle(destinationRectangle.X, destBottom, leftSlice, bottomHeight);
            batch.Draw(texture, destBottomLeft, sourceBottomLeft, color);

            // 8. Bottom center
            Rectangle sourceBottomCenter = new Rectangle(leftSlice, bottomSlice, innerArea.Width, bottomHeight);
            Rectangle destBottomCenter = new Rectangle(destinationRectangle.X + leftSlice, destBottom, destInnerWidth, bottomHeight);
            batch.Draw(texture, destBottomCenter, sourceBottomCenter, color);

            // 9. Bottom right
            Rectangle sourceBottomRight = new Rectangle(rightSlice, bottomSlice, rightWidth, bottomHeight);
            Rectangle destBottomRight = new Rectangle(destRight, destBottom, rightWidth, bottomHeight);
            batch.Draw(texture, destBottomRight, sourceBottomRight, color);



        }
    }
}
