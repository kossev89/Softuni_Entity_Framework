namespace MusicHub
{
    using System;
    using System.Text;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;
    using MusicHub.Data.Models;

    public class StartUp
    {
        public static void Main()
        {
            MusicHubDbContext context =
                new MusicHubDbContext();

            DbInitializer.ResetDatabase(context);
            int duration = 4;
            Console.WriteLine(ExportSongsAboveDuration(context, duration));


        }

        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            var albums = context.Albums
             .Where(p => p.ProducerId == producerId)
             .Select(e => new
             {
                 e.Name,
                 e.ReleaseDate,
                 ProducerName = e.Producer.Name,
                 e.Price,
                 Songs = e.Songs
                    .Select(s => new
                    {
                        s.Name,
                        s.Price,
                        WriterName = s.Writer.Name
                    })
                    .OrderByDescending(s => s.Name)
                    .ThenBy(w => w.WriterName)
                    .ToList()
             })
             .OrderByDescending(s => s.Songs.Sum(x => x.Price))
             .ToList();

            StringBuilder output = new();

            foreach (var a in albums)
            {
                output.AppendLine($"-AlbumName: {a.Name}");
                output.AppendLine($"-ReleaseDate: {a.ReleaseDate.ToString("MM/dd/yyyy")}");
                output.AppendLine($"-ProducerName: {a.ProducerName}");
                output.AppendLine($"-Songs:");
                int songNumber = 0;
                foreach (var s in a.Songs)
                {
                    songNumber++;
                    output.AppendLine($"---#{songNumber}");
                    output.AppendLine($"---SongName: {s.Name}");
                    output.AppendLine($"---Price: {s.Price:f2}");
                    output.AppendLine($"---Writer: {s.WriterName}");
                }
                output.AppendLine($"-AlbumPrice: {a.Price:f2}");
            }
            return output.ToString().Trim();
        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            TimeSpan durationInSeconds = TimeSpan.FromSeconds(duration);
            var songs = context.Songs
                .Where(x => x.Duration > durationInSeconds)
                .Select(e => new
                {
                    e.Name,
                    Performers = e.SongPerformers
                    .Select(sp => new
                    {
                        sp.Performer.FirstName,
                        sp.Performer.LastName
                    })
                    .OrderBy(fn => fn.FirstName)
                    .ThenBy(ln=>ln.LastName)
                    .ToList(),
                    WriterName = e.Writer.Name,
                    ProducerName = e.Album.Producer.Name,
                    e.Duration
                })
                .OrderBy(n => n.Name)
                .ThenBy(wn => wn.WriterName)
                .ToList();

            StringBuilder sb = new();
            int songNumber = 0;

            foreach (var s in songs)
            {
                songNumber++;
                sb.AppendLine($"-Song #{songNumber}");
                sb.AppendLine($"---SongName: {s.Name}");
                sb.AppendLine($"---Writer: {s.WriterName}");
                if (s.Performers.Any())
                {
                    foreach (var p in s.Performers)
                    {
                        sb.AppendLine($"---Performer: {p.FirstName} {p.LastName}");
                    }
                }
                sb.AppendLine($"---AlbumProducer: {s.ProducerName}");
                sb.AppendLine($"---Duration: {s.Duration.ToString("c")}");
            }
            return sb.ToString().Trim();
        }
    }
}
