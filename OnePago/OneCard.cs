using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using System.Threading;

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
            public bool temp = false;
            public int ID = 0;

            public CardInfo()
            {
            }

            public CardInfo(Shape card_Shape, int card_Number)
            {
                Card_Shape = card_Shape;
                Card_Number = card_Number;
                if (Card_Shape == Shape.Heart || Card_Shape == Shape.Diamond || Card_Shape == Shape.ColorJocker)
                {
                    Color = true;
                }
            }

            public static bool operator ==(CardInfo obj1, CardInfo obj2)
            {
                if(obj1 is null || obj2 is null)
                {
                    return false;
                }
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
                if (!temp)
                {
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
                }
                return target;
            }

            public override bool Equals(object obj)
            {
                var info = obj as CardInfo;
                return info != null &&
                       Card_Shape == info.Card_Shape &&
                       Card_Number == info.Card_Number;
            }

            public override int GetHashCode()
            {
                var hashCode = 859603374;
                hashCode = hashCode * -1521134295 + Card_Shape.GetHashCode();
                hashCode = hashCode * -1521134295 + Card_Number.GetHashCode();
                return hashCode;
            }
        }

        public abstract class Player
        {
            public int ID = 0;
            public List<CardInfo> Cards = new List<CardInfo>();
            public bool AmAI;
            public int x_margin = 0;
            public int y_margin = 0;
            public bool Horizon;

            public abstract void Do(OneCard system, Player who, List<Player> Players);

            public abstract void ForgetCard();

            public abstract void RememberCard(CardInfo card);

  
        }

        public class AI : Player
        {
            public AI()
            { AmAI = true; }

            public override void Do(OneCard system, Player who, List<Player> Players)
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
                            {
                                goto K;
                            }

                            goto Exit;
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
                                    system.GameTurn.Reverse(system);
                                    break;

                                case 13:
                                    //K
                                    OneMore = true;
                                    break;

                                case 7:
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
                                        system.LastCard = new CardInfo(tempcard, 7)
                                        {
                                            temp = true
                                        };
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
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.WriteLine(ID + ": " + card + "제출 남은 카드 " + Cards.Count);
                            if (tempcard != CardInfo.Shape.None)
                            {
                                Console.WriteLine("카드 모양 변경: " + tempcard);
                            }
                            Console.ForegroundColor = ConsoleColor.Gray;

                            Card_found = true;
                            system.Play(card, ref who, IsSeven);
                            if (OneMore)
                            {
                                goto K;
                            }

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
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine(ID + ": " + Cards.Count + "장");
                if (Cards.Count == 1)
                {
                    SpeechSynthesizer synthesizer = new SpeechSynthesizer
                    {
                        Volume = 100,  // 0...100
                        Rate = 7     // -10...10
                    };

                    synthesizer.Speak("OneCard");
                }
                Console.ForegroundColor = ConsoleColor.Gray;
                system.GameTurn.Next(system, Players, IsJ);
            }

            public override void ForgetCard()
            {
            }

            public override void RememberCard(CardInfo card)
            {
            }

        }

     

        public class Human : Player
        {
            public Human()
            { AmAI = false; }

            public override void Do(OneCard system, Player who, List<Player> Players)
            {
                K:
                bool OneMore = false;
                CardInfo.Shape tempcard = CardInfo.Shape.None;
                bool IsJ = false;
                bool IsSeven = false;
                Console.WriteLine("카드를 낼시간");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("마지막카드" + system.LastCard);
                Console.ForegroundColor = ConsoleColor.Gray;
                if (system.Attack != 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("경고! 공격받는중" + system.Attack + "장!");
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
                for (int i = 0; i != Cards.Count; i++)
                {
                    Cards[i].ID = i;
                    Console.WriteLine(i + ":" + Cards[i]);
                }
                Console.WriteLine("먹을려면" + Cards.Count);
                int want = 0;
                try
                {
                    want = int.Parse(Console.ReadLine());
                }
                catch(FormatException e)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("올바른 입력이 아닙니다");
                    Console.ForegroundColor = ConsoleColor.Gray;

                    goto K;
                }
                if (want == Cards.Count)
                {
                    if (system.Attack != 0)
                    {
                        for (int j = 0; j != system.Attack; j++)
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine(ID + ": " + system.Take(ref who) + "먹음");
                            Console.ForegroundColor = ConsoleColor.Gray;
                        }
                        system.Attack = 0;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine(ID + ": " + system.Take(ref who) + "먹음");
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
                    goto Exit;
                }
                if(want > Cards.Count)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("올바른 입력이 아닙니다");
                    Console.ForegroundColor = ConsoleColor.Gray;

                    goto K;
                }
                CardInfo card = Cards[want];
                if (system.Attack != 0) //공격받는중
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
                        system.Play(card, ref who, false);
                        if (OneMore)
                        {
                            goto K;
                        }

                        goto Exit;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("올바른 카드가 아닙니다!");
                        Console.ForegroundColor = ConsoleColor.Gray;
                        goto K;
                    }
                }
                else //평상시
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

                            case 11:
                                //J
                                IsJ = true;
                                break;

                            case 12:
                                //Q
                                system.GameTurn.Reverse(system);
                                break;

                            case 13:
                                //K
                                OneMore = true;
                                break;

                            case 7:
                                Console.ForegroundColor = ConsoleColor.Blue;
                                Console.WriteLine("무슨 모양으로 변경하겠습니까?");
                                Console.WriteLine("1. 다이아 2.하트 3.스페이드 4. 클로버");
                                try
                                {
                                    tempcard = (CardInfo.Shape)int.Parse(Console.ReadLine());
                                }
                                catch (FormatException e)
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("올바른 입력이 아닙니다");
                                    goto K;
                                }
                                system.LastCard = new CardInfo(tempcard, 7)
                                {
                                    temp = true
                                };
                                IsSeven = true;
                                Console.ForegroundColor = ConsoleColor.Gray;
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
                        {
                            Console.WriteLine("카드 모양 변경: " + tempcard);
                        }

                        system.Play(card, ref who, IsSeven);
                        if (OneMore)
                        {
                            goto K;
                        }

                        goto Exit;
                    }
                    else
                    {
                        Console.WriteLine("올바른 카드가 아닙니다!");
                        goto K;
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

            public override void ForgetCard()
            {
            }

            public override void RememberCard(CardInfo card)
            {
            }

        }

        public class Turn
        {
            private int Count_Turn = 0;
            public Player GameTurn = null;
            public bool reversed = false;
            private bool reset = false;
            public int cumulation;
            public bool PlayerChanged = false;
            public bool LastTurnWin = false;
            public bool Card_Mixed = false;

            public Player Next(OneCard system, List<Player> Players, bool IsJ = false)
            {
                if (system.GameStarted)
                {
                    if (Players.Count == 1)
                    {
                        if (!LastTurnWin)
                        {
                            system.Win(Players[0]);
                        }
                        system.End();
                        goto End;
                    }
                    LastTurnWin = false;
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
                                {
                                    Count_Turn = Players.Count - 1;
                                }
                            }
                            else
                            {
                                Count_Turn++;
                            }
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
                                {
                                    Count_Turn = Players.Count - 1;
                                }
                            }
                            else
                            {
                                Count_Turn++;
                            }
                        }

                        reset = false;
                    }
                    Player output = Players[Count_Turn];
                    system.GUI_system.whosturn = output;
                    output.Do(system, output, Players);
                    return output;
                }
                End:
                return new AI();
            }

            public void Reverse(OneCard system)
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
        public GUI GUI_system = null;

        protected OneCard()
        {
        }

        public OneCard(List<Player> players, GUI GUI_system)
        {
            this.GUI_system = GUI_system;
            GenDeck(false);
            Players = players;
            for (int who = 0; who < players.Count; who++)
            {
                Player whois = players[who];
                for (int i = 0; i != 7; i++)
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

        public void Play(CardInfo card, ref Player who, bool Seven)
        {
            who.Cards.Remove(card);
            GUI_system.DelGO(card, who);
            if (!Seven)
            {
                LastCard.temp = false;
                LastCard = card;
            }
            if (who.Cards.Count == 0)
            {
                Win(who);
                GameTurn.LastTurnWin = true;
            }
            for (int i = 0; i != Players.Count; i++)
            {
                if (Players[i].AmAI)
                {
                    who.RememberCard(card);
                }
            }
            //if (who.AmAI)
            //    Thread.Sleep(2000);
        }

        public bool Vaildate(CardInfo card)
        {
            if (Attack != 0) // 공격받는중
            {
                if (card.Card_Shape == CardInfo.Shape.BlackJocker || card.Card_Shape == CardInfo.Shape.ColorJocker) //조커 내고싶음
                {
                    if (LastCard.Card_Shape == CardInfo.Shape.BlackJocker || LastCard.Card_Shape == CardInfo.Shape.ColorJocker) //연속 조커
                    {
                        return true;
                    }
                    if (card.Color == LastCard.Color)//색일치
                    {
                        return true;
                    }
                }
                if (LastCard.Card_Shape == CardInfo.Shape.BlackJocker || LastCard.Card_Shape == CardInfo.Shape.ColorJocker) //조커로 공격 '받는'중
                {
                    if(LastCard.Card_Shape == CardInfo.Shape.BlackJocker) //흑조 시도
                    {
                        if (card.Card_Number == 3 && card.Card_Shape == CardInfo.Shape.Spade) //스페이드 3 방어 시도
                        {
                            return true;
                        }
                    }
                    return false;
                }
                if (card.Card_Number == 1 || card.Card_Number == 2 || card.Card_Number == 3) //A,2,3 공격및방어 시도중
                {
                    if (card.Card_Number == 3) //3시도
                    {
                        if (LastCard.Card_Number == 2 && card.Card_Shape == LastCard.Card_Shape)//마지막 카드 2이며 모양일치
                        {
                            return true;
                        }
                        return false;
                    }
                    return true;
                }
                return false;
            }
            else
            {
                if (card.Card_Shape == CardInfo.Shape.BlackJocker || card.Card_Shape == CardInfo.Shape.ColorJocker) //조커 내고싶음
                {
                    if (LastCard.Card_Shape == CardInfo.Shape.BlackJocker || LastCard.Card_Shape == CardInfo.Shape.ColorJocker) //연속 조커
                    {
                        return true;
                    }
                    if (card.Color == LastCard.Color)//색일치
                    {
                        return true;
                    }
                }
                if (LastCard.Card_Shape == CardInfo.Shape.BlackJocker || LastCard.Card_Shape == CardInfo.Shape.ColorJocker) //마지막 카드 조커
                {
                    if (card.Color == LastCard.Color)//색일치
                    {
                        return true;
                    }
                }
                if (card.Card_Number == LastCard.Card_Number || card.Card_Shape == LastCard.Card_Shape)//일반
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
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("파산." + who.ID);
            Console.ForegroundColor = ConsoleColor.Gray;
            CardDeck.AddRange(who.Cards);
            Players.Remove(who);
            GC.Collect();
            GameTurn.PlayerChanged = true;
            GUI_system.Bankrupt(who);
        }

        //승리
        public void Win(Player who)
        {
            Changed_PlayerID = who.ID;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("승리." + who.ID);
            Console.ForegroundColor = ConsoleColor.Gray;
            CardDeck.AddRange(who.Cards);
            Players.Remove(who);
            GC.Collect();
            GameTurn.PlayerChanged = true;
        }

        //끝(상대선수 없음)
        public void End()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("겜 종료" + GameTurn.cumulation);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public CardInfo Take(ref Player who)
        {
            if (CardDeck.Count == 0)
            {
                List<CardInfo> reject = new List<CardInfo>();
                foreach (Player player in Players)
                {
                    reject.AddRange(player.Cards);
                }
                reject.Add(LastCard);
                GenDeck(true, reject);
            }
            if (CardDeck.Count == 0)
            {
                Console.WriteLine("다 쳐먹음 겜끝남");
                GameStarted = false;
                Players.Clear();
                GC.Collect();
                goto End;
            }
            CardInfo temp = CardDeck[0];
            CardDeck.RemoveAt(0);
            who.Cards.Add(temp);
            GUI_system.RegGO(temp, who);
            return temp;
            End:
            return new CardInfo();
        }

        private void GenDeck(bool PlayerReq, List<CardInfo> rejects = null)
        {
            if (!PlayerReq) //첫번째 판
            {
                for (int shape = 1; shape != 5; shape++)
                {
                    for (int j = 1; j != 14; j++)
                    {
                        CardDeck.Add(new CardInfo((CardInfo.Shape)shape, j));
                    }
                }
                CardDeck.Add(new CardInfo(CardInfo.Shape.BlackJocker, 0));
                CardDeck.Add(new CardInfo(CardInfo.Shape.ColorJocker, 0));
                Shuffle(ref CardDeck);
                LastCard = CardDeck[0];
                GUI_system.Update_Trash(LastCard);
                //덱 완성(54장)
            }
            else
            {
                GameTurn.Card_Mixed = true;
                bool Is_reject = false;
                for (int shape = 1; shape != 5; shape++)
                {
                    for (int j = 1; j != 14; j++)
                    {
                        CardInfo gencard = null;
                        gencard = new CardInfo((CardInfo.Shape)shape, j);
                        foreach (CardInfo reject in rejects)
                        {
                            if (gencard == reject)
                            {
                                Is_reject = true;
                            }
                        }
                        if (!Is_reject)
                        {
                            CardDeck.Add(gencard);
                        }

                        Is_reject = false;
                    }
                }

                Is_reject = false;
                CardInfo gencard_jocker = new CardInfo(CardInfo.Shape.BlackJocker, 0);
                foreach (CardInfo reject in rejects)
                {
                    if (gencard_jocker == reject)
                    {
                        Is_reject = true;
                    }
                }
                if (!Is_reject)
                {
                    CardDeck.Add(gencard_jocker);
                }

                Is_reject = false;
                gencard_jocker = new CardInfo(CardInfo.Shape.ColorJocker, 0);
                foreach (CardInfo reject in rejects)
                {
                    if (gencard_jocker == reject)
                    {
                        Is_reject = true;
                    }
                }
                if (!Is_reject)
                {
                    CardDeck.Add(gencard_jocker);
                }
                //덱 완성(n 장)
                Shuffle(ref CardDeck);
            }
            Console.WriteLine("카드 섞음 " + LastCard);
            CardDeck.Remove(LastCard);
        }

        private Random rng = new Random();

        private void Shuffle(ref List<CardInfo> list)
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