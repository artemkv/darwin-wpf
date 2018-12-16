using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artemkv.Darwin.Common.Validation
{
	public class ValidationError
	{
		private List<ValidationError> _nestedErrors = new List<ValidationError>();

		public string Message { get; private set; }
		public ValidationError Parent { get; private set; }

		public ValidationError(string message)
		{
			if (String.IsNullOrWhiteSpace(message))
				throw new ArgumentNullException("message");

			this.Message = message;
		}

		public void AddErrorAsNested(ValidationError error)
		{
			if (error == null)
				throw new ArgumentNullException("error");

			error.Parent = this;
			_nestedErrors.Add(error);
		}

		public string ToXmlString()
		{
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < _nestedErrors.Count; i++)
			{
				sb.Append(String.Format(@"<InnerError>{0}</InnerError>", _nestedErrors[i].ToXmlString()));
			}

			return String.Format(@"<Error Message=""{0}""><InnerErrors>{1}</InnerErrors></Error>", Message, sb.ToString());
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < _nestedErrors.Count; i++)
			{
				sb.Append(String.Format("Error #{0}. {1};", (i + 1).ToString(), _nestedErrors[i].ToString()));
			}

			return String.Format("{0} ({1}).", Message, sb.ToString());
		}
	}
}
