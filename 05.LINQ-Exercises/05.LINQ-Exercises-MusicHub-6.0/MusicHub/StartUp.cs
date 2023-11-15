namespace MusicHub
{
    using System;
    using System.Text;
    using Data;
    using Initializer;

    public class StartUp
    {
        public static void Main()
        {
            MusicHubDbContext context =
                new MusicHubDbContext();

            DbInitializer.ResetDatabase(context);

            int producerId = 9;
            Console.WriteLine(ExportAlbumsInfo(context, producerId));
        }

        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            var albums = context.Albums
             .Where(p=>p.ProducerId==producerId)
             .Select(e => new
             {
                    e.Name,
                    e.ReleaseDate,
                    ProducerName=e.Producer.Name,
                    e.Price,
                    Songs=e.Songs
                    .Select(s=> new
                    {
                        s.Name,
                        s.Price,
                        WriterName=s.Writer.Name
                    })
                    .OrderByDescending(s=>s.Name)
                    .ThenBy(w=>w.WriterName)
                    .ToList()
             })
             .OrderByDescending(s=>s.Songs.Sum(x=>x.Price))
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
            throw new NotImplementedException();

        }
    }
}
