﻿namespace TwilioPOC.Model
{
    public class DroppedEvent : Event
    {
        public string Reason { get; set; }
        public string Status { get; set; }
    }
}
