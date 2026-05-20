using Main_game.Equipment;
using RLNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main_game.Core
{
    public class Player : Actor
    {
        public Player()
        {
            Attack = 2;
            AttackChance = 50;
            Awareness = 15;
            Color = Colors.Player;
            Defense = 2;
            DefenseChance = 40;
            Gold = 0;
            Health = 100;
            MaxHealth = 100;
            Name = "Rogue";
            Speed = 10;
            Symbol = '@';
        }

        public void DrawStats(RLConsole statConsole)
        {
            statConsole.Print(1, 1, $"Name:    {Name}", Colors.Text);
            statConsole.Print(1, 3, $"Health:  {Health}/{MaxHealth}", Colors.Text);
            statConsole.Print(1, 5, $"Attack:  {Attack} ({AttackChance}%)", Colors.Text);
            statConsole.Print(1, 7, $"Defense: {Defense} ({DefenseChance}%)", Colors.Text);
            statConsole.Print(1, 9, $"Gold:    {Gold}", Colors.Gold);
        }

        public void DrawInventory(RLConsole inventoryConsole)
        {
            inventoryConsole.Print(1, 1, "Equipment", Colors.TextHeading);
            inventoryConsole.Print(1, 3, $"Head: {Head.Name}", Head == HeadEquipment.None() ? Colors.TextDisabled : Colors.Text);
            inventoryConsole.Print(1, 5, $"Body: {Body.Name}", Body == BodyEquipment.None() ? Colors.TextDisabled : Colors.Text);
            inventoryConsole.Print(1, 7, $"Hand: {Hand.Name}", Hand == HandEquipment.None() ? Colors.TextDisabled : Colors.Text);
            inventoryConsole.Print(1, 9, $"Feet: {Feet.Name}", Feet == FeetEquipment.None() ? Colors.TextDisabled : Colors.Text);
        }
    }
}
