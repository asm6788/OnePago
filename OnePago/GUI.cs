using OpenTK;
using OpenTK.Graphics;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnePago
{
    public class GUI
    {
        private static Int32 WM_KEYDOWN = 0x100;
        private static Int32 WM_KEYUP = 0x101;

        public enum WMessages : int

        {

            WM_LBUTTONDOWN = 0x201, //Left mousebutton down

            WM_LBUTTONUP = 0x202,  //Left mousebutton up

            WM_LBUTTONDBLCLK = 0x203, //Left mousebutton doubleclick

            WM_RBUTTONDOWN = 0x204, //Right mousebutton down

            WM_RBUTTONUP = 0x205,   //Right mousebutton up

            WM_RBUTTONDBLCLK = 0x206, //Right mousebutton doubleclick

            WM_KEYDOWN = 0x100,  //Key down

            WM_KEYUP = 0x101,   //Key up

        }
        public enum VKeys : int

        {

            //Keys used for gaming.

            Heal_Complete = VK_3,

            Heal_Normal = VK_4,

            Heal_Team = VK_5,

            Assist = VK_2,

            //-----END-----

            VK_LBUTTON = 0x01,   //Left mouse button 

            VK_RBUTTON = 0x02,   //Right mouse button 

            VK_CANCEL = 0x03,   //Control-break processing 

            VK_MBUTTON = 0x04,   //Middle mouse button (three-button mouse) 

            VK_BACK = 0x08,   //BACKSPACE key 

            VK_TAB = 0x09,   //TAB key 

            VK_CLEAR = 0x0C,   //CLEAR key 

            VK_RETURN = 0x0D,   //ENTER key 

            VK_SHIFT = 0x10,   //SHIFT key 

            VK_CONTROL = 0x11,   //CTRL key 

            VK_MENU = 0x12,   //ALT key 

            VK_PAUSE = 0x13,   //PAUSE key 

            VK_CAPITAL = 0x14,   //CAPS LOCK key 

            VK_ESCAPE = 0x1B,   //ESC key 

            VK_SPACE = 0x20,   //SPACEBAR 

            VK_PRIOR = 0x21,   //PAGE UP key 

            VK_NEXT = 0x22,   //PAGE DOWN key 

            VK_END = 0x23,   //END key 

            VK_HOME = 0x24,   //HOME key 

            VK_LEFT = 0x25,   //LEFT ARROW key 

            VK_UP = 0x26,   //UP ARROW key 

            VK_RIGHT = 0x27,   //RIGHT ARROW key 

            VK_DOWN = 0x28,   //DOWN ARROW key 

            VK_SELECT = 0x29,   //SELECT key 

            VK_PRINT = 0x2A,   //PRINT key

            VK_EXECUTE = 0x2B,   //EXECUTE key 

            VK_SNAPSHOT = 0x2C,   //PRINT SCREEN key 

            VK_INSERT = 0x2D,   //INS key 

            VK_DELETE = 0x2E,   //DEL key 

            VK_HELP = 0x2F,   //HELP key

            VK_OEMCOMMA = 0xBC, //OEMComma

            VK_0 = 0x30,   //0 key 

            VK_1 = 0x31,   //1 key 

            VK_2 = 0x32,   //2 key 

            VK_3 = 0x33,   //3 key 

            VK_4 = 0x34,   //4 key 

            VK_5 = 0x35,   //5 key 

            VK_6 = 0x36,    //6 key 

            VK_7 = 0x37,    //7 key 

            VK_8 = 0x38,   //8 key 

            VK_9 = 0x39,    //9 key 

            VK_A = 0x41,   //A key 

            VK_B = 0x42,   //B key 

            VK_C = 0x43,   //C key 

            VK_D = 0x44,   //D key 

            VK_E = 0x45,   //E key 

            VK_F = 0x46,   //F key 

            VK_G = 0x47,   //G key 

            VK_H = 0x48,   //H key 

            VK_I = 0x49,    //I key 

            VK_J = 0x4A,   //J key 

            VK_K = 0x4B,   //K key 

            VK_L = 0x4C,   //L key 

            VK_M = 0x4D,   //M key 

            VK_N = 0x4E,    //N key 

            VK_O = 0x4F,   //O key 

            VK_P = 0x50,    //P key 

            VK_Q = 0x51,   //Q key 

            VK_R = 0x52,   //R key 

            VK_S = 0x53,   //S key 

            VK_T = 0x54,   //T key 

            VK_U = 0x55,   //U key 

            VK_V = 0x56,   //V key 

            VK_W = 0x57,   //W key 

            VK_X = 0x58,   //X key 

            VK_Y = 0x59,   //Y key 

            VK_Z = 0x5A,    //Z key

            VK_NUMPAD0 = 0x60,   //Numeric keypad 0 key 

            VK_NUMPAD1 = 0x61,   //Numeric keypad 1 key 

            VK_NUMPAD2 = 0x62,   //Numeric keypad 2 key 

            VK_NUMPAD3 = 0x63,   //Numeric keypad 3 key 

            VK_NUMPAD4 = 0x64,   //Numeric keypad 4 key 

            VK_NUMPAD5 = 0x65,   //Numeric keypad 5 key 

            VK_NUMPAD6 = 0x66,   //Numeric keypad 6 key 

            VK_NUMPAD7 = 0x67,   //Numeric keypad 7 key 

            VK_NUMPAD8 = 0x68,   //Numeric keypad 8 key 

            VK_NUMPAD9 = 0x69,   //Numeric keypad 9 key 

            VK_SEPARATOR = 0x6C,   //Separator key 

            VK_SUBTRACT = 0x6D,   //Subtract key 

            VK_DECIMAL = 0x6E,   //Decimal key 

            VK_DIVIDE = 0x6F,   //Divide key

            VK_F1 = 0x70,   //F1 key 

            VK_F2 = 0x71,   //F2 key 

            VK_F3 = 0x72,   //F3 key 

            VK_F4 = 0x73,   //F4 key 

            VK_F5 = 0x74,   //F5 key 

            VK_F6 = 0x75,   //F6 key 

            VK_F7 = 0x76,   //F7 key 

            VK_F8 = 0x77,   //F8 key 

            VK_F9 = 0x78,   //F9 key 

            VK_F10 = 0x79,   //F10 key 

            VK_F11 = 0x7A,   //F11 key 

            VK_F12 = 0x7B,   //F12 key

            VK_SCROLL = 0x91,   //SCROLL LOCK key 

            VK_LSHIFT = 0xA0,   //Left SHIFT key

            VK_RSHIFT = 0xA1,   //Right SHIFT key

            VK_LCONTROL = 0xA2,   //Left CONTROL key

            VK_RCONTROL = 0xA3,    //Right CONTROL key

            VK_LMENU = 0xA4,      //Left MENU key

            VK_RMENU = 0xA5,   //Right MENU key

            VK_PLAY = 0xFA,   //Play key

            VK_ZOOM = 0xFB, //Zoom key 

        }

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool PostMessage(IntPtr hWnd, int Msg, Int32 wParam, int lParam);


        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        public static extern IntPtr FindWindowByCaption(IntPtr zeroOnly, string lpWindowName);

        public static void SendKey(IntPtr hWnd, string input)
        {
            foreach (char c in input)
            {
                PostMessage(hWnd, WM_KEYDOWN, (int)(VKeys)Enum.Parse(typeof(VKeys), "VK_" + c), 0);
            }
            PostMessage(hWnd, WM_KEYDOWN, (int)VKeys.VK_RETURN, 0);
        }

        public class GameObject
        {
            public Sprite sprite;
            public OneCard.CardInfo card;
            public OneCard.Player who;

            public GameObject(Sprite textrue, OneCard.CardInfo card, OneCard.Player who)
            {
                this.sprite = textrue;
                this.card = card;
                this.who = who;
            }

            public override bool Equals(object obj)
            {
                var @object = obj as GameObject;
                return @object != null &&
                       EqualityComparer<OneCard.CardInfo>.Default.Equals(card, @object.card);
            }

            public override int GetHashCode()
            {
                return -964371657 + EqualityComparer<OneCard.CardInfo>.Default.GetHashCode(card);
            }

            public static bool operator ==(GameObject object1, GameObject object2)
            {
                return EqualityComparer<GameObject>.Default.Equals(object1, object2);
            }

            public static bool operator !=(GameObject object1, GameObject object2)
            {
                return !(object1 == object2);
            }
        }
        List<GameObject> Objs = new List<GameObject>();
        Dictionary<OneCard.CardInfo, string> FileDB = new Dictionary<OneCard.CardInfo, string>();
        RenderWindow window;
        Sprite trash = null;
        Text text_Trash = null;
        bool temp = false;
        public OneCard.Player whosturn;
        bool Only_two = false;
        public GUI(bool Only_two)
        {
            this.Only_two = Only_two;
            RegDictionary();
            window = new RenderWindow(new VideoMode(1500, 800), "원-파-고");
            window.SetFramerateLimit(60);
            window.MouseButtonReleased += Window_MouseButtonReleased;
            window.SetActive();
            Thread.Sleep(1000);
        }

        private void Window_MouseButtonReleased(object sender, MouseButtonEventArgs e)
        {
            Vector2f mouse_pos = window.MapPixelToCoords(Mouse.GetPosition(window));
            GameObject result = null;
            int index = 0;
            for (int i = 0; i < Objs.Count; i++)
            {
                if (Objs[i].sprite.GetGlobalBounds().Contains(mouse_pos.X, mouse_pos.Y) && Objs[i].who == whosturn)
                {
                    result = Objs[i];
                    index = i;
                }
            }
            if (result == null)
            {
                SendKey(FindWindowByCaption(IntPtr.Zero, Console.Title), whosturn.Cards.Count.ToString());
            }
            else
            {
                SendKey(FindWindowByCaption(IntPtr.Zero, Console.Title), result.card.ID.ToString());
            }
        }

        public void LOOP()
        {
            while (window.IsOpen)
            {
                window.Clear();
                window.DispatchEvents();
                for (int i = 0; i != Objs.Count; i++)
                {
                    if (i < Objs.Count)
                    {
                        window.Draw(Objs[i].sprite);
                    }
                }
                if (trash != null)
                {
                    window.Draw(trash);
                }
                if (temp && text_Trash != null)
                {
                    window.Draw(text_Trash);
                }
                window.Display();
            }
        }
        void RegDictionary()
        {
            for (int shape = 1; shape != 5; shape++)
            {
                for (int number = 1; number != 14; number++)
                {
                    string genfile = @"Cards\";
                    switch (number)
                    {
                        case 0:
                            break;
                        case 1:
                            genfile += "ace";
                            break;
                        case 11:
                            genfile += "jack";
                            break;
                        case 12:
                            genfile += "queen";
                            break;
                        case 13:
                            genfile += "king";
                            break;
                        default:
                            genfile += number;
                            break;
                    }
                    genfile += "_of_";
                    switch (shape)
                    {
                        case 1:
                            genfile += "diamonds";
                            break;
                        case 2:
                            genfile += "hearts";
                            break;
                        case 3:
                            genfile += "spades";
                            break;
                        case 4:
                            genfile += "clubs";
                            break;
                        default:
                            break;
                    }
                    genfile += ".png";
                    FileDB.Add(new OneCard.CardInfo((OneCard.CardInfo.Shape)shape, number), genfile);
                }
            }
            FileDB.Add(new OneCard.CardInfo(OneCard.CardInfo.Shape.BlackJocker, 0), @"Cards\black_joker.png");
            FileDB.Add(new OneCard.CardInfo(OneCard.CardInfo.Shape.ColorJocker, 0), @"Cards\red_joker.png");
        }

        public void RegGO(OneCard.CardInfo card, OneCard.Player who)
        {
            if (!Only_two)
            {
                if (who.ID == 0)
                {
                    Objs.Add(new GameObject(new Sprite(new Texture("Cards/Back.png")) { Scale = new Vector2f(0.3f, 0.3f), Position = new Vector2f(600 + 30 * who.x_margin, -100 + who.y_margin) }, card, who));
                    who.x_margin += 1;
                }
                else if (who.ID == 1)
                {
                    Objs.Add(new GameObject(new Sprite(new Texture("Cards/Back.png")) { Scale = new Vector2f(0.3f, 0.3f), Position = new Vector2f(1600, 210 + 10 * who.y_margin), Rotation = 90 }, card, who));
                    who.y_margin += 1;
                }
                else if (who.ID == 2)
                {
                    Objs.Add(new GameObject(new Sprite(new Texture(FileDB[card])) { Scale = new Vector2f(0.3f, 0.3f), Position = new Vector2f(600 + 30 * who.x_margin, 600) }, card, who));
                    who.x_margin += 1;
                }
                else if (who.ID == 3)
                {
                    Objs.Add(new GameObject(new Sprite(new Texture("Cards/Back.png")) { Scale = new Vector2f(0.3f, 0.3f), Position = new Vector2f(100, 210 + 10 * who.y_margin), Rotation = 90 }, card, who));
                    who.y_margin += 1;
                }
            }
            else
            {
                if (who.ID == 0)
                {
                    Objs.Add(new GameObject(new Sprite(new Texture("Cards/Back.png")) { Scale = new Vector2f(0.3f, 0.3f), Position = new Vector2f(600 + 30 * who.x_margin, -100 + who.y_margin) }, card, who));
                    who.x_margin += 1;
                }
                else if (who.ID == 1)
                {
                    Objs.Add(new GameObject(new Sprite(new Texture(FileDB[card])) { Scale = new Vector2f(0.3f, 0.3f), Position = new Vector2f(600 + 30 * who.x_margin, 600) }, card, who));
                    who.x_margin += 1;
                }
            }
        }

        public void Update_Trash(OneCard.CardInfo card)
        {
            temp = false;
            trash = new Sprite(new Texture(FileDB[card]))
            {
                Scale = new Vector2f(0.3f, 0.3f),
                Position = new Vector2f(700, 250)
            };
            if(card.temp)
            {
                temp = true;
                text_Trash = new Text(card.ToString(), new Font("ZillaSlab-Light.ttf"), 30) { Position = new Vector2f(600, 350) };
            }
        }

        public void DelGO(OneCard.CardInfo card, OneCard.Player who)
        {
            int index = -1;
            for (int i = 0; i < Objs.Count; i++)
            {
                if (Objs[i].card == card)
                {
                    index = i;
                }
                if (index != - 1 &&i > index && Objs[i].who == who)
                {
                    if (who.Horizon)
                    {
                        Objs[i].sprite.Position = new Vector2f(Objs[i].sprite.Position.X - 30, Objs[i].sprite.Position.Y);
                    }
                    else
                    {
                        Objs[i].sprite.Position = new Vector2f(Objs[i].sprite.Position.X, Objs[i].sprite.Position.Y-10);
                    }
                    index = i;
                }
            }
            Update_Trash(card);
            if(who.Horizon)
            {
                who.x_margin = (int)(Objs[index].sprite.Position.X - 600) / 30;
                who.x_margin += 1;
            }
            else
            {
                who.y_margin = (int)(Objs[index].sprite.Position.Y - 210) / 30;
                who.y_margin += 1;
            }
            Objs.RemoveAll(o => o.card == card);
         
        }

        public void Bankrupt(OneCard.Player who)
        {
            Objs.RemoveAll(o => o.who == who);
        }

    }


}
