using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Domain.Cards;
using Domain.Process;
using Plugin;//исправить
using System.Collections;
using System.Threading;

public class GameScript : MonoBehaviour {
    public GameObject AttackWayPanel;
    public Card ActionCard
    {
        get { return game.state.ActionCard; }
        set { game.state.ActionCard = value; }
    }
    public Card TargetCard
    {
        get { return (game.state.TargetCards == null) ? null : game.state.TargetCards.FirstOrDefault(); }
        set { game.state.TargetCards = new List<Card>() { value }; }
    }
    private CardActionEnum _attackWay;
    public CardActionEnum AttackWay
    {
        get { return _attackWay; }
        set
        {
            AttackWayPanel.SetActive(false);
            _attackWay = value;
            Move();
        }
    }
    GameUnity game;
    #region delegates and events
    public delegate void OnAfterMoveHandler();
    public event OnAfterMoveHandler onAfterMove;
    #endregion
    // Use this for initialization
    void Start () {
        var storage = new Storage();
        var types = ImportTypes();
        var rules = types.SelectInstancesOf<IRules>()?.FirstOrDefault();
        var cards = types.SelectInstancesOf<Card>().ToList();
        CreateGame(storage, rules, cards);
        CreatePlayers();
    }
    private void CreateGame(Storage storage, IRules rules, List<Card> cards)
    {
        game = new GameUnity(GameObject.FindWithTag("Scripts").GetComponent<GameScript>(), storage, rules, cards);
        game.Connection();
    }
    private void CreatePlayers()
    {
        CreatePlayer(GetComponent<Gamer>(), game.state.MovingPlayer);
        CreatePlayer(GetComponent<Enemy>(), game.state.WaitingPlayer);
    }
    private void CreatePlayer(IPlayer playerUnity, Player player)
    {
        playerUnity.player = player;
        playerUnity.LocateCards();
    }
    void Update()
    {
        if (ActionCard != null && TargetCard != null)
        {
            if(!ActionCard.Closed)
                AttackWayPanel.SetActive(true);            
        }
    }
    public void SetToZeroCards()
    {
        ActionCard = null;
        TargetCard = null;
    }
    public GameState GetGameState()
    {
        return game.state;
    }
    private void Move()
    {
        game.Move();
        SetToZeroCards();
        onAfterMove();
    }
    public void CompleteStep()
    {
        game.CompleteStep();
    }
    private IEnumerable<Type> ImportTypes()
    {
        string path = Path.Combine(Directory.GetCurrentDirectory(), "Assets\\Scripts\\DLL");
        var dllPaths = Directory.GetFiles(path, "*.dll");
        return dllPaths.Select(Assembly.LoadFrom).SelectMany(x => x.GetExportedTypes());
    }

}
