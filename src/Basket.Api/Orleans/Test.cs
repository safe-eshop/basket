﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace Basket.Api.Orleans
{
    public class HelloArchiveGrain : Grain, IHelloArchive
    {
        private readonly IPersistentState<GreetingArchive> _archive;

        public HelloArchiveGrain([PersistentState("archive", "ArchiveStorage")] IPersistentState<GreetingArchive> archive)
        {
            _archive = archive;
        }

        public async Task<string> SayHello(string greeting)
        {
            _archive.State.Greetings.Add(greeting);

            await _archive.WriteStateAsync();

            return $"You said: '{greeting}', I say: Hello!";
        }

        public Task<IEnumerable<string>> GetGreetings() => Task.FromResult<IEnumerable<string>>(_archive.State.Greetings);
    }

    public class GreetingArchive
    {
        public List<string> Greetings { get; } = new List<string>();
    }

    public interface IHelloArchive : Orleans.IGrainWithIntegerKey
    {
        Task<string> SayHello(string greeting);

        Task<IEnumerable<string>> GetGreetings();
    }
}