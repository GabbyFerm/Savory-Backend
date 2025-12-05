using Application.Interfaces;
using Microsoft.AspNetCore.Hosting;

namespace Infrastructure.Services;

/// <summary>
/// Service for handling recipe image file operations
/// </summary>
public class ImageService : IImageService
{
    private readonly string _imageFolder;
    private readonly long _maxFileSize = 5 * 1024 * 1024; // 5MB
    private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png", ".webp" };

    public ImageService(IWebHostEnvironment environment)
    {
        // Store images in wwwroot/images/recipes
        _imageFolder = Path.Combine(environment.WebRootPath, "images", "recipes");

        // Ensure directory exists
        if (!Directory.Exists(_imageFolder))
        {
            Directory.CreateDirectory(_imageFolder);
        }
    }

    /// <summary>
    /// Saves an image file and returns the relative path
    /// </summary>
    public async Task<string> SaveImageAsync(Stream file, string fileName)
    {
        // Generate unique filename
        var extension = Path.GetExtension(fileName).ToLowerInvariant();
        var uniqueFileName = $"{Guid.NewGuid()}{extension}";
        var filePath = Path.Combine(_imageFolder, uniqueFileName);

        // Save file
        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(fileStream);
        }

        // Return relative path (for storing in database)
        return $"/images/recipes/{uniqueFileName}";
    }

    /// <summary>
    /// Deletes an image file if it exists
    /// </summary>
    public Task DeleteImageAsync(string imagePath)
    {
        if (string.IsNullOrWhiteSpace(imagePath))
        {
            return Task.CompletedTask;
        }

        // Remove leading slash and convert to physical path
        var relativePath = imagePath.TrimStart('/');
        var physicalPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", relativePath.Replace('/', Path.DirectorySeparatorChar));

        if (File.Exists(physicalPath))
        {
            File.Delete(physicalPath);
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// Validates image file type and size
    /// </summary>
    public bool ValidateImage(string fileName, long fileSize)
    {
        // Check file size
        if (fileSize > _maxFileSize)
        {
            return false;
        }

        // Check file extension
        var extension = Path.GetExtension(fileName).ToLowerInvariant();
        return _allowedExtensions.Contains(extension);
    }
}