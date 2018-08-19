using System;
using System.ComponentModel.DataAnnotations;

namespace Vidly.Dtos
{
	public class CustomersDto
	{
		public int Id { get; set; }

		[Required]
		[StringLength(255)]
		public string Name { get; set; }

		public bool IsSubscribedToNewsletter { get; set; }

		public byte MembershipTypeId { get; set; }

		//[Min18IfAMember]
		public DateTime? Birthdate { get; set; }
	}
}