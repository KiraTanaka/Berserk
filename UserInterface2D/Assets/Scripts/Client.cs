using Domain.Cards;
using Domain.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class Client : NetworkBehaviour
{
    private PlayerUnity _player;
    public GameObject AttackWayPanel;
    Enemy enemy;
    public void Settings(PlayerUnity player)
    {
        _player = player;
        SubscribeToAddCoins();
        SubscribeToSrartStep();
        GetEnemy();
    }
    void GetEnemy() => CmdGetEnemy(_player.Id.ToString());
    [Command]
    void CmdGetEnemy(string playerId)
    {
        GameState state = GetGame().GetGameState();
        Player player=(state.MovingPlayer.Id.ToString() == playerId) ? state.WaitingPlayer : state.MovingPlayer;
        CardInfo heroInfo = new CardInfo() { _id = player.Hero.Id, _health = player.Hero.Health };
        RpcEnemyInitialization(player.Id.ToString(), heroInfo, player.ActiveDeck.Count(), player.Money.Count);
    }
    [ClientRpc]
    void RpcEnemyInitialization(string playerId, CardInfo heroInfo, int countCards, int countCoin)
    {
        if (!isLocalPlayer) return;

        enemy = GameObject.FindGameObjectsWithTag("Enemy").Select(x => x.GetComponent<Enemy>())
          .FirstOrDefault(x => x.localPlayerAuthority);
        enemy?.OnStartPlayer(playerId, heroInfo, countCards, countCoin);
    }
    void SubscribeToSrartStep() => CmdSubscribeToSrartStep();
    [Command]
    void CmdSubscribeToSrartStep()
    {
        if (!isServer) return;
        GetGame().onStartStep += CmdonStartStep;
    }
    [Command]
    void CmdonStartStep(string playerId) => RpconStartStep(playerId);
    [ClientRpc]
    void RpconStartStep(string playerId)
    {
        //if (!isLocalPlayer) return;
        //if (_player.Id.ToString() == playerId)
            OpenAll(playerId);
    }
    [Command]
    void CmdonAddCoin() => RpconAddCoin();
    [Command]
    void CmdonAddEnemyCoin() => RpconAddEnemyCoin();
    [ClientRpc]
    void RpconAddCoin()
    {
        if (!isLocalPlayer) return;
        _player.onAddCoin();
    }
    [ClientRpc]
    void RpconAddEnemyCoin()
    {
        if (!isLocalPlayer) return;
        enemy.onAddCoin();
    }
    public void SubscribeToSelectCardInHand(GameObject sprite) 
        => sprite.GetComponent<CardInHand>().onSelectCard += onSelectCard;
    public void SubscribeToSelectHero(GameObject sprite)
    {
        Hero hero = sprite.GetComponent<Hero>();
        hero.onSelectCard += onSelectActiveCard;
        CmdSubscribeToActiveCard(hero.CardId);
    }
    public void SubscribeToActiveCard(GameObject sprite)
    {
        CardUnity card  = sprite.GetComponent<CardUnity>();
        card.onSelectCard += onSelectActiveCard;
        CmdSubscribeToActiveCard(card.CardId);
    }
    public void SubscribeToAddCoins() => CmdSubscribeToAddCoins(_player.Id.ToString());
    [Command]
    void CmdSubscribeToAddCoins(string playerId)
    {
        GameState state = GetGame().GetGameState();
        Player player;
        Player enemy;
        if (state.MovingPlayer.Id.ToString() == playerId)
        {
            player = state.MovingPlayer;
            enemy = state.WaitingPlayer;
        }
        else
        {
            player = state.WaitingPlayer;
            enemy = state.MovingPlayer;
        }
        player.Money.onAddCoin  += CmdonAddCoin;
        enemy.Money.onAddCoin += CmdonAddEnemyCoin;
    }
    [Command]
    void CmdSubscribeToActiveCard(int cardId)
    {
        GameState state= GetGame().GetGameState();
        Card card = FindCard(state.MovingPlayer, cardId);
        card = card ?? FindCard(state.WaitingPlayer, cardId);
        card.onChangeHealth += CmdonChangeHealth;
        card.onChangeClosed += CmdonChangeClosed;
    }
    Card FindCard(Player player, int cardId)
        => (player.Hero.Id== cardId)? player.Hero: player.CardsInGame.FirstOrDefault(x => x.Id == cardId);
    void onSelectCard(int cardId)
    {
        if (!isLocalPlayer) return;        
        CmdHire(_player.Id.ToString(),cardId);
    }
    [Command]
    void CmdHire(string playerId,int cardId)
    {
        if (!isServer) return;
        Player movingPlayer = GetGame().GetGameState().MovingPlayer;
        if (playerId != movingPlayer.Id.ToString())
            return;
        bool result = movingPlayer.Hire(cardId);//когда придет событие с сервера, что карта нанята с таким то айдишником
        if (!result) return;
        Card card = movingPlayer.CardsInGame.FirstOrDefault(x => x.Id == cardId);
        RpcCreateActiveCard(new CardInfo() { _id = card.Id, _power = card.Power, _health = card.Health }, playerId);
        RpcCloseCoins(card.Cost,playerId);
        RpcDestroyCardInHand(cardId);
    }
    [ClientRpc]
    void RpcCreateActiveCard(CardInfo cardInfo, string playerId)
    {
        //Debug.Log(_player);
        GameObject.FindGameObjectsWithTag("Enemy").Select(x => x.GetComponent<Enemy>())
          .FirstOrDefault(x => x.localPlayerAuthority)?.CreateActiveCard(cardInfo,playerId);
        GameObject.FindGameObjectsWithTag("Gamer").Select(x => x.GetComponent<Gamer>())
          .FirstOrDefault(x => x.isLocalPlayer)?.CreateActiveCard(cardInfo, playerId);
        /*if (!isLocalPlayer) return;
        
        if (_player.Id.ToString() == playerId)
            _player.CreateActiveCard(cardInfo);
        else
            enemy.CreateActiveCard(cardInfo);*/
        
    }
    [ClientRpc]
    void RpcCloseCoins(int count, string playerID)
    {
        //if (!isLocalPlayer) return;
        GetCards("Coin").Select(x=>x.GetComponent<CoinUnity>()).Where(x=>!x.IsClosed() && x.PlayerId == playerID)
            .Take(count).ToList().ForEach(x=> x.Close());
    }
    [ClientRpc]
    void RpcDestroyCardInHand(int cardId)
    {
        if (!isLocalPlayer) return;
        GetCards("CardInHand").Select(x => x.GetComponent<CardInHand>())
            .FirstOrDefault(x => x.CardId == cardId).DestroyCard();
    }
    bool onSelectActiveCard(int cardId)
    {
        if (!isLocalPlayer) return false;
        CmdSaveActiveCards(_player.Id.ToString(),cardId);
        return true;
    }
    [Command]
    void CmdSaveActiveCards(string playerId,int cardId)
    {
        GameScript game = GetGame();
        Player movingPlayer = game.GetGameState().MovingPlayer;
        if (playerId != movingPlayer.Id.ToString()) return;

        if (game.ActionCardId == null)
            game.ActionCardId = cardId;
        else if (game.ActionCardId != cardId)
        {
            game.TargetCardId = cardId;
            RpcActiveAttackWayPanel();
        }
        else
            game.SetToZeroCards();
    }
    [ClientRpc]
    void RpcActiveAttackWayPanel()
    {
        if (!isLocalPlayer) return;

        AttackWayPanel.SetActive(true);
    }
    [Command]
    void CmdonChangeHealth(int cardId, int health) => RpconChangeHealth(cardId, health);
    [ClientRpc]
    void RpconChangeHealth(int cardId, int health)
    {
        if (!isLocalPlayer) return;
        if(!SetHealth(GetCards("Card"), cardId, health))
            SetHealth(GetCards("Hero"), cardId, health);
    }
    [Command]
    void CmdonChangeClosed(int cardId, bool closed) => RpconChangeClosed(cardId, closed);
    [ClientRpc]
    void RpconChangeClosed(int cardId, bool closed)
    {
        if (!isLocalPlayer) return;
        if (!SetClosed(GetCards("Card"), cardId, closed))
            SetClosed(GetCards("Hero"), cardId, closed);
    }
    bool SetHealth(List<GameObject> cards,int cardId, int value)
    {
        IActiveCard script = cards.Select(x => x.GetComponent<IActiveCard>()).FirstOrDefault(x => x.CardId == cardId);
        script?.ChangeHealth(value);
        return (script == null) ? false : true;
    }
    bool SetClosed(List<GameObject> cards, int cardId, bool value)
    {
        IActiveCard script= cards.Select(x => x.GetComponent<IActiveCard>()).FirstOrDefault(x => x.CardId == cardId);
        script?.SetClose(value);
        return (script == null) ? false : true;
    }
    public void SetAttackWay(CardActionEnum attackWay)
    {
        AttackWayPanel.SetActive(false);
        CmdSetAttackWay(attackWay);
    }
    [Command]
    void CmdSetAttackWay(CardActionEnum attackWay) => GetGame().AttackWay = attackWay;
    public void CompleteStep() => CmdCompleteStep(_player.Id.ToString());
    void OpenAll(string playerId)
    {
        GetCards("Card").Select(x=>x.GetComponent<CardUnity>()).Where(x => x.PlayerId == playerId)
            .ToList().ForEach(x=>x.SetClose(false));
        GetCards("Hero").Select(x => x.GetComponent<Hero>()).Where(x => x.PlayerId == playerId)
            .ToList().ForEach(x => x.SetClose(false));
        GetCards("Coin").Select(x => x.GetComponent<CoinUnity>()).Where(x=>x.PlayerId==playerId)
            .ToList().ForEach(x => x.Open());
    }
    [Command]
    void CmdCompleteStep(string playerId) => GetGame().CompleteStep(playerId);
    GameScript GetGame()=> GameObject.FindWithTag("Scripts").GetComponent<GameScript>();
    List<GameObject> GetCards(string tag) => GameObject.FindGameObjectsWithTag(tag).ToList();
}

