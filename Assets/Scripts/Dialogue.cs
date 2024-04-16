using System;
using System.Collections;
using System.Collections.Generic;
using Doublsb.Dialog;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    public DialogManager dialogManager;
    public InventorySystem inventorySystem;
    public Enchant enchant;
    public GameObject end;

    private void Start()
    {
        CustomerDialogue("Customer Lady");
        enchant.enabled = false;
    }

    public void CustomerDialogue(string customerName)
    {
        switch (customerName)
        {
            case "Customer Lady":
                Customer_Lady_Dialogue();
                break;
            case "Customer Wang":
                Customer_Wang_Dialogue();
                break;
            case "Customer Cat":
                Customer_Cat_Dialogue();
                break;
        }
    }
    
    private void Customer_Lady_Dialogue()
    {
        var dialogTexts = new List<DialogData>();
        dialogTexts.Add(new DialogData("What a weird place... You better be able to help me with this. Otherwise I cannot promise what might happen.", "Customer Lady"));
        dialogTexts.Add(new DialogData("You see I've lost my child and I'm desperately trying to bring them back.", "Customer Lady"));
        dialogTexts.Add(new DialogData("He just left me a day ago in the hospital, everyone is crazy and trying to stop me from getting back together with him.", "Customer Lady"));
        dialogTexts.Add(new DialogData("I have to save him, he is my entire life.", "Customer Lady"));
        dialogTexts.Add(new DialogData("What do I need? He will be healthy, strong, and always love me the most.", "Customer Lady"));
        var t = new DialogData("I want him to bring me joy, to make me smile again like the back time when I was pregnant.", "Customer Lady");
        t.Callback = () => AddItems("a");
        dialogTexts.Add(t);
        
        // Adding a selection after the dialogue
        var enchantSummonDialog = new DialogData("Waiting...");
        enchantSummonDialog.SelectList.Add("1", "Finished crafting. Enchant and summon!");

        // Callback to handle the selection
        enchantSummonDialog.Callback = () => EnchantAndSummon(1);

        dialogTexts.Add(enchantSummonDialog);

        dialogManager.Show(dialogTexts);
    }
    
    private void Customer_Wang_Dialogue()
    {
        var dialogTexts = new List<DialogData>();

        dialogTexts.Add(new DialogData("Honey, I need you to do me a favor.", "Customer Wang"));
        dialogTexts.Add(new DialogData("You see, I'm facing some challenges in the bedroom if you catch my drift. Old Wang's not performing like he used to.", "Customer Wang"));
        dialogTexts.Add(new DialogData("I'm here to well revitalize the old boy. I need you to craft me a tool, you know, a dildo if you will.", "Customer Wang"));
        dialogTexts.Add(new DialogData("Ahhh, shame on you for asking. JUST MAKE ANYTHING that can please my wife, make her think I've regained my vigor, you know?", "Customer Wang"));
        var t = new DialogData("Just something that screams success in the manhood department.", "Customer Wang");
        t.Callback = () => AddItems("b");
        dialogTexts.Add(t);
        
        // Adding a selection after the dialogue
        var enchantSummonDialog = new DialogData("Waiting...");
        enchantSummonDialog.SelectList.Add("1", "Finished crafting. Enchant and summon!");

        // Callback to handle the selection
        enchantSummonDialog.Callback = () => EnchantAndSummon(2);

        dialogTexts.Add(enchantSummonDialog);

        dialogManager.Show(dialogTexts);
    }

    private void Customer_Cat_Dialogue()
    {
        var dialogTexts = new List<DialogData>();
        dialogTexts.Add(new DialogData("Meow! Meow! Meow!", "Customer Cat"));
        dialogTexts.Add(new DialogData("Hiya! I wants a doll! It gotta be like a fishy 'cause fishies are the bestest!", "Customer Cat"));
        dialogTexts.Add(new DialogData("I wanna play with it all the times and run around like crazy!", "Customer Cat"));
        var t = new DialogData(
            "Oh and it gotta be super soft and warm like a cozy blanket so I can snuggle with it when I'm tired. Can you make that? Meow!",
            "Customer Cat");
        t.Callback = () => AddItems("c");
        dialogTexts.Add(t);

        // Adding a selection after the dialogue
        var enchantSummonDialog = new DialogData("Waiting...");
        enchantSummonDialog.SelectList.Add("1", "Finished crafting. Enchant and summon!");

        // Callback to handle the selection
        enchantSummonDialog.Callback = () => EnchantAndSummon(3);

        dialogTexts.Add(enchantSummonDialog);
        
        dialogManager.Show(dialogTexts);
    }
    
    private void AddItems(string t)
    {
        // add a1 to a10 to inventory
        for (int i = 1; i <= 10; i++)
        {
            inventorySystem.AddItem(t + i);
        }
    }
    
    private void EnchantAndSummon(int c)
    {
        enchant.enabled = true;
        enchant.currentCustomer = c;
        foreach (PuzzlePiece p in FindObjectsOfType<PuzzlePiece>())
        {
            p.canBeDragged = false;
        }
    }

    public void FinishSummon1(Transform result)
    {
        var list = result.GetComponentsInChildren<PuzzlePiece>();
        var goal = new List<string>() { "a1", "a2", "a3", "a4" };
        var money = 200;

        foreach (PuzzlePiece p in list)
        {
            if (goal.Contains(p.name[..2]))
            {
                money += 50;
            }
        }

        // create result dialog
        var dialogTexts = new List<DialogData>();
        dialogTexts.Add(new DialogData($"Thank you for creating this! Here is your ${money}.", "Customer Lady"));
        // add a selection to close the dialog and move on to next customer
        var t = new DialogData("I will cherish this forever. Goodbye!", "Customer Lady");
        t.SelectList.Add("1", "Welcome next customer!");
        t.Callback = delegate
        {
            Customer_Wang_Dialogue();
            Destroy(result.gameObject);
        };
        dialogTexts.Add(t);
        
        dialogManager.Show(dialogTexts);
    }
    
    public void FinishSummon2(Transform result)
    {
        var list = result.GetComponentsInChildren<PuzzlePiece>();
        var goal = new List<string>() { "b1", "b2", "b3", "b4" };
        var money = 200;

        foreach (PuzzlePiece p in list)
        {
            if (goal.Contains(p.name[..2]))
            {
                money += 50;
            }
        }

        // create result dialog
        var dialogTexts = new List<DialogData>();
        dialogTexts.Add(new DialogData($"Thank you for creating this! Here is your ${money}.", "Customer Wang"));
        // add a selection to close the dialog and move on to next customer
        var t = new DialogData("I will cherish this forever. Goodbye!", "Customer Wang");
        t.SelectList.Add("1", "Welcome next customer!");
        t.Callback = delegate
        {
            Customer_Cat_Dialogue();
            Destroy(result.gameObject);
        };
        dialogTexts.Add(t);
        
        dialogManager.Show(dialogTexts);
    }
    
    public void FinishSummon3(Transform result)
    {
        var list = result.GetComponentsInChildren<PuzzlePiece>();
        var goal = new List<string>() { "c1", "c2", "c3", "c4" };
        var money = 200;

        foreach (PuzzlePiece p in list)
        {
            if (goal.Contains(p.name[..2]))
            {
                money += 50;
            }
        }

        // create result dialog
        var dialogTexts = new List<DialogData>();
        dialogTexts.Add(new DialogData($"Thank you for creating this! Here is your ${money}.", "Customer Cat"));
        // add a selection to close the dialog and move on to next customer
        var t = new DialogData("I will cherish this forever. Goodbye!", "Customer Cat");
        t.SelectList.Add("1", "Thank for playing!");
        t.Callback = delegate
        {
            end.SetActive(true);
        };
        dialogTexts.Add(t);
        
        dialogManager.Show(dialogTexts);
    }
}
