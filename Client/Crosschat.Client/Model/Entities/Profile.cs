using System;
using SharedSquawk.Server.Application.DataTransferObjects;

namespace SharedSquawk.Client.Model.Entities
{
	public class Profile
	{
		public bool IsMe{ get; set; }
		public string PracticingLanguages  { get; set; }
		public string KnownLanguages  { get; set; }
		public int LocaleID  { get; set; }
		public int Age  { get; set; }
		public Gender Gender
		{
			get{return (Gender)GenderInt;}
			set{ GenderInt = (int)value; }
		}
		public int GenderInt  { get; set; }
		public string LastName  { get; set; }
		public string FirstName  { get; set; }
		public int UserId  { get; set; }

		public ProfileDetails Details{get;set;}
	}

	public class ProfileDetails
	{
		public string US  { get; set; }
		public string L { get; set; }
		public string Description { get; set; }
		public int XCM  { get; set; }
		public int XCC  { get; set; }
		public string RegistrationDate { get; set; }
	}
}

