/**
 * 抓圖
 *
 * @example
 ScreenShot.CaptureImage(axMapControl1.PointToScreen(Point.Empty), Point.Empty, new Rectangle(axMapControl1.Location, axMapControl1.Size), ScreenPath);   
 */
class ScreenShot{
    public static void CaptureImage(Point SourcePoint, Point DestinationPoint, Rectangle SelectionRectangle, string FilePath) {
        using (Bitmap bitmap = new Bitmap(SelectionRectangle.Width, SelectionRectangle.Height)) {
            using (Graphics g = Graphics.FromImage(bitmap)) {
                g.CopyFromScreen(SourcePoint, DestinationPoint, SelectionRectangle.Size);
            }
            bitmap.Save(FilePath, ImageFormat.Bmp);
        }
    }
}