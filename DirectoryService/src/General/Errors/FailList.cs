using System.Collections;

namespace General.Errors;

public class FailList : IEnumerable<Failure>
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
    
    // public static FailList FromIEnumerable(IEnumerable<Failure> failures) => new FailList(failures);
    public IEnumerator<Failure> GetEnumerator() => _failures.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_failures).GetEnumerator();

    public Failure this[int index]
    {
        get => _failures[index]; 
        set => _failures[index] = value;
    }
}