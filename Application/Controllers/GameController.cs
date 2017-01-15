using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using Application.Models;
using Domain;
using Domain.Cards;
using IStorage = Application.Models.IStorage;

namespace Application.Controllers
{
    public class GameController : ApiController
    {
        private readonly IEnumerable<Card> _cards;
        private readonly IStorage _storage;

        public GameController()
        {
            const string dllFolderPath = @"E:\Projects\kontur\Berserk\Plugins";
            var dllPaths = Directory.GetFiles(dllFolderPath, "*.dll");

            var exportedTypes = dllPaths
                .Select(Assembly.LoadFrom)
                .SelectMany(x => x.ExportedTypes);

            _cards = exportedTypes.SelectInstancesOf<Card>();
            _storage = new StorageMock();
        }

        [HttpPost]
        public bool Connect(int id)
        {
            return true;
            //write insert logic  

        }

        //[HttpPost]
        //public IHttpActionResult Connect(int userId)
        //{
        //    var player = _storage.FindById<User>(userId);
        //    if (player == null) return NotFound();
        //    return Ok();
        //}

        public IEnumerable<string> GetCards()
        {
            return _cards.Select(x => x.Name).ToList();
        }

        public IHttpActionResult GetCard(int id)
        {
            var card = _cards.FirstOrDefault(x => x.Id == id);
            if (card == null)
            {
                return NotFound();
            }
            return Ok(card.Name);
        }
    }

    public static class Extensions
    {
        public static IEnumerable<T> SelectInstancesOf<T>(this IEnumerable<Type> types)
            => types.Where(TypeIs<T>).Select(InstanceOf<T>);

        public static bool TypeIs<T>(Type t)
            => typeof(T).IsAssignableFrom(t) && t.GetConstructor(Type.EmptyTypes) != null;

        public static T InstanceOf<T>(Type t)
            => (T)Activator.CreateInstance(t);
    }
}
