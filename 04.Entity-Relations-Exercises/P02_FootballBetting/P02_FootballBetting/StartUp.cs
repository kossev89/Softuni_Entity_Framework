using P02_FootballBetting.Data;

namespace P02_FootballBetting
{
    internal class StartUp
    {
        static void Main(string[] args)
        {
            using FootballBettingContext context = new();

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }
    }
}