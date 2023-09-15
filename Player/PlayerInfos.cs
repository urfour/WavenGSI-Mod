using Ankama.Cube.Data;
using Ankama.Cube.Fight;
using Ankama.Cube.Fight.Entities;
using Ankama.Cube.Player;
using Ankama.Cube.UI.Fight;
using Channel;
using System.Collections.Generic;
using System.Reflection;
using WavenGSI.Player;

namespace WavenGSI
{
    public class PlayerInfos
    {
        public string AccountName { get; set; }
        public string HeroName { get; set; }
        public string Class { get; set; }
        public string Weapon { get; set; }
        public int Level { get; set; }
        public bool IsInFight { get; set; }
        public bool IsPlayerTurn { get; set; }
        public int BaseHealthPoints { get; set; }
        public int HealthPoints { get; set; }
        public int ActionPoints { get; set; }
        public int ReservePoints { get; set; }
        public int BaseMovementPoints { get; set; }
        public int MovementPoints { get; set; }
        public int SpellsCount { get; set; }
        public SpellSlots Spells { get; set; }
        public CompanionSlots Companions { get; set; }
        public int CompanionsCount { get; set; }

        public PlayerInfos() {}

        public void Update()
        {
            if (PlayerData.initialized)
            {
                AccountName = PlayerData.instance.nickName.Nickname;
                var currentHero = PlayerData.instance.heroCollectionData.currentPlayerHero;
                HeroName = currentHero.name;
                Class = currentHero.god.ToString();
                Weapon = currentHero.weaponDefinition.displayName.Split(Class.ToUpper() + " - ")[1];
                Level = currentHero.level;
                if (GameStatus.isInFight)
                {
                    PlayerStatus player = FightStatus.local.GetLocalPlayer();
                    LocalPlayerHUD playerHUD = FightUI.instance.GetLocalPlayerHUD();
                    IsInFight = true;
                    IsPlayerTurn = FightStatus.local.currentTurnPlayerId == player.id;
                    BaseHealthPoints = player.heroStatus.baseLife;
                    HealthPoints = player.heroStatus.life;
                    ActionPoints = player.actionPoints;
                    ReservePoints = player.reservePoints;
                    BaseMovementPoints = player.heroStatus.baseMovementPoints;
                    MovementPoints = player.heroStatus.movementPoints;
                    SpellsCount = player.m_spells.Count;
                    Spells = new SpellSlots();
                    var spells = new List<Spell>();
                    for (int i = 0; i < player.m_spells.Count; i++)
                    {
                        Spell newSpell = new Spell();
                        newSpell.Name = player.m_spells[i].spellDefinition.displayName;
                        newSpell.Element = player.m_spells[i].spellDefinition.details.element.ToString();
                        //if (playerHUD.m_spellBar.isActiveAndEnabled && playerHUD.GetSpellAtIndex(i) != null)
                        //{
                        //    newSpell.IsAvailable = playerHUD.GetSpellAtIndex(i).Value.canBeCast;
                        //}
                        spells.Add(newSpell);
                    }
                    if (SpellsCount >= 1)
                        Spells.Slot1 = spells[0];
                    if (SpellsCount >= 2)
                        Spells.Slot2 = spells[1];
                    if (SpellsCount >= 3)
                        Spells.Slot3 = spells[2];
                    if (SpellsCount >= 4)
                        Spells.Slot4 = spells[3];
                    if (SpellsCount >= 5)
                        Spells.Slot5 = spells[4];
                    if (SpellsCount >= 6)
                        Spells.Slot6 = spells[5];
                    if (SpellsCount >= 7)
                        Spells.Slot7 = spells[6];
                    Companions = new CompanionSlots();
                    CompanionsCount = player.m_availableCompanions.Count;
                    PropertyInfo[] rootProperties = typeof(CompanionSlots).GetProperties();
                    for (var i = 0; i < CompanionsCount; i++)
                    {
                        Companion newCompanion = new Companion();
                        newCompanion.Name = player.m_availableCompanions[i].definition.displayName;
                        newCompanion.Element = player.m_availableCompanions[i].definition.GetElement().Value.ToString();
                        newCompanion.State = player.m_availableCompanions[i].state.ToString();
                        if (playerHUD.GetCompanionAtIndex(i) != null)
                            newCompanion.IsAvailable = playerHUD.GetCompanionAtIndex(i).Value.canBeCast;
                        rootProperties[i].SetValue(Companions, newCompanion);
                    }
                }
                else
                {
                    IsInFight = false;
                }
            }
        }
    }
}