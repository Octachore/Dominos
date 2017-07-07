using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Tests
{
    public class AbstractApiTest
    {
        private Process _process;
        private static readonly HttpClient _client = new HttpClient();
        protected const string BASE_URL = "http://127.0.0.1:8080";

        public string ServerExecutable { get; } = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}/../../../Backend/bin/Debug/Backend.exe";

        [OneTimeSetUp]
        public void Initialize() => _process = Process.Start(ServerExecutable);

        [OneTimeTearDown]
        public void Terminate() => _process.Kill();

        public async Task<string> Post(string route, IDictionary<string, string> values = null)
        {
            values = values ?? new Dictionary<string, string>();
            HttpResponseMessage response = await _client.PostAsync($"{BASE_URL}{route}", new FormUrlEncodedContent(values));
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> Get(string route)
        {
            return await _client.GetStringAsync($"{BASE_URL}{route}");
        }
    }
}
