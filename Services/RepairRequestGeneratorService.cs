using Tracker.Models;

    public class RepairRequestGeneratorService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly Random _random = new Random();
        private readonly string[] _titles = { "Fix Sink", "Repair Light", "Install AC", "General Maintenance", "Replace Pipes", "Boiler Service" };

        public RepairRequestGeneratorService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var repairRequestService = scope.ServiceProvider.GetRequiredService<RepairRequestService>();

                repairRequestService.CreateRepairRequest(RandomTitle(), RandomRepairType(), RandomRequestDate());
            }

                await Task.Delay(TimeSpan.FromSeconds(3), stoppingToken);
            }
        }

        private string RandomTitle()
        {
            return _titles[_random.Next(_titles.Length)];
        }

        private RepairType RandomRepairType()
        {
            return (RepairType)_random.Next(Enum.GetNames(typeof(RepairType)).Length);
        }

        private DateTime RandomRequestDate()
        {
            // Generate a random date within the last 30 days.
            DateTime start = DateTime.UtcNow.AddDays(-30);
            return start.AddDays(_random.Next(30));
        }
    }