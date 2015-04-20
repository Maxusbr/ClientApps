using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Models.Car.Users
{
    public class UserSettingsModel
    {
        private bool _openCarDetails = true;

        public bool OpenCarDetails
        {
            get { return _openCarDetails; }
            set { _openCarDetails = value; }
        }

        private bool _openCarDetailsInNewWindow = false;

        public bool OpenCarDetailsInNewWindow
        {
            get { return _openCarDetailsInNewWindow; }
            set { _openCarDetailsInNewWindow = value; }
        }


        private List<WindowList> _wins = new List<WindowList>();

        public List<WindowList> Wins
        {
            get { return _wins; }
            set { _wins = value; }
        }





        public UserSettingsModel Clone()
        {
            UserSettingsModel temp = (UserSettingsModel)this.MemberwiseClone();
            return temp;
        }

        public override bool Equals(object other)
        {
            if (other == null)
                return false;
            if (object.ReferenceEquals(this, other))
                return true;
            if (this.GetType() != other.GetType())
                return false;
            UserSettingsModel usm = (UserSettingsModel)other;
            return this.Equals(usm);
        }

        public bool Equals(UserSettingsModel other)
        {
            if (other == null)
                return false;

            //Здесь сравнение по ссылкам необязательно.
            //Если вы уверены, что многие проверки на идентичность будут отсекаться на проверке по ссылке - //можно имплементировать.
            if (object.ReferenceEquals(this, other))
                return true;

            //Если по логике проверки, экземпляры родительского класса и класса потомка могут считаться равными,
            //то проверять на идентичность необязательно и можно переходить сразу к сравниванию полей.
            if (this.GetType() != other.GetType())
                return false;

            if (this.OpenCarDetails != other.OpenCarDetails ||
                this.OpenCarDetailsInNewWindow != other.OpenCarDetailsInNewWindow)
                return false;

            int max = this.Wins.Count();
            if (other.Wins.Count() != max)
                return false;

            else
            {
                for (int i = 0; i < max; i++)
                {
                    if (this.Wins[i].Equals(other.Wins[i]) == false)
                        return false;
                }
            }

            return true;
        }        





        public class WindowList
        {
            public int ID { get; set; }

            public int Left { get; set; }

            public int Top { get; set; }

            public int Width { get; set; }

            public int Heigt { get; set; }

            public string Title { get; set; }

            public override bool Equals(object other)
            {
                if (other == null)
                    return false;
                if (object.ReferenceEquals(this, other))
                    return true;
                if (this.GetType() != other.GetType())
                    return false;
                WindowList wl = (WindowList)other;
                return this.Equals(wl);
            }

            public bool Equals(WindowList other)
            {
                if (other == null)
                    return false;

                //Здесь сравнение по ссылкам необязательно.
                //Если вы уверены, что многие проверки на идентичность будут отсекаться на проверке по ссылке - //можно имплементировать.
                if (object.ReferenceEquals(this, other))
                    return true;

                //Если по логике проверки, экземпляры родительского класса и класса потомка могут считаться равными,
                //то проверять на идентичность необязательно и можно переходить сразу к сравниванию полей.
                if (this.GetType() != other.GetType())
                    return false;

                if (this.ID == other.ID &&
                    this.Heigt == other.Heigt &&
                    this.Left == other.Left &&
                    this.Title == other.Title &&
                    this.Top == other.Top &&
                    this.Width == other.Width)
                    return true;
                else
                    return false;
            }        
        }
    }
}
