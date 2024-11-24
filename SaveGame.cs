using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Barony_Saver
{
  public class SaveGame
  {
    public string Path { get; private set; }
    public readonly byte[] Bytes;
    public readonly DateTime LastWriteTime;

    public string Name => System.IO.Path.GetFileName(Path);

    public SaveGame(string path)
    {
      Path = path;

      Bytes = File.ReadAllBytes(Path);
      LastWriteTime = File.GetLastWriteTime(Path);
    }

    public SaveGame Update()
    {
      return new SaveGame(Path);
    }

    public bool IsDeleted()
    {
      return !File.Exists(Path);
    }

    public bool IsOverwritten()
    {
      if (!IsDeleted())
      {
        return File.GetCreationTime(Path) > LastWriteTime;
      }
      return false;
    }

    public bool IsUpdated()
    {
      if (!IsDeleted())
      {
        return !Bytes.SequenceEqual(File.ReadAllBytes(Path));
      }
      return false;
    }

    public void Restore()
    {
      File.WriteAllBytes(Path, Bytes);
    }

    public void Rename(string newName)
    {
      Path = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Path), newName);
    }
  }
}
