using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace Application2
{
    public abstract class Game
    {
        protected IStorage Storage;
        protected List<ICard> Cards;
        protected IRules Rules;
        protected bool GameInProgress;

        public void Run(IStorage storage)
        {
            Storage = storage;

            var types = ImportTypes().ToList();
            Cards = types.SelectInstancesOf<ICard>().ToList();
            Rules = types.SelectInstancesOf<IRules>()?.FirstOrDefault();
            
            var users   = LoadUsers();
            var players = LoadPlayers(users).ToList();
            var player  = SelectFirst(players);
            OfferToChangeCards(players);
            while (GameInProgress)
            {
                Play(players);
            }
        }

        public abstract IEnumerable<Type> ImportTypes();

        public abstract IEnumerable<User> LoadUsers();

        public abstract IEnumerable<Player> LoadPlayers(IEnumerable<User> users);

        public abstract Player SelectFirst(IEnumerable<Player> players);

        public abstract void OfferToChangeCards(IEnumerable<Player> players);

        public abstract void Play(IEnumerable<Player> players);
    }
}
