using AO;

/// <summary>
/// A collection where you can grab things randomly, and it guarantees that every element is gotten before it "restocks" to supply another full set.
/// todo Would be good to ensure the new restock next element is never be the same as the last item gotten   
/// </summary>
/// <typeparam name="T"></typeparam>
public class Stock<T>
{
    public List<T> Items = new();
    public List<int> Remaining = new();

    public int Size { get; private set; }
    public int RemainingBeforeRestock => Remaining.Count;

    public int LastElementCollected = -1;

    public Stock() { }

    public Stock(IEnumerable<T> items)
    {
        Size = items.Count();
        Items = items.ToList();
        Restock();
    }

    public void Restock()
    {
        Remaining.Clear();
        for (int i = 0; i < Items.Count; i++)
        {
            Remaining.Add(i);
        }

        Remaining.Shuffle();

        if (Remaining.Count > 1 && Remaining.Last() == LastElementCollected)
        {
            var random = Random.Shared.Next(0, Remaining.Count - 1);

            (Remaining[random], Remaining[^1]) = (Remaining[^1], Remaining[random]);
        }
    }

    public T GetRandom()
    {
        if (Remaining.Count == 0)
        {
            Restock();
        }

        int i = Remaining.Pop();
        LastElementCollected = i;
        return Items[i];
    }

    public void RemoveRemaining(T remove)
    {
        int i = Items.IndexOf(remove);
        if (i >= 0)
        {
            Remaining.Remove(i);
        }
    }
}