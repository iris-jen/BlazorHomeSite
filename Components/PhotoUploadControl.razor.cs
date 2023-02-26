using BlazorHomeSite.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace BlazorHomeSite.Components;

public partial class PhotoUploadControl
{
    [Parameter] public string? AlbumId { get; set; }
    [Inject] private IPhotoRepository PhotoRepository { get; set; } = null!;

    [Inject] private IWebHostEnvironment HostEnvironment { get; set; } = null!;

    private List<IBrowserFile> Files = new List<IBrowserFile>();
    private bool HideControl = false;

    private void AddFileToUploadList(InputFileChangeEventArgs args)
    {
        Files.Add(args.File);
    }

    private void ToggleHideControl()
    {
        HideControl = !HideControl;
    }

    private void AddFilesToUploadList(InputFileChangeEventArgs args)
    {
        Files.AddRange(args.GetMultipleFiles(50));
    }

    private void ClearUploadList()
    {
        Files.Clear();
    }

    private async Task Upload()
    {
        var albumId = int.Parse(AlbumId ?? "-1");
        await PhotoRepository.UploadPhotos(Files, albumId);
    }
}