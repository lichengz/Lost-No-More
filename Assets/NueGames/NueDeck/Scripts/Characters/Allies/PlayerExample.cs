using NueGames.NueDeck.Scripts.Managers;

namespace NueGames.NueDeck.Scripts.Characters.Allies
{
    public class PlayerExample : AllyBase
    {
        public override void BuildCharacter()
        {
            base.BuildCharacter();
            if (NueUIManager != null)
                CharacterStats.OnHealthChanged += NueUIManager.InformationCanvas.SetHealthText;
            CharacterStats.SetCurrentHealth(CharacterStats.CurrentHealth);
        }
    }
}