using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Domain.Cards;
using Domain.Process;
using Plugin;//исправить
using UnityEngine.Networking;

public class GameScript : NetworkBehaviour
{
    private int _playerCount = 0;
    public int? ActionCardId
    {
        get { return (game.state.ActionCard == null) ? null : (int?)game.state.ActionCard.Id; }
        set {
            Card card;
            if (value != null)
            {
                card = SearchCardAtPlayers(value.Value);
                game.state.ActionCard = (!card.Closed) ? card : null;
            }
            else game.state.ActionCard = null;
        }
    }
    public int? TargetCardId
    {
        get { return (game.state.TargetCards == null) ? null : (int?)game.state.TargetCards.FirstOrDefault().Id; }
        set { game.state.TargetCards = (value != null) ? new List<Card>() { SearchCardAtPlayers(value.Value) } : null; }
    }
    private CardActionEnum _attackWay;
    public CardActionEnum AttackWay
    {
        get { return _attackWay; }
        set
        { 
            _attackWay = value;
            Move();
        }
    }
    GameUnity game;
    #region delegates and events
    public delegate void OnAfterMoveHandler();
    public delegate void OnStartStepHandler(string playerId);
    public event OnStartStepHandler onStartStep;
    public event OnAfterMoveHandler onAfterMove;
    #endregion
    // Use this for initialization
    void Start () {
        var storage = new Storage();
        var types = ImportTypes();
        var rules = types.SelectInstancesOf<IRules>()?.FirstOrDefault();
        var cards = types.SelectInstancesOf<Card>().ToList();
        CreateGame(storage, rules, cards);
    }
    private void CreateGame(Storage storage, IRules rules, List<Card> cards)
    {
        game = new GameUnity(this, storage, rules, cards);
        game.Connection();
    }
    public Player GetPlayer()
    {
        if (_playerCount >= 2) _playerCount = 0;
        
        return (_playerCount++ == 0) ? game.state.MovingPlayer : game.state.WaitingPlayer;
    }
    
    public void SetToZeroCards()
    {
        ActionCardId = null;
        TargetCardId = null;
    }
    public GameState GetGameState() => game.state;
    private void Move()
    {
        game.Move();
        SetToZeroCards();
        onAfterMove?.Invoke();
    }
    public void CompleteStep(string playerId)
    {
        if (GetGameState().MovingPlayer.Id.ToString() == playerId)
        {
            game.CompleteStep();
            onStartStep?.Invoke(GetGameState().MovingPlayer.Id.ToString());
        }
    }
    private Card SearchCardAtPlayers(int id)
    {
        var state = GetGameState();
        return SearchCard(id, state.MovingPlayer) ?? SearchCard(id, state.WaitingPlayer);
    }
    private Card SearchCard(int id, Player player)
    {
        Card card = player.CardsInGame.FirstOrDefault(x => x.Id == id);
        return (card == null) ? ((player.Hero.Id == id) ? player.Hero : null) : card;
    }
    private IEnumerable<Type> ImportTypes()
    {
        string path = Path.Combine(Directory.GetCurrentDirectory(), "Assets\\Scripts\\DLL");
        var dllPaths = Directory.GetFiles(path, "*.dll");
        return dllPaths.Select(Assembly.LoadFrom).SelectMany(x => x.GetExportedTypes());
    }

}
