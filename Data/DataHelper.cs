using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Text.RegularExpressions;

namespace BlazorHomeSite.Data;

public static class DataHelper
{
    private const int OrientationKey = 0x0112;
    private const int NotSpecified = 0;
    private const int NormalOrientation = 1;
    private const int MirrorHorizontal = 2;
    private const int UpsideDown = 3;
    private const int MirrorVertical = 4;
    private const int MirrorHorizontalAndRotateRight = 5;
    private const int RotateLeft = 6;
    private const int MirorHorizontalAndRotateLeft = 7;
    private const int RotateRight = 8;

    private const int DateTaken = 36867;
    private static readonly Regex r = new(":");

    public static PhotoAlbum GetPhotoAlbum(IDbContextFactory<ApplicationDbContext> factory, int id)
    {
        using var context = factory.CreateDbContext();
        return context.PhotoAlbums
            .Include(x => x.Photos)
            .Single(x => x.Id == id);
    }

    public static DateTime ShrinkImage(string sourcePath, string destPath, int newWidth, int newHeight)
    {
        var outputDate = DateTime.Now;

        using var stream = new MemoryStream();

#pragma warning disable CA1416 // Validate platform compatibility
        using (var sourceImage = Image.FromFile(sourcePath))
        {
            using (var image = new Bitmap(sourceImage, new Size(newWidth, newHeight)))
            {
                if (sourceImage.PropertyIdList.Contains(DateTaken))
                {
                    var propItem = sourceImage.GetPropertyItem(DateTaken);
                    if (propItem != null && propItem.Value != null)
                    {
                        var dateTaken = r.Replace(Encoding.UTF8.GetString(propItem.Value), "-", 2);
                        _ = DateTime.TryParse(dateTaken, out outputDate);
                    }
                }

                if (sourceImage.PropertyIdList.Contains(OrientationKey))
                {
                    var orientationPropItem = sourceImage.GetPropertyItem(OrientationKey);
                    if (orientationPropItem != null && orientationPropItem.Value != null &&
                        orientationPropItem.Value.Length >= 1)
                    {
                        var orientation = (int)orientationPropItem.Value[0];
                        switch (orientation)
                        {
                            case NotSpecified:
                            case NormalOrientation:
                                break;

                            case MirrorHorizontal:
                                image.RotateFlip(RotateFlipType.RotateNoneFlipX);
                                break;

                            case UpsideDown:
                                image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                                break;

                            case MirrorVertical:
                                image.RotateFlip(RotateFlipType.Rotate180FlipX);
                                break;

                            case MirrorHorizontalAndRotateRight:
                                image.RotateFlip(RotateFlipType.Rotate90FlipX);
                                break;

                            case RotateLeft:
                                image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                                break;

                            case MirorHorizontalAndRotateLeft:
                                image.RotateFlip(RotateFlipType.Rotate270FlipX);
                                break;

                            case RotateRight:
                                image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                                break;

                            default:
                                throw new NotImplementedException(
                                    "An orientation of " + orientation + " isn't implemented.");
                        }
                    }
                }

                image.Save(stream, ImageFormat.Jpeg);
                File.WriteAllBytes(destPath, stream.ToArray());
                image.Dispose();
            }

            sourceImage.Dispose();
        }
#pragma warning restore CA1416 // Validate platform compatibility

        stream.Close();
        return outputDate;
    }
}