namespace SharedSquawk.Client.Model.Entities
{
    public class Country
    {
        public Country(int countryId, string name)
        {
			CountryId = countryId;
            Name = name;
        }

		public int CountryId { get; private set; }

        public string Name { get; private set; }

        public override string ToString()
        {
			return string.Format("+{0} ({1})", CountryId, Name);
        }
    }
}
