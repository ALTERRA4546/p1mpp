var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.RoadsOfRussiaAPI>("roadsofrussiaapi");

builder.Build().Run();
