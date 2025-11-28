namespace Application.Interfaces;

/// <summary>
/// Service for handling image file operations
/// </summary>
public interface IImageService
{
    /// <summary>
    /// Saves an image file and returns the file path
    /// </summary>
    /// <param name="file">Image file stream</param>
    /// <param name="fileName">Original file name</param>
    /// <returns>Relative path to saved image</returns>
    Task<string> SaveImageAsync(Stream file, string fileName);

    /// <summary>
    /// Deletes an image file
    /// </summary>
    /// <param name="imagePath">Path to image file</param>
    Task DeleteImageAsync(string imagePath);

    /// <summary>
    /// Validates image file type and size
    /// </summary>
    /// <param name="fileName">File name with extension</param>
    /// <param name="fileSize">File size in bytes</param>
    /// <returns>True if valid, false otherwise</returns>
    bool ValidateImage(string fileName, long fileSize);
}