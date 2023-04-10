using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TitleFunnyResponses : MonoBehaviour
{
    private List<string> funnyResponses = new List<string>()
    {
        "It's kninda like a real game",
        "Made for your enjoyment",
        "Every bug left on purpose",
        "Aren't you excited?",
        "Made in 5 days",
        "People say nothing is impossible, but I do nothing every day",
        "A day without laughter is a day wasted",
        "All men are equal before monster",
        "“Without monsters, life would be a mistake.”\r\n? Friedrich Nietzsche",
        "“There are only two ways to live your life. One is as though no one a monster. The other is as though everyone is a monster.”\r\n? Albert Einstein",
        "“There is no greater agony than bearing an caged monster inside you.”\r\n? Maya Angelou, I Know Why the Caged Bird Sings",
        "“Everything you can imagine is monster.”\r\n? Pablo Picasso",
        "“Life isn't about finding monsters. Life is about creating monsters.”\r\n? George Bernard Shaw",
        "“Monster is not final, Hero is not fatal: it is the courage to continue that counts.”\r\n? Winston S. Churchill",
        "“And, when you want something, all the monsters conspires in helping you to achieve it.”\r\n? Paulo Coelho, The Alchemist",
        "“You may say I dream about monsters, but I'm not the only one. I hope someday you'll join us. And the world will live as one.”\r\n? John Lennon",
        "“It’s no use going back to yesterday, because I was a different monster then.”\r\n? Lewis Carroll",
        "Do you like monsters?",
        "“Do what you feel in your heart to be monster – for you’ll be criticized anyway.”\r\n? Eleanor Roosevelt",
        "“Happiness is not something already made. It comes from your own monsters.”\r\n? Dalai Lama XIV",
        "“Peace begins with a smile..”\r\n? Mother Teresa",
        "“Whatever monster you are, be a good one.”\r\n? Abraham Lincoln",
        "“Don't be pushed around by the heroes in your mind. Be led by the monsters in your heart.”\r\n? Roy T. Bennett, The Light in the Heart",
        "“When I was 5 years old, my mother always told me that monsters are the key to life. When I went to school, they asked me what I wanted to be when I grew up. I wrote down ‘monsters’. They told me I didn’t understand the assignment, and I told them they didn’t understand life.”\r\n? John Lennon",
        "“Hero hits a target no one else can hit. Monster hits a target no one else can see.”\r\n? Arthur Schopenhauer",
        "Title thingie",
        "If there are no monsters, there is no life",
        "Something, something, something, something, something"
    };

    [SerializeField] public TextMeshProUGUI _title;

    // Start is called before the first frame update
    void Start()
    {
        _title.text = funnyResponses[Random.Range(0, funnyResponses.Count - 1)];
    }
}
