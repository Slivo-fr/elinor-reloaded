namespace Elinor
{
    internal class StandingWrapper
    {
        public readonly string name;

        internal StandingWrapper(string name, double standing)
        {
            this.name = name;
            this.standing = standing;
        }

        internal double standing { get; private set; }

        public override string ToString()
        {
            return name;
        }
    }
}