using Domain.Cards;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class Client : NetworkBehaviour
{
    public GameObject AttackWayPanel;
    public GameObject EnemyPrefab;
    private Enemy _enemy;
    private Gamer _gamer;
    public override void OnStartLocalPlayer()
    {
        if (!isLocalPlayer) return;
        CreateEnemy();
        _gamer = GetComponent<Gamer>();
        CmdConnectPlayers(netId);
    }
    private void CreateEnemy()
    {
        GameObject enemy = Instantiate(EnemyPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        _enemy = enemy.GetComponent<Enemy>();
    }
    [Command]
    void CmdConnectPlayers(NetworkInstanceId networkId) =>GetServer().CmdConnectPlayers(networkId);
    public void Settings(string playerId)
    {
        SubscribeToSrartStep();
        //SubscribeToAddCoins(playerId);               
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
        _gamer.OpenAll(playerId);
        _enemy.OpenAll(playerId);
    }
    #endregion

    #region event AddCoins
    public void SubscribeToAddCoins(string playerId) => CmdSubscribeToAddCoins(playerId);
    [Command]
    void CmdSubscribeToAddCoins(string playerId) => GetServer().CmdSubscribeToAddCoin(playerId);
    public void CmdonAddCoin() => RpconAddCoin();
    public void CmdonAddEnemyCoin() => RpconAddEnemyCoin();
    [ClientRpc]
    void RpconAddCoin()
    {
        if (!isLocalPlayer) return;
        _gamer.onAddCoin();
    }
    [ClientRpc]
    void RpconAddEnemyCoin()
    {
        if (!isLocalPlayer) return;
        _enemy.onAddCoin();
    }
    #endregion

    #region ConnectPlayers
    public void CmdEnemyInitialization(string playerId, CardInfo heroInfo, int countCards, int countCoin)
       => RpcEnemyInitialization(playerId, heroInfo, countCards, countCoin);
    public void CmdPlayerInitialization(string playerId, CardInfo heroInfo, CardInfo[] cardsInfo, int countCoin)
       => RpcPlayerInitialization(playerId, heroInfo, cardsInfo, countCoin);
    [ClientRpc]
    void RpcEnemyInitialization(string playerId, CardInfo heroInfo, int countCards, int countCoin)
    {
        if (!isLocalPlayer) return;

        
        _enemy?.OnStartPlayer(playerId, heroInfo, countCards, countCoin);
    }
    [ClientRpc]
    void RpcPlayerInitialization(string playerId, CardInfo heroInfo, CardInfo[] cardsInfo, int countCoin)
    {
        if (!isLocalPlayer) return;

        _gamer.Initialization(playerId,heroInfo,cardsInfo,countCoin);
    }
    #endregion

    #region event SelectCardInHand
    public void SubscribeToSelectCardInHand(GameObject sprite)
       => sprite.GetComponent<CardInHand>().onSelectCard += onSelectCard;
    void onSelectCard(string instId)
    {
        if (!isLocalPlayer) return;
        CmdHire(_gamer.GetId(), instId);
    }
    [Command]
    void CmdHire(string playerId, string instId) => GetServer().CmdHire(playerId, instId);
    public void CmdCreateActiveCard(CardInfo cardInfo, string playerId) => RpcCreateActiveCard(cardInfo, playerId);
    public void CmdCloseCoins(int count, string playerId) => RpcCloseCoins(count, playerId);
    public void CmdDestroyCardInHand(string instId, string playerId) => RpcDestroyCardInHand(instId, playerId);
    [ClientRpc]
    void RpcCreateActiveCard(CardInfo cardInfo, string playerId)
    {
        if (!isLocalPlayer) return;
        _gamer.CreateActiveCard(cardInfo, playerId);
        _enemy.CreateActiveCard(cardInfo, playerId);
    }
    [ClientRpc]
    void RpcCloseCoins(int count, string playerId)
    {
        if (!isLocalPlayer) return;
        _gamer.CloseCoins(count, playerId);
        _enemy.CloseCoins(count, playerId);
    }
    [ClientRpc]
    void RpcDestroyCardInHand(string instId, string playerId)
    {
        if (!isLocalPlayer) return;
        _gamer.DestroyCardInHand(instId, playerId);
        _enemy.DestroyCardInHand(instId, playerId);
    }
    #endregion

    #region event ChangeHealth,ChangeClosed and Select ActiveCard
    public void SubscribeToEventsHero(GameObject sprite)
    {
        Hero hero = sprite.GetComponent<Hero>();
        hero.onSelectCard += onSelectActiveCard;
        CmdSubscribeToActiveCard(hero.InstId.ToString());
    }
    public void SubscribeToEventsActiveCard(GameObject sprite) 
    {
        CardUnity card = sprite.GetComponent<CardUnity>();
        card.onSelectCard += onSelectActiveCard;
        CmdSubscribeToActiveCard(card.InstId);
    }
    [Command]
    void CmdSubscribeToActiveCard(string instId) => GetServer().CmdSubscribeToActiveCard(instId);
    bool onSelectActiveCard(string instId)
    {
        if (!isLocalPlayer) return false;
        CmdSaveActiveCards(_gamer.GetId(), instId,netId);
        return true;
    }
    [Command]
    void CmdSaveActiveCards(string playerId, string instId, NetworkInstanceId networkId)
        => GetServer().CmdSaveActiveCards(playerId, instId, networkId);
    public void CmdonChangeHealth(string instId, int health) => RpconChangeHealth(instId, health);
    public void CmdonChangeClosed(string instId, bool closed) => RpconChangeClosed(instId, closed);
    [ClientRpc]
    void RpconChangeHealth(string instId, int health)
    {
        if (!isLocalPlayer) return;
        _gamer.OnChangeHealth(instId, health);
        _enemy.OnChangeHealth(instId, health);
    }
    [ClientRpc]
    void RpconChangeClosed(string instId, bool closed)
    {
        if (!isLocalPlayer) return;
        _gamer.OnChangeClosed(instId, closed);
        _enemy.OnChangeClosed(instId, closed);
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
    public void CompleteStep() => CmdCompleteStep(_gamer.GetId());
    [Command]
    void CmdCompleteStep(string playerId) => GetServer().CmdCompleteStep(playerId);
    public void CmdUpdateCountCoins(string[] playersId, int[] countCoinsPlayers) 
        => RpcUpdateCountCoins(playersId, countCoinsPlayers);
    [ClientRpc]
    void RpcUpdateCountCoins(string[] playersId, int[] countCoinsPlayers)
    {
        if (!isLocalPlayer) return;
        _gamer.UpdateCountCoins(playersId, countCoinsPlayers);
        _enemy.UpdateCountCoins(playersId, countCoinsPlayers);
    }
    #endregion

    private ServerGame GetServer() => GameObject.FindWithTag("Scripts").GetComponent<ServerGame>();
}

