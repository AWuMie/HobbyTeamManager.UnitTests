using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading;
using System.Diagnostics.CodeAnalysis;

namespace HobbyTeamManager.UnitTests.UnitTests;

internal class DummySession : ISession
{
    public DummySession()
    {
        Values = new Dictionary<string, byte[]>();
    }

    public Dictionary<string, byte[]> Values { get; set; }

    public bool IsAvailable => true;

    public string Id => "session_id";

    public IEnumerable<string> Keys => Values.Keys;

    public void Clear()
    {
        Values.Clear();
    }

    public Task CommitAsync(CancellationToken cancellationToken = default)
    {
        throw new System.NotImplementedException();
    }

    public Task LoadAsync(CancellationToken cancellationToken = default)
    {
        throw new System.NotImplementedException();
    }

    public void Remove(string key)
    {
        Values.Remove(key);
    }

    public void Set(string key, byte[] value)
    {
        if (Values.ContainsKey(key))
        {
            Remove(key);
        }
        Values.Add(key, value);
    }

    public bool TryGetValue(string key, [NotNullWhen(true)] out byte[]? value)
    {
        if (Values.ContainsKey(key))
        {
            value = Values[key];
            return true;
        }
        value = new byte[0];
        return false;
    }
}