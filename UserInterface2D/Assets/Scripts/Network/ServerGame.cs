using Domain.Cards;
using Domain.Process;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;


public class ServerGame : NetworkBehaviour
{
    GameScript game;
    void Start()
    {
        game = GetComponent<GameScript>();
    }  

    #region event SrartStep
    public void CmdSubscribeToSrartStep()
    {
        if (!isServer) return;
        game.OnStartStep += CmdonStartStep;
    }
    void CmdonStartStep(string playerId) => GetClients().ForEach(x=>x.CmdonStartStep(playerId));
    #endregion

    #region event AddCoin
    public void CmdSubscribeToAddCoin(string playerId)
    {
        if (!isServer) return;
        GameState state = game.GetGameState();
        if (state.MovingPlayer.Id.ToString() == playerId)
            SubscribePlayersToAddCoin(state.MovingPlayer, state.WaitingPlayer);
        else
            SubscribePlayersToAddCoin(state.WaitingPlayer, state.MovingPlayer);
    }
    private void SubscribePlayersToAddCoin(Player playerGamer, Player playerEnemy)
    {
        SubscribePlayerToAddCoin(playerGamer, CmdonAddCoin);
        SubscribePlayerToAddCoin(playerEnemy, CmdonAddEnemyCoin);
    }
    private void SubscribePlayerToAddCoin(Player player, Money.OnAddCoinHandler method)
        => player.Money.onAddCoin += method;
    void CmdonAddCoin() => GetClients().ForEach(x => x.CmdonAddCoin());
    void CmdonAddEnemyCoin() => GetClients().ForEach(x => x.CmdonAddEnemyCoin());
    #endregion

    #region SetEnemy
    public void CmdSetEnemy(string playerId)
    {
        if (!isServer) return;
        GameState state = game.GetGameState();
        Player player = (state.MovingPlayer.Id.ToString() == playerId) ? state.WaitingPlayer : state.MovingPlayer;
        CardInfo heroInfo = new CardInfo() { _id = player.Hero.Id, _health = player.Hero.Health };
        GetClients().ForEach(x=>
        x.CmdEnemyInitialization(player.Id.ToString(), heroInfo, player.ActiveDeck.Count(), player.Money.Count));
    } 
    #endregion

    #region event SelectCardInHand
    public void CmdHire(string playerId, int cardId)
    {
        if (!isServer) return;
        Player movingPlayer = game.GetGameState().MovingPlayer;
        if (playerId != movingPlayer.Id.ToString())
            return;
        bool result = movingPlayer.Hire(cardId);
        if (!result) return;
        Card card = movingPlayer.CardsInGame.FirstOrDefault(x => x.Id == cardId);
        GetClients().ForEach(x=> {
            x.CmdCreateActiveCard(new CardInfo() { _id = card.Id, _power = card.Power, _health = card.Health }, playerId);
            x.CmdCloseCoins(card.Cost, playerId);
            x.CmdDestroyCardInHand(cardId);
        });
    }  
    #endregion

    #region event ChangeHealth,ChangeClosed and Select ActiveCard
    public void CmdSubscribeToActiveCard(int cardId)
    {
        if (!isServer) return;
        GameState state = game.GetGameState();
        Card card = FindCard(state.MovingPlayer, cardId);
        card = card ?? FindCard(state.WaitingPlayer, cardId);
        card.onChangeHealth += CmdonChangeHealth;
        card.onChangeClosed += CmdonChangeClosed;
    }
    Card FindCard(Player player, int cardId)
        => (player.Hero.Id == cardId) ? player.Hero : player.CardsInGame.FirstOrDefault(x => x.Id == cardId);
    void CmdonChangeHealth(int cardId, int health) => GetClients().ForEach(x=>x.CmdonChangeHealth(cardId, health));
    void CmdonChangeClosed(int cardId, bool closed) => GetClients().ForEach(x => x.CmdonChangeClosed(cardId, closed));   
    public void CmdSaveActiveCards(string playerId, int cardId)
    {
        if (!isServer) return;
        Player movingPlayer = game.GetGameState().MovingPlayer;
        if (playerId != movingPlayer.Id.ToString()) return;

        if (game.ActionCardId == null)
            game.ActionCardId = cardId;
        else if (game.ActionCardId != cardId)
        {
            game.TargetCardId = cardId;
            GetClients().ForEach(x => x.CmdActiveAttackWayPanel());
        }
        else
            game.SetToZeroCards();
    }
    #endregion

    #region SetAttackWay
    public void CmdSetAttackWay(CardActionEnum attackWay) => game.AttackWay = attackWay;
    #endregion

    #region CompleteStep
    public void CmdCompleteStep(string playerId) => game.CompleteStep(playerId);
    #endregion

    private Client GetClient() => GameObject.FindGameObjectsWithTag("Gamer")
        .Select(x=>x.GetComponent<Client>()).FirstOrDefault(x=>x.isLocalPlayer);
    private List<Client> GetClients() => GameObject.FindGameObjectsWithTag("Gamer")
        .Select(x => x.GetComponent<Client>()).ToList();
}

