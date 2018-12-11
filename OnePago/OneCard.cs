using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnePago
{
    public class OneCard
    {
        public class CardInfo
        {
            public enum Shape
            {
                None,
                Diamond,
                Heart,
                Spade,
                Clover,
                ColorJocker,
                BlackJocker
            }
            public Shape Card_Shape = Shape.None;
            public int Card_Number = 0; // A = 1, J=11 ,Q=12, K=13
            public bool Color = false;

            public CardInfo()
            {

            }

            public CardInfo(Shape card_Shape, int card_Number, bool color)
            {
                Card_Shape = card_Shape;
                Card_Number = card_Number;
                Color = color;
            }

            public static bool operator ==(CardInfo obj1, CardInfo obj2)
            {
                bool Check_Shape = obj1.Card_Shape == obj2.Card_Shape;
                bool Check_Number = obj1.Card_Number == obj2.Card_Number;
                return (Check_Number && Check_Shape);
            }
            public static bool operator !=(CardInfo obj1, CardInfo obj2)
            {
                return !(obj1 == obj2);
            }
            public override string ToString()
            {
                string target = "";
                target += Card_Shape.ToString();
                switch (Card_Number)
                {
                    case 1:
                        target += "A";
                        break;
                    case 11:
                        target += "J";
                        break;
                    case 12:
                        target += "Q";
                        break;
                    case 13:
                        target += "K";
                        break;
                    default:
                        target += Card_Number;
                        break;
                }
                return target;
            }

        }

        public class Player
        {
            public int ID = 0;
            bool Am_Onecard;
            public List<CardInfo> Cards = new List<CardInfo>();


            public Player()
            { }

            public virtual void Do(OneCard system, Player who, List<Player> Players)
            {
                K:
                bool Card_found = false;
                bool OneMore = false;
                CardInfo.Shape tempcard = CardInfo.Shape.None;
                bool IsJ = false;
                bool IsSeven = false;
                if (system.Attack != 0) //공격받는중
                {
                    for (int i = 0; i < Cards.Count; i++)
                    {
                        CardInfo card = Cards[i];
                        if (card.Card_Shape == CardInfo.Shape.BlackJocker || card.Card_Shape == CardInfo.Shape.ColorJocker)
                        {
                            if (system.Vaildate(card))
                            {
                                switch (card.Card_Number)
                                {
                                    case 1:
                                        //A
                                        system.Attack += 3;
                                        break;
                                    case 2:
                                        system.Attack += 2;
                                        break;
                                    case 3:
                                        system.Attack = 0;
                                        break;
                                }
                                switch (card.Card_Shape)
                                {
                                    case CardInfo.Shape.BlackJocker:
                                        system.Attack += 5;
                                        break;
                                    case CardInfo.Shape.ColorJocker:
                                        system.Attack += 10;
                                        break;
                                }
                                Console.WriteLine(ID + ": 공격대응 " + card + "제출");
                                Card_found = true;
                                system.Play(card, ref who, false);
                                if (OneMore)
                                    goto K;
                                goto Exit;
                            }

                        }
                    }
                    if (!Card_found)
                    {
                        Card_found = false;
                        for (int j = 0; j != system.Attack; j++)
                        {
                            Console.WriteLine(ID + ": " + system.Take(ref who) + "먹음");
                        }
                        system.Attack = 0;
                        goto Exit;
                    }
                }
                else //평상시
                {
                    for (int i = 0; i < Cards.Count; i++)
                    {
                        CardInfo card = Cards[i];
                        if (system.Vaildate(card))
                        {
                            switch (card.Card_Number)
                            {
                                case 1:
                                    //A
                                    system.Attack += 3;
                                    break;
                                case 2:
                                    system.Attack += 2;
                                    break;
                                case 3:
                                    system.Attack = 0;
                                    break;
                                case 11:
                                    //J
                                    IsJ = true;
                                    break;
                                case 12:
                                    //Q
                                    system.GameTurn.Reverse(system, Players);
                                    break;
                                case 13:
                                    //K
                                    OneMore = true;
                                    break;
                                case 7:
                                    bool Iscolor = false;
                                    int[] Check_list = new int[5];
                                    if (Cards.Count != 0)
                                    {
                                        foreach (CardInfo Check_card in Cards)
                                        {
                                            switch (Check_card.Card_Shape)
                                            {
                                                case CardInfo.Shape.Spade:
                                                    Check_list[3]++;
                                                    break;
                                                case CardInfo.Shape.Heart:
                                                    Check_list[2]++;
                                                    break;
                                                case CardInfo.Shape.Diamond:
                                                    Check_list[1]++;
                                                    break;
                                                case CardInfo.Shape.Clover:
                                                    Check_list[4]++;
                                                    break;
                                            }
                                        }
                                        tempcard = (CardInfo.Shape)Check_list.ToList().IndexOf(Check_list.Max());
                                        if (tempcard == CardInfo.Shape.Heart || tempcard == CardInfo.Shape.Diamond)
                                        {
                                            Iscolor = true;
                                        }
                                        system.LastCard = new CardInfo(tempcard, 7, Iscolor);
                                        IsSeven = true;
                                    }
                                    break;
                            }

                            switch (card.Card_Shape)
                            {
                                case CardInfo.Shape.BlackJocker:
                                    system.Attack += 5;
                                    break;
                                case CardInfo.Shape.ColorJocker:
                                    system.Attack += 10;
                                    break;
                            }

                            Console.WriteLine(ID + ": " + card + "제출 남은 카드 " + Cards.Count);
                            if (tempcard != CardInfo.Shape.None)
                                Console.WriteLine("카드 모양 변경: " + tempcard);
                            Card_found = true;
                            system.Play(card, ref who, IsSeven);
                            if (OneMore)
                                goto K;
                            goto Exit;
                        }
                    }
                    if (!Card_found)
                    {
                        Card_found = false;
                        Console.WriteLine(ID + ": " + system.Take(ref who) + "먹음");
                        goto Exit;
                    }
                }
                Exit:
                if (who.Cards.Count >= 20)
                {
                    system.Bankrupt(who);
                }
                Console.WriteLine(ID + ": " + Cards.Count + "장");
                system.GameTurn.Next(system, Players, IsJ);
            }

            public void Find(OneCard system)
            {
              foreach(CardInfo card in Cards)
                {

                }
            }
        }
    
        public class Turn
        {
            int Count_Turn = 0;
            public Player GameTurn = null;
            public bool reversed = false;
            bool reset = false;
            public int cumulation;
            public bool PlayerChanged = false;

            public Player Next(OneCard system, List<Player> Players,bool IsJ = false)
            {
                if (system.GameStarted)
                {
                    if (Players.Count == 1)
                    {
                        system.Win(Players[0]);
                        system.End();
                        goto End;
                    }

                    cumulation++;

                    if (PlayerChanged && Count_Turn >= Players.Count)
                    {
                        if (Count_Turn >= Players.Count - 1 && !reversed)
                        {
                            reset = true;
                            Count_Turn = 0;
                        }
                        if (Count_Turn <= -1 && reversed)
                        {
                            reset = true;
                            Count_Turn = Players.Count;
                        }

                        if (!reset)
                        {
                            if (reversed)
                            {
                                Count_Turn--;
                                if (Count_Turn < 0)
                                    Count_Turn = Players.Count - 1;
                            }
                            else
                                Count_Turn++;
                        }
                        reset = false;
                    }
                    int repeat = IsJ ? 2 : 1;
                    for (int i = 0; i != repeat; i++)
                    {
                        if (Count_Turn == Players.Count - 1 && !reversed)
                        {
                            reset = true;
                            Count_Turn = 0;
                        }
                        if (Count_Turn == -1 && reversed)
                        {
                            reset = true;
                            Count_Turn = Players.Count;
                        }

                        if (!reset)
                        {
                            if (reversed)
                            {
                                Count_Turn--;
                                if (Count_Turn < 0)
                                    Count_Turn = Players.Count - 1;
                            }
                            else
                                Count_Turn++;
                        }

                        reset = false;
                    }
                    Player output = Players[Count_Turn];
                    output.Do(system, output, Players);
                    return output;
                }
                End:
                return new Player();
            }   

            public void Reverse(OneCard system, List<Player> Players)
            {
                reversed = !reversed;
            }
        }

        public List<CardInfo> CardDeck = new List<CardInfo>(52);
        public List<Player> Players = new List<Player>();
        public CardInfo LastCard = null;
        public Turn GameTurn = new Turn();
        protected int Changed_PlayerID = 0;
        public int Attack;
        public bool GameStarted;

        protected OneCard()
        {
        }

        public OneCard(List<Player> players)
        {
            GenDeck(false);
            Players = players;
            for (int who = 0; who < players.Count; who++)
            {
                Player whois = players[who];
                whois.ID = who;
                for(int i = 0; i != 7;i++)
                {
                    Take(ref whois);
                }
            }
        }
        
        public void Start()
        {
            GameStarted = true;
            GameTurn.Next(this, Players);
        }
        public void Play(CardInfo card, ref Player who,bool Seven)
        {
            who.Cards.Remove(card);
            if (!Seven)
            {
                LastCard = card;
            }
            if (who.Cards.Count == 0)
            {
                Win(who);
            }
        }

        public bool Vaildate(CardInfo card)
        {
            if (card.Card_Shape == CardInfo.Shape.BlackJocker || card.Card_Shape == CardInfo.Shape.ColorJocker)
            {
                if (LastCard.Card_Shape == CardInfo.Shape.BlackJocker || LastCard.Card_Shape == CardInfo.Shape.ColorJocker)
                {
                    return true;
                }
                if (card.Color == LastCard.Color)
                {
                    return true;
                }
            }
            else
            {
                if (LastCard.Card_Shape == CardInfo.Shape.BlackJocker || LastCard.Card_Shape == CardInfo.Shape.ColorJocker)
                {
                    if (card.Color == LastCard.Color)
                    {
                        return true;
                    }
                }
                if (card.Card_Number == LastCard.Card_Number || card.Card_Shape == LastCard.Card_Shape)
                {
                    return true;
                }
            }
            return false;
        }

        //파산
        public void Bankrupt(Player who)
        {
            Changed_PlayerID = who.ID;
            Console.WriteLine("파산." + who.ID);
            Players.Remove(who);
            GC.Collect();
            GameTurn.PlayerChanged = true;
        }

        //승리
        public void Win(Player who)
        {
            Changed_PlayerID = who.ID;
            Console.WriteLine("승리." + who.ID);
            Players.Remove(who);
            GC.Collect();
            GameTurn.PlayerChanged = true;
        }

        //끝(상대선수 없음)
        public void End()
        {
            Console.WriteLine("겜 종료"+ GameTurn.cumulation+"턴");
        }

        public CardInfo Take(ref Player who)
        {
            List<CardInfo> reject = new List<CardInfo>();
            foreach(Player player in Players)
            {
                reject.AddRange(player.Cards);
            }
            if(CardDeck.Count == 0)
            {
                GenDeck(true,reject);
            }
            if(CardDeck.Count == 0)
            {
                Console.WriteLine("다 쳐먹음ㅋㅋㅋㅋ겜끝남");
                GameStarted = false;
                Players.Clear();
                GC.Collect();
                goto End;
            }
            CardInfo temp = CardDeck[0];
            CardDeck.RemoveAt(0);
            who.Cards.Add(temp);
            return temp;
            End:
            return new CardInfo();
        }


        void GenDeck(bool PlayerReq,List<CardInfo> rejects = null)
        {
            int shape = 1;
            if (!PlayerReq) //첫번째 판
            {
                for (int i = 0; i != 4; i++)
                {
                    for (int j = 1; j != 14; j++)
                    {
                        if((CardInfo.Shape)i == CardInfo.Shape.Heart || (CardInfo.Shape)i == CardInfo.Shape.Diamond)
                        {
                            CardDeck.Add(new CardInfo((CardInfo.Shape)shape, j,true));
                        }
                        CardDeck.Add(new CardInfo((CardInfo.Shape)shape, j,false));
                    }
                    shape++;
                }
                CardDeck.Add(new CardInfo(CardInfo.Shape.BlackJocker, 0,false));
                CardDeck.Add(new CardInfo(CardInfo.Shape.ColorJocker, 0,true));
                Shuffle(ref CardDeck);
                LastCard = CardDeck[0];
                //덱 완성(54장)
            }
            else
            {
                bool Is_reject = false;
                for (int i = 0; i != 4; i++)
                {
                    for (int j = 1; j != 14; j++)
                    {
                        CardInfo gencard = null;
                        if ((CardInfo.Shape)i == CardInfo.Shape.Heart || (CardInfo.Shape)i == CardInfo.Shape.Diamond)
                        {
                            gencard = new CardInfo((CardInfo.Shape)shape, j, true);
                        }
                        gencard = new CardInfo((CardInfo.Shape)shape, j,false);
                        foreach (CardInfo reject in rejects)
                        {
                            if (gencard == reject)
                            {
                                Is_reject = true;
                            }
                        }
                        if(!Is_reject)
                            CardDeck.Add(gencard);

                        Is_reject = false;
                    }
                    shape++;
                }

                Is_reject = false;
                CardInfo gencard_jocker = new CardInfo(CardInfo.Shape.BlackJocker, 0,false);
                foreach (CardInfo reject in rejects)
                {
                    if (gencard_jocker == reject)
                    {
                        Is_reject = true;
                    }     
                }
                if (!Is_reject)
                    CardDeck.Add(gencard_jocker);

                Is_reject = false;
                gencard_jocker = new CardInfo(CardInfo.Shape.ColorJocker, 0,true);
                foreach (CardInfo reject in rejects)
                {
                    if (gencard_jocker == reject)
                    {
                        Is_reject = true;
                    }
                }
                if (!Is_reject)
                    CardDeck.Add(gencard_jocker);

                //덱 완성(n 장)
                Shuffle(ref CardDeck);
            }
            Console.WriteLine("카드 섞음 " + LastCard);
            CardDeck.Remove(LastCard);
        }
        Random rng = new Random();
        void Shuffle(ref List<CardInfo> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                CardInfo value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
