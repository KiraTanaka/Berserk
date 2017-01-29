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
            onAfterMove();
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
        var rules = new Rules();
        var cards = new List<Card>() { new Card87242(), new Card92314(), new Card87065(), new Card95380(), new Card87693(),new Card87689() };
        game = new GameUnity(GameObject.FindWithTag("Scripts").GetComponent<GameScript>(), storage, rules, cards);
        game.Connection();
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
    void Move()
    {
        game.Move();
        SetToZeroCards();
    }
    public void CompleteStep()
    {
        game.CompleteStep();
    }
    /* private static IEnumerable<Type> ImportTypes()
     {
         /*const string dllFolderPath = @"C:\Users\Akira\Berserk\UserInterface2D\Assets\Plugins";
         var dllPaths = Directory.GetFiles(dllFolderPath, "*.dll");
         return dllPaths.Select(Assembly.LoadFrom).SelectMany(x => x.ExportedTypes);*/
    // }*/

}
