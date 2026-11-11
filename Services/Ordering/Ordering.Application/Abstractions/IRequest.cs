namespace Ordering.Application.Abstractions;

public interface IRequest<TResponse>;

public interface IRequest;

public readonly struct Unit : IEquatable<Unit>, IComparable<Unit>, IComparable
{
    public static readonly Unit Value = new Unit();
    
    public override int GetHashCode() => 0;

    public override bool Equals(object? obj) => obj is Unit;

    public bool Equals(Unit other) => true;

    public static bool operator ==(Unit left, Unit right) => true;

    public static bool operator !=(Unit left, Unit right) => false;
    
    public int CompareTo(Unit other) => 0;

    int IComparable.CompareTo(object? obj) => obj is Unit ? 0 : -1;

    public override string ToString() => "()";
}