using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;

namespace Pkmn
{
    /// <summary>
    /// Définition d'un personnage
    /// </summary>
    public class Character
    {
        /// <summary>
        /// Stat de base, HP
        /// </summary>
        int _baseHealth;
        /// <summary>
        /// Stat de base, ATK
        /// </summary>
        int _baseAttack;
        /// <summary>
        /// Stat de base, DEF
        /// </summary>
        int _baseDefense;
        /// <summary>
        /// Stat de base, SPE
        /// </summary>
        int _baseSpeed;
        /// <summary>
        /// Type de base
        /// </summary>
        TYPE _baseType;

        public Character(int baseHealth, int baseAttack, int baseDefense, int baseSpeed, TYPE baseType)
        {
            _baseHealth = baseHealth;
            CurrentHealth = _baseHealth;
            _baseAttack = baseAttack;
            _baseDefense = baseDefense;
            _baseSpeed = baseSpeed;
            _baseType = baseType;
        }
        /// <summary>
        /// HP actuel du personnage
        /// </summary>
        public int CurrentHealth { get; private set; }
        public TYPE BaseType { get => _baseType;}
        /// <summary>
        /// HPMax, prendre en compte base et equipement potentiel
        /// </summary>
        public int MaxHealth
        {
            get
            {
                return (CurrentEquipment == null) ? _baseHealth : _baseHealth + CurrentEquipment.BonusHealth;
            }
        }
        /// <summary>
        /// ATK, prendre en compte base et equipement potentiel
        /// </summary>
        public int Attack
        {
            get
            {
                return (CurrentEquipment == null )? _baseAttack : _baseAttack + CurrentEquipment.BonusAttack;
            }
        }
        /// <summary>
        /// DEF, prendre en compte base et equipement potentiel
        /// </summary>
        public int Defense
        {
            get
            {
                return CurrentEquipment == null ? _baseDefense : _baseDefense + CurrentEquipment.BonusDefense;
            }
        }
        /// <summary>
        /// SPE, prendre en compte base et equipement potentiel
        /// </summary>
        public int Speed
        {
            get
            {
                return CurrentEquipment == null ? _baseSpeed : _baseSpeed + CurrentEquipment.BonusSpeed;
            }
        }
        /// <summary>
        /// Equipement unique du personnage
        /// </summary>
        public Equipment CurrentEquipment { get; private set; }
        /// <summary>
        /// null si pas de status
        /// </summary>
        public StatusEffect CurrentStatus { get; private set; }

        public bool IsAlive => CurrentHealth > 0;


        /// <summary>
        /// Application d'un skill contre le personnage
        /// On pourrait potentiellement avoir besoin de connaitre le personnage attaquant,
        /// Vous pouvez adapter au besoin
        /// </summary>
        /// <param name="s">skill attaquant</param>
        /// <exception cref="NotImplementedException"></exception>
        public void ReceiveAttack(Skill s)
        {
            
            if (s == null) throw new NotImplementedException();
            CurrentHealth -= (int)(s.Power*TypeResolver.GetFactor(s.Type, _baseType)) - (CurrentEquipment == null ? _baseDefense : CurrentEquipment.BonusDefense + _baseDefense);
            CurrentHealth = Math.Max(0, CurrentHealth);
            if (CurrentStatus == null)
            {
                CurrentStatus = StatusEffect.GetNewStatusEffect(s.Status);
            }
        }
        /// <summary> 
        /// Equipe un objet au personnage
        /// </summary>
        /// <param name="newEquipment">equipement a appliquer</param>
        /// <exception cref="ArgumentNullException">Si equipement est null</exception>
        public void Equip(Equipment newEquipment)
        {
            if (newEquipment == null ) throw new ArgumentNullException();
            CurrentEquipment = newEquipment;
            
        }
        /// <summary>
        /// Desequipe l'objet en cours au personnage
        /// </summary>
        public void Unequip()
        {
            CurrentEquipment = null;
            CurrentHealth = (int)MathF.Min(MaxHealth, CurrentHealth);
        }

        public void takeStatusDmg(int dmg)
        {
            CurrentHealth = Math.Max(0, CurrentHealth-dmg);

        }

        public void setStatus(StatusPotential s)
        {
            CurrentStatus = StatusEffect.GetNewStatusEffect(s);
        }

    }
}
