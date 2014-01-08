namespace Elinor
{
    internal class CharWrapper
    {
        internal long CharId { get; set; }
        internal string KeyId { get; set; }
        internal string Charname { get; set; }
        internal string VCode { get; set; }

        public override string ToString()
        {
            return Charname;
        }
    }
}