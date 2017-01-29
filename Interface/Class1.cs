using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plugin;
using Domain;
using Infrastructure;
namespace Interface
{
    public class Class1
    {
        Rules rul;
        Domain.Cards.Card card;
        int random;
        public Class1 ()
        {
            rul = new Rules();
            card = new Domain.Cards.Card();
            random = Infrastructure.Random.RandomHelper.Next(1);
        }
    }
}
