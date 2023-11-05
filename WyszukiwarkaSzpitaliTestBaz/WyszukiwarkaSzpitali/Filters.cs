namespace Program
{
    public class Filters
    {
        public string Location;
        public string LekarzName;
        public string OddzialType;
        public string SzpitalName;
        public int LekarzId;

        public int OddzialId;
        public int SzpitalId;

        public bool HasLocationOrOddzial()
        {
            return (LocationEmpty() || OddzialEmpty());
        }

        public Filters()
        {
            this.Location = string.Empty;
            this.LekarzName = string.Empty;
            this.OddzialType = string.Empty;
            this.SzpitalName = string.Empty;
            this.LekarzId = -1;
            this.SzpitalId = -1;
        }

        public void SetLocation(string location)
        {
            Console.WriteLine("location: " + location);
            if (location[0] != '_')

            {
                this.Location = location;
            }

            return;
        }
        public void SetSzpital(string name, int id = -1)
        {
            if (name[0] != '_')
            {
                this.SzpitalName = name;
                this.SzpitalId = id;
            }

            return;
        }

        public void SetOddzialType(string oddzial)
        {
            if (oddzial[0] != '_')
            {
                this.OddzialType = oddzial;
            }

            return;
        }

        public void SetLekarz(int lekarzId)
        {
            if (lekarzId != 0)
            {
                this.LekarzId = lekarzId;
            }

            return;
        }

        public bool LekarzEmpty()
        {
            return (this.LekarzId < 0);
        }

        public bool LocationEmpty()
        {
            return (this.Location.Length == 0);
        }
        public bool OddzialEmpty()
        {
            return (this.OddzialType.Length == 0);
        }

        public bool SzpitalEmpty()
        {
            return (this.SzpitalName.Length == 0);
        }

        public bool Empty()
        {
            return (LocationEmpty() && OddzialEmpty() && SzpitalEmpty() && LekarzEmpty());
        }
    }

}