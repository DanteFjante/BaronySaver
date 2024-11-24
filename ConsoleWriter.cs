using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barony_Saver
{

  public class ConsoleWriter
  {
    List<string> lines = new List<string>();
    public void Write()
    {
      Console.Clear();
      foreach (var line in lines)
      {
        Console.WriteLine(line);
      }
    }

    public void AddLine(string line, int? index = null)
    {
      if (index.HasValue)
      {
        lines.Insert(index.Value, line);
        return;
      }
      lines.Add(line);
    }

    public void ClearLine(string line)
    {
      lines.Remove(line);
    }

    public void ClearLine(int index)
    {
      lines.RemoveAt(index);
    }

    public void ClearLine()
    {
      lines.Remove(lines.Last());
    }

    public void Clear()
    {
      lines.Clear();
    }

    public void AddUniqueLine(string line)
    {
      if (!lines.Contains(line))
      {
        lines.Add(line);
      }
    }

    public bool HasLine(string line)
    {
      return lines.Contains(line);
    }

    public void WriteLine(string line)
    {
      AddLine(line);
      Write();
    }
  }
}
