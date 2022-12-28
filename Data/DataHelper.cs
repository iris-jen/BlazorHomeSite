using System.Drawing;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

using System.Drawing.Imaging;
using System.IO;

namespace BlazorHomeSite.Data;

public static class DataHelper
{
    public static PhotoAlbum GetPhotoAlbum(IDbContextFactory<ApplicationDbContext> factory, int id)
    {
        using var context = factory.CreateDbContext();
        return context.PhotoAlbums
                       .Include(x=>x.Photos)
                       .Single(x=>x.Id == id);
    }

    public static void ShrinkImage(FileInfo file, string destPath, int newWidth, int newHeight)
    {
        using var stream = new MemoryStream();
        using var image = new Bitmap(Image.FromFile(file.FullName), new Size(newWidth, newHeight));
        image.Save(stream, ImageFormat.Jpeg);   
        var bytes = stream.ToArray();
        
        File.WriteAllBytes(destPath, bytes);
    }
    
    
    
}