// See https://aka.ms/new-console-template for more information
using Barony_Saver;

using System.Diagnostics;

ConsoleWriter writer = new ConsoleWriter();
writer.AddLine("Barony Saver 1.0 by DanteFjante");

var process = FindBaronyProcess();

List<SaveGame> saves = new();

int tries = 360;
int delay = 5;

int tryIndex = 0;
while (process == null && tryIndex < tries)
{
  string ErrorMessage = "Barony is not running. Please start the game.";
  writer.AddUniqueLine(ErrorMessage);
  writer.Write();
  
  process = FindBaronyProcess();
  if(process != null)
  {
    break;
  }
  Task.Delay(1000 * delay).Wait();
  tryIndex++;
}

if(process == null)
{
  writer.ClearLine();
  writer.WriteLine($"Barony didnt start for {(tries * delay) / 60} minutes and {(tries * delay) % 60} seconds. Closing the program");

  return;
}
else
{
  writer.ClearLine();
  writer.WriteLine("Barony is running.");

  saves = GetSaveGames(process);

  foreach (var save in saves)
  {
    writer.WriteLine($"Saving file: {save.Name} to memory");
  }

  while (!process.HasExited)
  {
    foreach (var save in GetNewSaves(process, saves))
    {
      writer.WriteLine($"New file {save.Name} found. Saving file to memory");
      saves.Add(save);
    }

    List<SaveGame> savesToUpdate = new();
    List<SaveGame> saveGamesToDelete = new();

    foreach (var save in saves)
    {
      if (save.IsUpdated())
      {
        writer.WriteLine($"File {save.Name} has changed. Saving update to memory");
        savesToUpdate.Add(save.Update());
        saveGamesToDelete.Add(save);
      }

      if (save.IsDeleted())
      {
        writer.WriteLine($"File {save.Name} has been deleted. Restoring file");
        save.Restore();
      }
    }

    foreach(var save in savesToUpdate)
    {
      saves.Add(save);
    }

    foreach (var save in saveGamesToDelete)
    {
      saves.Remove(save);
    }

    Task.Delay(1000 * delay).Wait();
  }

  writer.WriteLine("Barony has closed. Closing the program");
  return;
}

static Process? FindBaronyProcess()
{
  string processName = "barony";

  var processes = Process.GetProcessesByName(processName);

  if (processes.Length == 0)
  {
    return null;
  }
  return processes.FirstOrDefault();
  }

static string GetProcessFolder(Process process)
{
  string processPath = process.MainModule.FileName;
  return System.IO.Path.GetDirectoryName(processPath);
}

static string GetSaveFolder(Process process)
{
  string processPath = GetProcessFolder(process);
  return System.IO.Path.Combine(processPath, "savegames");
}

static List<SaveGame> GetSaveGames(Process process)
{
  string saveFolder = GetSaveFolder(process);
  string[] files = System.IO.Directory.GetFiles(saveFolder).Where(p => p.EndsWith(".baronysave")).ToArray();
  return files.Select(f => new SaveGame(f)).ToList();
}

static List<SaveGame> GetNewSaves(Process process, List<SaveGame> saves)
{
  string saveFolder = GetSaveFolder(process);
  string[] files = System.IO.Directory.GetFiles(saveFolder).Where(p => p.EndsWith(".baronysave")).ToArray();
  List<SaveGame> newSaves = new();
  foreach (var file in files)
  {
    if (!saves.Any(s => s.Path == file))
    {
      newSaves.Add(new SaveGame(file));
    }
  }
  return newSaves;
}
