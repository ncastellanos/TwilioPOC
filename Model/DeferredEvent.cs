namespace TwilioPOC.Model
{
    public class DeferredEvent : DeliveredEvent
    {
        public int Attempt { get; set; }

    }
}
