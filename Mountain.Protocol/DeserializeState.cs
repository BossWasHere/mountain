namespace Mountain.Protocol
{
    public enum DeserializeState
    {
        Done,
        TooLong,
        TooShort,
        UnknownPacket,
        BadData
    }
}
