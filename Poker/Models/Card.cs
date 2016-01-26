namespace Poker.Models
{
    using Poker.Contracts;

    public class Card
    {
        public byte Strenght { get; private set; }

        public SuitType Suit { get; private set; }
    }
}