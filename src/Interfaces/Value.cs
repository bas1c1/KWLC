namespace KWLC
{
    public interface Value
    {
        double asDouble();
        string asString();
        int asNumber();
        char asChar();
        ValueType type();
    }
}
