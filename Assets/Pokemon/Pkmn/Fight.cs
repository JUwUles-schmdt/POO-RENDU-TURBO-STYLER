
using System;

namespace Pkmn
{
    public class Fight
    {
        public Fight(Character character1, Character character2)
        {
            Character1 = character1;
            Character2 = character2;
            IsFightFinished = false;

            if (character1 == null || character2 == null) throw new ArgumentNullException();
        }

        public Character Character1 { get; }
        public Character Character2 { get; }
        /// <summary>
        /// Est-ce la condition de victoire/défaite a été rencontré ?
        /// </summary>
        public bool IsFightFinished;

        /// <summary>
        /// Jouer l'enchainement des attaques. Attention à bien gérer l'ordre des attaques par apport à la speed des personnages
        /// </summary>
        /// <param name="skillFromCharacter1">L'attaque selectionné par le joueur 1</param>
        /// <param name="skillFromCharacter2">L'attaque selectionné par le joueur 2</param>
        /// <exception cref="ArgumentNullException">si une des deux attaques est null</exception>
        public void ExecuteTurn(Skill skillFromCharacter1, Skill skillFromCharacter2)
        {
            if (Character1.Speed >= Character2.Speed && Character1.CurrentStatus.CanAttack)
            {
                Character2.ReceiveAttack(skillFromCharacter1);
                Character1.takeStatusDmg((int)(skillFromCharacter1.Power * Character1.CurrentStatus?.DamageOnAttack??0));
                if (Character2.IsAlive)
                {
                    Character1.ReceiveAttack(skillFromCharacter2);
                    Character2.takeStatusDmg((int)(skillFromCharacter2.Power * Character2.CurrentStatus?.DamageOnAttack ?? 0));
                }
            }

            else if (Character2.CurrentStatus.CanAttack)
            {
                Character1.ReceiveAttack(skillFromCharacter2);
                Character2.takeStatusDmg((int)(skillFromCharacter2.Power * Character2.CurrentStatus?.DamageOnAttack ?? 0));
                if (Character1.IsAlive)
                {
                    Character2.ReceiveAttack(skillFromCharacter1);
                    Character1.takeStatusDmg((int)(skillFromCharacter1.Power * Character1.CurrentStatus?.DamageOnAttack??0));
                }

            }

            else if (Character1.CurrentStatus.CanAttack)
            {
                Character2.ReceiveAttack(skillFromCharacter1);
                Character1.takeStatusDmg((int)(skillFromCharacter1.Power * Character1.CurrentStatus?.DamageOnAttack ?? 0));
            }

            Character1.takeStatusDmg(Character1.CurrentStatus?.DamageEachTurn ?? 0);
            if (Character1.CurrentStatus.RemainingTurn == 0) Character1.setStatus(StatusPotential.NONE); else Character1.CurrentStatus.EndTurn();
            Character2.takeStatusDmg(Character2.CurrentStatus?.DamageEachTurn ?? 0);
            if (Character2.CurrentStatus.RemainingTurn == 0) Character2.setStatus(StatusPotential.NONE); else Character2.CurrentStatus.EndTurn();









            if (!Character1.IsAlive || !Character2.IsAlive){IsFightFinished = true;}
        }

    }
}
