using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.Cards;
using Domain.Process;
using Infrastructure.Loop;

public abstract class Game
    {
    private readonly IStorage _storage;
    private readonly IRules _rules;
    private readonly List<Card> _cards;
    public GameState state = new GameState();

    protected Game(IStorage storage, IRules rules, List<Card> cards)
    {
        _storage = storage;
        _rules = rules;
        _cards = cards;
    }
    public void Connection()//изменение
    {
        var users = ConnectUsers();
        List<Player> players = CreatePlayers(users).ToList();
        SetStartState(players);
    }
    private void SetStartState(IEnumerable<Player> players)//изменение
    {
        state.MovingPlayer = players.First();
        state.WaitingPlayer = players.Last();
        state.TargetCards = new List<Card>();
    }
    private IEnumerable<User> ConnectUsers()
    {
        int id1 = ConnectUser();
        int id2 = ConnectUser();
        User user1 = _storage.FindById<User>(id1).First();
        User user2 = _storage.FindById<User>(id2).First();
        return new[] { user1, user2 };
    }

    public abstract int ConnectUser();

    private IEnumerable<Player> CreatePlayers(IEnumerable<User> users)
    {
        var players = new List<Player>();
        users.ForEach(user =>
        {
            var playerCards = user.CardList.Select(id => _cards.FirstOrDefault(x => x.Id == id)?.Clone()).ToList();
            players.Add(new Player(user.Name, playerCards, _rules));
        });
        return players;
    }

    public abstract void ShowPlayers(IEnumerable<Player> players);

    public abstract void OfferToChangeCards(IEnumerable<Player> players);
    public void Move()
    {
        CardActionEnum actionWay = GetAttackWay();
        state.ActionCard.Action(actionWay, state);
        //ShowActionResult(moveResult);

        AfterMove();
    }
    /*private MoveResult GetValue()
    {
        CardActionEnum actionWay = GetAttackWay();
        MoveResult moveResult = state.ActionCard.Action(actionWay, state);
        ShowActionResult(moveResult);

        AfterMove();
        return moveResult;
    }*/
    public void AfterMove() => state.MovingPlayer.AfterMove();
    public void CompleteStep()
    {
        ChangeOfMovingPlayer();
        StartStep();
    }
    public void StartStep() => state.MovingPlayer.StartStep();
    public void ChangeOfMovingPlayer()//изменение
    {
        Player waitingPlayer = state.MovingPlayer;
        state.MovingPlayer = state.WaitingPlayer;
        state.WaitingPlayer = waitingPlayer;       
    }
    public abstract void ShowInfo(Player current, Player another);

    public abstract Card GetActionCard(Player actionPlayer);

    public abstract List<Card> GetTargetCards(Player targetPlayer);

    public abstract CardActionEnum GetAttackWay();

    public abstract void InformAboutAttack(
        Card actionCard, IEnumerable<Card> targetCards, CardActionEnum actionWay);

    public abstract void ShowActionResult(MoveResult moveResult);

    public abstract void ShowWinner(Player winner);
}

