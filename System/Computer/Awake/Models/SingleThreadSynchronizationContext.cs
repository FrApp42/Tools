// Source Microsoft Powertoys
// License MIT
// https://github.com/microsoft/PowerToys

namespace FrApp42.System.Computer.Awake.Models
{
    internal sealed class SingleThreadSynchronizationContext : SynchronizationContext
    {
        private readonly Queue<Tuple<SendOrPostCallback, object>> queue =
            new();

#pragma warning disable CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).
        public override void Post(SendOrPostCallback d, object state)
#pragma warning restore CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).
        {
            lock (queue)
            {
                queue.Enqueue(Tuple.Create(d, state));
                Monitor.Pulse(queue);
            }
        }

        public void BeginMessageLoop()
        {
            while (true)
            {
                Tuple<SendOrPostCallback, object> work;
                lock (queue)
                {
                    while (queue.Count == 0)
                    {
                        Monitor.Wait(queue);
                    }

                    work = queue.Dequeue();
                }

                if (work == null)
                {
                    break;
                }

                work.Item1(work.Item2);
            }
        }

        public void EndMessageLoop()
        {
            lock (queue)
            {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                queue.Enqueue(null);  // Signal the end of the message loop
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
                Monitor.Pulse(queue);
            }
        }
    }
}
