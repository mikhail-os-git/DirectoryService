using System.Collections;

namespace General.Errors;

public class FailList : IEnumerable<Failure>, IEquatable<FailList>
{
    private readonly List<Failure> _failures;
    public int Count => _failures.Count;
    public FailList(IEnumerable<Failure> failures)
    {
        _failures = [..failures];
    }

    public static implicit operator FailList(Failure failure) => new FailList([failure]);
    public static implicit operator FailList(List<Failure> failures) => new FailList(failures);
    public static implicit operator FailList(Failure[] failures) => new FailList(failures);

    public void AddFailure(Failure failure)
    {
        this._failures.Add(failure);
    }

    public void AddFailures(IEnumerable<Failure> failures)
    {
        this._failures.AddRange(failures);
    }
    
    public IEnumerator<Failure> GetEnumerator() => _failures.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_failures).GetEnumerator();

    public Failure this[int index]
    {
        get => _failures[index]; 
        set => _failures[index] = value;
    }

    public bool Equals(FailList? other) => other is not null && _failures.SequenceEqual(other._failures);
    public override bool Equals(object? obj) => Equals(obj as FailList);
    public override int GetHashCode() => _failures.Aggregate(0, HashCode.Combine);
}