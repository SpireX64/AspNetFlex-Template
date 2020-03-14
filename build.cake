#addin "Cake.Incubator&version=5.1.0"

// arguments
var target = Argument("target", "Default");
var configuration = Argument("c", "Debug");

// constants
var mainProject = "Estimate.WebApp";
var framework = "netcoreapp3.1";

var environments = new[] {
    "Development",
    "Production"
};

foreach (var env in environments)
{
    Task($"Run{env}")
        .Does(() => {
        var runSettings = new DotNetCoreRunSettings {
            Framework = framework,
            Configuration = configuration,
            EnvironmentVariables = new Dictionary<string, string> {
                ["ASPNETCORE_ENVIRONMENT"] = env
            }
        };
        DotNetCoreRun(mainProject, "--args", runSettings);
    });
}

Task("Default");

RunTarget(target);