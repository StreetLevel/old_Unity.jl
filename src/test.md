# Funktionale Spezifikation

###

```graphviz
digraph hierarchy {
    compound=true;
    nodesep=.75
    ranksep=.5
    
    node [color=dimgray,fontname=Courier,shape=box] 
    edge [color=dodgerblue3, style=dashed] 
    1.->2.->3.
    ETH->wallet1  [dir="both"]
    ICO->wallet1
            
    wallet1->hot_wallets
    wallet1->cold_wallets [dir="both"]
        
    hot_wallets:w20->cold_wallets:w1
    cold_wallets:w1->hot_wallets:w20
        
    Trading_Backend->hot_wallets
    hot_wallets->Trading_Backend
           
    node[shape=record];
    "TPs" [label="{Trading Platforms | {P1 | ... | Pn } }"]
      
    Trading_Backend->TPs
    TPs->Trading_Backend
    Reb_Cap_Algorithm->Trading_Backend [dir="both"]
    Admin->Trading_Backend [label="trading freigabe"]
    
    {rank=same;1.;ETH;ICO}        
    {rank=same;2.;hot_wallets,cold_wallets}        
    {rank=same;3.;Trading_Backend}        
    {rank=same;TPs;Reb_Cap_Algorithm;Admin}
        
       
}
```
1. User Schnittstelle (Frontend)
    * Bargeldeingang durch Shareholder in Form von ICO's, Cryptowährungen oder Bargeld
    * Bargeldeingang auf zentrales Wallet, danach weiterleitung zu "Hot-" oder "Cold-Wallets"

--> Erstellung ROIC Coins innerhalb ETH ?!

2. Hot- und Cold-Wallets
    * Zur Absicherung gegen Cyberattacken o.ä. werden die Coins dezentral in Verschiedenen Wallets verwahrt.
        * Sicherheitskonzepte
    * Cold Wallets:
        *  genügen höheren Sicherheitsstandarts (evtl offline?), sind aber nicht fürs schnelle traden geeignet,
        *  Hier liegt das meiste Kapital
    * Hot Wallets:
        * Hier liegt nur der Teil des Kapitals, was für das Rebalancing des Portfolios gebraucht wird.
    * Cryptocoin Trading
        * Coins müssen an verschiedenen Trading Plattformen gekauft und verkauft werden, dafür müssen verschiedene Cryptowährungen getauscht werden. Hierbei scheint es bei verschiedenen Plattformen unterschiedliche Wechselkurse zu geben ?!
3. Trading Backend
    * Schnittstelle zu Trading Plattformen
        * [cctx](https://github.com/ccxt/ccxt): 
            * A JavaScript / Python / PHP cryptocurrency trading library with support for more than 100 bitcoin/altcoin exchanges
            * Auf Funktionalität zu prüfen
    * Rebalancing/Capping-Algorithmus gibt die zu tradenden Coins vor
    * Admin erteilt Freigabe fürs Rebalancing, wenn nötig (Regeln sind zu definieren)

### Stichworte
* Kubernetes

## Entwicklung Frontend (Kunde<->ROIC)

## Entwicklung eines Trading-Backends (ROIC<->Cryptomarkt) 

## Sicherheit

## Algorithmische Umsetzung der Trading-Strategie

### Cryptocurrency Index Fond

Bisher implementiert (siehe CIF200: a cryprocurrency index fund):

* passiver CIF mit Rebalancing/Capping-Strategie:
![](https://i.imgur.com/fLh400X.jpg)
    * statische Anzahl der Coins (200)
    * statisches Rebalancing (14 tägig) 
    * statisches Capping (2%)
    
Weiterer Entwicklungsbedarf:
* dynamische Parameter einführen:
* größere Parameterstudie (adaptive Gridmethoden)

### Gant Chart

```mermaid
gantt
        dateFormat  YYYY-MM-DD
        title Adding GANTT diagram functionality to mermaid
        section A section
        Completed task            :done,    des1, 2014-01-06,2014-01-08
        Active task               :active,  des2, 2014-01-09, 3d
        Future task               :         des3, after des2, 5d
        Future task2               :         des4, after des3, 5d
        section Critical tasks
        Completed task in the critical line :crit, done, 2014-01-06,24h
        Implement parser and jison          :crit, done, after des1, 2d
        Create tests for parser             :crit, active, 3d
        Future task in critical line        :crit, 5d
        Create tests for renderer           :2d
        Add to mermaid                      :1d
```

```mermaid
        gantt
        dateFormat  YYYY-MM-DD
        title Adding GANTT diagram functionality to mermaid

        section A section
        Completed task            :done,    des1, 2014-01-06,2014-01-08
        Active task               :active,  des2, 2014-01-09, 3d
        Future task               :         des3, after des2, 5d
        Future task2               :         des4, after des3, 5d

        section Critical tasks
        Completed task in the critical line :crit, done, 2014-01-06,24h
        Implement parser and jison          :crit, done, after des1, 2d
        Create tests for parser             :crit, active, 3d
        Future task in critical line        :crit, 5d
        Create tests for renderer           :2d
        Add to mermaid                      :1d

        section Documentation
        Describe gantt syntax               :active, a1, after des1, 3d
        Add gantt diagram to demo page      :after a1  , 20h
        Add another diagram to demo page    :doc1, after a1  , 48h

        section Last section
        Describe gantt syntax               :after doc1, 3d
        Add gantt diagram to demo page      : 20h
        Add another diagram to demo page    : 48h
```
    







