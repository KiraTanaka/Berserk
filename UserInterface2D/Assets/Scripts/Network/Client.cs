using Domain.Cards;
using System.Collections.Generic;
using System.Linq;
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
        SubscribeToSrartStep();
        SubscribeToAddCoins();        
        SetEnemy();
    }
    #region event SrartStep
    void SubscribeToSrartStep() => CmdSubscribeToSrartStep();
    [Command]
    void CmdSubscribeToSrartStep() => GetServer().CmdSubscribeToSrartStep();
    public void CmdonStartStep(string playerId) => RpconStartStep(playerId);
    [ClientRpc]
    void RpconStartStep(string playerId) => OpenAll(playerId);
    private void OpenAll(string playerId)
    {
        if (!isLocalPlayer) return;
        GetCards("Card").Select(x => x.GetComponent<CardUnity>()).Where(x => x.PlayerId == playerId)
            .ToList().ForEach(x => x.SetClose(false));
        GetCards("Hero").Select(x => x.GetComponent<Hero>()).Where(x => x.PlayerId == playerId)
            .ToList().ForEach(x => x.SetClose(false));
        GetCards("Coin").Select(x => x.GetComponent<CoinUnity>()).Where(x => x.PlayerId == playerId)
            .ToList().ForEach(x => x.Open());
    }
    #endregion

    #region event AddCoins
    public void SubscribeToAddCoins() => CmdSubscribeToAddCoins(_player.Id.ToString());
    [Command]
    void CmdSubscribeToAddCoins(string playerId) => GetServer().CmdSubscribeToAddCoin(playerId);
    public void CmdonAddCoin() => RpconAddCoin();
    public void CmdonAddEnemyCoin() => RpconAddEnemyCoin();
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
    #endregion

    #region SetEnemy
    private void SetEnemy() => CmdSetEnemy(_player.Id.ToString());
    [Command]
    private void CmdSetEnemy(string playerId) => GetServer().CmdSetEnemy(playerId);
    public void CmdEnemyInitialization(string playerId, CardInfo heroInfo, int countCards, int countCoin)
       => RpcEnemyInitialization(playerId, heroInfo, countCards, countCoin);
    [ClientRpc]
    void RpcEnemyInitialization(string playerId, CardInfo heroInfo, int countCards, int countCoin)
    {
        if (!isLocalPlayer) return;

        enemy = GameObject.FindGameObjectsWithTag("Enemy").Select(x => x.GetComponent<Enemy>())
          .FirstOrDefault(x => x.localPlayerAuthority);
        enemy?.OnStartPlayer(playerId, heroInfo, countCards, countCoin);
    }
    #endregion

    #region event SelectCardInHand
    public void SubscribeToSelectCardInHand(GameObject sprite)
       => sprite.GetComponent<CardInHand>().onSelectCard += onSelectCard;
    void onSelectCard(int cardId)
    {
        if (!isLocalPlayer) return;
        CmdHire(_player.Id.ToString(), cardId);
    }
    [Command]
    void CmdHire(string playerId, int cardId) => GetServer().CmdHire(playerId, cardId);
    public void CmdCreateActiveCard(CardInfo cardInfo, string playerId) => RpcCreateActiveCard(cardInfo, playerId);
    public void CmdCloseCoins(int count, string playerID) => RpcCloseCoins(count, playerID);
    public void CmdDestroyCardInHand(int cardId) => RpcDestroyCardInHand(cardId);
    [ClientRpc]
    void RpcCreateActiveCard(CardInfo cardInfo, string playerId)
    {
        if (!isLocalPlayer) return;
        GameObject.FindGameObjectsWithTag("Enemy").Select(x => x.GetComponent<Enemy>())
          .FirstOrDefault(x => x.localPlayerAuthority)?.CreateActiveCard(cardInfo, playerId);
        GameObject.FindGameObjectsWithTag("Gamer").Select(x => x.GetComponent<Gamer>())
          .FirstOrDefault(x => x.isLocalPlayer)?.CreateActiveCard(cardInfo, playerId);
    }
    [ClientRpc]
    void RpcCloseCoins(int count, string playerID)
    {
        if (!isLocalPlayer) return;
        GetCards("Coin").Select(x => x.GetComponent<CoinUnity>()).Where(x => !x.IsClosed() && x.PlayerId == playerID)
            .Take(count).ToList().ForEach(x => x.Close());
    }
    [ClientRpc]
    void RpcDestroyCardInHand(int cardId)
    {
        if (!isLocalPlayer) return;
        GetCards("CardInHand").Select(x => x.GetComponent<CardInHand>())
            .FirstOrDefault(x => x.CardId == cardId).DestroyCard();
    }
    #endregion

    #region event ChangeHealth,ChangeClosed and Select ActiveCard
    public void SubscribeToSelectHero(GameObject sprite)
    {
        Hero hero = sprite.GetComponent<Hero>();
        hero.onSelectCard += onSelectActiveCard;
        CmdSubscribeToActiveCard(hero.CardId);
    }
    public void SubscribeToSelectActiveCard(GameObject sprite) 
    {
        CardUnity card = sprite.GetComponent<CardUnity>();
        card.onSelectCard += onSelectActiveCard;
        CmdSubscribeToActiveCard(card.CardId);
    }
    [Command]
    void CmdSubscribeToActiveCard(int cardId) => GetServer().CmdSubscribeToActiveCard(cardId);
    bool onSelectActiveCard(int cardId)
    {
        if (!isLocalPlayer) return false;
        CmdSaveActiveCards(_player.Id.ToString(), cardId);
        return true;
    }
    [Command]
    void CmdSaveActiveCards(string playerId, int cardId) => GetServer().CmdSaveActiveCards(playerId, cardId);
    public void CmdonChangeHealth(int cardId, int health) => RpconChangeHealth(cardId, health);
    public void CmdonChangeClosed(int cardId, bool closed) => RpconChangeClosed(cardId, closed);
    [ClientRpc]
    void RpconChangeHealth(int cardId, int health)
    {
        if (!isLocalPlayer) return;
        if (!SetHealth(GetCards("Card"), cardId, health))
            SetHealth(GetCards("Hero"), cardId, health);
    }
    [ClientRpc]
    void RpconChangeClosed(int cardId, bool closed)
    {
        if (!isLocalPlayer) return;
        if (!SetClosed(GetCards("Card"), cardId, closed))
            SetClosed(GetCards("Hero"), cardId, closed);
    }
    bool SetHealth(List<GameObject> cards, int cardId, int value)
    {
        IActiveCard script = cards.Select(x => x.GetComponent<IActiveCard>()).FirstOrDefault(x => x.CardId == cardId);
        script?.ChangeHealth(value);
        return (script == null) ? false : true;
    }
    bool SetClosed(List<GameObject> cards, int cardId, bool value)
    {
        IActiveCard script = cards.Select(x => x.GetComponent<IActiveCard>()).FirstOrDefault(x => x.CardId == cardId);
        script?.SetClose(value);
        return (script == null) ? false : true;
    }    
    public void CmdActiveAttackWayPanel() => RpcActiveAttackWayPanel();
    [ClientRpc]
    void RpcActiveAttackWayPanel()
    {
        if (!isLocalPlayer) return;

        AttackWayPanel.SetActive(true);
    }
    #endregion

    #region SetAttackWay
    public void SetAttackWay(CardActionEnum attackWay)
    {
        AttackWayPanel.SetActive(false);
        CmdSetAttackWay(attackWay);
    }
    [Command]
    void CmdSetAttackWay(CardActionEnum attackWay) => GetServer().CmdSetAttackWay(attackWay);
    #endregion

    #region CompleteStep
    public void CompleteStep() => CmdCompleteStep(_player.Id.ToString());
    [Command]
    void CmdCompleteStep(string playerId) => GetServer().CmdCompleteStep(playerId);
    #endregion

    private ServerGame GetServer() => GameObject.FindWithTag("Scripts").GetComponent<ServerGame>();
    private List<GameObject> GetCards(string tag) => GameObject.FindGameObjectsWithTag(tag).ToList();
}

