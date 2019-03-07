using CommandLine;
using System;
using System.IO;

namespace SpotifyResourceCleaner
{
  class Program
  {
    public class Options
    {
      [Option('f', "folder", Required = false, HelpText = "Set Spotify('%localappdata%') folder.")]
      public string Folder { get; set; }
    }

    private static readonly string[] FoldersToDelete = { "Storage", "Data", "Browser" };

    private static void Main(string[] args)
    {
      Parser.Default
        .ParseArguments<Options>(args)
        .WithParsed(Run);
    }

    static void Run(Options options)
    {
      var localSpotifyFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Spotify");
      if (!string.IsNullOrWhiteSpace(options.Folder))
        localSpotifyFolderPath = options.Folder;

      if (!Directory.Exists(localSpotifyFolderPath))
      {
        WriteLine($"Spotify folder at path '{localSpotifyFolderPath}' doesn't exist.");
        Exit();
      }

      foreach (var folderToDelete in FoldersToDelete)
      {
        var folderToDeletePath = Path.Combine(localSpotifyFolderPath, folderToDelete);
        if (Directory.Exists(folderToDeletePath))
        {
          WriteLine($"Cleaning folder {folderToDeletePath}...");
          Directory.Delete(folderToDeletePath, true);
          Directory.CreateDirectory(folderToDeletePath);
          WriteLine($"Cleaned folder {folderToDeletePath}.");
        }
        else
        {
          WriteLine($"Skipping folder {folderToDeletePath} because it doesn't exits.");
        }
      }
      Exit();
    }

    private static void WriteLine(string message)
    {
      Console.WriteLine(message);
    }

    private static void Exit()
    {
      WriteLine("Press enter to exit...");
      Console.Read();
    }
  }
}
