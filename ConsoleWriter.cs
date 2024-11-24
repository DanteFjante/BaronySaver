using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barony_Saver
{

  public class ConsoleWriter
  {
    List<ConsoleLine> lines = new List<ConsoleLine>();
    public void Write()
    {
      Console.Clear();
      foreach (var line in lines)
      {
        Console.WriteLine(line);
      }
    }
    public void AddLineWithoutTimeStamp(string line)
    {
      lines.Add(new(line, DateTime.MinValue));
    }

    public void AddLine(string line, int? index = null)
    {
      if (index.HasValue)
      {
        lines.Insert(index.Value, new(line));
        return;
      }
      lines.Add(new(line));
    }

    public void ClearLine(string line)
    {
      var lineToRemove = lines.FirstOrDefault(l => l.line.Equals(line));
      if (lineToRemove != null)
      {
        lines.Remove(lineToRemove);
      }
    }

    public void ClearLine(int index)
    {
      lines.RemoveAt(index);
    }

    public void ClearLine(DateTime timestamp)
    {
      var lineToRemove = lines.FirstOrDefault(l => l.timeStamp == timestamp);
      if (lineToRemove != null)
      {
        lines.Remove(lineToRemove);
      }
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
      if(HasLine(line))
      {
        ClearLine(line);
      }

      if (!HasLine(line))
      {
        lines.Add(new(line));
      }
    }

    public bool HasLine(string line)
    {
      return lines.Any(l => l.line.Equals(line));
    }

    public void WriteLine(string line)
    {
      AddLine(line);
      Write();
    }

    public static string GetTimeStamp(DateTime? time = null)
    {
      return time.HasValue ? time.Value.ToString("HH:mm:ss") : DateTime.Now.ToString("HH:mm:ss");
    }


    public static DateTime GetTimeStamp()
    {
      return DateTime.Now;
    }


    private class ConsoleLine
    {
      public DateTime timeStamp;
      public string line;

      public ConsoleLine(string line, DateTime? timestamp = null)
      {
        this.timeStamp = timestamp ?? GetTimeStamp();
        this.line = line;
      }

      public override string ToString()
      {
        if(DateTime.MinValue == timeStamp)
        {
          return line;
        }
        return $"{GetTimeStamp(timeStamp)}: {line}";
      }
    }
  }
}
