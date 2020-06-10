using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using TestRailApiMock = Ycode.TestRailClient.Test.ApiMock.Program;

namespace Ycode.TestRailClient.V2.NUnit.Test
{
    public class TestBase
    {
        protected TestRailApiMock TestRailApiMock
            => SetUp.TestRailApiMock;

        public DirectoryInfo FindProjectDirByType()
        {
            var assembly = Assembly.GetAssembly(GetType());
            var assemblyFile = new FileInfo(assembly.Location);
            var dir = assemblyFile.Directory;
            while (dir != null)
            {
                var projFiles = dir.GetFiles("*.csproj");
                if (projFiles.Length > 0)
                {
                    return dir;
                }
                dir = dir.Parent;
            }
            throw new Exception($"Failed to find a project file.");
        }

        public FileInfo GetSampleProjectFile(string name)
        {
            var baseDir = FindProjectDirByType();
            return baseDir.Parent
                .GetDirectories("samples").FirstOrDefault()
                ?.GetDirectories(name).FirstOrDefault()
                ?.GetFiles(name + ".csproj").FirstOrDefault();
        }

        public async Task<NUnitResult> RunNUnitTestAsync(string projFilePath)
        {
            var command = new ProcessStartInfo("dotnet")
            {
                ArgumentList = { "test", projFilePath },
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                RedirectStandardInput = true,
            };

            using (var process = Process.Start(command))
            {
                while (!process.HasExited)
                {
                    await Task.Delay(100);
                }

                var std = process.StandardOutput.ReadToEnd();
                var err = process.StandardError.ReadToEnd();

                await Console.Out.WriteLineAsync(std);
                await Console.Error.WriteLineAsync(err);

                return new NUnitResult(std, err);
            }
        }
    }
}
